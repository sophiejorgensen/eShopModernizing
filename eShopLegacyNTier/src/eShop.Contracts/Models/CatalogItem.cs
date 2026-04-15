using System.Runtime.Serialization;

namespace eShop.Contracts.Models
{
    [DataContract]
    public class CatalogItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string PictureFileName { get; set; }

        [DataMember]
        public int CatalogBrandId { get; set; }

        [DataMember]
        public int CatalogTypeId { get; set; }

        [DataMember]
        public CatalogType CatalogType { get; set; }

        [DataMember]
        public CatalogBrand CatalogBrand { get; set; }
    }
}
