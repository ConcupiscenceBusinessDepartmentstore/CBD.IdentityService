using CBD.IdentityService.Core.Contracts.Requests.Information;
using CBD.IdentityService.Core.Contracts.Responses.Information;
using CBD.IdentityService.Core.DTO;
using CBD.IdentityService.Core.Services;
using CBD.IdentityService.Core.Services.Information;
using CBD.IdentityService.Core.ValueObjects;
using CBD.IdentityService.Port.Database;

using Microsoft.EntityFrameworkCore;

namespace CBD.IdentityService.Port.Services.Information; 

public class AuthenticationInformationService : IAuthenticationInformationService {
	private readonly ApplicationDbContext _applicationDbContext;
	private readonly IMappingService _mappingService;

	public AuthenticationInformationService(ApplicationDbContext applicationDbContext, IMappingService mappingService) {
		this._applicationDbContext = applicationDbContext;
		this._mappingService = mappingService;
	}

	public async Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest request) {
		var user = await this._applicationDbContext.Users.FindAsync(request.UserId);

		return new GetUserByIdResponse {
			Succeeded = user is not null,
			Messages = user is not null
				? new[] {GetUserByIdResponse.Message.UserReturnedSuccessfully}
				: Enumerable.Empty<Info>(),
			Errors = user is not null ? Enumerable.Empty<Info>() : new[] {GetUserByIdResponse.Error.UserNotFound},
			Content = user is not null
				? new GetUserByIdResponse.Body {User = this._mappingService.MapTo<UserDTO>(user)!}
				: null
		};
	}

	public async Task<GetUsersByNameOrEmailResponse> GetUsersByNameOrEmailAsync(GetUsersByNameOrEmailRequest request) {
		if (string.IsNullOrWhiteSpace(request.UsernameEmail)) {
			return new GetUsersByNameOrEmailResponse {
				Succeeded = true,
				Messages = new[] {GetUsersByNameOrEmailResponse.Message.SuitableUsersReturned},
				Content = new GetUsersByNameOrEmailResponse.Body {
					Users = Enumerable.Empty<UserDTO>()
				}
			};
		}

		var users = await this._applicationDbContext.Users
			.Where(user => user.Email.ToLower().Contains(request.UsernameEmail.ToLower()) ||
			               user.UserName.ToLower().Contains(request.UsernameEmail.ToLower()))
			.Select(user => this._mappingService.MapTo<UserDTO>(user)!)
			.ToArrayAsync();

		return new GetUsersByNameOrEmailResponse {
			Succeeded = true,
			Messages = new[] {GetUsersByNameOrEmailResponse.Message.SuitableUsersReturned},
			Content = new GetUsersByNameOrEmailResponse.Body {
				Users = users
			}
		};
	}
}