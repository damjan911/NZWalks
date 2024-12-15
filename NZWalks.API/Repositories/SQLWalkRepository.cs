﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public class SQLWalkRepository : IWalkRepository
	{
		private readonly NZWalksDbContext _nZWalksDbContext;

		public SQLWalkRepository(NZWalksDbContext nZWalksDbContext)
        {
			_nZWalksDbContext = nZWalksDbContext;
		}
        public async Task<Walk> CreateAsync(Walk walk)
		{
			await _nZWalksDbContext.Walks.AddAsync(walk);
			await _nZWalksDbContext.SaveChangesAsync();
			return walk;
		}


		public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
			string? sortBy = null,bool? isAscending = true, int pageNumber = 1, int pageSize = 1000)
		{
			
			var walks = _nZWalksDbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
			
			// Filtering
			if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
			{
				if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = walks.Where(x => x.Name.Contains(filterQuery));
				}
			}

			// Sorting
			if(string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					walks = (bool)isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
				}
			}

			// Pagination

			var skipResults = (pageNumber - 1) * pageSize;

				return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
		}

		public async Task<Walk?> GetByIdAsync(int id)
		{
			return await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Walk> UpdateAsync(int id, Walk walk)
		{
			var existingWalk = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

			if (existingWalk == null) 
			{
				return null;
			}

			existingWalk.Name = walk.Name;
			existingWalk.Description = walk.Description;
			existingWalk.LengthInKm = walk.LengthInKm;
			existingWalk.DifficultyId = walk.DifficultyId;
			existingWalk.RegionId = walk.RegionId;

			await _nZWalksDbContext.SaveChangesAsync();
			return existingWalk;

		}

		public async Task<Walk?> DeleteAsync(int id)
		{
			var existingWalk = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

			if (existingWalk == null)
			{
				return null;
			}

			_nZWalksDbContext.Walks.Remove(existingWalk);
			await _nZWalksDbContext.SaveChangesAsync();
			return existingWalk;

		}

	}
}
