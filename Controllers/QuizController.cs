using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuizDB_API.Models;
using Microsoft.EntityFrameworkCore;

// Define the QuizController class inheriting ControllerBase
[ApiController]
[Route("api")]
public class QuizController : ControllerBase
{
    // Reference to QuizDbContext for database access
    private readonly QuizDbContext _dbContext;
    private readonly IConfiguration _configuration;

    // Constructor with dependency injection for QuizDbContext
    public QuizController(QuizDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    // API endpoint to generate a quiz. Route examples: 
    // http://localhost:5223/api/GenerateQuiz
    // http://localhost:5223/api/GenerateQuiz?qty=100
    // http://localhost:5223/api/GenerateQuiz?topic=biology
    // http://localhost:5223/api/GenerateQuiz?topic=chemistry&qty=5
    [HttpGet("GenerateQuiz")] 
    public async Task<IActionResult> GenerateQuizAsync(string? topic = null, string? qty = null) // both parameters are optional
    {
        // Read the OpenAI API settings section from appsettings.json
        var openAiApiSettings = _configuration.GetSection("OpenAI");

        // Check if the OpenAI section exists and if all the required values are present
        if (!openAiApiSettings.Exists() ||
            string.IsNullOrEmpty(openAiApiSettings["APIURL"]) ||
            string.IsNullOrEmpty(openAiApiSettings["APIKey"]) ||
            string.IsNullOrEmpty(openAiApiSettings["Model"]) ||
            string.IsNullOrEmpty(openAiApiSettings["MaxTokens"]) ||
            string.IsNullOrEmpty(openAiApiSettings["Temperature"]))
        {
            // If any of the required values are missing or empty, return a BadRequest response
            return BadRequest("Enter required values for OpenAI into the appsettings.json file.");
        }

        // Read OpenAI API settings from appsettings.json
        string? openAiApiURL = _configuration["OpenAI:APIURL"];
        string? openAiApiKey = _configuration["OpenAI:APIKey"];
        string? model = _configuration["OpenAI:Model"];
        int max_tokens = int.Parse(_configuration["OpenAI:MaxTokens"]!);
        double temperature = double.Parse(_configuration["OpenAI:Temperature"]!);
        string? prompt = _configuration["OpenAI:Prompt"];

        string responseContent = string.Empty; // Variabled to store the entire response content
        string quizContent = string.Empty; // Variable to store the generated quiz content

        // Modify the prompt if a specific topic is provided
        if (!string.IsNullOrEmpty(topic))
            prompt = prompt!.Replace("general", topic);

        // Modify the prompt if a specific qty is provided
        if (!string.IsNullOrEmpty(qty))
            prompt = prompt!.Replace("three", qty);


        // Create the data object to hold prompt, model, max_tokens, and temperature
        var data = new
        {
            prompt,
            model,
            max_tokens,
            temperature
        };

        // Serialize the data object to JSON
        string json = JsonConvert.SerializeObject(data);

        // Use HttpClient to communicate with the OpenAI API
        using (var client = new HttpClient())
        {
            // Set the Authorization header using the OpenAI API key
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiApiKey);

            // Send a POST request to the OpenAI API with the JSON data
            var response = await client.PostAsync(openAiApiURL, new StringContent(json, Encoding.UTF8, "application/json"));

            // If the response is not successful, return a BadRequest response
            if (!response.IsSuccessStatusCode)
                return BadRequest($"There was a problem communicating with OpenAI API. Status code: {response.StatusCode}");

            // Read the response content as string
            responseContent = await response.Content.ReadAsStringAsync();
        }

        try
        {
            // Try to deserialize the response content as dynamic object, or use a new ExpandoObject if null
            dynamic result = JsonConvert.DeserializeObject(responseContent) ?? new System.Dynamic.ExpandoObject();

            // Check if the generated quiz is present in the result and get the content if it exists
            if (result.choices != null && result.choices.Count > 0 && result.choices[0].text != null)
                quizContent = result.choices[0].text;
        }
        catch (Exception exception)
        {
            // Return a BadRequest response if there's an error during deserialization
            return BadRequest($"An error occurred during the deserialization of response from OpenAI API.\nException message: {exception.Message}");
        }

        try
        {
            // Deserialize quizContent as a JObject
            JObject quizData = JObject.Parse(quizContent);

            // Save the generated quiz data to the database
            var quizPrompt = new QuizPrompt
            {
                PromptText = prompt
            };
            _dbContext.QuizPrompts?.Add(quizPrompt);
            _dbContext.SaveChanges();

            // Get the generated prompt ID
            int promptId = quizPrompt.Id;


            // Process each question in quizData and save it to the database
            foreach (var questionEntry in quizData)
            {
                string questionKey = questionEntry.Key;
                JObject questionObject = (JObject)questionEntry.Value!;

                // Extract question text and correct answer from the questionObject
                string questionText = questionObject["Question"]!.ToString();
                string correctAnswer = questionObject["Answer"]!.ToString();

                // Save the question to the database
                var quizQuestion = new QuizQuestion
                {
                    QuestionText = questionText,
                    OptionName = correctAnswer[0], // Take the first character of the correctAnswer as OptionName
                    Duplicate = false,
                    PromptId = promptId
                };

                _dbContext.QuizQuestions?.Add(quizQuestion);
                _dbContext.SaveChanges();

                // Get the generated question ID
                int questionId = quizQuestion.Id;

                // Process options for the question and save them to the database
                var optionsObject = (JObject)questionObject["Options"]!;
                foreach (var optionEntry in optionsObject)
                {
                    string optionName = optionEntry.Key;
                    string optionText = optionEntry.Value!.ToString();

                    var quizOption = new QuizOption
                    {
                        OptionName = optionName,
                        OptionText = optionText,
                        QuestionId = questionId
                    };

                    _dbContext.QuizOptions?.Add(quizOption);
                }
                _dbContext.SaveChanges();
            }

            // Execute the SQL statement to mark duplicates in quizquestions table
            string markDuplicatesSql = @"
                UPDATE quizquestions
                SET duplicate = TRUE
                WHERE id NOT IN (
                    SELECT MIN(id)
                    FROM quizquestions
                    GROUP BY questiontext
                )";

            // Execute the SQL statement
            _dbContext.Database.ExecuteSqlRaw(markDuplicatesSql);

        }
        catch (Exception exception)
        {
            // Return a BadRequest response if there's an error during processing
            return BadRequest($"An error occurred during processing of generated quiz.\nException message: {exception.Message}");
        }

        // Return the generated quiz content as an OK response
        return Ok(quizContent);

    } // End GenerateQuizAsync()
}
