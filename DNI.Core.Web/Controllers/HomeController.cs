﻿using DNI.Core.Services.Abstraction;
using DNI.Core.Services.Attributes;
using DNI.Core.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DNI.Core.Web.Controllers
{
    public class HomeController : DefaultControllerBase
    {
        public async Task<ActionResult> Index([FromQuery] HomeIndexRequestViewModel homeIndexRequestViewModel)
        {
            await Task.FromResult(true);
            return View(new HomeIndexViewModel {
                PageRequest = new PageViewComponentRequestViewModel
                {
                    PageName = "Home"
                }
            });
        }

        [HandleException]
        public async Task<string> GetValue(string value)
        {
            Console.WriteLine("GetValue");
                throw new UnauthorizedAccessException();
            return await Task.FromResult(value);
        }

        public async Task<bool> DisplayValue([FromQuery, Required]bool displayValue, [FromQuery, Required] string moo, [FromQuery, Required] string roo)
        {
            return await Task.FromResult(displayValue);
        }

        public async Task<string> GetElseValue(string value)
        {
            Console.WriteLine("GetElseValue");
            return await Task.FromResult(value);
        }

    }
}