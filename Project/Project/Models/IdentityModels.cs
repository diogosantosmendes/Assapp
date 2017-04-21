using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        // *****************************************************************
        // User data required for this system
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Partner Number")]
        public string Partner { get; set; }
        
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        //********************************************************************
        
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
        /**************************************************************************************************************************
         * especificar onde será criada a Base de dados
         * a localização é especificada no ficheiro Web.config
         * ----------------------------------------------------
         *  depois de definidas as configurações -> NuGet Console:
         *          PM> Enable-Migrations -EnableAutomaticMigrations
         *      para habilitar as migrações entre a base de dados definida e a aplicação
         *          PM> Update-Database
         *      para atualizar a base de dados tendo em conta as migrações definidas em Migrations/Configurations.cs
         ***************************************************************************************************************************/
        public ApplicationDbContext(): base("DefaultConnection", throwIfV1Schema: false) { }

        /**************************************************************************************************************************
         * representar as tabelas a criar na base de dados
         * -----------------------------------------------
         *  DbSet<Action> Action            -> representa uma tabela da base de dados (neste caso chamada 'Action') com a classe Action
         *  DbSet<Event> Event              -> representa uma tabela da base de dados (neste caso chamada 'Event') com a classe Event
         *  DbSet<Option> Option            -> representa uma tabela da base de dados (neste caso chamada 'Option') com a classe Option
         *  DbSet<Poll> Poll                -> representa uma tabela da base de dados (neste caso chamada 'Poll') com a classe Poll
         *  DbSet<Publication> Publication  -> representa uma tabela da base de dados (neste caso chamada 'Publication') com a classe Publication
         *  DbSet<eply> Reply               -> representa uma tabela da base de dados (neste caso chamada 'Reply') com a classe Reply
         *  DbSet<Type> Type                -> representa uma tabela da base de dados (neste caso chamada 'Type') com a classe Type
         *  DbSet<Vote> Vote                -> representa uma tabela da base de dados (neste caso chamada 'Vote') com a classe Vote
         **************************************************************************************************************************/
        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<Publication> Publication { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Poll> Poll { get; set; }
        public virtual DbSet<Reply> Reply { get; set; }
        public virtual DbSet<Type> Type { get; set; }
        public virtual DbSet<Option> Option { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }
        //************************************************************************************************************************

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}