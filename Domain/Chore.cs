using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Chore
    {
        public Guid ChoreId { get; set; }

        public string ChoreName { get; set; }

        public string CompletionDate { get; set; }
        public string CompletionTime { get; set; }

        public Guid HouseId { get; set; }
        public short ChoreTypeId { get; set; }
    }
}
