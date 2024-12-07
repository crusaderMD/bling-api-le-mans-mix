using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "protNFe")]
    public class ProtNFe
    {

        [XmlElement(ElementName = "infProt")]
        public InfProt? InfProt { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public double Versao { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
