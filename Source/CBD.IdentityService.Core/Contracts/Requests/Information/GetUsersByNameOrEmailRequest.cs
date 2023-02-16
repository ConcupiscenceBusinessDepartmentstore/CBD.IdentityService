using System.ComponentModel.DataAnnotations;

namespace CBD.IdentityService.Core.Contracts.Requests.Information; 

public class GetUsersByNameOrEmailRequest {
	[Required] public string UsernameEmail { get; set; }
}