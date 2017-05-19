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
            RegisterDate = DateTime.Now;
        }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Partner Number")]
        public string Partner { get; set; }

        [Display(Name = "Due")]
        public Boolean Due  { get; set; }

        [Required]
        [Display(Name = "Registration date")]
        [DisplayFormat(DataFormatString = "{hh:mm - d MMM yyyy}")]
        public DateTime RegisterDate { get; set; }

        //***************************************************************************
        //* Refers the relationship between VOTE and the USER
        //* A USER may have multiple VOTE  
        public ICollection<Vote> Votes { get; set; }
        //* Refers the relationship between USER and the PUBLICATION
        //* A USER may have multiple PUBLICATION
        public ICollection<Publication> Publications { get; set; }
        //* Refers the relationship between USER and the LOG
        //* A USER may have multiple LOG
        public ICollection<Log> Logs { get; set; }
        //* Refers the relationship between USER and the REPLY
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
         * Specifies where the database is created
         * The location is specified in the 'Web.config' file 
         * ----------------------------------------------------
         *  Note:
         *      PM> Enable-Migrations -EnableAutomaticMigrations  | To enable migrations between the defined database and the application
         *      PM> Update-Database | To update the database taking into account migrations defined in Migrations / Configurations.cs
         ***************************************************************************************************************************/
        public ApplicationDbContext(): base("DefaultConnection", throwIfV1Schema: false) { }

        /**************************************************************************************************************************
         * representar as tabelas a criar na base de dados
         * -----------------------------------------------
         *  DbSet<Log> Log                  -> represents a table from database ('Log') with the class Log
         *  DbSet<Event> Event              -> represents a table from database ('Event') with the class Event
         *  DbSet<Option> Option            -> represents a table from database ('Option') with the class Option
         *  DbSet<Poll> Poll                -> represents a table from database ('Poll') with the class Poll
         *  DbSet<Publication> Publication  -> represents a table from database ('Publication') with the class Publication
         *  DbSet<eply> Reply               -> represents a table from database ('Reply') with the class Reply
         *  DbSet<Vote> Vote                -> represents a table from database ('Vote') with the class Vote
         *  DbSet<Choice> Choice            -> represents a table from database ('Choice') with the class Choice
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