using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace additude.thesaurus.Models
{
    /// <summary>
    /// This represents the context-object which is used for accessing the database via Entity Framework
    /// </summary>
    public partial class ThesaurusContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<MeaningGroup> MeaningGroups { get; set; }

        public ThesaurusContext(DbContextOptions<ThesaurusContext> options) : base(options)
        {
        }
        /// <summary>
        /// Making WordName and MeaningID the primary key for the MeaningGroup-model
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeaningGroup>()
                .HasKey(c => new { c.WordName, c.MeaningID });
        }
    }
}
