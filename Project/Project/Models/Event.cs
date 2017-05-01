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
        public Event()
        {
            Publications = new HashSet<Publication>();
        }

        [Key]
        public int ID { get; set; }

        public DateTime Day { get; set; }

        public String Local { get; set; }

        //***************************************************************************
        //* Refers to the relationship between EVENT and the PUBLICATION
        //* A EVENT may have multiple PUBLICATION   
        public ICollection<Publication> Publications { get; set; }
        //***************************************************************************
    }
}