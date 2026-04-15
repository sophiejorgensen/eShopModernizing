using System;
using System.Runtime.Serialization;

namespace eShop.Contracts.Models
{
    [DataContract]
    public class CatalogItemStock
    {
        [DataMember]
        public int StockId { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int CatalogItemId { get; set; }

        [DataMember]
        public int AvailableStock { get; set; }
    }
}
