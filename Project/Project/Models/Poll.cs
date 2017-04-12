using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Poll
    {
        // Construtor
        public Poll()
        {
            Votes = new HashSet<Vote>();
        }

        [Key]
        public String ID { get; set; }

        public String Matter { get; set; }

        //*********************************************************************************************
        //*********************    Foreign Keys definition      ***************************************
        //*********************************************************************************************

        public Publication Publication { get; set; } // associates in C# the POLL with the PUBLICATION
        [ForeignKey("Publication")]
        public int PublicationFK { get; set; } // associates in SQL the POLL with the PUBLICATION

        //*********************   END Foreign Keys definition    *************************************
        //********************************************************************************************

        //***************************************************************************
        //* Refers to the relationship between POOL and the VOTE
        //* A POLL may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //***************************************************************************
    }
}