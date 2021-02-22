using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace additude.thesaurus.Models
{
    /// <summary>
    /// This model represents the words, all synonyms must be added here once
    /// </summary>
    [Table("Word")]
    public class Word
    {
        [Column("Name")]
        [Key]
        public string Name { get; set; }
    }
}