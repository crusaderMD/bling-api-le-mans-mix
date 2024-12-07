using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "imposto")]
    public class Imposto
    {

        [XmlElement(ElementName = "vTotTrib")]
        public double VTotTrib { get; set; }

        [XmlElement(ElementName = "ICMS")]
        public ICMS? ICMS { get; set; }

        [XmlElement(ElementName = "IPI")]
        public IPI? IPI { get; set; }

        [XmlElement(ElementName = "PIS")]
        public PIS? PIS { get; set; }

        [XmlElement(ElementName = "COFINS")]
        public COFINS? COFINS { get; set; }
    }
}
