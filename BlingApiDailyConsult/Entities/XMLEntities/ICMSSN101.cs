using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "ICMSSN101")]
    public class ICMSSN101
    {

        [XmlElement(ElementName = "orig")]
        public int Orig { get; set; }

        [XmlElement(ElementName = "CSOSN")]
        public int CSOSN { get; set; }

        [XmlElement(ElementName = "pCredSN")]
        public double PCredSN { get; set; }

        [XmlElement(ElementName = "vCredICMSSN")]
        public double VCredICMSSN { get; set; }
    }
}
