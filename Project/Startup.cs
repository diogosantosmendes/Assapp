using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(Project.Startup))]
namespace Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            InitialSeed();
        }

        private void InitialSeed()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            // Creates the roles list
            var roles = new String[] { "admin", "collaborator", "partner", "associated", "pending" };
            // Inicializes the tool that manage the roles in the database
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            // Inserts roles in the database
            foreach (string role in roles)
            {
                roleManager.Create(new IdentityRole(role));
            }
            // Creates the users list
            var users = new List<User>
                {
                    // Admin
                    new User {
                        Name ="Administrador exemplo",
                        Email ="admin@a.a",
                        UserName = "admin@a.a",
                        PhoneNumber ="+351987654321",
                        Partner =1,
                        DuePayday = DateTime.Now,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        PayPlan = 1
                    },
                    // Collaborator
                    new User {
                        Name ="Colaborador exemplo",
                        Email ="colaborador@a.a",
                        UserName ="colaborador@a.a",
                        PhoneNumber ="+351987654321",
                        Partner =2,
                        DuePayday = DateTime.Now,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        PayPlan = 1
                    },
                    // Partner
                    new User {
                        Name ="Sócio exemplo",
                        Email ="socio@a.a",
                        UserName ="socio@a.a",
                        PhoneNumber ="+351987654321",
                        Partner =3,
                        DuePayday = DateTime.Now,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        PayPlan = 1
                    },
                    // Associated
                    new User {
                        Name ="Associado exemplo",
                        Email ="associado@a.a",
                        UserName ="associado@a.a",
                        PhoneNumber ="+351987654321",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    }
                };
            // Inicializes the tool that manage the users in the database
            var userManager = new UserManager<User>(new UserStore<User>(context));
            // Inserts users in the database
            foreach (User user in users)
            {
                userManager.Create(user, "123qwe");
            }
            context.SaveChanges();
            // Assigns roles on "admin@a.a"
            userManager.AddToRoles(users[0].Id, roles.Take(4).ToArray<String>());
            // Assigns roles on "collaborator@a.a"
            userManager.AddToRoles(users[1].Id, roles.Skip(1).Take(3).ToArray<String>());
            // Assigns roles on "partner@a.a"
            userManager.AddToRoles(users[2].Id, roles.Skip(2).Take(2).ToArray<String>());
            // Assigns role on "associated@a.a"
            userManager.AddToRole(users[3].Id, roles[3]);
            // saves the changes in the database
            context.SaveChanges();
            //*****************************************************************************************
        }
    }
}
