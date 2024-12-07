using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "DigestMethod")]
    public class DigestMethod
    {

        [XmlAttribute(AttributeName = "Algorithm")]
        public string? Algorithm { get; set; }
    }
}
