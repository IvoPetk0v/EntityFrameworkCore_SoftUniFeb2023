

using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardGameDto
    {
        [XmlElement]
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string Name { get; set; }

        [XmlElement]
        [Required]
        [Range(1,10.00)]
        public double Rating { get; set; } // range 1 10.00 

        [XmlElement]
        [Required]
        [Range(2018,2023)]
        public int YearPublished { get; set; } // rannge 2018 2023

        [XmlElement]
        [Required]
        [Range(0,4)]
        public int CategoryType { get; set; }

        [XmlElement]
        [Required]
        public string Mechanics { get; set; } = null!;

    }
}
