using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "PISOutr")]
    public class PISOutr
    {

        [XmlElement(ElementName = "CST")]
        public int CST { get; set; }

        [XmlElement(ElementName = "vBC")]
        public double VBC { get; set; }

        [XmlElement(ElementName = "pPIS")]
        public double PPIS { get; set; }

        [XmlElement(ElementName = "vPIS")]
        public double VPIS { get; set; }
    }
}
