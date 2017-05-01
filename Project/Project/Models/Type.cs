using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Type
    {
        // Construtor
        public Type()
        {
            Polls = new HashSet<Poll>();
        }

        [Key]
        public int ID { get; set; }

        public String Name { get; set; }

        //***************************************************************************
        //* Refers to the relationship between TYPE and the OPTION
        //* A TYPE may have multiple OPTION   
        public ICollection<Poll> Polls { get; set; }
        //***************************************************************************
    }
}