﻿namespace NZWalks.API.Models.DTO
{
	public class WalkDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public double LengthInKm { get; set; }

		public int DifficultyId { get; set; }

		public int RegionId { get; set; }
	}
}
