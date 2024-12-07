using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "detPag")]
    public class DetPag
    {

        [XmlElement(ElementName = "indPag")]
        public int IndPag { get; set; }

        [XmlElement(ElementName = "tPag")]
        public int TPag { get; set; }

        [XmlElement(ElementName = "xPag")]
        public string? XPag { get; set; }

        [XmlElement(ElementName = "vPag")]
        public double VPag { get; set; }
    }
}
