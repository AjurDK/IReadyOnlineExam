using IReadyonlineexam.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IReadyonlineexam.Startup))]
namespace IReadyonlineexam
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         
            ConfigureAuth(app);
            CreateRoles();
        }

        private void CreateRoles()
        {
            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
           
                role.Name = "Admin";
                roleManager.Create(role);
            }
            // creating Creating Manager role     
            else if (!roleManager.RoleExists("Student"))
            {
              
                role.Name = "Student";
                roleManager.Create(role);

            }
            else if (!roleManager.RoleExists("Contributor"))
            {
              
                role.Name = "Contributor";
                roleManager.Create(role);

            }
            

        }


    }
}
