
namespace MusicHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;
    using Enums;

    public class Song
    {
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.SongNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Genre Genre { get; set; }

        public decimal Price { get; set; }

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }

        public virtual Album? Album { get; set; }

        [ForeignKey(nameof(Writer))]
        public int WriterId { get; set; }

        public Writer Writer { get; set; } = null!;

        public virtual ICollection<SongPerformer> SongPerformers { get; set; }
    }
}
