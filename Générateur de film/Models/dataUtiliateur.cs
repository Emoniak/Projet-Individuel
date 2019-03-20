using Générateur_de_film.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Générateur_de_film
{
    public class dataUtiliateur
    {
        private NASA nasa;
        private string type;
        private List<FilmExistant> filmExistants;
        private string MaRecherRandom;

        public dataUtiliateur()
        {
            this.filmExistants = new List<FilmExistant>();
        }
        public dataUtiliateur(List<FilmExistant> ancienneListe)
        {
            this.filmExistants = ancienneListe;
        }

        public List<FilmExistant> FilmExistants
        {
            get { return filmExistants; }
            set { filmExistants = value; }
        }

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

        internal async System.Threading.Tasks.Task GetFilmAsync()
        {
            FilmExistant film = new FilmExistant();
            using (var client = new HttpClient())
            {
                MaRecherRandom = GetMotClef();
                string uri = HomeController.Baseurl + "?apikey=" + HomeController.API_KEY + "&s=" + MaRecherRandom +"&t="+this.type;
                client.BaseAddress = new Uri(HomeController.Baseurl);

                client.DefaultRequestHeaders.Clear();  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync(uri);
                if (Res.IsSuccessStatusCode)
                {
                    var FilmResponce = Res.Content.ReadAsStringAsync().Result;
                    film = JsonConvert.DeserializeObject<FilmExistant>(FilmResponce);
                    FilmExistants.Add(film);
                    await VerifierPageAsync();
                }
            }

        }

        private string GetMotClef()
        {
            string motclef = "";
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings.Get("cs")))
            {
                connection.Open();
                Random random = new Random();
                int max = 0;
                SqlCommand cmd = new SqlCommand("select count(codefilm) from tfilm", connection);
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    max = Convert.ToInt32(dr[0]);
                }
                dr.Close();
                cmd.CommandText = @"select motclef from tfilm where codefilm=" + random.Next(1, max);
                dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    motclef = dr[0].ToString();
                }
            }
            return motclef;
        }

        private async System.Threading.Tasks.Task VerifierPageAsync()
        {
            int nbpage = 0;
            nbpage = Int32.Parse(filmExistants[0].totalResults);
            if (nbpage <= 10)
                return;
            else
                nbpage = (nbpage / 10)+1;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(HomeController.Baseurl);
                for (int i = 2; i < nbpage; i++)
                {
                    FilmExistant temp = new FilmExistant();

                    string uri = HomeController.Baseurl + "?apikey=" + HomeController.API_KEY + "&s=" + MaRecherRandom + "&t=" + this.type+"&page="+i;

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = await client.GetAsync(uri);
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var FilmResponce = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        temp = JsonConvert.DeserializeObject<FilmExistant>(FilmResponce);
                        filmExistants.Add(temp);
                    }
                }                
            }
        }

        internal string GetRandomFilm()
        {
            Random random = new Random();
            int page = random.Next(0,FilmExistants.Count);
            int result = random.Next(0,FilmExistants[page].Search.Count);

            string id = FilmExistants[page].Search[result].imdbID;
            filmExistants[page].Search.Remove(filmExistants[page].Search[result]);

            if (FilmExistants[page].Search.Count == 0)
                FilmExistants.Remove(FilmExistants[page]);

            return id;
        }
    }
}