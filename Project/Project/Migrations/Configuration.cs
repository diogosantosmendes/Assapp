namespace Project.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Project.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<Project.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override async void Seed(ApplicationDbContext context)
        {
            // Verifies if the database is empty, if yes then proceed to the seed
            if (!context.Users.Any())
            {
                //****************************************************************************************
                // Users and Roles seed
                // Achieved with support on:    http://stackoverflow.com/a/20521530
                //****************************************************************************************
                
                // Creates the roles list
                var roles = new String[] { "admin", "collaborator", "partner", "associated" };
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
                        Name ="Admin Name",
                        Email ="admin@a.a",
                        UserName = "admin@a.a",
                        PhoneNumber ="+351987654321",
                        Partner ="0000001",
                        Due =true,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    },
                    // Collaborator
                    new User {
                        Name ="Collaborator Name",
                        Email ="collaborator@a.a",
                        UserName ="collaborator@a.a",
                        PhoneNumber ="+351987654321",
                        Partner ="0000002",
                        Due =true,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    },
                    // Partner
                    new User {
                        Name ="Partner Name",
                        Email ="partner@a.a",
                        UserName ="partner@a.a",
                        PhoneNumber ="+351987654321",
                        Partner ="0000003",
                        Due =true,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    },
                    // Associated
                    new User {
                        Name ="Associated Name",
                        Email ="associated@a.a",
                        UserName ="associated@a.a",
                        PhoneNumber ="+351987654321",
                        Partner ="0000004",
                        Due =true,
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
                // Assigns roles on "admin@a.a"
                userManager.AddToRoles(users[0].Id, roles);
                // Assigns roles on "collaborator@a.a"
                userManager.AddToRoles(users[1].Id, roles.Skip(1).ToArray<String>());
                // Assigns roles on "partner@a.a"
                userManager.AddToRoles(users[2].Id, roles.Skip(2).ToArray<String>());
                // Assigns role on "associated@a.a"
                userManager.AddToRole(users[3].Id, roles[3]);
                // saves the changes in the database
                await context.SaveChangesAsync();
                //*****************************************************************************************
                // Creates the options list
                var options = new List<Option>
                {
                    new Option{Name="Yes"},
                    new Option{Name="No"},
                    new Option{Name="Join"},
                    new Option{Name="Maybe"},
                    new Option{Name="Decline"},
                    new Option{Name="Monday"},
                    new Option{Name="Tuesday"},
                    new Option{Name="Wednesday"},
                    new Option{Name="Thursday"},
                    new Option{Name="Friday"},
                    new Option{Name="Saturday"},
                    new Option{Name="Sunday"},
                    new Option{Name="Me"},
                    new Option{Name="Not Me"}
                };
                // Inserts options in the database
                options.ForEach(option => context.Option.AddOrUpdate(o => o.ID, option));
                context.SaveChanges();
                // Creates the polls list
                var polls = new List<Poll>
                {
                    new Poll{Finished=false, Visible=false, Matter="Is it true cannibals don't eat clowns because they taste funny?", Options=options.Take(2).ToList()},
                    new Poll{Finished=false, Visible=false, Matter="If you write a book about failure, and it doesn't sell, is it called success?", Options=options.Take(2).ToList()},
                    new Poll{Finished=false, Visible=true, Matter="What day do you want dinner?", Options=options.Skip(5).Take(7).ToList()},
                    new Poll{Finished=true, Visible=false, Matter="Does looking at a picture of the sun hurt your eyes?", Options=options.Take(2).ToList()},
                    new Poll{Finished=true, Visible=true, Matter="Who wants new elections?", Options=options.Skip(12).ToList()}

                };
                // Inserts polls in the database
                polls.ForEach(poll => context.Poll.AddOrUpdate(p => p.ID, poll));
                context.SaveChanges();
                // Creates the votes list
                var votes = new List<Vote>
                {
                    new Vote{User=users[0], Option=options[12], Poll=polls[4]},
                    new Vote{User=users[1], Option=options[12], Poll=polls[4]},
                    new Vote{User=users[2], Option=options[13], Poll=polls[4]},
                    new Vote{User=users[3], Option=options[12], Poll=polls[4]}
                };
                // Inserts votes in the database
                votes.ForEach(vote => context.Vote.Add(vote));
                context.SaveChanges();
                // Creates the events list
                var events = new List<Event>
                {
                    new Event{Local="Tianzi mountains", Day=DateTime.Now},
                    new Event{Local="The Giant’s Causeway", Day=DateTime.Now},
                    new Event{Local="The Hand in the Desert", Day=DateTime.Now}
                };
                // Inserts events in the database
                events.ForEach(even => context.Event.AddOrUpdate(p => p.ID, even));
                context.SaveChanges();
                // Creates the publications list
                var publications = new List<Publication>
                {
                    new Publication { Name="Search for coconuts in the Tianzi mountains", Accepted=true, Description="Great adventure to look for coconuts in the mountain. Good luck.", User=users[1],  Image="Tianzi-mountains.jpg", Event= events[0] },
                    new Publication { Name="Someone wants to be president", Accepted=false, User=users[1], Poll= polls[4] },
                    new Publication { Name="Dinner in honor of NeverWere", Accepted=true, User=users[1], Poll= polls[2] },
                    new Publication { Name="Let's put the wedding ring", Accepted=true, Description="Because he's alone", User=users[1], Image="The-Hand-in-the-Desert.jpg", Event= events[2] },
                    new Publication { Name="13th run for pregnant women", Accepted=true, Description="For children born healthy", User=users[1], Image="Giant_s-Causeway.jpg", Event= events[1] },
                    new Publication { Name="What you can eat", Accepted=true, Description="description", User=users[1], Image="clown.jpg", Poll= polls[0] },
                    new Publication { Name="A story of success", Accepted=true, User=users[1], Poll= polls[1] },
                    new Publication { Name="The real question", Accepted=true, User=users[1], Image="the-sun-in-the-sky.jpg", Poll= polls[3] }
                };
                // Inserts publications in the database
                publications.ForEach(publication => context.Publication.AddOrUpdate(p => p.ID, publication));
                context.SaveChanges();
                // Creates the replies list
                var replies = new List<Reply>
                {
                    new Reply{ Hour=DateTime.Now, Publication=publications[0], User=users[2], Visible=true, Content="dont like coconuts"},
                    new Reply{ Hour=DateTime.Now, Publication=publications[0], User=users[1], Visible=true, Content="more for me"},
                    new Reply{ Hour=DateTime.Now, Publication=publications[0], User=users[2], Visible=false, Content="f... you mate"},
                    new Reply{ Hour=DateTime.Now, Publication=publications[3], User=users[2], Visible=true, Content="And who is not religious?"},
                };
                // Inserts replies in the database
                replies.ForEach(reply => context.Reply.AddOrUpdate(r => r.ID, reply));
                context.SaveChanges();
                // Creates the logs list
                var logs = new List<Log>
                {
                    new Log{ User=users[0], Hour=DateTime.Now, Description="Was successfully registered"},
                    new Log{ User=users[1], Hour=DateTime.Now, Description="Was successfully registered"},
                    new Log{ User=users[2], Hour=DateTime.Now, Description="Was successfully registered"},
                    new Log{ User=users[3], Hour=DateTime.Now, Description="Was successfully registered"},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Create a new event: {0}", publications[0].Name)},
                    new Log{ User=users[2], Hour=DateTime.Now, Description=String.Format("Comment on the publication: {0}", publications[0].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Comment on the publication: {0}", publications[0].Name)},
                    new Log{ User=users[2], Hour=DateTime.Now, Description=String.Format("Comment on the publication: {0}", publications[0].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Create a new event: {0}", publications[3].Name)},
                    new Log{ User=users[2], Hour=DateTime.Now, Description=String.Format("Comment on the publication: {0}", publications[3].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Create a new event: {0}", publications[4].Name)}
                };
                // Inserts logs in the database
                logs.ForEach(log => context.Log.AddOrUpdate(l => l.ID, log));
                context.SaveChanges();
            }
        }
    }
}
