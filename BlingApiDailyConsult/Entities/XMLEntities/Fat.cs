using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "fat")]
    public class Fat
    {

        [XmlElement(ElementName = "nFat")]
        public long NFat { get; set; }

        [XmlElement(ElementName = "vOrig")]
        public double VOrig { get; set; }

        [XmlElement(ElementName = "vDesc")]
        public double VDesc { get; set; }

        [XmlElement(ElementName = "vLiq")]
        public double VLiq { get; set; }
    }
}
