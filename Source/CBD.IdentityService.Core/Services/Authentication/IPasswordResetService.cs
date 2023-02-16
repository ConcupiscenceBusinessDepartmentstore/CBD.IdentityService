using CBD.IdentityService.Core.Contracts.Requests.Authentication;
using CBD.IdentityService.Core.Contracts.Responses.Authentication;

namespace CBD.IdentityService.Core.Services.Authentication; 

public interface IPasswordResetService {
	Task<RequestChangePasswordResponse> RequestChangePasswordTokenAsync(RequestChangePasswordRequest request);
	Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request);
}