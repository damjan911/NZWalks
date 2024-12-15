using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
	public class NZWalksDbContext : DbContext
	{
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions) 
        {
            
        }


        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Seed data for Difficulties
			// Easy,Medium and Hard.

			var difficulties = new List<Difficulty>()
			{
				new Difficulty()
				{
					Id = 1,
					Name = "Easy"
				},
				new Difficulty()
				{
					Id = 2,
					Name = "Medium"
				},
				new Difficulty()
				{
					Id = 3,
					Name = "Hard"
				}
			};

			// Seed difficulties to the database.
			modelBuilder.Entity<Difficulty>().HasData(difficulties);

			// Seed data for Regions.


			var regions = new List<Region>()
			{
				new Region()
				{
					Id= 1,
					Name = "Auckland",
					Code = "AKL"
				},
				new Region()
				{
					Id= 2,
					Name = "Northland",
					Code = "NTL"
				},
				new Region()
				{
					Id= 3,
					Name = "Bay of Plenty",
					Code = "BOP"
				},
				new Region()
				{
					Id= 4,
					Name = "Wellington",
					Code = "WGN"
				},
			};

			modelBuilder.Entity<Region>().HasData(regions);

		}

	}
}
