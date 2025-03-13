using CoffeeAPI.Models;
using CoffeeAPI.Repository;

namespace CoffeeAPI.Services
{
    public interface IUserService
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> GetUserByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<Users>> GetUsersByRoleAsync(string role);
        Task<Users> CreateUserAsync(Users user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(int userId);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<IEnumerable<Users>> GetUsersByRoleAsync(string role)
        {
            return await _userRepository.GetByRoleAsync(role);
        }

        public async Task<Users> CreateUserAsync(Users user)
        {
            // Logic nghiệp vụ: Kiểm tra email đã tồn tại chưa
            if (await EmailExistsAsync(user.Email))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            // Logic nghiệp vụ: Mã hóa mật khẩu (giả sử dùng BCrypt)
            // user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(Users user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Cập nhật các thuộc tính cần thiết
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Role = user.Role;

            _userRepository.Update(existingUser);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
