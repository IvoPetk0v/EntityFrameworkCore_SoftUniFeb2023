using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [XmlElement]
        [Required]
        [MaxLength(7)]
        [MinLength(2)]
        public string FirstName { get; set; } = null!;

        [XmlElement]
        [Required]
        [MaxLength(7)]
        [MinLength(2)] 
        public string LastName { get; set; } = null!;

        [XmlArray]
        public ImportBoardGameDto[] Boardgames { get; set; } = null!;
    }
}
