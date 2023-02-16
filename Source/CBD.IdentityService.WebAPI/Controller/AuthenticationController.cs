using CBD.IdentityService.Core.Contracts.Requests.Authentication;
using CBD.IdentityService.Core.Contracts.Responses.Authentication;
using CBD.IdentityService.Core.Services.Authentication;
using CBD.IdentityService.WebAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CBD.IdentityService.WebAPI.Controller; 

[ApiController]
[Route("authentication")]
public class AuthenticationController : ControllerBase {
	private readonly ILogger<AuthenticationController> logController;
	private readonly ISignUpService serviceSignUp;
	private readonly ILoginService<IdentityUser> serviceLogin;
	private readonly IPasswordResetService servicePasswordReset;

	public AuthenticationController(ILogger<AuthenticationController> logController, ISignUpService serviceSignUp, ILoginService<IdentityUser> serviceLogin, IPasswordResetService servicePasswordReset) {
		this.logController = logController;
		this.serviceSignUp = serviceSignUp;
		this.serviceLogin = serviceLogin;
		this.servicePasswordReset = servicePasswordReset;
	}
	
	[HttpPost]
	[Route("signup")]
	[ProducesResponseType(typeof(SignUpResponse), 200)]
	
	public async Task<IActionResult> SignUpUserAsync([FromBody] SignUpRequest request) {
		if (!this.HasValidModelState(out SignUpResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this.serviceSignUp.SignUpUserAsync(request));
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.SignUpUserAsync)} threw an exception");
			return this.InternalServerError<SignUpResponse>(e);
		}
	}

	[HttpPost]
	[Route("login")]
	[ProducesResponseType(typeof(LoginResponse), 200)]
	public async Task<IActionResult> LoginUserAsync([FromBody] LoginRequest request) {
		if (!this.HasValidModelState(out LoginResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this.serviceLogin.LoginUserAsync(request));
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.LoginUserAsync)} threw an exception");
			return this.InternalServerError<LoginResponse>(e);
		}
	}
	
	[HttpPost]
	[Route("requestresetpw")]
	[ProducesResponseType(typeof(RequestChangePasswordResponse), 200)]
	public async Task<IActionResult> RequestResetPasswordAsync([FromBody] RequestChangePasswordRequest request) {
		if (!this.HasValidModelState(out RequestChangePasswordResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this.servicePasswordReset.RequestChangePasswordTokenAsync(request));
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.RequestResetPasswordAsync)} threw an exception");
			return this.InternalServerError<RequestChangePasswordResponse>(e);
		}
	}
	
	[HttpPost]
	[Route("resetpw")]
	[ProducesResponseType(typeof(ChangePasswordResponse), 200)]
	public async Task<IActionResult> ResetPasswordAsync([FromBody] ChangePasswordRequest request) {
		if (!this.HasValidModelState(out ChangePasswordResponse? response))
			return this.BadRequest(response);
		
		try {
			return this.Ok(await this.servicePasswordReset.ChangePasswordAsync(request));
		}
		catch (Exception e) {
			this.logController.LogError(e, $"{nameof(this.LoginUserAsync)} threw an exception");
			return this.InternalServerError<ChangePasswordResponse>(e);
		}
	}
}