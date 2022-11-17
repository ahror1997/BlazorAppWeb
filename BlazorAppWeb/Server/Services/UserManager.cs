using BlazorAppWeb.Server.Data;
using BlazorAppWeb.Server.Interfaces;
using BlazorAppWeb.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWeb.Server.Services
{
    public class UserManager : IUser
    {
        private readonly DataContext dataContext;

        public UserManager(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public List<User> GetUsers()
        {
            try
            {
                return dataContext.Users.ToList();
            }
            catch
            {
                throw;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                dataContext.Users.Add(user);
                dataContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                var user = dataContext.Users.Find(id);
                if (user is not null)
                {
                    return user;
                }
                throw new ArgumentNullException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                dataContext.Entry(user).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                var user = dataContext.Users.Find(id);
                if (user != null)
                {
                    dataContext.Users.Remove(user);
                    dataContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
