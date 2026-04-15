using System.Runtime.Serialization;

namespace eShop.Contracts.Models
{
    [DataContract]
    public class CatalogBrand
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Brand { get; set; }
    }
}
