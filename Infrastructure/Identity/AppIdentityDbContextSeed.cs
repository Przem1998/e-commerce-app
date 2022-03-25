using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userMenager)
        {
           
           if(!userMenager.Users.Any())
           {
               var user = new AppUser
               {
                   DisplayName = "Jan",
                   Email = "jan_kowalski@gmail.com",
                   UserName = "jan_kowalski",
                   Role="ADMIN",
                   Address = new Address
                   {
                       FirstName = "Jan",
                       Surname = "Kowalski",
                       Street = "Daleka 10",
                       City = "Biała Podlaska",
                       PostCode = "21-500 Biała Podlaska"
                   }
               };

               await userMenager.CreateAsync(user, "al@MaK0ta");
           } 
        }
    }
}