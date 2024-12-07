using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{

    [XmlRoot(ElementName = "enderDest")]
    public class EnderDest
    {

        [XmlElement(ElementName = "xLgr")]
        public string? XLgr { get; set; }

        [XmlElement(ElementName = "nro")]
        public string? Nro { get; set; }

        [XmlElement(ElementName = "xBairro")]
        public string? XBairro { get; set; }

        [XmlElement(ElementName = "cMun")]
        public string? CMun { get; set; }

        [XmlElement(ElementName = "xMun")]
        public string? XMun { get; set; }

        [XmlElement(ElementName = "UF")]
        public string? UF { get; set; }

        [XmlElement(ElementName = "CEP")]
        public string? CEP { get; set; }

        [XmlElement(ElementName = "cPais")]
        public string? CPais { get; set; }

        [XmlElement(ElementName = "xPais")]
        public string? XPais { get; set; }

        [XmlElement(ElementName = "fone")]
        public string? Fone { get; set; }
    }
}