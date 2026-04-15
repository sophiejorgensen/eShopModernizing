using System;
using System.Runtime.Serialization;

namespace eShop.Contracts.Models
{
    [DataContract]
    public class DiscountItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public double Size { get; set; }

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public DateTime End { get; set; }
    }
}
