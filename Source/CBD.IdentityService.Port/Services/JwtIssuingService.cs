using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using CBD.IdentityService.Core.Options;
using CBD.IdentityService.Core.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CBD.IdentityService.Port.Services; 

public class JwtIssuingService : IJwtIssuingService {
	private readonly IOptionsMonitor<JwtBearerOptions> _jwtBearerOptions;
	
	public JwtIssuingService(IOptionsMonitor<JwtBearerOptions> jwtBearerOptions) {
		this._jwtBearerOptions = jwtBearerOptions;
	}

	public string CreateToken(JwtIssuingOptions options, IEnumerable<Claim> claims) {
		var secretBytes = Encoding.ASCII.GetBytes(options.Secret);
		var tokenHandler = new JwtSecurityTokenHandler();

		var tokenDescriptor = new SecurityTokenDescriptor() {
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow + options.ExpiringTime,
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretBytes), SecurityAlgorithms.HmacSha512Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	public IEnumerable<Claim>? GetClaimsFromToken(string token) {
		var tokenHandler = new JwtSecurityTokenHandler();
		var securityToken = tokenHandler.ReadToken(token);

		return securityToken is not JwtSecurityToken jwtSecurityToken ? null : jwtSecurityToken.Claims;
	}

	public void ValidateToken(string token) {
		new JwtSecurityTokenHandler().ValidateToken(token, this._jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme)?.TokenValidationParameters, out _);
	}
	
	public bool IsValidToken(string token) {
		try {
			this.ValidateToken(token);
			return true;
		} catch (Exception) {
			return false;
		}
	}
}