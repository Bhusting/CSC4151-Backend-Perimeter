using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
        public class House
        {
            public Guid HouseId { get; set; }

            public string HouseName { get; set; }

            public Guid Channel { get; set; }
        }
    }
}
