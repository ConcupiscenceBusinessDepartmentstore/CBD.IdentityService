using CBD.IdentityService.Core.Contracts.Requests.Authorization;
using CBD.IdentityService.Core.Contracts.Responses.Authentication;
using CBD.IdentityService.Core.Contracts.Responses.Authorization;
using CBD.IdentityService.Core.Services.Authorization;
using CBD.IdentityService.Port.Database;
using CBD.IdentityService.WebAPI.Config;
using CBD.IdentityService.WebAPI.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace CBD.IdentityService.WebAPI.Controller; 

[ApiController]
[Route("authorize")]
public class AuthorizationController : ControllerBase {
	private readonly IGlobalAuthorizationService serviceGlobalAuthorization;
	private readonly ILogger<AuthorizationController> logController;
	private readonly ApplicationDbContext ctxApplication; 
	private readonly IMessageProducer producerMessagePublisher;
	public AuthorizationController(IGlobalAuthorizationService serviceGlobalAuthorization, ILogger<AuthorizationController> logController, ApplicationDbContext ctxApplication, IMessageProducer producerMessagePublisher) {
		this.serviceGlobalAuthorization = serviceGlobalAuthorization;
		this.logController = logController;
		this.ctxApplication = ctxApplication;
		this.producerMessagePublisher = producerMessagePublisher;
	}

	[HttpPost]
	[Route("user")]
	[ProducesResponseType(typeof(AuthorizeResponse), 200)]
	public async Task<IActionResult> AuthorizeUser([FromBody] AuthorizeRequest request) {
		if (!this.HasValidModelState(out AuthorizeResponse? response))
		{
			return this.BadRequest(response);
		}
		
		try {
			await this.ctxApplication.SaveChangesAsync();
			AuthorizeResponse responseAuthorize = await this.serviceGlobalAuthorization.AuthorizeUserAsync(request);
			this.producerMessagePublisher.SendMessage(responseAuthorize);
			return this.Ok(responseAuthorize);
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.AuthorizeUser)} threw an exception");
			return this.InternalServerError<AuthorizeResponse>(e);
		}
	}
	[HttpPost]
	[Route("role")]
	[ProducesResponseType(typeof(AuthorizeGlobalRoleResponse), 200)]
	public async Task<IActionResult> AuthorizeUserGlobalRoleAsync([FromBody] AuthorizeGlobalRoleRequest request) {
		if (!this.HasValidModelState(out AuthorizeGlobalRoleResponse? response))
		{
			return this.BadRequest(response);
		}
		
		try {
			await this.ctxApplication.SaveChangesAsync();
			AuthorizeGlobalRoleResponse responseAuthorizeGlobalRole = await this.serviceGlobalAuthorization.AuthorizeUserGlobalRoleAsync(request);
			this.producerMessagePublisher.SendMessage(responseAuthorizeGlobalRole);
			return this.Ok(responseAuthorizeGlobalRole);
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.AuthorizeUserGlobalRoleAsync)} threw an exception");
			return this.InternalServerError<AuthorizeGlobalRoleResponse>(e);
		}
	}
}