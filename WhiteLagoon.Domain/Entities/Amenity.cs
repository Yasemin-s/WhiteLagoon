﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    //amenity - kolaylik
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        //yabanci anahtar iliskisi icin

        [ForeignKey("Villa")]
        public int VillaId { get; set; }

        [ValidateNever]
        public Villa Villa { get; set; }
    }
}
