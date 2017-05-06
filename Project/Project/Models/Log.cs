using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Log
    {
        [Key]
        public int ID { get; set; }

        public String Description { get; set; }

        public DateTime Hour { get; set; }
        
        //******************************************************************************************
        //*********************    Foreign Keys definition      ************************************
        //******************************************************************************************

        public ApplicationUser User { get; set; } // associates in C# the USER with the ACTION
        [ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the USER with the ACTION

        //*********************   END Foreign Keys definition    **********************************
        //*****************************************************************************************
    }
}