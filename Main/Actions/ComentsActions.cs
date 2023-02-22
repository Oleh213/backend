using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.DBContext;
using WebShop.Main.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebShop.Models;
using WebShop.Main.DTO;
using WebShop.Main.BusinessLogic;


namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("ComentsActions")]
    public class ComentsActions : ControllerBase
    {
        private ShopContext _context;

        private IComentsActionsBL _comentsActionsBL;

        public ComentsActions(ShopContext context, IComentsActionsBL comentsActionsBL)
        {
            _context = context;
            _comentsActionsBL = comentsActionsBL;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddComent")]
        public async Task<IActionResult> AddComent([FromBody] ComentsModel model)
        {
            var user = await _comentsActionsBL.GetUser(UserId);

            var product = await _comentsActionsBL.GetProduct(model.ProductId);

            if (user != null)
            {
                await _comentsActionsBL.AddComent(model, UserId);

                await _comentsActionsBL.CountRating(product);

                var ok = new Response<string>()
                {
                    IsError = false,
                    ErrorMessage = "",
                    Data = "Coment was successfully added"
                };

                return Ok(ok);
            }
            else
            {
                var resError = new Response<string>()
                {
                    IsError = true,
                    ErrorMessage = "",
                    Data = "Please log in to add coment!"
                };
                return NotFound(resError);
            }
        }

        [HttpGet("GetComent")]
        public async Task<IActionResult> GetComent([FromQuery] Guid ProductId)
        {

            var coments = await _comentsActionsBL.GetComents(ProductId);

            if (coments != null)
            {
                var comentsDTO = _comentsActionsBL.ComentsDTO(coments);

                return Ok(comentsDTO);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("UpdateComent")]
        public async Task<IActionResult> UpdateComent([FromBody] PatchModel model)
        {

            var coment = await _comentsActionsBL.GetComent(model.ComentId);

            if(coment!=null)
            {
                await _comentsActionsBL.ChangeComent(coment, model.Body);

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete("DeleteComent")]
        public async Task<IActionResult> DeleteComent([FromQuery] Guid ComentId)
        {
            var coment = await _comentsActionsBL.GetComent(ComentId);

            if (coment != null)
            {
                var child = await _comentsActionsBL.GetChildComents(ComentId);

                await _comentsActionsBL.RemoveComent(coment, child);

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}

