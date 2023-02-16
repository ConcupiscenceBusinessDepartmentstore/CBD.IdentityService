using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CBD.IdentityService.Port.Extensions; 

public static class DbContextExtensions {
	/// <summary>
	/// Gets the tracked entity reference for an object with the same primary key values.
	/// </summary>
	public static async Task<TEntity?> FindTrackedAsync<TEntity>(this DbContext context, TEntity entity) where TEntity : class {
		// get the EF Core model type definition for TEntity, which contains all the model information including keys, unique constraints, ...
		var entityType = context.Model.FindRuntimeEntityType(typeof(TEntity));
		
		// Get the collection of properties (columns) which define the primary or composite primary key
		var keyProperties = entityType?.FindPrimaryKey()?.Properties;

		// If the entity has not primary or composite primary key, this methode cannot continue
		if (keyProperties is null)
			return null;

		// get all values from the primary key properties of the entity instance
		var keyValues = keyProperties.Select(prop => prop.GetGetter().GetClrValue(entity)).ToArray();
		
		// Look for existing tracked entity or execute db query to look for an entry with the given primary key values.
		return await context.FindAsync<TEntity>(keyValues);
	}
}