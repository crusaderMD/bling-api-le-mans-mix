using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "SignedInfo")]
    public class SignedInfo
    {

        [XmlElement(ElementName = "CanonicalizationMethod")]
        public CanonicalizationMethod? CanonicalizationMethod { get; set; }

        [XmlElement(ElementName = "SignatureMethod")]
        public SignatureMethod? SignatureMethod { get; set; }

        [XmlElement(ElementName = "Reference")]
        public Reference? Reference { get; set; }
    }
}
