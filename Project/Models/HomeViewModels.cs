using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class HomeIndexViewModel
    {
        [DisplayName("Nome da organização")]
        public String Name { get; set; }
        [DisplayName("Sobre nós")]
        public String AboutUs { get; set; }
        [DisplayName("Missão")]
        public String Mission { get; set; }
        [DisplayName("Quem somos")]
        public String WhoWeAre { get; set; }
        [DisplayName("Fotografia de equipa")]
        public String TeamPhoto { get; set; }
        [DisplayName("Coordenada de Latitude do google maps")]
        public String Location_Lat { get; set; }
        [DisplayName("Coordenada de Longitude do google maps")]
        public String Location_Lon { get; set; }
        [DisplayName("Ficheiro de regulamento")]
        public String Regulation { get; set; }
        [DisplayName("Imagem de topo")]
        public String Image { get; set; }
    }
}