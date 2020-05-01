using System;

namespace Domain
{
    public class Profile
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string XP { get; set; }

        public Guid HouseId { get; set; }
    }
}