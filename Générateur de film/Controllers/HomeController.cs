using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Générateur_de_film.Controllers
{
    public class HomeController : Controller
    {
        public static string Baseurl = "http://www.omdbapi.com/";
        public static string API_KEY = ConfigurationManager.AppSettings.Get("apikey");

        public async Task<ActionResult> Index()
        {
            NASA nasa = new NASA();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                string uri = "https://NasaAPIdimasV1.p.rapidapi.com/getPictureOfTheDay";
                client.BaseAddress = new Uri("https://NasaAPIdimasV1.p.rapidapi.com");

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "00736acc5emshca23db8603d3ac0p16e82bjsn90f451f99895");
                //Sending request 
                HttpResponseMessage Res = await client.PostAsync(uri, null);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var photooftheday = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    nasa = JsonConvert.DeserializeObject<NASA>(photooftheday);
                }
                dataUtiliateur data = new dataUtiliateur();
                data.Nasa = nasa;
                return View(data);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Sugestion(dataUtiliateur data)
        {
            await data.GetFilmAsync();

            //Hosted web API REST Service base url  
            Film FilmInfo = new Film();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                string uri = Baseurl + "?apikey="+API_KEY+"&i="+data.GetRandomFilm();
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync(uri);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var FilmResponce = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    FilmInfo = JsonConvert.DeserializeObject<Film>(FilmResponce);
                }
                SugestionFilm sugestion = new SugestionFilm(data.FilmExistants, FilmInfo);
                return View(sugestion);

            }
        }
    }
}
