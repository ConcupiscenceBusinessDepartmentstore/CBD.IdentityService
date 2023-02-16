using CBD.IdentityService.Core.ValueObjects;

namespace CBD.IdentityService.Core.Contracts.Responses.Authorization;

public class AuthorizeResponse : AuthorizeResponse<AuthorizeResponse.Body> {
	public new class Body : AuthorizeResponse<Body>.Body { }
	
	public static class Message {
		public static readonly Info Authorized = new() {
			Code = nameof(Authorized),
			Description = "The User is authorized"
		};
		
		public static readonly Info Unauthorized = new() {
			Code = nameof(Authorized),
			Description = "The User is not authorized"
		};
	}
	
	public static class Error {
		public static readonly Info InvalidToken = new() {
			Code = nameof(InvalidToken),
			Description = "The provided token is not valid"
		};
		
		public static readonly Info ExpiredToken = new() {
			Code = nameof(ExpiredToken),
			Description = "The token expired and is not valid anymore"
		};
	}
}

public class AuthorizeResponse<TBody> : BaseResponse<TBody> where TBody : AuthorizeResponse<TBody>.Body {
	public class Body {
		public bool Authorized { get; set; }
	}
}