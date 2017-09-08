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
                // Assigns roles on "admin@a.a"
                userManager.AddToRoles(users[0].Id, roles.Take(4).ToArray<String>());
                // Assigns roles on "collaborator@a.a"
                userManager.AddToRoles(users[1].Id, roles.Skip(1).Take(3).ToArray<String>());
                // Assigns roles on "partner@a.a"
                userManager.AddToRoles(users[2].Id, roles.Skip(2).Take(2).ToArray<String>());
                // Assigns role on "associated@a.a"
                userManager.AddToRole(users[3].Id, roles[3]);
                // saves the changes in the database
                await context.SaveChangesAsync();
                //*****************************************************************************************
                
                // Creates the polls list
                var polls = new List<Poll>
                {
                    new Poll{IsFinished=false, IsInclusive=true, IsVisible=true, Matter="É verdade que canibais não comem palhaços porque eles sabem a engraçado?", LinkToForm="https://www.google.pt/"},
                    new Poll{IsFinished=false, IsInclusive=true, IsVisible=false, Matter="Se escreveres um livro sobre o fracasso e ele não se vender, considera-se um sucesso?"},
                    new Poll{IsFinished=false, IsInclusive=true, IsVisible=true, Matter="Em que dia querem jantar?"},
                    new Poll{IsFinished=false, IsInclusive=true, IsVisible=true, Matter="Será que olhar para um imagem do sol magoa os olhos?"},
                    new Poll{IsFinished=true, IsInclusive=false, IsVisible=true, Matter="Quem quer novas eleições?"},
                    new Poll{IsFinished=false, IsInclusive=true, IsVisible=true, Matter="Vamos conquistar a lua."}
                };
                // Inserts polls in the database
                polls.ForEach(poll => context.Poll.AddOrUpdate(p => p.ID, poll));
                context.SaveChanges();
                // Creates the options list
                var options = new List<Option>
                {
                    new Option{Name="Sim", Poll=polls[0], Count=0},
                    new Option{Name="Sim", Poll=polls[1], Count=0},
                    new Option{Name="Sim", Poll=polls[3], Count=0},
                    new Option{Name="Não", Poll=polls[0], Count=0},
                    new Option{Name="Não", Poll=polls[1], Count=0},
                    new Option{Name="Não", Poll=polls[3], Count=0},
                    new Option{Name="Aderir", Poll=polls[5], Count=0},
                    new Option{Name="Talvez", Poll=polls[5], Count=0},
                    new Option{Name="Rejeitar", Poll=polls[5], Count=0},
                    new Option{Name="Segunda", Poll=polls[2], Count=0},
                    new Option{Name="Terça", Poll=polls[2], Count=0},
                    new Option{Name="Quarta", Poll=polls[2], Count=0},
                    new Option{Name="Quinta", Poll=polls[2], Count=0},
                    new Option{Name="Sexta", Poll=polls[2], Count=0},
                    new Option{Name="Sábado", Poll=polls[2], Count=0},
                    new Option{Name="Domingo", Poll=polls[2], Count=0},
                    new Option{Name="Eu", Poll=polls[4], Count=3},
                    new Option{Name="Eu não", Poll=polls[4], Count=1}
                };
                // Inserts options in the database
                options.ForEach(option => context.Option.AddOrUpdate(o => o.ID, option));
                context.SaveChanges();
                // Creates the votes list
                var votes = new List<Vote>
                {
                    new Vote{User=users[0], Option=options[16]},
                    new Vote{User=users[1], Option=options[16]},
                    new Vote{User=users[2], Option=options[17]},
                    new Vote{User=users[3], Option=options[16]},

                };
                // Inserts votes in the database
                votes.ForEach(vote => context.Vote.Add(vote));
                context.SaveChanges();
                
                // Creates the events list
                var events = new List<Event>
                {
                    new Event{Local="Montanhas Tianzi - China", Day= new DateTime(2017,11,4)},
                    new Event{Local="Giant’s Causeway - Irlanda do Norte", Day= new DateTime(2017,11,20)},
                    new Event{Local="Mano del Desierto - Chile", Day= new DateTime(2017,2,19)},
                    new Event{Local="Lado visível da lua - Lua", Day=DateTime.Now},
                    new Event{Local="Moulin Rouge - Paris", Day= new DateTime(2017,10,7)}
                };
                // Inserts events in the database
                events.ForEach(even => context.Event.AddOrUpdate(p => p.ID, even));
                context.SaveChanges();
                // Creates the publications list
                var publications = new List<Publication>
                {
                    new Publication { Name="Procurar côcos nas montanhas Tianzi", Accepted=true, User=users[1],  Image="Tianzi-mountains.jpg", Event= events[0], Description="Sir! said the mate, astonished at an order seldom or never given on ship-board except in some extraordinary case. Send everybody aft, repeated Ahab. Mast-heads, there! come down When the entire ships company were assembled, and with curious and not wholly unapprehensive faces, were eyeing him, for he looked not unlike the weather horizon when a storm is coming up, Ahab, after rapidly glancing over the bulwarks, and then darting his eyes among the crew, started from his standpoint; and as though not a soul were nigh him resumed his heavy turns upon the deck. With bent head and half-slouched" },
                    new Publication { Name="Alguém quer ser presidente", Accepted=true, User=users[1], Poll= polls[4], Description="'Soles and eels, of course,' the Gryphon replied rather impatiently: 'any shrimp could have told you that.' 'If I'd been the whiting,' said Alice, whose thoughts were still running on the song, 'I'd have said to the porpoise, Keep back, please: we dont want YOU with us! They were obliged to have him with them, the Mock Turtle said: no wise fish would go anywhere without a porpoise. Wouldnt it really ? said Alice in a tone of great surprise. Of course not said the Mock Turtle: why, if a fish came to ME, and told" },
                    new Publication { Name="Jantar em honra dos nunca foram", Accepted=false, User=users[1], Poll= polls[2], Description="Neverland is a fictional location featured in the works of J. M. Barrie and those based on them. It is an imaginary faraway place, where Peter Pan, Tinker Bell, the Lost Boys and other mythical creatures and beings live. Although not all people who come to Neverland cease to age, its best known resident famously refused to grow up, and it is often used as a metaphor for eternal childhood (and childishness), immortality, and escapism. It was first introduced as the Never Never Land in the theatre play Peter Pan, or The Boy Who Wouldn't Grow Up by Scottish writer J. M. Barrie, first staged in 1904."},
                    new Publication { Name="Vamos pôr uma aliança", Accepted=true, Image="The-Hand-in-the-Desert.jpg", User=users[1], Event= events[2], Description="Because he's alone Bacon ipsum dolor amet boudin ham cow, pork bacon landjaeger pork belly. Jerky pork chop chicken tri-tip sausage. Venison ball tip doner shank ribeye rump, swine meatloaf shankle ground round andouille fatback jowl boudin. Beef ribs pork chop filet mignon, cow leberkas salami drumstick chicken. Cow spare ribs shankle, landjaeger drumstick turkey turducken pork hamburger strip steak leberkas. Venison beef ribs pig rump brisket strip steak filet mignon short ribs burgdoggen" },
                    new Publication { Name="13ª corrida pelas grávidas", Accepted=false, Description="For children born healthy Hexagon franzen ethical tilde chia copper mug. Fixie pickled blog before they sold out occupy deep v. Authentic chartreuse neutra godard brooklyn. Cred offal keffiyeh sustainable, iceland schlitz master cleanse hashtag. Cliche everyday carry squid, adaptogen cray waistcoat keffiyeh artisan ennui health goth prism sartorial shabby chic DIY. Gochujang godard everyday carry tattooed pok pok health goth. Put a bird on it cronut la croix slow-carb hoodie, chartreuse paleo.", User=users[1], Image="Giant_s-Causeway.jpg", Event= events[1] },
                    new Publication { Name="O que podes comer", Accepted=true, Description="description Hodor hodor HODOR! Hodor hodor; hodor hodor; hodor hodor?! Hodor! Hodor hodor, HODOR hodor, hodor hodor? Hodor. Hodor hodor; hodor hodor; hodor hodor hodor! Hodor. Hodor hodor, hodor. Hodor hodor hodor hodor?! Hodor hodor HODOR! Hodor hodor hodor hodor... Hodor hodor hodor. Hodor. Hodor, hodor - hodor hodor hodor, hodor, hodor hodor.Pipeline moving the goalposts i don't want to drain the whole swamp, i just want to shoot some alligators. Close the loop cloud strategy for touch base for we're ahead of the curve on that one. Touch base cross-pollination. Quick-win after I ran into Helen at a restaurant, I realized she was just office pretty at the end of the day, nor guerrilla marketing, out of the loop, after I ran into Helen at a restaurant", User=users[1], Image="clown.jpg", Poll= polls[0] },
                    new Publication { Name="As fotografias estão disponíveis", Accepted = true, User = users[1], Image = "attention.png", Description="Pommy ipsum The Doctor squirrel Elementary my dear Watson ey up a tenner, chav wibbly-wobbly timey-wimey stuff hard cheese old boy Geordie football damn off t'pub, bent as a nine bob note see a man about a dog ee bah gum bowler hat Moriarty. Had a barney with the inlaws gobsmacked twiglets at the boozer what a load of guff tip-top, flabbergasted drizzle ever so lovely indeed spam fritters Sherlock, it's just not cricket trouble and strife pulled a right corker nonsense. Penny-dreadful sling one's hook Doctor Who pie-eyed Shakespeare munta 10 pence mix scrote cup of tea, ever so chin up muck about fake tan punter ended up brown bread." },
                    new Publication { Name="Uma história de sucesso", Accepted=true, User=users[1], Poll= polls[1], Description="I realized she was just office pretty. Blue money deliverables or price point, or make sure to include in your wheelhouse we don't want to boil the ocean wheelhouse, so low-hanging fruit. Data-point that jerk from finance really threw me under the bus when does this sunset? yet price point churning anomalies. Bake it in ramp up we need a recap by eod, cob or whatever comes first, nor timeframe dog and pony show work flows . Let's unpack that later push back re-inventing the wheel, nor are there any leftovers in the kitchen? tbrand terrorists are there any leftovers in the kitchen? organic growth. Hammer out powerPointless. Not the long pole in my tent it just needs more cowbell we need a recap by eod, cob or whatever comes first for re-inventing the wheel pixel pushing the last person we talked to said this would be ready we want to see more charts. Cloud strategy we need distributors to evangelize the new line to local markets back of the net highlights ." },
                    new Publication { Name="'May the force be with us'", Accepted = true, User = users[1], Image = "moon.jpg", Event=events[3], Poll=polls[5], Description = "Death! I shouted. Death is coming!Death! and leaving him to digest that if he could, I hurried on after the artillery-man. At the corner I looked back. The soldier had left him, and he was still standing by his box, with the pots of orchids on the lid of it, and staring vaguely over the trees.The boy did not exactly utter these words, but something that remotely resembled them and that was more guttural and explosive and economical of qualifying phrases. His speech showed distant kinship with that of the old man, and the latter's speech was approximately an English that had gone through a bath of corrupt usage." },
                    new Publication { Name="Musica e dança para levantar o espírito", Accepted = true, User = users[1], Image = "moulinrouge.jpg", Event=events[4], Description = "Flexitarian banh mi migas listicle. Letterpress chambray yuccie schlitz disrupt iPhone. Ennui banh mi intelligentsia, sartorial biodiesel wolf selvage blog. Hella kogi green juice truffaut vape. Cray keytar pitchfork, kogi brooklyn chillwave tousled tbh pop-up shabby chic meditation affogato ugh. Cray vice selvage whatever thundercats, umami cardigan hexagon tumblr pickled waistcoat. Art party gochujang iceland, polaroid knausgaard pickled actually. Authentic skateboard single-origin coffee pinterest dreamcatcher scenester coloring book tbh butcher cold-pressed. Ramps taiyaki coloring book squid subway tile selvage tbh, 8-bit butcher brunch fanny pack 3 wolf moon meditation. Neutra ramps letterpress, green juice pok pok dreamcatcher sartorial mustache unicorn literally. Cred occupy franzen pitchfork, coloring book craft beer kombucha VHS thundercats shaman. Bushwick copper mug pitchfork jianbing poutine +1. 8-bit mlkshk locavore adaptogen roof party helvetica meggings cred glossier XOXO you probably haven't heard of them pitchfork. Try-hard chicharrones squid, pour-over keytar microdosing you probably haven't heard of them kombucha venmo twee copper mug poke church-key jean shorts. Franzen chambray raclette pok pok portland bespoke normcore shaman lyft. Quinoa hot chicken echo park, selvage heirloom art party fam lyft celiac portland narwhal prism chia scenester taiyaki. Ramps fanny pack shoreditch PBR&B disrupt typewriter. Microdosing fixie cold-pressed subway tile, kickstarter coloring book prism ennui iceland tbh austin waistcoat knausgaard occupy DIY. Ugh quinoa unicorn vexillologist jianbing coloring book, selfies tattooed franzen listicle man braid master cleanse artisan. Occupy cloud bread squid copper mug. Neutra distillery mlkshk hammock raw denim tumeric. Listicle pop-up austin truffaut, yuccie cred plaid master cleanse edison bulb. La croix 3 wolf moon paleo franzen, PBR&B celiac art party raw denim tumblr literally." },
                    new Publication { Name="Doações são aceites para a nossa viagem", Accepted = false, User = users[1], Image = "attention.png", Description="Tousled occupy unicorn, squid fingerstache tote bag shabby chic locavore hashtag poke hella iPhone. Cronut synth blog shabby chic helvetica. Church-key freegan next level before they sold out schlitz, pop-up messenger bag DIY leggings mustache godard. Godard selvage tote bag church-key succulents food truck marfa mlkshk quinoa tilde. Celiac kitsch trust fund iPhone ethical jean shorts. Pour-over squid chicharrones truffaut shaman put a bird on it before they sold out scenester readymade etsy air plant bespoke quinoa. Keffiyeh snackwave stumptown cornhole wolf. 8-bit readymade sriracha, typewriter selvage chia humblebrag semiotics. XOXO meditation cray gluten-free, lyft celiac art party four loko kogi gochujang before they sold out dreamcatcher freegan authentic distillery. Ramps meh pabst actually jean shorts stumptown." },
                };
                System.Threading.Thread.Sleep(1000);
                publications.Add(new Publication { Name = "A real questão", Accepted = true, User = users[1], Image = "the-sun-in-the-sky.jpg", Poll = polls[3], Description= "Vivamizzle yippiyo mah nizzle eget nisi sure pretizzle. Vivamizzle sit amizzle lacus. Mah nizzle eu fo shizzle mah nizzle fo rizzle, mah home g-dizzle boofron lacizzle auctor boom shackalack. Praesent shizznit viverra doggy. Curabitizzle in arcu. Vestibulizzle enizzle enim, auctizzle you son of a bizzle, congue eu, dignissizzle check out this, libero. Nullam vitae pede that's the shizzle eros posuere pharetra. Quisque pede , the bizzle pulvinizzle, go to hizzle a, gizzle check out this amizzle, erizzle. Pot izzle brizzle. Aliquizzle da bomb purizzle, elementizzle consectetizzle, daahng dawg in, consequat imperdizzle, get down get down. Check it out a break yo neck, yall eu mi boofron vehicula." });
                // Inserts publications in the database
                publications.ForEach(publication => context.Publication.AddOrUpdate(p => p.ID, publication));
                context.SaveChanges();
                // Creates the replies list
                var replies = new List<Reply>
                {
                    new Reply{ Hour=DateTime.Now, Publication=publications[0], User=users[2], IsVisible=true, Content="Não gosto de côcos"},
                    new Reply{ Hour=DateTime.Now, Publication=publications[0], User=users[1], IsVisible=true, Content="mais para mim"},
                    new Reply{ Hour=DateTime.Now, Publication=publications[0], User=users[2], IsVisible=false, Content="e se fosses.."},
                    new Reply{ Hour=DateTime.Now, Publication=publications[3], User=users[2], IsVisible=true, Content="e quem não e religioso?"},
                };
                // Inserts replies in the database
                replies.ForEach(reply => context.Reply.AddOrUpdate(r => r.ID, reply));
                context.SaveChanges();
                // Creates the logs list
                var logs = new List<Log>
                {
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[0].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[0].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[1].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[1].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[2].Name)},
                    new Log{ User=users[2], Hour=DateTime.Now, Description=String.Format("Comentou na publicação: {0}", publications[0].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Comentou na publicação: {0}", publications[0].Name)},
                    new Log{ User=users[2], Hour=DateTime.Now, Description=String.Format("Comentou na publicação: {0}", publications[0].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[2].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[3].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[3].Name)},
                    new Log{ User=users[2], Hour=DateTime.Now, Description=String.Format("Comentou na publicação: {0}", publications[3].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[4].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[4].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[5].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[5].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[6].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[6].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[7].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[7].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[8].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[8].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[9].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[9].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[10].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[10].Name)},
                    new Log{ User=users[1], Hour=DateTime.Now, Description=String.Format("Criou uma nova publicação: {0}", publications[11].Name)},
                    new Log{ User=users[0], Hour=DateTime.Now, Description=String.Format("Permitiu a publicação: {0}", publications[11].Name)},
                };
                // Inserts logs in the database
                logs.ForEach(log => context.Log.AddOrUpdate(l => l.ID, log));
                context.SaveChanges();
            }
        }
    }
}
