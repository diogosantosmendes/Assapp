using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Reply
    {
        [Key]
        public string ID { get; set; }

        public DateTime Hour { get; set; }

        public string Content { get; set; }

        //*********************************************************************************************
        //*********************    Foreign Keys definition      ***************************************
        //*********************************************************************************************

        public Publication Publication { get; set; } // associates in C# the PUBLICATION with the REPLY
        [ForeignKey("Publication")]
        public int PublicationFK { get; set; } // associates in SQL the PUBLICATION with the REPLY

        public ApplicationUser User { get; set; } // associates in C# the USER with the REPLY
        [ForeignKey("User")]
        public int UserFK { get; set; } // associates in SQL the USER with the REPLY

        //*********************   END Foreign Keys definition    *************************************
        //********************************************************************************************
    }
}