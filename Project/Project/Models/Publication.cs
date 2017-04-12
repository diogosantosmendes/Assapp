using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Publication
    {
        // Construtor
        public Publication()
        {
            Replies = new HashSet<Reply>();
        }

        [Key]
        public String ID { get; set; }

        public String Name { get; set; }

        public String Image { get; set; }

        public String Description { get; set; }

        public String Summary { get; set; }

        public Boolean Accepted { get; set; }

        //******************************************************************************************
        //*********************    Foreign Keys definition      ************************************
        //******************************************************************************************

        public ApplicationUser User { get; set; } // associates in C# the USER with the PUBLICATION
        [ForeignKey("User")]
        public int UserFK { get; set; } // associates in SQL the USER with the PUBLICATION

        //*********************   END Foreign Keys definition    **********************************
        //*****************************************************************************************

        //***************************************************************************
        //* Refers to the relationship between REPLY and the EVENT
        //* A EVENT may have multiple REPLY   
        public ICollection<Reply> Replies { get; set; }
        //***************************************************************************

        //***************************************************************************
        //* Refers to the relationship between PUBLICATION and the EVENT
        //* A PUBLICATION may have only a EVENT 
        public virtual Event Event { get; set; }
        //* Refers to the relationship between PUBLICATION and the POLL
        //* A EVENT may have only a POLL
        public virtual Poll Poll { get; set; }
        //**************************************************************************


    }
}