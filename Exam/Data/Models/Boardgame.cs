﻿using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models
{

    public class Boardgame
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)] // min 10
        public string Name { get; set; }

        [Required]
        public double Rating { get; set; } // range 1 10.00 

        [Required]
        public int YearPublished { get; set; } // rannge 2018 2023

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }

        public virtual Creator Creator { get; set; } = null!;

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new HashSet<BoardgameSeller>();

    }
}
