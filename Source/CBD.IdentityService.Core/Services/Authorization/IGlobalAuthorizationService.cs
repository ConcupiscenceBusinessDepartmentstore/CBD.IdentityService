using CBD.IdentityService.Core.Contracts.Requests.Authorization;
using CBD.IdentityService.Core.Contracts.Responses.Authorization;

namespace CBD.IdentityService.Core.Services.Authorization; 

public interface IGlobalAuthorizationService {
	Task<AuthorizeResponse> AuthorizeUserAsync(AuthorizeRequest request);
	Task<AuthorizeGlobalRoleResponse> AuthorizeUserGlobalRoleAsync(AuthorizeGlobalRoleRequest request);
}