using Microsoft.AspNetCore.Mvc;
using Villa_API.Models;
using Villa_API.Models.Dto;

namespace Villa_API.Controllers
{
	[Route("Api/VillaAPI")]
	[ApiController]
	public class VillaAPIController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<VillaDTO> VillaList()
		{
			return new List<VillaDTO>() {
				new VillaDTO { Id = 1, Name = "Villa1" },
				new VillaDTO { Id = 2, Name = "Villa2" },
			};
		}
	}
}
