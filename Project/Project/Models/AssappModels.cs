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

        [DisplayFormat(DataFormatString = "{hh:mm:ss - d MMM yyyy}")]
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
            this.CreatedIn = DateTime.Now;
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

        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}")]
        public DateTime CreatedIn { get; set; }

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

        [DisplayFormat(DataFormatString = "{0:dddd, d MMM yyyy hh:mm}")]
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
            Choices = new HashSet<Choice>();
        }

        [Key]
        public int ID { get; set; }

        public String Matter { get; set; }

        public Boolean IsFinished { get; set; }

        public Boolean IsVisible { get; set; }

        //*******************************************************************************************
        //* Refers to the relationship between POOL and the VOTE
        //* A POLL may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //* Refers to the relationship between POOL and the PUBLICATION
        //* A POLL may have multiple PUBLICATION   
        public ICollection<Publication> Publications { get; set; }
        //* Refers to the relationship between POOL and the PUBLICATION
        //* A POLL may have multiple CHOICES
        public ICollection<Choice> Choices { get; set; }
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
            Choices = new HashSet<Choice>();
        }

        [Key]
        public int ID { get; set; }

        public String Name { get; set; }

        //*****************************************************************************************
        //* Refers to the relationship between OPTION and the VOTE
        //* A OPTION may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //* Refers to the relationship between OPTION and the VOTE
        //* A OPTION may have multiple CHOICES 
        public ICollection<Choice> Choices { get; set; }
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

    public class Choice
    {
        //*****************************************************************************************
        //*********************    Foreign Keys definition      ***********************************

        public Poll Poll { get; set; } // associates in C# the CHOICE with the POLL
        [Column(Order = 0), Key, ForeignKey("Poll")]
        public int PollFK { get; set; } // associates in SQL the CHOICE with the POLL

        public virtual Option Option { get; set; } // associates in C# the CHOICE with the OPTION
        [Column(Order = 1), Key, ForeignKey("Option")]
        public int OptionFK { get; set; } // associates in SQL the CHOICE with the OPTION

        //*********************   END Foreign Keys definition    *********************************
        //****************************************************************************************

        [DefaultValue(0)]
        public int Count { get; set; }
    }
}