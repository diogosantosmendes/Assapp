using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Option
    {
        //Construtor
        public Option()
        {
            Votes = new HashSet<Vote>();
        }

        [Key]
        public String ID { get; set; }

        public String Name { get; set; }

        //*********************************************************************************************
        //*********************    Foreign Keys definition      ***************************************
        //*********************************************************************************************

        public Type Type { get; set; } // associates in C# the OPTION with the TYPE of the POLL
        [ForeignKey("Type")]
        public int TypeFK { get; set; } // associates in SQL the OPTION with the TYPE of the POLL

        //*********************   END Foreign Keys definition    *************************************
        //********************************************************************************************

        //***************************************************************************
        //* Refers to the relationship between OPTION and the VOTE
        //* A OPTION may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //***************************************************************************
    }
}