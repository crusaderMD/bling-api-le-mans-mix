using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "dest")]
    public class Dest
    {

        [XmlElement(ElementName = "CNPJ")]
        public string? CNPJ { get; set; }

        [XmlElement(ElementName = "xNome")]
        public string? XNome { get; set; }

        [XmlElement(ElementName = "enderDest")]
        public EnderDest? EnderDest { get; set; }

        [XmlElement(ElementName = "indIEDest")]
        public int? IndIEDest { get; set; }

        [XmlElement(ElementName = "IE")]
        public double? IE { get; set; }
    }
}
