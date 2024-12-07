using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "NFe")]
    public class NFe
    {

        [XmlElement(ElementName = "infNFe")]
        public InfNFe? InfNFe { get; set; }

        [XmlElement(ElementName = "Signature")]
        public Signature? Signature { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string? Xmlns { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
