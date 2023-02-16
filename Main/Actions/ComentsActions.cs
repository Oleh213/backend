using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebShop.Main.DBContext;
using WebShop.Models;
using WebShop.Main.Context;
using WebShop.Main.DTO;
using System.Data.Entity;

namespace WebShop.Main.Actions
{
    [ApiController]
    [Route("ComentsActions")]
    public class ComentsActions : ControllerBase
    {
        private ShopContext _context;

        public ComentsActions(ShopContext context)
        {
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        [HttpPost("AddComent")]
        public IActionResult AddComent([FromBody] ComentsModel model)
        {
            _context.users.Load();
            _context.coments.Load();

            var user = _context.users.FirstOrDefault(user => user.UserId == UserId);

            var product = _context.products.FirstOrDefault(x => x.ProductId == model.ProductId);

            if (user != null)
            {
                _context.coments.Add(new Coments
                {
                    ComentId = Guid.NewGuid(),
                    Body = model.Body,
                    ParentId = model.ParentId,
                    CreatedAt = DateTime.Now,
                    ProductId = model.ProductId,
                    UserId = UserId,
                    Rating = model.Rating,
                });

                _context.SaveChanges();

                int reting = 0;

                int count = 0;


                foreach(var item in _context.coments)
                {
                    if(item.ProductId == model.ProductId)
                    {
                        if(item.Rating!=null)
                        {
                            reting += (int)item.Rating;
                            count++;
                        }
                    }
                }

                product.Rating = reting / count;

                _context.SaveChanges();


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
        public IActionResult GetComent([FromQuery] Guid ProductId)
        {
            _context.users.Load();
            _context.coments.Load();

            var coments = _context.coments.Where(x => x.ProductId == ProductId);

            if (coments != null)
            {
                var comentsDTO = new List<ComentsDTO>();
                foreach(var item in coments.ToList())
                {
                    var user = _context.users.ToList().FirstOrDefault(x => x.UserId == item.UserId);
                    comentsDTO.Add(new ComentsDTO
                    {
                        Body = item.Body,
                        UserId = item.UserId,
                        Username = user.Name,
                        ComentId = item.ComentId,
                        CreatedAt = item.CreatedAt,
                        ParentId = item.ParentId,
                        ProductId = item.ProductId,
                        Rating = item.Rating,
                    });
                }
                return Ok(comentsDTO);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("UpdateComent")]
        public IActionResult UpdateComent([FromBody] PatchModel model)
        {
            _context.users.Load();
            _context.coments.Load();

            var coment = _context.coments.FirstOrDefault(x=> x.ComentId == model.ComentId);

            if(coment!=null)
            {
                coment.Body = model.Body;

                _context.SaveChanges();

                return Ok("Ok");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpDelete("DeleteComent")]
        public IActionResult DeleteComent([FromQuery] Guid ComentId)
        {
            _context.users.Load();
            _context.coments.Load();

            var coment = _context.coments.FirstOrDefault(x => x.ComentId == ComentId);

            if (coment != null)
            {
                var child = _context.coments.Where(x => x.ParentId == ComentId);

                foreach(var item in _context.coments)
                {
                    if (item.ParentId == ComentId)
                    {
                        _context.coments.Remove(item);
                    }
                }

                _context.coments.RemoveRange(child); 

                _context.coments.Remove(coment);

                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
    public class PatchModel
    {
        public string Body { get; set; }

        public Guid ComentId { get; set; }
    }
}

