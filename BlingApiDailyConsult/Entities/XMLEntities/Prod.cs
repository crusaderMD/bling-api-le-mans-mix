using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "prod")]
    public class Prod
    {

        [XmlElement(ElementName = "cProd")]
        public string? CProd { get; set; }

        [XmlElement(ElementName = "cEAN")]
        public string? CEAN { get; set; }

        [XmlElement(ElementName = "xProd")]
        public string? XProd { get; set; }

        [XmlElement(ElementName = "NCM")]
        public int NCM { get; set; }

        [XmlElement(ElementName = "CEST")]
        public int CEST { get; set; }

        [XmlElement(ElementName = "CFOP")]
        public int CFOP { get; set; }

        [XmlElement(ElementName = "uCom")]
        public string? UCom { get; set; }

        [XmlElement(ElementName = "qCom")]
        public double QCom { get; set; }

        [XmlElement(ElementName = "vUnCom")]
        public double VUnCom { get; set; }

        [XmlElement(ElementName = "vProd")]
        public double VProd { get; set; }

        [XmlElement(ElementName = "cEANTrib")]
        public string? CEANTrib { get; set; }

        [XmlElement(ElementName = "uTrib")]
        public string? UTrib { get; set; }

        [XmlElement(ElementName = "qTrib")]
        public double QTrib { get; set; }

        [XmlElement(ElementName = "vUnTrib")]
        public double VUnTrib { get; set; }

        [XmlElement(ElementName = "indTot")]
        public int IndTot { get; set; }
    }
}
