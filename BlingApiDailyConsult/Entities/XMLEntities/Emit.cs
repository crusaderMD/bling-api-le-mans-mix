using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{

    [XmlRoot(ElementName = "emit")]
    public class Emit
    {

        [XmlElement(ElementName = "CNPJ")]
        public double CNPJ { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string? XNome { get; set; }

        [XmlElement(ElementName = "xFant")]
        public string? XFant { get; set; }

        [XmlElement(ElementName = "enderEmit")]
        public EnderEmit EnderEmit { get; set; }

        [XmlElement(ElementName = "IE")]
        public double IE { get; set; }

        [XmlElement(ElementName = "CRT")]
        public int CRT { get; set; }
    }
}
