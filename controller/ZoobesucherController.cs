using Microsoft.AspNetCore.Mvc;
using ZooAPI.Model;
using ZooAPI.Service;

namespace ZooAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")] // Basispfad
    public class ZoobesucherController : ControllerBase
    {
        private readonly ZoobesucherService _service;

        // Service Injektion
        public ZoobesucherController(ZoobesucherService service)
        {
            _service = service;
        }

        // Alle Tiere
        [HttpGet("tiere")] // api/zoobesucher/tiere
        public async Task<ActionResult<List<Tier>>> GetAllTiere()
        {
            return await _service.GetAllTiere();
        }

        // Tier nach Gattung
        [HttpGet("tiere/{gattung}")] // api/zoobesucher/tiere/{gattung}
        public async Task<ActionResult<Tier>> GetTierByGattung(string gattung)
        {
            var tier = await _service.GetTierByGattung(gattung);
            if (tier == null)
            {
                return NotFound();
            }

            return tier;
        }
    }
}