using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "transporta")]
    public class Transporta
    {

        [XmlElement(ElementName = "CNPJ")]
        public double CNPJ { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string? XNome { get; set; }

        [XmlElement(ElementName = "IE")]
        public double IE { get; set; }

        [XmlElement(ElementName = "xEnder")]
        public string? XEnder { get; set; }

        [XmlElement(ElementName = "xMun")]
        public string? XMun { get; set; }

        [XmlElement(ElementName = "UF")]
        public string? UF { get; set; }
    }
}
