using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HospitalStaffManagement.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "نام را وارد کنید")]
        [DisplayName("نام")]
        public string Name { get; set; }
        [Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
        [DisplayName("نام خانوادگی")]
        public string LastName { get; set; }
        [DisplayName("کدملی")]
        public string NationalCode { get; set; }
        public string DecriptPass { get; set; }
        [Required(ErrorMessage = "شماره همراه را وارد کنید")]
        [DisplayName("شماره همراه")]
        public string Mobile { get; set; }  
        [DisplayName("آدرس")]
        public string Address { get; set; }
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        [DisplayName("نام کاربری")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        [Required(ErrorMessage = "رمز عبور کاربر را وارد کنید")]
        [DisplayName("رمز عبور")]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        public Gender Gender { get; set; }
        [DisplayName("عکس پرسنلی")]
        public string Url { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, configure>());
        }

        class configure : System.Data.Entity.Migrations.DbMigrationsConfiguration<ApplicationDbContext>
        {
            public configure()
            {
                this.AutomaticMigrationsEnabled = true;
                this.AutomaticMigrationDataLossAllowed = true;
            }
            protected override void Seed(HospitalStaffManagement.Models.ApplicationDbContext context)
            {
                if (!context.Roles.Any())
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    string roleName = "Normal";
                    IdentityResult roleResult;
                    if (!RoleManager.RoleExists(roleName))
                    {
                        roleResult = RoleManager.Create(new IdentityRole(roleName));
                    }
                    roleName = "Admin";
                    if (!RoleManager.RoleExists(roleName))
                    {
                        roleResult = RoleManager.Create(new IdentityRole(roleName));
                    }
                }
                if (!context.Users.Any())
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = "Admin", Name = "مدیر سایت", LastName = "مدیریت کل", NationalCode = "1111111111", Email = "Admin@yahoo.com", DecriptPass = "", Mobile="09131111111"};
                    manager.Create(user, "123456");
                    manager.AddToRole(user.Id, "Admin");
                }
   
            }
        }

        public System.Data.Entity.DbSet<HospitalStaffManagement.Models.Symptoms> Symptoms { get; set; }

        public System.Data.Entity.DbSet<HospitalStaffManagement.Models.Patients> Patients { get; set; }
    }
}