using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Helpers
{
	public static class JsonWebTokens
	{
		public static string NewJsonWebTokens(User appUser, IConfiguration configuration)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
			var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var userClaims = new[] {
				new Claim(ClaimTypes.NameIdentifier, $"{appUser.Id}"),
				new Claim(ClaimTypes.Name, $"{appUser.FirstName!} {appUser.LastName!}"),
				new Claim(ClaimTypes.Actor, $"{appUser.Username}")
			};

			var token = new JwtSecurityToken(
				issuer: configuration["Jwt:Issuer"],
				audience: configuration["Jwt:Audience"],
				claims: userClaims,
				expires: DateTime.Now.AddDays(7),
				signingCredentials: credential
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

}

