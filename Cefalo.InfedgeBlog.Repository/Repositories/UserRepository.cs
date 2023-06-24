using Cefalo.InfedgeBlog.Database.Context;
using Cefalo.InfedgeBlog.Database.Models;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.InfedgeBlog.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public UserRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await _dbcontext.Users.ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(int Id)
        {
            var user = await _dbcontext.Users.FindAsync(Id);
            return user;
        }
        public async Task<User> GetUserByUsernameAsync(string Username)
        {
            var user = await _dbcontext.Users.FindAsync(Username);
            return user;
        }
        public async Task<User> PostUserAsync(User user)
        {
            var newUser = _dbcontext.Users.Add(user);
            await _dbcontext.SaveChangesAsync();
            return newUser.Entity;
        }
        public async Task<User> UpdateUserByIdAsync(int Id, User user)
        {
            var userData = await _dbcontext.Users.FindAsync(Id);
            //userData.Username = User.Username;
            userData.Username = "abcd";
            await _dbcontext.SaveChangesAsync();
            return userData;
        }
        public async Task<Boolean> DeleteUserByIdAsync(int Id)
        {
            var user = await _dbcontext.Users.FindAsync(Id);
            _dbcontext.Users.Remove(user);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
    }
}
