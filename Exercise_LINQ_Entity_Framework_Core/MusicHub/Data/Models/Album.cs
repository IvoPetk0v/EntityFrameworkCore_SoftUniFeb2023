namespace MusicHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.AlbumNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime ReleasedOn { get; set; }


        [NotMapped]
        public decimal Price => this.Songs.Sum(s=>s.Price); 

        public virtual ICollection<Song> Songs { get; set; }

    }
}
