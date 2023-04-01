
using System.ComponentModel.DataAnnotations;
using Trucks.Common;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    public  class ImportClientDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(ModelConstants.ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(2)]        [MaxLength(ModelConstants.ClientNationalityMaxLength)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        public int[]  Trucks { get; set; } = null!;
    }
}
