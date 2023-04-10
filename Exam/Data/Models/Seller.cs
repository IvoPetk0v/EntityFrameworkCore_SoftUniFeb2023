using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models
{

    public class Seller
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)] // min 5 
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(30)] // min 2 
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]  // regex => First four characters are "www.", followed by upper and lower letters, digits or '-' and the last three characters are ".com".
        public string Website { get; set; } = null!;

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }= new HashSet<BoardgameSeller>();


    }
}
