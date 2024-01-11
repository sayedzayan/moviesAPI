using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Services;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;
        //ApplicationDbContext _context;


        public GenresController( IGenresService genresService)
        {
            _genresService = genresService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresService.GetAll();
            return Ok(genres);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _genresService.Add(genre);
            return Ok(genre);
        }


        [HttpPut(template: "{id}")]
        //api/genres/1
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _genresService.GetById(id);
         if(genre == null)
            
                return NotFound(value: $"No genre was found with id: {id}");
           genre.Name = dto.Name;
                _genresService.Update(genre);
                return Ok(genre);
            
        }


        [HttpDelete(template: "{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
                return NotFound(value: $"No genre was found with id: {id}");

           _genresService.Delete(genre);
            return Ok(genre);

        }




    }
}
