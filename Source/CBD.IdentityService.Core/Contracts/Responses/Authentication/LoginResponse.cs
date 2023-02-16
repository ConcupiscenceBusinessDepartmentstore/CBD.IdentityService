using CBD.IdentityService.Core.ValueObjects;

namespace CBD.IdentityService.Core.Contracts.Responses.Authentication; 

public class LoginResponse : BaseResponse<LoginResponse.Body> {
	public class Body {
		public string Token { get; set; } = "";
	}

	public static class Message {
		public static readonly Info LoggedIn = new() {
			Code = nameof(LoggedIn),
			Description = "User logged in successfully"
		};
	}
	
	public static class Error {
		public static readonly Info UsernameOrEmailNotRegistered = new() {
			Code = nameof(UsernameOrEmailNotRegistered),
			Description = "The username or email is not registered"
		};
		
		public static readonly Info PasswordNotCorrect = new() {
			Code = nameof(PasswordNotCorrect),
			Description = "The password is not correct"
		};
	}
}