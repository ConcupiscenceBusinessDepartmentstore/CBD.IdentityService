using CBD.IdentityService.Core.DTO;
using CBD.IdentityService.Core.ValueObjects;

namespace CBD.IdentityService.Core.Contracts.Responses.Information;

public class GetUserByIdResponse : BaseResponse<GetUserByIdResponse.Body> {
	public class Body {
		public UserDTO User { get; set; }
	}

	public static class Message {
		public static readonly Info UserReturnedSuccessfully = new Info {
			Code = nameof(UserReturnedSuccessfully),
			Description = "User was returned successfully"
		};
	}
	
	public static class Error {
		public static readonly Info UserNotFound = new Info {
			Code = nameof(UserNotFound),
			Description = "The id does not belong to an existing user"
		};
	}
}