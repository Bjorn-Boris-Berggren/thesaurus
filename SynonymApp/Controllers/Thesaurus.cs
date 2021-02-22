
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using additude.thesaurus.Interfaces;
using additude.thesaurus.Models;


namespace additude.thesaurus.Controllers
{
    /// <summary>
    /// Thesaurus class that implements IThesaurus
    /// It adds and gets synomyms.
    /// </summary>
    public class Thesaurus : ControllerBase, IThesaurus
    {
        private readonly ThesaurusContext context;
        public readonly object dbLock = new object();

        public Thesaurus()
        {
            DbContextOptionsBuilder<ThesaurusContext> dbContextBuilder = new DbContextOptionsBuilder<ThesaurusContext>();
            dbContextBuilder.UseSqlServer($"server={ServerConfig.Server};uid={ServerConfig.User};pwd={ServerConfig.Password};database={ServerConfig.Database}");
            this.context = new ThesaurusContext(dbContextBuilder.Options);
        }
        /// <summary>
        /// Maps synonyms with a meaning-ID and adds them to the MeaningGroup table. It also adds all synonyms to the Word-table if not already added. Only a-z and 0-9 are allowed.
        /// </summary>
        public void AddSynonyms(IEnumerable<string> synonyms)
        {

            try
            {
                if (synonyms == null)
                {
                    throw new ArgumentNullException("The 'synonyms' argument has a null-value");
                }
                if (synonyms.Count() < 2)
                {
                    throw new Exception("Requires at least two words, to be able to store synonyms");
                }

                foreach (string synonym in synonyms)
                {
                    Regex onlyLettersAndDigits = new Regex("^[0-9a-z_]*$");
                    string synonymLowerCase = synonym.ToLower();

                    // Make sure that the synonym does not contain illegal characters
                    if (!onlyLettersAndDigits.IsMatch(synonymLowerCase))
                    {
                        // words with illegal characters
                        throw new Exception($"The synonym {synonymLowerCase} has illegal characters");
                    }

                    if (context.Words.Find(synonymLowerCase) == null)
                    {
                        context.Words.Add(new Word
                        {
                            Name = synonymLowerCase,
                        });
                    }
                    // The meaningID that will be mapped with the synonyms
                    int nextMeaningID = 1;

                    // Generates a new meaningID-value if there are already stored MeaningGroups
                    if (context.MeaningGroups.Any())
                    {
                        nextMeaningID = context.MeaningGroups.Max(p => p.MeaningID) + 1;
                    }
                    // Maps the synonyms with the meaningID
                    context.MeaningGroups.Add(new MeaningGroup
                    {
                        WordName = synonymLowerCase,
                        MeaningID = nextMeaningID
                    });

                }
                // Locks the database while editing
                lock (dbLock)
                {
                    // Stores the synonyms with the meaningID
                    context.SaveChanges();
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
        }
        /// <summary>
        /// Gets synonyms for the given word from the MeaningGroup-table. Only a-z and 0-9 are allowed as an argument.
        /// </summary>
        /// <returns>
        /// A list of synonyms for the given word
        /// </returns>
        public IEnumerable<string> GetSynonyms(string word)
        {
            Regex onlyLettersAndDigits = new Regex("^[0-9a-z_]*$");
            word = word.ToLower();

            try
            {
                if (!onlyLettersAndDigits.IsMatch(word))
                {
                    // Word with illegal characters
                    throw new Exception($"The word {word} has illegal characters");
                }
                lock (dbLock)
                {
                    IEnumerable<int> meaningIDs = new List<int>(context.MeaningGroups.Where(w => String.Equals(word, w.WordName)).Select(w => w.MeaningID));
                    if (meaningIDs == null)
                        throw new Exception("No synonym was found");
                    IEnumerable<string> synonyms = context.MeaningGroups.Where(p => meaningIDs.Contains(p.MeaningID)).Where(p => String.Equals(word, p.WordName) == false).Select(w => w.WordName).ToList();

                    return synonyms;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return null;
        }
        /// <summary>
        /// Gets all the words in the Words-table
        /// </summary>
        /// <returns>
        /// A list of words
        /// </returns>
        public IEnumerable<string> GetWords()
        {
            lock (dbLock)
            {
                IEnumerable<string> words = new List<string>(context.Words.Select(p => p.Name)).ToList();
                return words;
            }
        }

    }
}