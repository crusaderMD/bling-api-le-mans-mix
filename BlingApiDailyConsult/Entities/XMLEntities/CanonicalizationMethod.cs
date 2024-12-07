using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "CanonicalizationMethod")]
    public class CanonicalizationMethod
    {

        [XmlAttribute(AttributeName = "Algorithm")]
        public string? Algorithm { get; set; }
    }
}
