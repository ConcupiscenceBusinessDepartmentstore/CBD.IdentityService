using CBD.IdentityService.Core.ValueObjects;

namespace CBD.IdentityService.Core.Contracts.Responses.Authentication; 

public class RequestChangePasswordResponse : BaseResponse<RequestChangePasswordResponse.Body> {
	public class Body { }
	
	public static class Message {
		public static readonly Info EmailSendIfRegistered = new() {
			Code = nameof(EmailSendIfRegistered),
			Description = "An email containing the reset token was send to the provided email if registered"
		};
	}
	
	public static class Error {
		public static readonly Info UserNotFound = new() {
			Code = nameof(UserNotFound),
			Description = "The user for the provided email address was not found"
		};
	}
}