using CBD.IdentityService.Core.Contracts.Requests.Authentication;
using CBD.IdentityService.Core.Contracts.Responses.Authentication;

namespace CBD.IdentityService.Core.Services.Authentication; 

public interface ISignUpService {
	Task<SignUpResponse> SignUpUserAsync(SignUpRequest request);
}