using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SD_Turizm.Tests.Integration.Helpers
{
    public static class AuthenticationHelper
    {
        public static string GenerateTestJwtToken(string username = "testuser", string userId = "1")
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TestKeyWithMinimum32CharactersLong"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("sub", userId),
                new Claim("username", username)
            };

            var token = new JwtSecurityToken(
                issuer: "TestIssuer",
                audience: "TestAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static void AddAuthorizationHeader(this HttpClient client, string? token = null)
        {
            var jwtToken = token ?? GenerateTestJwtToken();
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);
        }
    }
}
