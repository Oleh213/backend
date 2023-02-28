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
using WebShop.Main.Context;

namespace Shop.Main.Actions
{
    [ApiController]
    [Route("CategoryActions")]
    public class CategoryActions : ControllerBase
    {
        private ICategoryActionsBL _categoryActionsBL;

        private readonly ILoggerBL _loggerBL;

        public CategoryActions(ICategoryActionsBL categoryActionsBL, ILoggerBL loggerBL)
        {
            _categoryActionsBL = categoryActionsBL;
            _loggerBL = loggerBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [Authorize]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryModel model)
        {
            try
            {
                var user = await _categoryActionsBL.GetUser(UserId);

                if (user != null)
                {
                    if (user.Role == UserRole.Admin)
                    {
                        if (!await _categoryActionsBL.CheckCategory(model.CategoryName))
                        {
                            await _categoryActionsBL.AddCategory(model.CategoryName);

                            var resOk = new Response<string>()
                            {
                                IsError = false,
                                ErrorMessage = "",
                                Data = "Category was successfully added!"
                            };

                            _loggerBL.AddLog(LoggerLevel.Info, $"UserId:'{UserId}' added new Category:'{model.CategoryName}'");
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

                            _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted add new Category:'{model.CategoryName}'(Category name already exists)");
                            return NotFound(resError);
                        }
                    }
                    else
                    {
                        _loggerBL.AddLog(LoggerLevel.Warn, $"UserId:'{UserId}' wanted add new Category:'{model.CategoryName}'(User not Admin!)");
                        return Unauthorized();
                    }
                }
                else
                    return Unauthorized();
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categoriesDTO = _categoryActionsBL.CategoriesDTO();

                return Ok(categoriesDTO);
            }
            catch (Exception ex)
            {
                _loggerBL.AddLog(LoggerLevel.Error, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}