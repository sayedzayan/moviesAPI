﻿
using System.ComponentModel.DataAnnotations.Schema;

namespace moviesAPI.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [MaxLength(length:100)]
        public string Name { get; set; }


    }
}
