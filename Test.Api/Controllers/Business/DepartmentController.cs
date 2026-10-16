using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Core.Entities.Business.Departments.Dtos;
using Test.Core.Interfaces.Services;

namespace Test.Api.Controllers.Business
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound(new { message = "Department not found." });
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDto dto)
        {
            if (dto == null) return BadRequest(new { message = "Department is required." });
            try
            {
                var id = await _service.CreateAsync(dto);
                var created = await _service.GetByIdAsync(id);
                return CreatedAtAction(nameof(GetById), new { id }, created);
            }
            catch (Exception ex)
            {
                return MapError(ex);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentDto dto)
        {
            if (dto == null || id != dto.Id) return BadRequest(new { message = "Department id does not match." });
            try
            {
                await _service.UpdateAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return MapError(ex);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("cannot be deleted", StringComparison.Ordinal))
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return MapError(ex);
            }
        }

        private static IActionResult MapError(Exception ex)
        {
            if (ex is KeyNotFoundException)
                return new NotFoundObjectResult(new { message = ex.Message });
            if (ex is ArgumentException or InvalidOperationException)
                return new BadRequestObjectResult(new { message = ex.Message });
            return new ObjectResult(new { message = "Request failed." }) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
