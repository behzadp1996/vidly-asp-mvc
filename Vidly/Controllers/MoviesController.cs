﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModel;
using System.Data.Entity;

namespace Vidly.Controllers
{
    [Authorize(Roles = RoleName.CanManageMovies)]
    public class MoviesController : Controller
    {
        ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        [AllowAnonymous]
        // GET: Movies / Random
        public ActionResult Index()
        {
            // var movies = _context.Movies.Include(m=>m.Genre).ToList();


            // return View(movies);

            if (User.IsInRole(RoleName.CanManageMovies))
            {
                return View("List"); //List Is Complete View To Show TO the Admin
            }
            

            return View("ReadOnlyList"); //Simple View Which Just List For The Guests
        }

        
        public ActionResult Detail (int id)
        {
            var movie = _context.Movies.Include(c => c.Genre).SingleOrDefault(c => c.Id == id);

            if (movie == null)
                 return HttpNotFound();
            
            return View(movie);
        }


        
        public ActionResult NewMovie()
        {

            var viewModel = new MovieFormViewModel()
            {
                //Movie = new Movie() ,
                Genres = _context.Genres.ToList()
                
            };


            return View("MovieForm" , viewModel);
        }

        
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel(movie)
            {
                //Movie = movie,
                Genres = _context.Genres.ToList()
            };
            return View("MovieForm", viewModel);
        }


        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult Save (Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    //Movie = movie , 
                    Genres = _context.Genres.ToList()
                    
                };

                return View("MovieForm", viewModel);

            }
            //Add New Movie
            if (movie.Id == 0)
            {
                movie.NumberAvailable = movie.NumberInStock;
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);

                
            }
            //Save Edited Movie
            else
            {
                var movieInDB = _context.Movies.Single(m => m.Id == movie.Id);
                movieInDB.Name = movie.Name;
                movieInDB.ReleaseDate = movie.ReleaseDate;
                
                movieInDB.NumberInStock  = movie.NumberInStock;
                movieInDB.GenreId = movie.GenreId;
                
            }
             _context.SaveChanges();
            return RedirectToAction("Index" , "Movies");
        }







        //Related TO old Customers And First Example Of Mvc's
        [AllowAnonymous]
        public ActionResult Random()
        {

            var movie = new Movie() { Name = "Avengers" };
            List<Customer> customerList = new List<Customer>
            {
                new Customer() { Name = "Behzad Parsa" },
                new Customer() { Name = "Tom Hanks" },
                new Customer() { Name = "Tom Cruise" },
                new Customer() { Name = "Emilia Clark" },
                new Customer() { Name = "Daniel Day Lweis" },
                new Customer() { Name = "Michelle Mongahana" }

            };



            var viewModel = new RandomMovieViewModel()
            {
                Movie = movie,
                Customers = customerList
            };






            //return View(movie);
            return View(viewModel);
        }
        //private IEnumerable<Movie> GetMovies()
        //{

        //    return new List<Movie>()
        //    {
        //        new Movie(){Id=1 , Name="Mission Impossible"},
        //        new Movie(){Id=2 , Name="Dune"},
        //        new Movie(){Id=3 , Name="Batman"}
        //    };
        //}

    }
}