using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Models
{
    public class PublicationListViewModel
    {
        public PublicationListViewModel()
        {
            List = new List<Publication>();
            Header = new List<Publication>();
        }

        public int Index { get; set; }
        public Boolean HaveMore { get; set; }
        public Boolean HaveLess { get; set; }
        public List<Publication> List { get; set; }
        public List<Publication> Header { get; set; }
    }

    public class PublicationCreateViewModel
    {
        [Required(ErrorMessage = "O Título é um campo obrigatório")]
        [DisplayName("Titulo")]
        public String Name { get; set; }
        
        [DisplayName("Resumo")]
        public String Resume { get; set; }

        [Required(ErrorMessage = "A Descrição é um campo obrigatório")]
        [DisplayName("Descrição")]
        [AllowHtml]
        public String Description { get; set; }

        [DefaultValue(false)]
        [DisplayName("É um evento?")]
        public Boolean IsEvent { get; set; }

        [DisplayName("É um inquérito?")]
        [DefaultValue(false)]
        public Boolean IsPoll { get; set; }

        [DisplayName("Dia do evento")]
        public String Day { get; set; }

        [DisplayName("Local do evento")]
        public String Local { get; set; }

        [DisplayName("Questão do inquérito")]
        public String Matter { get; set; }

        [DisplayName("Os resultados serão visíveis em tempo real?")]
        [DefaultValue(false)]
        public Boolean IsVisible { get; set; }

        [DisplayName("O inquérito será disponível a Associados?")]
        [DefaultValue(false)]
        public Boolean IsInclusive { get; set; }

        [DisplayName("Link de um formulário externo")]
        [DefaultValue(null)]
        public String LinkToForm { get; set; }

        [DisplayName("Possíveis respostas ao inquérito")]
        public String[] OptionName { get; set; }
    }

    public class PublicationEditViewModel
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage = "O Título é um campo obrigatório")]
        [DisplayName("Titulo")]
        public String Name { get; set; }

        [DisplayName("Imagem")]
        [DefaultValue(null)]
        public String Image { get; set; }

        [DisplayName("Resumo")]
        public String Resume { get; set; }

        [Required(ErrorMessage = "A Descrição é um campo obrigatório")]
        [DisplayName("Descrição")]
        [AllowHtml]
        public String Description { get; set; }

        [DefaultValue(false)]
        [DisplayName("É um evento?")]
        public Boolean IsEvent { get; set; }

        [DisplayName("É um inquérito?")]
        [DefaultValue(false)]
        public Boolean IsPoll { get; set; }

        [DisplayName("Dia do evento")]
        [DisplayFormat(DataFormatString = "{0:dddd, d MMM yyyy hh:mm}")]
        public DateTime? Day { get; set; }

        [DisplayName("Local do evento")]
        public String Local { get; set; }

        [DisplayName("Questão do inquérito")]
        public String Matter { get; set; }

        [DisplayName("Os resultados serão visíveis em tempo real?")]
        [DefaultValue(false)]
        public Boolean IsVisible { get; set; }

        [DisplayName("O inquérito será disponível a Associados?")]
        [DefaultValue(false)]
        public Boolean IsInclusive { get; set; }

        [DisplayName("Link de um formulário externo")]
        [DefaultValue(null)]
        public String LinkToForm { get; set; }

        [DisplayName("Possíveis respostas ao inquérito")]
        public String[] OptionName { get; set; }
    }
}