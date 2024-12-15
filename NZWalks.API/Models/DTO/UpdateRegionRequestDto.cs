using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
	public class UpdateRegionRequestDto
	{
		[Required]
		[MinLength(3, ErrorMessage = "Code has to be minimum of 3 Characters!")]
		[MaxLength(3, ErrorMessage = "Code has to be maximum of 3 Characters!")]
		public string Code { get; set; }

		[Required]
		[MaxLength(100, ErrorMessage = "Name has to be maximum of 100 Characters!")]
		public string Name { get; set; }
	}
}
