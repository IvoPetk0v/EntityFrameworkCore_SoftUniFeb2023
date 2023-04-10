using Boardgames.Data.Models;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlElement]
        public string CreatorName { get; set; } = null!;

        [XmlArray]
        public ExportBoardGameDto[] Boardgames { get; set; } = null!;


    }
}
