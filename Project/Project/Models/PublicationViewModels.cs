using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class PublicationFeedViewModel
    {
        public int ID { get; set; }

        public String Name { get; set; }
        
        public String Image { get; set; }
        
        public String Summary { get; set; }

        [DisplayFormat(DataFormatString = "{0:d MMM yyyy}")]
        public DateTime CreatedIn { get; set; }

        [DefaultValue(false)]
        public Boolean IsEvent { get; set; }

        [DefaultValue(false)]
        public Boolean IsPoll { get; set; }
    }

    public class PublicationVoteViewModel
    {
        public int OptionID { get; set; }
        public int PollID { get; set; }
        public int PublicationID { get; set; }
    }

    public class PublicationCreateViewModels
    {

    }

    public class PublicationListViewModels
    {

    }
}