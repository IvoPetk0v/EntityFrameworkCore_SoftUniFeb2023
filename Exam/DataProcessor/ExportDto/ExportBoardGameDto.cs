using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportBoardGameDto
    {
        [XmlElement("BoardgameName")]
        public string Name { get; set; }

        [XmlElement("BoardgameYearPublished")]
        public int YearPublished { get; set; }
     
    }
}
