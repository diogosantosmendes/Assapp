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
            Polls = new HashSet<Poll>();
        }

        [Key]
        public int ID { get; set; }

        public String Name { get; set; }

        //***************************************************************************
        //* Refers to the relationship between OPTION and the VOTE
        //* A OPTION may have multiple VOTE   
        public ICollection<Vote> Votes { get; set; }
        //* Refers to the relationship between OPTION and the VOTE
        //* Many OPTIONS may have multiple POLLS   
        public ICollection<Poll> Polls { get; set; }
        //***************************************************************************
    }
}