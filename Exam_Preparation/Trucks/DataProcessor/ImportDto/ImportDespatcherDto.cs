namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Common;
    using Data.Models;

    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(ModelConstants.DespatcherNameMinLength)]
        [MaxLength(ModelConstants.DespatcherNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        [Required]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        public  ImportTruckSubDto[] Trucks { get; set; } 

    }
}
