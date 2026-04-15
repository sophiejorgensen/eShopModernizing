using System.Runtime.Serialization;

namespace eShop.Contracts.Models
{
    [DataContract]
    public class CatalogType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}
