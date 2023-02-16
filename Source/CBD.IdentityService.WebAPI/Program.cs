using System.Reflection;
using System.Text;
using CBD.IdentityService.Core.Enums;
using CBD.IdentityService.Core.Options;
using CBD.IdentityService.Core.Services;
using CBD.IdentityService.Core.Services.Authentication;
using CBD.IdentityService.Core.Services.Authorization;
using CBD.IdentityService.Core.Services.Information;
using CBD.IdentityService.Port.Database;
using CBD.IdentityService.Port.Services;
using CBD.IdentityService.Port.Services.Authentication;
using CBD.IdentityService.Port.Services.Authorization;
using CBD.IdentityService.Port.Services.Information;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CBD.IdentityService.WebAPI; 

public static class Program {
	public static async Task Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);
		Program.ConfigureServices(builder);
		var app = builder.Build();
		Program.ConfigurePipeline(app);

		using (var scope = app.Services.CreateScope()) {
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			await context.Database.EnsureCreatedAsync();
		}
		using (var scope = app.Services.CreateScope())
			await InsertUsersForTestsAsync(scope.ServiceProvider);
		await app.RunAsync();
	}

	private static void ConfigureServices(WebApplicationBuilder builder) {
		builder.Configuration.AddEnvironmentVariables();

		Program.ConfigureOptions(builder, out var jwtIssuingOptions);
		Program.ConfigureControllers(builder);
		Program.ConfigureScopedServices(builder);
		Program.ConfigureEntityFramework(builder);
		Program.ConfigureIdentity(builder);
		Program.ConfigureJwtAuthentication(builder, jwtIssuingOptions);
		
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(swaggerOptions => {
				swaggerOptions.SwaggerDoc("v1", new OpenApiInfo {
					Title = "Identity-Service",
					Version = "v1",
					Description =
						"IdentityService",
					Contact = new OpenApiContact {
						Name = "Dustin Eikmeier",
						Email = "s0569494@htw-berlin.de",
						Url = new Uri("https://github.com/orgs/ConcupiscenceBusinessDepartmentstore/CBD.IdentityService")
					}
				});
				swaggerOptions.CustomSchemaIds(type => type.FullName);
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				swaggerOptions.IncludeXmlComments(xmlPath);
			}
			);
	}
	
	private static void ConfigureOptions(WebApplicationBuilder builder, out JwtIssuingOptions jwtIssuingOptions) {
		builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
		
		jwtIssuingOptions = new JwtIssuingOptions();
		builder.Configuration.Bind(JwtIssuingOptions.AppSettingsKey, jwtIssuingOptions);
		builder.Services.Configure<JwtIssuingOptions>(builder.Configuration.GetSection(JwtIssuingOptions.AppSettingsKey));
		
		builder.Services.Configure<EmailHostOptions>(builder.Configuration.GetSection(EmailHostOptions.AppSettingsKey));
	}

	private static void ConfigureControllers(WebApplicationBuilder builder) {
		builder.Services.AddControllers();
	}
	
	private static void ConfigureScopedServices(WebApplicationBuilder builder) {
		builder.Services.AddScoped<IJwtIssuingService, JwtIssuingService>();
		builder.Services.AddScoped<IEmailService, EmailService>();
		builder.Services.AddScoped<IMappingService, MappingService>();
		builder.Services.AddScoped<ISignUpService, SignUpService>();
		builder.Services.AddScoped<ILoginService<IdentityUser>, LoginService>();
		builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
		builder.Services.AddScoped<IGlobalAuthorizationService, GlobalAuthorizationService>();
		builder.Services.AddScoped<IAuthenticationInformationService, AuthenticationInformationService>();
	}

	private static void ConfigureEntityFramework(WebApplicationBuilder builder) {
		builder.Services.AddDbContext<ApplicationDbContext>(
			(serviceProvider, optionsBuilder) => optionsBuilder
				.UseNpgsql(
					builder.Configuration.GetConnectionString("postgres")
						.Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
						.Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"))
						.Replace("$POSTGRES_DB", Environment.GetEnvironmentVariable("POSTGRES_DB")),
					optionsBuilder => {
						optionsBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
					})
				//.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>())
		);
	}

	private static void ConfigureIdentity(WebApplicationBuilder builder)
	{
		builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+"; // removed @ so username cannot be an email
			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();
	}
	
	private static void ConfigureJwtAuthentication(WebApplicationBuilder builder, JwtIssuingOptions jwtIssuingOptions) {
		builder.Services.AddAuthentication(
			options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}
		).AddJwtBearer(
			options => {
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters {
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtIssuingOptions.Secret)),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true
				};
			}
		);
	}

	private static void ConfigurePipeline(WebApplication app) {
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();
	}

	private static async Task InsertUsersForTestsAsync(IServiceProvider dependencyInjectionServiceProvider)
	{
		var userManager = dependencyInjectionServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
		var roleManager = dependencyInjectionServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		IdentityUser adminUser = new()
		{
			UserName = "admin",
			Email = "admin@mail.com",
		};
		IdentityUser traderUser = new()
		{
			UserName = "trader",
			Email = "trader@mail.com",
		};
		IdentityUser consumerUser = new()
		{
			UserName = "consumer",
			Email = "consumer@mail.com",
		};

		var c = await userManager.CreateAsync(adminUser, "Admin-pw2023");
		await userManager.CreateAsync(traderUser, "Trader-pw2023");
		await userManager.CreateAsync(consumerUser, "Consumer-pw2023");

		await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Administrator)));
		await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Trader)));
		await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Consumer)));
		
		await userManager.AddToRoleAsync(adminUser, nameof(Roles.Administrator));
		await userManager.AddToRoleAsync(traderUser, nameof(Roles.Trader));
		await userManager.AddToRoleAsync(consumerUser, nameof(Roles.Consumer));
	}
}