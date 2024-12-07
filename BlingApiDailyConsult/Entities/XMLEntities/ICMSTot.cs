using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "ICMSTot")]
    public class ICMSTot
    {

        [XmlElement(ElementName = "vBC")]
        public decimal VBC { get; set; }

        [XmlElement(ElementName = "vICMS")]
        public decimal VICMS { get; set; }

        [XmlElement(ElementName = "vICMSDeson")]
        public decimal VICMSDeson { get; set; }

        [XmlElement(ElementName = "vFCP")]
        public decimal VFCP { get; set; }

        [XmlElement(ElementName = "vBCST")]
        public decimal VBCST { get; set; }

        [XmlElement(ElementName = "vST")]
        public decimal VST { get; set; }

        [XmlElement(ElementName = "vFCPST")]
        public decimal VFCPST { get; set; }

        [XmlElement(ElementName = "vFCPSTRet")]
        public decimal VFCPSTRet { get; set; }

        [XmlElement(ElementName = "vProd")]
        public decimal VProd { get; set; }

        [XmlElement(ElementName = "vFrete")]
        public decimal VFrete { get; set; }

        [XmlElement(ElementName = "vSeg")]
        public decimal VSeg { get; set; }

        [XmlElement(ElementName = "vDesc")]
        public decimal VDesc { get; set; }

        [XmlElement(ElementName = "vII")]
        public decimal VII { get; set; }

        [XmlElement(ElementName = "vIPI")]
        public decimal VIPI { get; set; }

        [XmlElement(ElementName = "vIPIDevol")]
        public decimal VIPIDevol { get; set; }

        [XmlElement(ElementName = "vPIS")]
        public decimal VPIS { get; set; }

        [XmlElement(ElementName = "vCOFINS")]
        public decimal VCOFINS { get; set; }

        [XmlElement(ElementName = "vOutro")]
        public decimal VOutro { get; set; }

        [XmlElement(ElementName = "vNF")]
        public decimal VNF { get; set; }

        [XmlElement(ElementName = "vTotTrib")]
        public decimal VTotTrib { get; set; }
    }
}
