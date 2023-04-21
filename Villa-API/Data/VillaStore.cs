using Villa_API.Models.Dto;

namespace Villa_API.Data
{
	public class VillaStore
	{
		public static List<VillaDTO> VillaList = new List<VillaDTO>()
		{
			new VillaDTO { Id = 1, Name = "Villa1", Sqft = 200, occupency = 3 },
			new VillaDTO { Id = 2, Name = "Villa2", Sqft = 300, occupency = 4 },
		};
	}
}
