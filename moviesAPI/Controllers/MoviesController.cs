using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Services;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        //database
        private readonly ImoviesServices _moviesservices;
        private readonly IGenresService _genresService;

        private new List<string> _allowedExtentions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;


        public MoviesController(ImoviesServices moviesservices, IGenresService genresService)
        {
            _moviesservices = moviesservices;
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesservices.GetAll();
            //todo map movies to dto

            return Ok(movies);
        }

        [HttpGet(template: "{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _moviesservices.GetById(id);
            if (movie == null)
                return NotFound();

            var dto = new MovieDetailsDto
            {

                Id = movie.Id,
                GenreId = movie.GenreId,
                GenreName = movie.Genre?.Name,
                Rate = movie.Rate,
                Storeline = movie.Storeline,
                Title = movie.Title,
                Year = movie.Year,
                Poster = movie.Poster


            };

            return Ok(dto);

        }



        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _moviesservices.GetAll(genreId);
            //todo map movies to dto


            return Ok(movies);
        }




        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if(dto.Poster==null)
            {
                return BadRequest(error: "Poster is requried");
            }

            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max Allowed Size For Poster is 1MB!");


            var isValidGenre = await _genresService.isValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("invalid genre id");


            //نتعامل ع البوستر  ال هتيجي ونحولها لاراي بايت ونبعتها للدومين موديل
            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);


            var movie = new Movie
            {

                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = datastream.ToArray(),
                Rate = dto.Rate,
                Storeline = dto.Storeline,
                Year = dto.Year,

            };
          _moviesservices.Add(movie);
            return Ok(movie);
        }

        [HttpPut(template: "{id}")]

        public async Task<IActionResult> UpdateAsync(int id , [FromForm] MovieDto dto)
        {
            var movie = await _moviesservices.GetById(id); ;
            if (movie == null)
                return NotFound($"No movie was found with id {id}");

            var isValidGenre = await _genresService.isValidGenre(dto.GenreId);
            if (!isValidGenre)
                return BadRequest("invalid genre id");


            if (dto.Poster != null)
            {
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max Allowed Size For Poster is 1MB!");

                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);

                movie.Poster = datastream.ToArray();

            }

            movie.Title=dto.Title;
            movie.Year=dto.Year;
            movie.Rate=dto.Rate;
            movie.Storeline=dto.Storeline;
            movie.GenreId=dto.GenreId;

            _moviesservices.Update(movie);
            return Ok(movie);

        }


            [HttpDelete(template: "{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        { 
            var movie=await _moviesservices.GetById(id);
            if(movie == null) 
                return NotFound($"No movie was found with id {id}");
        _moviesservices.Delete(movie);
            return Ok(movie);
        }

    }


}
