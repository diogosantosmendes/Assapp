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
        public Log()
        {
            Hour = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public String Description { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:hh:mm - d MMM yyyy}")]
        public DateTime Hour { get; set; }

        //******************************************************************************************
        //*********************    Foreign Keys definition      ************************************

        public virtual User User { get; set; } // associates in C# the USER with the ACTION
        [Required]
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
            CreatedIn = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        [DefaultValue(null)]
        public String Image { get; set; }

        [MaxLength]
        [DefaultValue(null)]
        public String Description { get; set; }

        [DefaultValue(false)]
        public Boolean Accepted { get; set; }

        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}")]
        public DateTime CreatedIn { get; set; }

        //******************************************************************************************
        //*********************    Foreign Keys definition      ************************************

        public virtual User User { get; set; } // associates in C# the USER with the PUBLICATION
        [Required]
        [ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the USER with the PUBLICATION

        public virtual Poll Poll { get; set; } // associates in C# the PUBLICATION with the POLL
        [ForeignKey("Poll")]
        public int? PollFK { get; set; } // associates in SQL the PUBLICATION with the POLL

        public virtual Event Event { get; set; } // associates in C# the PUBLICATION with the EVENT
        [ForeignKey("Event")]
        public int? EventFK { get; set; } // associates in SQL the PUBLICATION with the EVENT

        //*********************   END Foreign Keys definition    **********************************
        //*****************************************************************************************

        //*****************************************************************************************
        //* Refers the relationship between REPLY and the EVENT
        //* A EVENT may have multiple REPLY   
        public virtual ICollection<Reply> Replies { get; set; }
        //*****************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////               REPLY               ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Reply
    {
        public Reply()
        {
            Hour = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy - hh:mm}")]
        public DateTime Hour { get; set; }

        [Required]
        public String Content { get; set; }

        [DefaultValue(true)]
        public Boolean IsVisible { get; set; }

        //*********************************************************************************************
        //*********************    Foreign Keys definition      ***************************************

        public virtual Publication Publication { get; set; } // associates in C# the PUBLICATION with the REPLY
        [ForeignKey("Publication")]
        public int PublicationFK { get; set; } // associates in SQL the PUBLICATION with the REPLY

        public virtual User User { get; set; } // associates in C# the USER with the REPLY
        [ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the USER with the REPLY

        //*********************   END Foreign Keys definition    *************************************
        //********************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////               EVENT               ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Event
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dddd, d MMM yyyy hh:mm}")]
        public DateTime Day { get; set; }

        [Required]
        public String Local { get; set; }
        
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////              POLL                 ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Poll
    {
        // Construtor
        public Poll()
        {
            Options = new HashSet<Option>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public String Matter { get; set; }

        [DefaultValue(false)]
        public Boolean IsFinished { get; set; }

        [DefaultValue(false)]
        public Boolean IsVisible { get; set; }

        [DefaultValue(false)]
        public Boolean IsInclusive { get; set; }

        [DefaultValue(null)]
        public String LinkToForm { get; set; }

        //*******************************************************************************************
        //* Refers the relationship between POOL and the OPTION
        //* A POLL may have multiple OPTION
        public virtual ICollection<Option> Options { get; set; }
        //*******************************************************************************************
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
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public String Name { get; set; }

        public int Count { get; set; }

        //*****************************************************************************************
        //*********************    Foreign Keys definition      ***********************************

        public virtual Poll Poll { get; set; } // associates in C# the OPTION with the POLL
        [ForeignKey("Poll")]
        public int PollFK { get; set; } // associates in SQL the OPTION with the POLL

        //*********************   END Foreign Keys definition    *********************************
        //****************************************************************************************

        //*****************************************************************************************
        //* Refers the relationship between OPTION and the VOTE
        //* A OPTION may have multiple VOTE   
        public virtual ICollection<Vote> Votes { get; set; }
        //*****************************************************************************************
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////               VOTE                ///////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////
    public class Vote
    {
        //*****************************************************************************************
        //*********************    Foreign Keys definition      ***********************************

        public virtual User User { get; set; } // associates in C# the USER with the VOTE
        [Column(Order = 0), Key, ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the VOTE with the USER

        public virtual Option Option { get; set; } // associates in C# the VOTE with the OPTION
        [Column(Order = 1), Key, ForeignKey("Option")]
        public int OptionFK { get; set; } // associates in SQL the VOTE with the OPTION

        //*********************   END Foreign Keys definition    *********************************
        //****************************************************************************************
    }
}