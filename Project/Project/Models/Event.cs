using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Event
    {
        [Key]
        public string ID { get; set; }

        public DateTime Day { get; set; }

        public string Local { get; set; }

        public Boolean Accepted { get; set; }

        //*********************************************************************************************
        //*********************    Foreign Keys definition      ***************************************
        //*********************************************************************************************

        public Publication Publication { get; set; } // associates in C# the EVENT with the PUBLICATION
        [ForeignKey("Publication")]
        public int PublicationFK { get; set; } // associates in SQL the EVENT with the PUBLICATION

        //*********************   END Foreign Keys definition    *************************************
        //********************************************************************************************
    }
}