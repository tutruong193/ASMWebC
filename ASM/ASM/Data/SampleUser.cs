using Microsoft.AspNetCore.Identity;

namespace ASM.Data
{
    public class SampleUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

    }
}
