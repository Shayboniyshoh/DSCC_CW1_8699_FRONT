using DSCC_CW1_FRONT_8699.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DSCC_CW1_FRONT_8699.Controllers
{
    public class MovieController : Controller
    {
        //Hosted web API REST Service base url 
        string Baseurl = "https://localhost:44300/";
        // GET: Movie
        public async Task<ActionResult> Index()
        {
            List<Movie> movieInfo = new List<Movie>();
            using (var client = new HttpClient())
            {
                //Passing service base url 
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllMovies using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Movie");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Movie list
                    movieInfo = JsonConvert.DeserializeObject<List<Movie>>(PrResponse);
                }
                //returning the Movie list to view 
                return View(movieInfo);
            }
        }
        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Movie/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Name, Description, IssueYear, Genre")] Movie movie)
        {
            Movie movieDetails = new Movie();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync($"api/Movie", movie);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        movieDetails = JsonConvert.DeserializeObject<Movie>(result);
                        return RedirectToAction("Index");
                    }
                }
            }
            catch
            {
                return View();
            }
            return View(movieDetails);
        }
        // GET: Movie/Edit/{id}
        public async Task<ActionResult> Edit(int id)
        {
            Movie mov = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Movie/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Movie list
                    mov = JsonConvert.DeserializeObject<Movie>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(mov);
        }
        // POST: Movie/Edit/{id}
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Movie mov)
        {
            try
            {
                // TODO: Add update logic here
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage Res = await client.GetAsync("api/Movie/" + id);
                    Movie _movie = null;
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api 
                        var PrResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Movie list
                        _movie = JsonConvert.DeserializeObject<Movie>(PrResponse);
                    }
                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<Movie>("api/Movie/" + mov.Id, mov);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Movie/Delete/{id}
        public async Task<ActionResult> Delete(int? id)
        {
            Movie movie = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage res = await client.GetAsync($"api/Movie/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    movie = JsonConvert.DeserializeObject<Movie>(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
            }

            return View(movie);
        }
        // DELETE: Movie/Delete/{guid}
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage response = await client.GetAsync($"api/Movie/{id}");
                    Movie movie = null;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        movie = JsonConvert.DeserializeObject<Movie>(result);
                    }
                    var deleteResponse = await client.DeleteAsync($"api/Movie/{id}");
                    var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
        // GET: Movie/Details/{id}
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var result = await client.GetAsync($"api/Movie/{id}");

                if (result.IsSuccessStatusCode)
                {
                    movie = await result.Content.ReadAsAsync<Movie>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }
    }
}
