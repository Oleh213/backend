using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Main.DBContext;
using WebShop.Main.DTO;
using WebShop.Main.Interfaces;
using WebShop.Models;

namespace WebShop.Main.BusinessLogic
{
	public class UserActionsBL : IUserActionsBL
    {
        private ShopContext _context;

        public UserActionsBL(ShopContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<DeliveryOptions> GetUserDeliveryInfo(Guid userId)
        {
            return await _context.deliveryOptions.FirstOrDefaultAsync(user => user.UserId == userId);
        }

        public async Task<bool> CheckName(string name)
        {
            return await _context.users.AnyAsync(x => x.Name == name);
        }

        public async Task<bool> CheckEmail(string email)
        {
            return await _context.users.AnyAsync(x => x.Email == email);
        }

        public async Task<string> ChangeUserInfo(UserInfoModel model, User user)
        {
            user.Name = model.Name;
            user.Email = model.Email;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber.ToString();
            user.Birthday = model.Birthday;

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public async Task<string> ChangeDeliveryInfo(DeliveryOptionsModel model, DeliveryOptions user)
        {
            user.Country = model.Country;
            user.Region = model.Region;
            user.City = model.City;
            user.Address = model.Address;
            user.Address2 = model.Address2;
            user.ZipCode = model.ZipCode;

            await _context.SaveChangesAsync();

            return "Ok";
        }

        public UserDTO UserDTO(User user)
        {
            byte permition = 1;

            if (user.Role == UserRole.Admin)
            {
                permition = 0;
            }
            else if (user.Role == UserRole.Manager)
            {
                permition = 2;
            }

            var userId = new UserDTO { UserId = user.UserId, UserRole = permition };

            return userId;
        }

        public async Task<DeliveryOptionsModel> DeliveryDTO(DeliveryOptions user)
        {
            if (user.Country == null)
            {
                user.Country = "Not specified";
            }
            if (user.Region == null)
            {
                user.Region = "Not specified";
            }
            if (user.City == null)
            {
                user.City = "Not specified";
            }
            if (user.Address == null)
            {
                user.Address = "Not specified";
            }
            if (user.Address2 == null)
            {
                user.Address2 = "Not specified";
            }
            if (user.ZipCode == null)
            {
                user.ZipCode = "Not specified";
            }


            var outUser = new DeliveryOptionsModel()
            {
                Country = user.Country,
                Region = user.Region,
                City = user.City,
                Address = user.Address,
                Address2 = user.Address2,
                ZipCode = user.ZipCode
            };

            await _context.SaveChangesAsync();

            return outUser;
        }

        public async Task<UserInfoModel> UserInfoDTO(User user)
        {
            if (user.LastName == null)
            {
                user.LastName = "Not specified";
            }
            if (user.Email == null)
            {
                user.Email = "Not specified";
            }
            if (user.PhoneNumber == null)
            {
                user.PhoneNumber = "Not specified";
            }
            if (user.Birthday == null)
            {
                user.Birthday = "Not specified";
            }

            var outUser = new UserInfoModel()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Birthday = user.Birthday
            };

            await _context.SaveChangesAsync();

            return outUser;
        }
    }
}

