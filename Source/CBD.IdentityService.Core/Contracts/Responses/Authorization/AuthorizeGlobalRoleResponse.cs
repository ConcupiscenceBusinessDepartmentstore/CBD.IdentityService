using CBD.IdentityService.Core.ValueObjects;

namespace CBD.IdentityService.Core.Contracts.Responses.Authorization;

public class AuthorizeGlobalRoleResponse : AuthorizeResponse<AuthorizeGlobalRoleResponse.Body> {
	public new class Body : AuthorizeResponse<Body>.Body { }

	public static class Error {
		public static readonly Info UserNotFound = new() {
			Code = nameof(UserNotFound),
			Description = "No user was found for the provided token"
		};
		
		public static readonly Info UserNotInRole = new() {
			Code = nameof(UserNotInRole),
			Description = "The user does not belong to the provided global role"
		};
	}
}