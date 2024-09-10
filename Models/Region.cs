using System;
using System.Collections.Generic;

namespace NorthwindRestApi.Models
{
    public partial class Region
    {
        public Region()
        {
            Shippers = new HashSet<Shipper>();
            Territories = new HashSet<Territory>();
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; } = null!;

        public virtual ICollection<Shipper> Shippers { get; set; }
        public virtual ICollection<Territory> Territories { get; set; }
    }
}
