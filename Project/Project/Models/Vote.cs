using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Vote
    {
        //*********************************************************************************************
        //*********************    Foreign Keys definition      ***************************************
        //*********************************************************************************************

        public virtual Option Option { get; set; } // associates in C# the VOTE with the OPTION
        [Column(Order = 0), Key, ForeignKey("Option")]
        public int OptionFK { get; set; } // associates in SQL the VOTE with the OPTION

        public Poll Poll { get; set; } // associates in C# the VOTE with the POLL
        [Column(Order = 1), Key, ForeignKey("Poll")]
        public int PollFK { get; set; } // associates in SQL the VOTE with the POLL

        public ApplicationUser User { get; set; } // associates in C# the USER with the VOTE
        [ForeignKey("User")]
        public String UserFK { get; set; } // associates in SQL the VOTE with the USER

        //*********************   END Foreign Keys definition    *************************************
        //********************************************************************************************
        
    }
}