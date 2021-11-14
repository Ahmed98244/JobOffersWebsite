using JobOffersWebsite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JobOffersWebsite.Startup))]
namespace JobOffersWebsite
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }

        public void CreateDefaultRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            IdentityRole role = new IdentityRole();

            if(!roleManager.RoleExists("Admins"))
            {
                role.Name = "Admins";
                roleManager.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Ahmed-aty";
                user.Email = "ahmedabdelatyeelu@gmail.com";
                var Check = userManager.Create(user, "123456a");
                if (Check.Succeeded)
                {
                    userManager.AddToRole(user.Id,"Admins");   
                }    
            }
        }
    }
}
