using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "dup")]
    public class Dup
    {

        [XmlElement(ElementName = "nDup")]
        public int NDup { get; set; }

        [XmlElement(ElementName = "dVenc")]
        public DateTime DVenc { get; set; }

        [XmlElement(ElementName = "vDup")]
        public double VDup { get; set; }
    }
}
