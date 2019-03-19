using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Générateur_de_film
{
    public class dataUtiliateur
    {
        private NASA nasa;
        private string idFilm;
        private string type;

        private static List<SelectListItem> listeType = new List<SelectListItem>()
        {
            new SelectListItem{Text="Film",Value="movie",Selected=true},
            new SelectListItem{Text="Serie",Value="series"},
            new SelectListItem{Text="Episode",Value="episode"},
        };


        [Display(Name ="Type")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        public string IdFilm
        {
            get { return idFilm; }
            set { idFilm = value; }
        }
        public NASA Nasa
        {
            get { return nasa; }
            set { nasa = value; }
        }

        public List<SelectListItem> ListeType
        {
            get { return listeType; }
            set { listeType = value; }

        }

    }
}