namespace CoffeeAPI.DTO
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
