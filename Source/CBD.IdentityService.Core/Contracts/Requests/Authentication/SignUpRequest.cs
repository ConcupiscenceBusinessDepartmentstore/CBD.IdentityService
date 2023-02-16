using System.ComponentModel.DataAnnotations;

namespace CBD.IdentityService.Core.Contracts.Requests.Authentication;

public class SignUpRequest {
	[Required]
	public string Username { get; set; }
	
	[Required, EmailAddress]
	public string Email { get; set; }
	
	[Required]
	public string Password { get; set; }
}