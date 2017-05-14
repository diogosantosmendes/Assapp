using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////            LOG            ///////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Log
    {
        [Key]
        public int ID { get; set; }

        public String Description { get; set; }

        public DateTime Hour { get; set; }

        //******************************************************************************************
        //*********************    Foreign Keys definition      ************************************

        public User User { get; set; } // associates in C# the USER with the ACTION
        [ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the USER with the ACTION

        //*********************   END Foreign Keys definition    **********************************
        //*****************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////            PUBLICATION            ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Publication
    {
        // Constructor
        public Publication()
        {
            Replies = new HashSet<Reply>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        [DefaultValue(null)]
        public String Image { get; set; }

        [DefaultValue(null)]
        public String Description { get; set; }

        [DefaultValue(null)]
        public String Summary { get; set; }

        [DefaultValue(false)]
        public Boolean Accepted { get; set; }

        [DefaultValue(null)]
        public String LinkToForm { get; set; }

        //******************************************************************************************
        //*********************    Foreign Keys definition      ************************************

        public User User { get; set; } // associates in C# the USER with the PUBLICATION
        [Required]
        [ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the USER with the PUBLICATION

        public Poll Poll { get; set; } // associates in C# the PUBLICATION with the POLL
        [ForeignKey("Poll")]
        public int? PollFK { get; set; } // associates in SQL the PUBLICATION with the POLL

        public Event Event { get; set; } // associates in C# the PUBLICATION with the EVENT
        [ForeignKey("Event")]
        public int? EventFK { get; set; } // associates in SQL the PUBLICATION with the EVENT

        //*********************   END Foreign Keys definition    **********************************
        //*****************************************************************************************

        //*****************************************************************************************
        //* Refers to the relationship between REPLY and the EVENT
        //* A EVENT may have multiple REPLY   
        public ICollection<Reply> Replies { get; set; }
        //*****************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////               EVENT               ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Event
    {
        public Event()
        {
            Publications = new HashSet<Publication>();
        }

        [Key]
        public int ID { get; set; }

        public DateTime Day { get; set; }

        public String Local { get; set; }

        //*****************************************************************************************
        //* Refers to the relationship between EVENT and the PUBLICATION
        //* A EVENT may have multiple PUBLICATION   
        public ICollection<Publication> Publications { get; set; }
        //*****************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////              POLL                 ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Poll
    {
        // Construtor
        public Poll()
        {
            Votes = new HashSet<Vote>();
            Publications = new HashSet<Publication>();
            Options = new HashSet<Option>();
        }

        [Key]
        public int ID { get; set; }

        public String Matter { get; set; }

        public Boolean Finished { get; set; }

        public Boolean Visible { get; set; }

        //*******************************************************************************************
        //* Refers to the relationship between POOL and the VOTE
        //* A POLL may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //* Refers to the relationship between POOL and the PUBLICATION
        //* A POLL may have multiple PUBLICATION   
        public ICollection<Publication> Publications { get; set; }
        //* Refers to the relationship between POOL and the PUBLICATION
        //* Many POLLS may have multiple OPTIONS
        public ICollection<Option> Options { get; set; }
        //*******************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////               REPLY               ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Reply
    {
        [Key]
        public int ID { get; set; }

        public DateTime Hour { get; set; }

        public String Content { get; set; }

        public Boolean Visible { get; set; }

        //*********************************************************************************************
        //*********************    Foreign Keys definition      ***************************************

        public Publication Publication { get; set; } // associates in C# the PUBLICATION with the REPLY
        [ForeignKey("Publication")]
        public int PublicationFK { get; set; } // associates in SQL the PUBLICATION with the REPLY

        public User User { get; set; } // associates in C# the USER with the REPLY
        [ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the USER with the REPLY

        //*********************   END Foreign Keys definition    *************************************
        //********************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////              OPTION               ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Option
    {
        //Construtor
        public Option()
        {
            Votes = new HashSet<Vote>();
            Polls = new HashSet<Poll>();
        }

        [Key]
        public int ID { get; set; }

        public String Name { get; set; }

        //*****************************************************************************************
        //* Refers to the relationship between OPTION and the VOTE
        //* A OPTION may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //* Refers to the relationship between OPTION and the VOTE
        //* Many OPTIONS may have multiple POLLS   
        public ICollection<Poll> Polls { get; set; }
        //*****************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////               VOTE                ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Vote
    {
        //*****************************************************************************************
        //*********************    Foreign Keys definition      ***********************************

        public User User { get; set; } // associates in C# the USER with the VOTE
        [Column(Order = 0), Key, ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the VOTE with the USER

        public Poll Poll { get; set; } // associates in C# the VOTE with the POLL
        [Column(Order = 1), Key, ForeignKey("Poll")]
        public int PollFK { get; set; } // associates in SQL the VOTE with the POLL

        public virtual Option Option { get; set; } // associates in C# the VOTE with the OPTION
        [ForeignKey("Option")]
        public int OptionFK { get; set; } // associates in SQL the VOTE with the OPTION

        //*********************   END Foreign Keys definition    *********************************
        //****************************************************************************************

    }
}