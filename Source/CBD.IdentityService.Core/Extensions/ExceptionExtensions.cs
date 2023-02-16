namespace CBD.IdentityService.Core.Extensions; 

public static class ExceptionExtensions {
	/// <summary>
	/// Gets an yield-enumerable containing all exceptions present in the exception-hierarchy (child/inner exceptions).
	/// The enumerable will include the root exception itself.
	/// </summary>
	public static IEnumerable<Exception> GetExceptionHierarchy(this Exception exception) {
		var ex = exception;

		// Add root exception to the enumerable
		yield return ex;
		
		// add all inner exceptions and their inner exception until not defined.
		while (ex.InnerException is not null)
			yield return ex = ex.InnerException;
	}
}