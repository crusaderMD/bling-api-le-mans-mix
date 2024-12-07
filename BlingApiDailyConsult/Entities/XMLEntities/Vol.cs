using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "vol")]
    public class Vol
    {

        [XmlElement(ElementName = "qVol")]
        public int QVol { get; set; }

        [XmlElement(ElementName = "esp")]
        public string? Esp { get; set; }

        [XmlElement(ElementName = "pesoL")]
        public double PesoL { get; set; }

        [XmlElement(ElementName = "pesoB")]
        public double PesoB { get; set; }
    }
}
