using CBD.IdentityService.Core.Contracts.Requests.Information;
using CBD.IdentityService.Core.Contracts.Responses.Information;
using CBD.IdentityService.Core.Services.Information;
using CBD.IdentityService.Port.Database;
using CBD.IdentityService.WebAPI.Config;
using CBD.IdentityService.WebAPI.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace CBD.IdentityService.WebAPI.Controller; 

[ApiController]
[Route("information/authentication")]
public class AuthenticationInformationController : ControllerBase {
	private readonly IAuthenticationInformationService serviceAuthenticationInformation;
	private readonly ILogger<AuthenticationInformationController> logController;
	private readonly ApplicationDbContext ctxApplication; 
	private readonly IMessageProducer producerMessagePublisher;

	public AuthenticationInformationController(IAuthenticationInformationService serviceAuthenticationInformation, ILogger<AuthenticationInformationController> logController, ApplicationDbContext ctxApplication, IMessageProducer producerMessagePublisher) {
		this.serviceAuthenticationInformation = serviceAuthenticationInformation;
		this.logController = logController;
		this.ctxApplication = ctxApplication;
		this.producerMessagePublisher = producerMessagePublisher;
	}
	
	[HttpGet]
	[Route("user")]
	[ProducesResponseType(typeof(GetUserByIdResponse), 200)]
	public async Task<IActionResult> GetUserByIdAsync([FromQuery] GetUserByIdRequest request) {
		if (!this.HasValidModelState(out GetUserByIdResponse? response))
		{
			return this.BadRequest(response);
		}
		
		try {
			await this.ctxApplication.SaveChangesAsync();
			GetUserByIdResponse responseGetUserById = await this.serviceAuthenticationInformation.GetUserByIdAsync(request);
			this.producerMessagePublisher.SendMessage(responseGetUserById);
			return this.Ok(responseGetUserById);
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
		{
			return this.BadRequest(response);
		}
		
		try {
			await this.ctxApplication.SaveChangesAsync();
			GetUsersByNameOrEmailResponse responseGetUsersByNameOrEmail = await this.serviceAuthenticationInformation.GetUsersByNameOrEmailAsync(request);
			this.producerMessagePublisher.SendMessage(responseGetUsersByNameOrEmail);
			return this.Ok(responseGetUsersByNameOrEmail);
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.GetUsersByNameOrEmailAsync)} threw an exception");
			return this.InternalServerError<GetUsersByNameOrEmailResponse>(e);
		}
	}
}