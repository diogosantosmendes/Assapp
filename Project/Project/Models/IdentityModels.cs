using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Project.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        // *****************************************************************
        // User data required for this system
        
        //Constructor
        public User()
        {
            Replies = new HashSet<Reply>();
            Votes = new HashSet<Vote>();
            Publications = new HashSet<Publication>();
            Logs = new HashSet<Log>();
        }

        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Partner Number")]
        public string Partner { get; set; }

        [Display(Name = "Due")]
        public Boolean Due  { get; set; }

        //***************************************************************************
        //* Refers to the relationship between VOTE and the USER
        //* A USER may have multiple VOTE  
        public ICollection<Vote> Votes { get; set; }
        //* Refers to the relationship between USER and the PUBLICATION
        //* A USER may have multiple PUBLICATION
        public ICollection<Publication> Publications { get; set; }
        //* Refers to the relationship between USER and the LOG
        //* A USER may have multiple LOG
        public ICollection<Log> Logs { get; set; }
        //* Refers to the relationship between USER and the REPLY
        //* A USER may have multiple REPLY
        public ICollection<Reply> Replies { get; set; }
        //***************************************************************************
        
        //***************************************************************************

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<User>
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
         *  DbSet<Log> Log                  -> representa uma tabela da base de dados (neste caso chamada 'Log') com a classe Log
         *  DbSet<Event> Event              -> representa uma tabela da base de dados (neste caso chamada 'Event') com a classe Event
         *  DbSet<Option> Option            -> representa uma tabela da base de dados (neste caso chamada 'Option') com a classe Option
         *  DbSet<Poll> Poll                -> representa uma tabela da base de dados (neste caso chamada 'Poll') com a classe Poll
         *  DbSet<Publication> Publication  -> representa uma tabela da base de dados (neste caso chamada 'Publication') com a classe Publication
         *  DbSet<eply> Reply               -> representa uma tabela da base de dados (neste caso chamada 'Reply') com a classe Reply
         *  DbSet<Vote> Vote                -> representa uma tabela da base de dados (neste caso chamada 'Vote') com a classe Vote
         *  DbSet<Choice> Choice            -> representa uma tabela da base de dados (neste caso chamada 'Choice') com a classe Choice
         **************************************************************************************************************************/
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Publication> Publication { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Poll> Poll { get; set; }
        public virtual DbSet<Reply> Reply { get; set; }
        public virtual DbSet<Option> Option { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }
        public virtual DbSet<Choice> Choice { get; set; }
        //************************************************************************************************************************

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}