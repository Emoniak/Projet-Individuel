using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Générateur_de_film
{
    public class SugestionFilm
    {
        private List<FilmExistant> filmExistants;
        private Film filmSelectionner;

        public SugestionFilm(List<FilmExistant> listeselectioner, Film film)
        {
            this.filmExistants = listeselectioner;
            this.filmSelectionner = film;
        }
        public Film FilmSelectioner
        {
            get { return filmSelectionner; }
            set { filmSelectionner = value; }
        }

        public List<FilmExistant> FilmExistants
        {
            get { return filmExistants; }
            set { filmExistants = value; }
        }

    }
}