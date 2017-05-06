using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
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
        //******************************************************************************************

        public ApplicationUser User { get; set; } // associates in C# the USER with the PUBLICATION
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

        //***************************************************************************
        //* Refers to the relationship between REPLY and the EVENT
        //* A EVENT may have multiple REPLY   
        public ICollection<Reply> Replies { get; set; }
        //***************************************************************************
    }
}