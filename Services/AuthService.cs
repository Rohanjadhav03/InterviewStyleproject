using Microsoft.IdentityModel.Tokens;
using PracticeApi.Data.Repository;
using PracticeApi.Models;
using PracticeApi.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PracticeApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository Repo;
        private readonly IConfiguration Config;
        public AuthService(IAuthRepository _repo,IConfiguration _Config)
        {
            Repo = _repo;
            Config = _Config;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await Repo.GetUserByUserNameAsync(request.Username);

            if (user == null)
            {
                throw new Exception("Invalid Username or Password");
            }

            string hashed = Hash(request.Password);

            if (hashed != user.Password)
            {
                throw new Exception("Invalid Username or Password");
            }

            string token = GenerateToken(user);
            string refreshToken = GenerateRefreshToken();
            DateTime expiry = DateTime.UtcNow.AddDays(7);

            await Repo.UpdateRefreshTokenAsync(user.UserId, refreshToken, expiry);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                RefreshToken = refreshToken
            };
        }

        public async Task<int> RegisterAsync(RegisterRequeset requeset)
        {
            string hashed = Hash(requeset.Password);
            var user = new UserAuth
            {
                Username = requeset.Username,
                Password = hashed,
                Role = "User"
            };
            return await Repo.RegisterAsync(user);
        }

        public static string Hash(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashbytes = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hashbytes);
            }
        }

        private string GenerateToken(UserAuth user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
              new Claim("UserId", user.UserId.ToString()),
              new Claim(ClaimTypes.Role, user.Role)
             };

            var token = new JwtSecurityToken(
                issuer: Config["Jwt:Issuer"],
                audience: Config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await Repo.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            string newAccessToken = GenerateToken(user);
            string newRefreshToken = GenerateRefreshToken();
            DateTime newExpiry = DateTime.UtcNow.AddDays(7);

            await Repo.UpdateRefreshTokenAsync(user.UserId, newRefreshToken, newExpiry);

            return new LoginResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Username = user.Username
            };
        }
    }
}
