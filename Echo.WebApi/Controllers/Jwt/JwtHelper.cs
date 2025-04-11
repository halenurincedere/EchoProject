using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Echo.Business.Jwt
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(JwtDto jwtInfo)
        {
            // Create signing key from secret
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Build user claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtInfo.Id.ToString()), // Subject
                new Claim(JwtClaimNames.Id, jwtInfo.Id.ToString())
            };

            if (!string.IsNullOrWhiteSpace(jwtInfo.Email))
                claims.Add(new Claim(JwtClaimNames.Email, jwtInfo.Email));

            if (!string.IsNullOrWhiteSpace(jwtInfo.FirstName))
                claims.Add(new Claim(JwtClaimNames.FirstName, jwtInfo.FirstName));

            if (!string.IsNullOrWhiteSpace(jwtInfo.LastName))
                claims.Add(new Claim(JwtClaimNames.LastName, jwtInfo.LastName));

            if (jwtInfo.UserRole != null)
            {
                claims.Add(new Claim(JwtClaimNames.UserRole, jwtInfo.UserRole.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, jwtInfo.UserRole.ToString())); // For [Authorize(Roles = "Admin")]
            }

            // Set expiration time
            var expireTime = DateTime.UtcNow.AddMinutes(jwtInfo.ExpireMinutes);

            // Create JWT
            var token = new JwtSecurityToken(
                issuer: jwtInfo.Issuer,
                audience: jwtInfo.Audience,
                claims: claims,
                expires: expireTime,
                signingCredentials: credentials
            );

            // Return encoded JWT token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}