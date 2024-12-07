using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "ICMS")]
    public class ICMS
    {

        [XmlElement(ElementName = "ICMSSN101")]
        public ICMSSN101 ICMSSN101 { get; set; }
    }
}
