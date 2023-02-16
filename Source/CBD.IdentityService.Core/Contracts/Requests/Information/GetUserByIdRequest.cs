using System.ComponentModel.DataAnnotations;

namespace CBD.IdentityService.Core.Contracts.Requests.Information;

public class GetUserByIdRequest {
	[Required] public string UserId { get; set; }
}