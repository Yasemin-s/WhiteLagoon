﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.ViewModels
{
    public class AmenityVM
    {
        public Amenity? Amenity { get; set; } //? ile null olabilir dedik
        [ValidateNever]
        //dropdown icin gerekli
        public IEnumerable<SelectListItem>? VillaList { get; set; }
    }
}
