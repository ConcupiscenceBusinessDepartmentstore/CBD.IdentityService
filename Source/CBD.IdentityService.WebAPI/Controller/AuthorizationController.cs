using CBD.IdentityService.Core.Contracts.Requests.Authorization;
using CBD.IdentityService.Core.Contracts.Responses.Authentication;
using CBD.IdentityService.Core.Contracts.Responses.Authorization;
using CBD.IdentityService.Core.Services.Authorization;
using CBD.IdentityService.WebAPI.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace CBD.IdentityService.WebAPI.Controller; 

[ApiController]
[Route("authorize")]
public class AuthorizationController : ControllerBase {
	private readonly IGlobalAuthorizationService _globalAuthorizationService;
	private readonly ILogger<AuthorizationController> _logger;

	public AuthorizationController(IGlobalAuthorizationService globalAuthorizationService, ILogger<AuthorizationController> logger) {
		this._globalAuthorizationService = globalAuthorizationService;
		this._logger = logger;
	}

	[HttpPost]
	[Route("user")]
	[ProducesResponseType(typeof(AuthorizeResponse), 200)]
	public async Task<IActionResult> AuthorizeUser([FromBody] AuthorizeRequest request) {
		if (!this.HasValidModelState(out AuthorizeResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this._globalAuthorizationService.AuthorizeUserAsync(request));
		}
		catch (Exception e) {
			this._logger.LogError(e, $"{nameof(this.AuthorizeUser)} threw an exception");
			return this.InternalServerError<AuthorizeResponse>(e);
		}
	}
	[HttpPost]
	[Route("role")]
	[ProducesResponseType(typeof(AuthorizeGlobalRoleResponse), 200)]
	public async Task<IActionResult> AuthorizeUserGlobalRoleAsync([FromBody] AuthorizeGlobalRoleRequest request) {
		if (!this.HasValidModelState(out AuthorizeGlobalRoleResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this._globalAuthorizationService.AuthorizeUserGlobalRoleAsync(request));
		}
		catch (Exception e) {
			this._logger.LogError(e, $"{nameof(this.AuthorizeUserGlobalRoleAsync)} threw an exception");
			return this.InternalServerError<AuthorizeGlobalRoleResponse>(e);
		}
	}
}