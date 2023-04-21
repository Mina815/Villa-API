using System.ComponentModel.DataAnnotations;

namespace Villa_API.Models.Dto
{
    public class VillaDTO
    {
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
		public int Sqft { get; set; }
		public int occupency { get; set; }

	}
}
