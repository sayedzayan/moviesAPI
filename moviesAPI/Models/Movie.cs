using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace moviesAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(length: 250)]
        public string Title { get; set; }
        
       
        public int Year { get; set; }

        public double Rate { get; set; }

        [MaxLength(length: 2500)]
        public string Storeline { get; set; }

        public byte [] Poster { get; set; }
         

        public byte GenreId { get; set; }  // fk  genres  


        public Genre Genre { get; set; }  //navigation prop



    }
}
