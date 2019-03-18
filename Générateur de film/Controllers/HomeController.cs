using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        string Baseurl = "http://www.omdbapi.com/";
        // GET: Home
        public async Task<ActionResult> Index()
        {
        //Hosted web API REST Service base url  
            
            Film FilmInfo = new Film();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                string uri = Baseurl + "?apikey=df5969bf&t=django";
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
                //returning the employee list to view  
                return View(FilmInfo);
            }
        }
    }
}