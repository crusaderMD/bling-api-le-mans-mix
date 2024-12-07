using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NfeProc
    {

        [XmlElement(ElementName = "NFe")]
        public NFe? NFe { get; set; }

        [XmlElement(ElementName = "protNFe")]
        public ProtNFe? ProtNFe { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string? Xmlns { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public double Versao { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
