using Xunit;
using additude.thesaurus.Controllers;
using additude.thesaurus.Models;
using additude.thesaurus;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace additude.Tests
{
    /// <summary>
    /// Tests the Thesaurus class
    /// Functions being tested are AddSynonyms, GetSynonyms and GetWords.
    /// </summary>
    public class ThesaurusTest
    {
        Result result;
        private readonly ThesaurusContext context;
        private readonly Thesaurus mockObject;

        /// <summary>
        /// This will be be run several times during execution i.e. before every test function
        /// </summary>
        public ThesaurusTest()
        {
            mockObject = new Thesaurus();
            result = new Result();
            DbContextOptionsBuilder<ThesaurusContext> dbContextBuilder = new DbContextOptionsBuilder<ThesaurusContext>();
            dbContextBuilder.UseSqlServer($"server={ServerConfig.Server};uid={ServerConfig.User};pwd={ServerConfig.Password};database={ServerConfig.Database}");
            this.context = new ThesaurusContext(dbContextBuilder.Options);
            // Delete all rows before execution
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Word");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE MeaningGroup");
        }

        /// <summary>
        /// Tests that it is possible to add synonyms to the database
        /// </summary>
        [Fact]
        public void AddSynonyms()
        {
            List<string> synonymsInput = new List<string>();
            List<string> synonymsOutput = new List<string>();
            string testWord = "test";

            // All words must be in lower case only for the comparison, because the AddSynonyms-function converts the words to lower case 
            // before storing to database to avoid duplicates. So, it is possible to use upper case letters when storing to database but they will be converted 
            // to lower case
            testWord = testWord.ToLower();
            synonymsInput.Add(testWord);
            int amountOfSynonyms = 4;
            for (int i = 1; i < amountOfSynonyms; i++)
            {
                synonymsInput.Add(testWord + i);
            }
            mockObject.AddSynonyms(synonymsInput);
            synonymsOutput = (List<string>)mockObject.GetSynonyms(testWord);
            // synonymsOutput + testWord should be the same as synonymsInput
            synonymsOutput.Add(testWord);
            result = Compare(synonymsInput, synonymsOutput);

            Assert.True(result.Value, result.Message);
        }
        /// <summary>
        /// Tests that it is possible to get synonyms from the database when using more than one MeaningGroup
        /// </summary>
        [Fact]
        public void GetSynonyms()
        {
            List<string> synonymInputTest = new List<string>();
            List<string> synonymInputData = new List<string>();
            List<string> synonymOutput;
            int amountOfWords;
            string testWord = "test";
            string dataWord = "data";

            // All words must be in lower case for the comparison, because the AddSynonyms-function converts the strings to lower case 
            // before storing to database to avoid duplicates
            testWord = testWord.ToLower();
            dataWord = dataWord.ToLower();
            synonymInputTest.Add(testWord);
            amountOfWords = 4;
            for (int i = 1; i < amountOfWords; i++)
            {
                synonymInputTest.Add(testWord + i);
            }
            mockObject.AddSynonyms(synonymInputTest);

            synonymInputData.Add(dataWord);
            amountOfWords = 100;
            for (int i = 1; i < amountOfWords; i++)
            {
                synonymInputData.Add(dataWord + i);
            }

            mockObject.AddSynonyms(synonymInputData);

            synonymOutput = (List<string>)mockObject.GetSynonyms(testWord);
            synonymOutput.Add(testWord);

            result = Compare(synonymInputTest, synonymOutput);
            Assert.True(result.Value, result.Message);
        }
        /// <summary>
        /// Tests that it is possible get all the words used by Thesaurus
        /// </summary>
        [Fact]
        public void GetWords()
        {
            List<string> wordsInput = new List<string>();
            List<string> wordsOutput;
            string testWord = "test";

            // All words must be in lower case for the comparison, because the AddSynonyms-function converts the strings to lower case 
            // before storing to database to avoid duplicates
            testWord = testWord.ToLower();
            wordsInput.Add(testWord);
            int amountOfWords = 100;
            for (int i = 1; i < amountOfWords; i++)
            {
                wordsInput.Add(testWord + i);
            }
            mockObject.AddSynonyms(wordsInput);

            wordsOutput = (List<string>)mockObject.GetWords();

            result = Compare(wordsInput, wordsOutput);
            Assert.True(result.Value, result.Message);
        }
        /// <summary>
        /// Tests that it is NOT possible to add other characters than a-z and 1-9 to the database
        /// </summary>
        [Fact]
        public void AddBadData()
        {
            result.Value = true;
            result.Message = "";
            List<string> wordsInput = new List<string>();
            List<string> wordsOutput;
            string testWord = "te&st/";

            // All words must be in lower case for the comparison, because the AddSynonyms-function converts the strings to lower case 
            // before storing to database to avoid duplicates
            testWord = testWord.ToLower();
            wordsInput.Add(testWord.ToLower());
            int amountOfWords = 4;
            for (int i = 1; i < amountOfWords; i++)
            {
                wordsInput.Add(testWord + i);
            }
            mockObject.AddSynonyms(wordsInput);

            wordsOutput = (List<string>)mockObject.GetSynonyms(testWord);

            if (wordsOutput != null)
            {
                result.Value = false;
                result.Message = "It was possible to add invalid characters";
            }

            Assert.True(result.Value, result.Message);
        }
        /// <summary>
        /// Checks if the input and output match
        /// </summary>
        /// <returns>
        /// Result-object which contains true if they match and false if not. If they did not match the object also contains a message explaining why.
        /// </returns>
        public Result Compare(List<string> input, List<string> output)
        {
            result.Value = true;
            result.Message = "";

            foreach (string item in input)
            {
                result.Message += item;
            }
            // Checks that the input and output have the exact same amounts of words
            if (input.Count != output.Count)
            {
                result.Value = false;
                result.Message = $"Output amount {output.Count} does not match input amount {input.Count}";
                return result;
            }
            // All words in the output have to be in the input
            foreach (string synonymOutput in output)
            {

                if (input.Contains(synonymOutput.ToLower()) == false)
                {
                    result.Value = false;
                    result.Message += $"{synonymOutput} does not exists in the synonyms input";

                    return result;
                }
            }

            return result;

        }
    }
}

