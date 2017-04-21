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
            Publications = new HashSet<Publication>();
        }

        [Key]
        public int ID { get; set; }

        public String Matter { get; set; }

        //***************************************************************************
        //* Refers to the relationship between POOL and the VOTE
        //* A POLL may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //* Refers to the relationship between POOL and the PUBLICATION
        //* A POLL may have multiple PUBLICATION   
        public ICollection<Publication> Publications { get; set; }
        //***************************************************************************
    }
}