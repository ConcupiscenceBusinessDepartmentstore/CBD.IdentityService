namespace CBD.IdentityService.Core.Services; 

public interface IMappingService {
	TTo? MapTo<TTo>(object obj);
}