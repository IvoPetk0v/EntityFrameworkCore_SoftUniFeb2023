
using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ResourceNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!; // not unicode


        [Required]
        public ResourceType ResourceType { get; set; }

        [Required]
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; } = null!;

    }
}
