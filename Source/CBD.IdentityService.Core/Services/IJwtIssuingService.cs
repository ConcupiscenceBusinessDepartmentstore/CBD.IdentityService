using System.Security.Claims;

using CBD.IdentityService.Core.Options;

namespace CBD.IdentityService.Core.Services; 

public interface IJwtIssuingService {
	string CreateToken(JwtIssuingOptions options, IEnumerable<Claim> claims);
	IEnumerable<Claim>? GetClaimsFromToken(string token);
	void ValidateToken(string token);
	bool IsValidToken(string token);
}