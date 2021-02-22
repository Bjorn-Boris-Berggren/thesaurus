using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace additude.thesaurus.Models
{
    /// <summary>
    /// This model represents the mapping between meanings and words, one can word can have several meanings and one meaning can have several words 
    /// Both WordName and MeaningID belong to the primary key
    /// </summary>
    [Table("MeaningGroup")]
    public class MeaningGroup
    {
        [ForeignKey("Word")]
        [Column("WordName")]
        public string WordName { get; set; }
        [Column("MeaningID")]
        public int MeaningID { get; set; }
    }
}