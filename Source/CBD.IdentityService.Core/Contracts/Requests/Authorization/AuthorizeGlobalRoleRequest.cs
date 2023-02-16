namespace CBD.IdentityService.Core.Contracts.Requests.Authorization; 

public class AuthorizeGlobalRoleRequest : AuthorizeRequest {
	public string GlobalRole { get; set; }
}