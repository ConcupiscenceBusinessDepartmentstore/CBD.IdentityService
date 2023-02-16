using System.ComponentModel.DataAnnotations;

namespace CBD.IdentityService.Core.Contracts.Requests.Authentication; 

public class LoginRequest {
	[Required]
	public string UsernameEmail { get; set; }
	
	[Required]
	public string Password { get; set; }
}