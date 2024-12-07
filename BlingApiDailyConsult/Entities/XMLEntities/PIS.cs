using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "PIS")]
    public class PIS
    {

        [XmlElement(ElementName = "PISOutr")]
        public PISOutr? PISOutr { get; set; }
    }
}
