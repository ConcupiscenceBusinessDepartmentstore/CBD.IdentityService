using System.IdentityModel.Tokens.Jwt;

using CBD.IdentityService.Core.Contracts.Requests.Authorization;
using CBD.IdentityService.Core.Contracts.Responses.Authorization;
using CBD.IdentityService.Core.Options;
using CBD.IdentityService.Core.Services;
using CBD.IdentityService.Core.Services.Authentication;
using CBD.IdentityService.Core.Services.Authorization;
using CBD.IdentityService.Core.ValueObjects;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CBD.IdentityService.Port.Services.Authorization; 

public class GlobalAuthorizationService : IGlobalAuthorizationService {
	private readonly UserManager<IdentityUser> _userManager;
	private readonly ILoginService<IdentityUser> _loginService;
	private readonly IJwtIssuingService _jwtIssuingService;
	private readonly ILogger<GlobalAuthorizationService> _logger;

	public GlobalAuthorizationService(UserManager<IdentityUser> userManager, ILoginService<IdentityUser> loginService, IJwtIssuingService jwtIssuingService, ILogger<GlobalAuthorizationService> logger) {
		this._userManager = userManager;
		this._loginService = loginService;
		this._jwtIssuingService = jwtIssuingService;
		this._logger = logger;
	}

	public async Task<AuthorizeResponse> AuthorizeUserAsync(AuthorizeRequest request) {
		if (string.IsNullOrWhiteSpace(request.Token))
			return new AuthorizeResponse() {Errors = new[] {AuthorizeResponse.Error.InvalidToken}};

		try {
			this._jwtIssuingService.ValidateToken(request.Token);
			
			var claims = this._jwtIssuingService.GetClaimsFromToken(request.Token);
			var sub = claims?.FirstOrDefault(claim => claim.Type.Equals(JwtRegisteredClaimNames.Sub))?.Value;
			
			if (sub is null)
				return new AuthorizeResponse() {Errors = new[] {AuthorizeResponse.Error.InvalidToken}};

			var user = await this._userManager.FindByIdAsync(sub);
			
			if (user is null)
				return new AuthorizeResponse() {Errors = new[] {AuthorizeResponse.Error.InvalidToken}};
			
			return new AuthorizeResponse() {
				Succeeded = true,
				Messages = new[] {AuthorizeResponse.Message.Authorized},
				Content = new AuthorizeResponse.Body() {
					Authorized = true,
				}
			};
		} catch (Exception e) {
			this._logger.LogError(e, $"{nameof(this.AuthorizeUserAsync)} threw an exception");
			
			return new AuthorizeResponse() {
				Succeeded = true,
				Messages = new[] {AuthorizeResponse.Message.Unauthorized},
				Errors = new[] {
					e is SecurityTokenExpiredException
						? AuthorizeResponse.Error.ExpiredToken
						: AuthorizeResponse.Error.InvalidToken
				}
			};
		}
	}

	public async Task<AuthorizeGlobalRoleResponse> AuthorizeUserGlobalRoleAsync(AuthorizeGlobalRoleRequest request) {
		var authTokenResponse = await this.AuthorizeUserAsync(request);

		if (authTokenResponse.Content is null || !authTokenResponse.Content.Authorized) {
			return new AuthorizeGlobalRoleResponse() {
				Succeeded = authTokenResponse.Succeeded,
				
				Messages = authTokenResponse.Messages,
				Errors = authTokenResponse.Errors
			};
		}

		var user = await this._loginService.GetUserForTokenAsync(request.Token);

		if (user is null) {
			return new AuthorizeGlobalRoleResponse() {
				Succeeded = true,
				Messages = new[] {AuthorizeResponse.Message.Unauthorized},
				Errors = new[] {AuthorizeGlobalRoleResponse.Error.UserNotFound}
			};
		}

		var isInRole = await this._userManager.IsInRoleAsync(user, request.GlobalRole);

		return new AuthorizeGlobalRoleResponse() {
			Succeeded = true,
			Messages = new[] {isInRole ? AuthorizeResponse.Message.Authorized : AuthorizeResponse.Message.Unauthorized},
			Errors = isInRole ? Enumerable.Empty<Info>() : new[] {AuthorizeGlobalRoleResponse.Error.UserNotInRole},
			Content = new AuthorizeGlobalRoleResponse.Body() {
				Authorized = isInRole,
			}
		};
	}
}