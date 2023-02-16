using CBD.IdentityService.Core.Contracts.Requests.Authentication;
using CBD.IdentityService.Core.Contracts.Responses.Authentication;

namespace CBD.IdentityService.Core.Services.Authentication; 

public interface ILoginService<TUser> {
	Task<LoginResponse> LoginUserAsync(LoginRequest request);
	Task<TUser?> GetUserForTokenAsync(string token);
}