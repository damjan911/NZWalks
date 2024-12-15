﻿using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public class SQLRegionRepository : IRegionRepository
	{
		private readonly NZWalksDbContext _dbContext;

		public SQLRegionRepository(NZWalksDbContext dbContext)
        {
			_dbContext = dbContext;
		}


		public async Task<List<Region>> GetAllAsync()
		{
			return await _dbContext.Regions.ToListAsync();
		}

		public async Task<Region?> GetByIdAsync(int id)
		{
			return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Region> CreateAsync(Region region)
		{
			await _dbContext.Regions.AddAsync(region);
			await _dbContext.SaveChangesAsync();
			return region;
		}

		public async Task<Region> UpdateAsync(int id, Region region)
		{
			var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

			if (existingRegion == null) 
			{
				return null;
			}
			 existingRegion.Name = region.Name;
			 existingRegion.Code = region.Code;

			await _dbContext.SaveChangesAsync();
			return existingRegion;

		}

		public async Task<Region?> DeleteAsync(int id)
		{
			var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id==id);

			if (existingRegion == null)
			{
				return null;
			}

			_dbContext.Regions.Remove(existingRegion);
			await _dbContext.SaveChangesAsync();
			return existingRegion;
		}
	}
}
