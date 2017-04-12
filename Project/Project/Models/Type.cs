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
            Options = new HashSet<Option>();
        }

        [Key]
        public String ID { get; set; }

        public String Name { get; set; }

        //***************************************************************************
        //* Refers to the relationship between TYPE and the OPTION
        //* A TYPE may have multiple OPTION   
        public ICollection<Option> Options { get; set; }
        //***************************************************************************
    }
}