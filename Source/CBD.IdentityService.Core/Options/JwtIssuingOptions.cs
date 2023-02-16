namespace CBD.IdentityService.Core.Options; 

public class JwtIssuingOptions {
	public const string AppSettingsKey = nameof(JwtIssuingOptions);
	
	public string Secret { get; set; }
	public TimeSpan ExpiringTime { get; set; }
}