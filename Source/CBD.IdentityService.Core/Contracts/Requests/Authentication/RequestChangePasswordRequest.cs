using System.ComponentModel.DataAnnotations;

namespace CBD.IdentityService.Core.Contracts.Requests.Authentication; 

public class RequestChangePasswordRequest {
	[Required, EmailAddress]
	public string Email { get; set; }
}