using BlazorAppWeb.Shared.Models;

namespace BlazorAppWeb.Server.Interfaces
{
    public interface IUser
    {
        public List<User> GetUsers();
        public void AddUser(User user);
        public void UpdateUser(User user);
        public User GetUserById(int id);
        public void DeleteUser(int id);
    }
}
