using System;

namespace Domain
{
    public class Profile
    {
        public Guid ProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int XP { get; set; }

        public Guid HouseId { get; set; }

        public string Email { get; set; }
    }
}