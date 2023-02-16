using CBD.IdentityService.Core.Contracts.Requests.Information;
using CBD.IdentityService.Core.Contracts.Responses.Information;

namespace CBD.IdentityService.Core.Services.Information; 

public interface IAuthenticationInformationService {
	Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest request);
	Task<GetUsersByNameOrEmailResponse> GetUsersByNameOrEmailAsync(GetUsersByNameOrEmailRequest request);
}