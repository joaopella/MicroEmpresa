using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using Microsoft.AspNetCore.Mvc;

namespace MicroEmpresa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfisController : ControllerBase
    {
        private readonly IPerfisLogic _logic;
        public PerfisController(IPerfisLogic logic) => _logic = logic;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var (items, total) = await _logic.ListAsync(page, pageSize, search);
            return Ok(new { total, page, pageSize, items });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _logic.GetAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ResponseMessage> Create([FromBody] PerfisEntity entity)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                responseMessage = await _logic.CreateAsync(entity);

                return responseMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PerfisEntity entity)
        {
            var (ok, error, updated) = await _logic.UpdateAsync(id, entity);
            if (!ok) return BadRequest(new { error });
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (ok, error) = await _logic.DeleteAsync(id);
            if (!ok) return NotFound(new { error });
            return NoContent();
        }
    }
}
