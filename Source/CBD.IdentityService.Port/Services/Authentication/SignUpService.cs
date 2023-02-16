using CBD.IdentityService.Core.Contracts.Requests.Authentication;
using CBD.IdentityService.Core.Contracts.Responses.Authentication;
using CBD.IdentityService.Core.Services.Authentication;
using CBD.IdentityService.Core.ValueObjects;
using CBD.IdentityService.Port.Database;

using Microsoft.AspNetCore.Identity;

namespace CBD.IdentityService.Port.Services.Authentication; 

public class SignUpService : ISignUpService {
	private readonly ApplicationDbContext _dbContext;
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public SignUpService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
		this._dbContext = dbContext;
		this._userManager = userManager;
		this._roleManager = roleManager;
	}
	
	public async Task<SignUpResponse> SignUpUserAsync(SignUpRequest request) {
		var user = new IdentityUser() {
			UserName = request.Username,
			Email = request.Email
		};

		var creationResult = await this._userManager.CreateAsync(user, request.Password);
	
		if (!creationResult.Succeeded) {
			return new SignUpResponse {
				Errors = creationResult.Errors.Select(error => new Info() {
					Code = error.Code,
					Description = error.Description
				})
			};
		}
		
		return new SignUpResponse {
			Succeeded = true,
			Messages = new[] {SignUpResponse.Message.SignedUp}
		};
	}
}