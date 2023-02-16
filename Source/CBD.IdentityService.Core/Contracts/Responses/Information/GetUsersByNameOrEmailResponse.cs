using CBD.IdentityService.Core.DTO;
using CBD.IdentityService.Core.ValueObjects;

namespace CBD.IdentityService.Core.Contracts.Responses.Information; 

public class GetUsersByNameOrEmailResponse : BaseResponse<GetUsersByNameOrEmailResponse.Body> {
	public class Body {
		public IEnumerable<UserDTO> Users { get; set; }
	}

	public static class Message {
		public static readonly Info SuitableUsersReturned = new Info {
			Code = nameof(SuitableUsersReturned),
			Description = "All suitable user for the specified query where returned successfully"
		};
	}
}