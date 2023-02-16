using CBD.IdentityService.Core.Contracts.Responses;
using CBD.IdentityService.Core.Extensions;
using CBD.IdentityService.Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CBD.IdentityService.WebAPI.Extensions;

public static class ControllerBaseExtensions {
	public static bool HasValidModelState<TResponse>(this ControllerBase controller, out TResponse? response) where TResponse : BaseResponse {
		if (controller.ModelState.IsValid) {
			response = null;
			return true;
		}

		response = Activator.CreateInstance<TResponse>();
		response.Errors = controller.ModelState.Values.SelectMany(value => value.Errors).Select(error => new Info {
			Code = error.Exception?.GetType().Name ?? "",
			Description = error.ErrorMessage
		});

		return false;
	}

	public static IActionResult InternalServerError<TResponse>(this ControllerBase controller, Exception exception) where TResponse : BaseResponse {
		var response = Activator.CreateInstance<TResponse>();
		response.Errors = exception.GetExceptionHierarchy().Select(ex => new Info {
			Code = ex.GetType().Name,
			Description = ex.Message
		});
		
		return controller.StatusCode(500, response);
	}
}