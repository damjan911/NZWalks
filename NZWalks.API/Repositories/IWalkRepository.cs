using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public interface IWalkRepository
	{
		Task<List<Walk>> GetAllAsync(string? filterOn = null, string? 
			filterQuery = null,string? sortBy = null, bool? isAscending = true,int pageNumber = 1, int pageSize = 1000);

		Task<Walk?> GetByIdAsync(int id);
		Task<Walk> CreateAsync (Walk walk);

		Task<Walk> UpdateAsync(int id, Walk walk);

		Task<Walk?> DeleteAsync(int id);
	}
}
