using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "COFINSOutr")]
    public class COFINSOutr
    {

        [XmlElement(ElementName = "CST")]
        public int CST { get; set; }

        [XmlElement(ElementName = "vBC")]
        public double VBC { get; set; }

        [XmlElement(ElementName = "pCOFINS")]
        public double PCOFINS { get; set; }

        [XmlElement(ElementName = "vCOFINS")]
        public double VCOFINS { get; set; }
    }
}
