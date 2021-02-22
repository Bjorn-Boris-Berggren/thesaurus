using System.Collections.Generic;
/// <summary>
/// A simple thesaurus ///
/// </summary >


namespace additude.thesaurus.Interfaces
{
    public interface IThesaurus
    {
        /// <summary>
        /// Adds the given words as synonyms to each other
        /// </summary>
        void AddSynonyms(IEnumerable<string> synonyms);
        /// <summary>
        /// Gets the synonyms for a word
        /// </summary>
        IEnumerable<string> GetSynonyms(string word);
        /// <summary>
        /// Gets all words that are stored in the thesaurus
        /// </summary>
        IEnumerable<string> GetWords();
    }
}
