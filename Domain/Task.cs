using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Task
    {
        public Guid TaskId { get; set; }

        public string TaskName { get; set; }

        public string EndTime { get; set; }

        public Guid HouseId { get; set; }
    }
}
