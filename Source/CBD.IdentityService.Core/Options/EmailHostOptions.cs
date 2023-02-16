namespace CBD.IdentityService.Core.Options; 

public class EmailHostOptions {
	public const string AppSettingsKey = nameof(EmailHostOptions);

	public string AuthEmailHost { get; set; }
	public int AuthEmailHostPort { get; set; }
	public string AuthEmail { get; set; }
	public string AuthEmailPassword { get; set; }
}