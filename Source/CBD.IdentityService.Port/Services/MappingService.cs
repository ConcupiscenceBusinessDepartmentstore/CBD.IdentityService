using CBD.IdentityService.Core.DTO;
using CBD.IdentityService.Core.Services;

using Mapster;

using Microsoft.AspNetCore.Identity;

namespace CBD.IdentityService.Port.Services; 

public class MappingService : IMappingService {
	private TypeAdapterConfig _typeAdapterConfig;
	
	public MappingService() {
		this._typeAdapterConfig = new TypeAdapterConfig();
		this._typeAdapterConfig.NewConfig<IdentityUser, UserDTO>()
			.Map(dest => dest.Username, src => src.UserName);
	}
	
	public TTo? MapTo<TTo>(object? obj) {
		return obj is null ? default : obj.Adapt<TTo>(this._typeAdapterConfig);
	}
}