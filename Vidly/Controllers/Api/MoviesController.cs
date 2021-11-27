﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<MovieDto> GetAllMovies()
        {
            return _context.Movies.ToList().Select(Mapper.Map<Movie , MovieDto>);

        }

        public IHttpActionResult GetMovie(int id)
        {
            var movieInDB = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movieInDB == null)
            {
                return NotFound();
            }


            return Ok(Mapper.Map<Movie , MovieDto>(movieInDB));
        }

        [HttpPost]
        public IHttpActionResult CreateMovie (MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var movie = Mapper.Map<MovieDto, Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;

            return Created(new Uri(Request.RequestUri +"/"+movie.Id ) , movieDto);
        }

        [HttpPut]
        public void UpdateMovie(MovieDto movieDto , int id)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

            }
            var movieInDB = _context.Movies.SingleOrDefault(c=>c.Id == id);

            if (movieInDB == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }


            Mapper.Map<MovieDto, Movie>(movieDto , movieInDB);
            _context.SaveChanges();

        }



        [HttpDelete]
        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            _context.Movies.Remove(movie);

            _context.SaveChanges();

        }








    }
}