using System;
using System.Collections.Generic;
using Shop.Main.Actions;
using System.Xml.Linq;
using System.Linq;
using WebShop.Main.Conext;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;
using WebShop.Main.DTO;
using System.Data.Entity;
//using WebShop.Reguests;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebShop.Models;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("CategoryActions")]
    public class CategoryActions : ControllerBase
    {
        private ShopContext _context;

        private ICategoryActionsBL _categoryActionsBL;

        public CategoryActions(ShopContext context, ICategoryActionsBL categoryActionsBL)
        {
            _context = context;
            _categoryActionsBL = categoryActionsBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [Authorize]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryModel model)
        {
            var user = await _categoryActionsBL.GetUser(UserId);

            if (user!=null)
            {
                if (user.Role == UserRole.Admin)
                {
                    if(!await _categoryActionsBL.CheckCategory(model.CategoryName))
                    {
                        await _categoryActionsBL.AddCategory(model.CategoryName);

                        var resOk = new Response<string>()
                        {
                            IsError = false,
                            ErrorMessage = "",
                            Data = "Category was successfully added!"
                        };
                        return Ok(resOk);
                    }
                    else
                    {
                        var resError = new Response<string>()
                        {
                            IsError = true,
                            ErrorMessage = "",
                            Data = "Enter another name of category!"
                        };
                        return NotFound(resError);
                    }
                    
                }
                else
                    return Unauthorized();
            }
            else
                return Unauthorized();
        }


        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var categoriesDTO = _categoryActionsBL.CategoriesDTO();

            return Ok(categoriesDTO);
        }
    }
}