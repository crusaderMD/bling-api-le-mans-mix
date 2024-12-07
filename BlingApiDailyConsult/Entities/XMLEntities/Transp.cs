using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "transp")]
    public class Transp
    {

        [XmlElement(ElementName = "modFrete")]
        public int ModFrete { get; set; }

        [XmlElement(ElementName = "transporta")]
        public Transporta? Transporta { get; set; }

        [XmlElement(ElementName = "vol")]
        public Vol? Vol { get; set; }
    }
}
