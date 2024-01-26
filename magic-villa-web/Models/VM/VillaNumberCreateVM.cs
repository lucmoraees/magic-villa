using System;
using magic_villa_web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace magic_villa_web.Models.VM
{
    public class VillaNumberCreateVM
    {
        public VillaNumberCreateVM()
        {
            VillaNumber = new VillaNumberCreateDTO();
        }

        public VillaNumberCreateDTO VillaNumber { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}

