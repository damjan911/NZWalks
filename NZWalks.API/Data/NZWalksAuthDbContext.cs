using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
	public class NZWalksAuthDbContext : IdentityDbContext
	{
		public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelbuilder)
		{
			base.OnModelCreating(modelbuilder);

			var readerRoleId = "1";
			var writerRoleId = "2";

			var roles = new List<IdentityRole>()
			{
				new IdentityRole()
				{
					Id = readerRoleId,
					ConcurrencyStamp = readerRoleId,
					Name = "Reader",
					NormalizedName = "Reader".ToUpper()

				},
				new IdentityRole()
				{
					Id = writerRoleId,
					ConcurrencyStamp = writerRoleId,
					Name = "Writer",
					NormalizedName = "Writer".ToUpper()

				}
			};

			modelbuilder.Entity<IdentityRole>().HasData(roles);
		}
	}
}
