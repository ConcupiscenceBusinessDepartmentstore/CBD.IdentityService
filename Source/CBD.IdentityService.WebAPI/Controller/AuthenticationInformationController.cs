using CBD.IdentityService.Core.Contracts.Requests.Information;
using CBD.IdentityService.Core.Contracts.Responses.Information;
using CBD.IdentityService.Core.Services.Information;
using CBD.IdentityService.WebAPI.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace CBD.IdentityService.WebAPI.Controller; 

[ApiController]
[Route("information/authentication")]
public class AuthenticationInformationController : ControllerBase {
	private readonly IAuthenticationInformationService serviceAuthenticationInformation;
	private readonly ILogger<AuthenticationInformationController> logController;

	public AuthenticationInformationController(IAuthenticationInformationService serviceAuthenticationInformation, ILogger<AuthenticationInformationController> logController) {
		this.serviceAuthenticationInformation = serviceAuthenticationInformation;
		this.logController = logController;
	}
	
	[HttpGet]
	[Route("user")]
	[ProducesResponseType(typeof(GetUserByIdResponse), 200)]
	public async Task<IActionResult> GetUserByIdAsync([FromQuery] GetUserByIdRequest request) {
		if (!this.HasValidModelState(out GetUserByIdResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this.serviceAuthenticationInformation.GetUserByIdAsync(request));
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.GetUserByIdAsync)} threw an exception");
			return this.InternalServerError<GetUserByIdResponse>(e);
		}
	}
	
	[HttpGet]
	[Route("users")]
	[ProducesResponseType(typeof(GetUsersByNameOrEmailResponse), 200)]
	public async Task<IActionResult> GetUsersByNameOrEmailAsync([FromQuery] GetUsersByNameOrEmailRequest request) {
		if (!this.HasValidModelState(out GetUsersByNameOrEmailResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this.serviceAuthenticationInformation.GetUsersByNameOrEmailAsync(request));
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.GetUsersByNameOrEmailAsync)} threw an exception");
			return this.InternalServerError<GetUsersByNameOrEmailResponse>(e);
		}
	}
}