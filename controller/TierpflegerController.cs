using Microsoft.AspNetCore.Mvc;
using ZooAPI.Model;
using ZooAPI.Service;

namespace ZooAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")] // Basispfad für den Controller
    public class TierpflegerController : ControllerBase
    {
        private readonly TierpflegerService _service;

        // Konstruktor: Service-Injektion
        public TierpflegerController(TierpflegerService service)
        {
            _service = service;
        }

        // Endpoint: Tiere nach Pfleger-ID abrufen
        [HttpGet("{pflegerId}/tiere")]
        public async Task<ActionResult<List<Tier>>> GetTiereByPflegerIdAsync(int pflegerId)
        {
            try
            {
                return await _service.GetTiereByPflegerIdAsync(pflegerId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Endpoint: Tier aktualisieren
        [HttpPut("tiere/{id}/{column}")]
        public async Task<IActionResult> UpdateTier(int id, Tier column)
        {
            try
            {
                await _service.UpdateTierAsync(id, column);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}