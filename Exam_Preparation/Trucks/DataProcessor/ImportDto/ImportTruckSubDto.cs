namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    using Common;
    using Data.Models.Enums;

    [XmlType("Truck")]
    public class ImportTruckSubDto
    {
        [XmlElement("RegistrationNumber")]
        [Required]
        [MinLength(ModelConstants.TruckRegNumLength)]
        [MaxLength(ModelConstants.TruckRegNumLength)]
        [RegularExpression(ModelConstants.TruckRegNumRegex)]
        public string? RegistrationNumber { get; set; } = null!;

        [XmlElement("VinNumber")]
        [Required]
        [MinLength(ModelConstants.TruckVinNumLength)]
        [MaxLength(ModelConstants.TruckVinNumLength)]
        public string VinNumber { get; set; } = null!;

        [XmlElement("TankCapacity")]
        [Required]
        [Range(ModelConstants.TruckTankCapacityMinValue,
               ModelConstants.TruckTankCapacityMaxValue)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Required]
        [Range(ModelConstants.TruckCargoCapacityMinValue, 
               ModelConstants.TruckCargoCapacityMaxValue)]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        [Range(0,3)]
        public int CategoryType { get; set; }

        [XmlElement("MakeType")]
        [Required]
        [Range(0,4)]
    
        public int MakeType { get; set; }

    }
}
