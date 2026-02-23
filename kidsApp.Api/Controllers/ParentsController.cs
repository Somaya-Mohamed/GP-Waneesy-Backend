using kidsApp.Application.DTOs.ParentDTOs;
using kidsApp.Application.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kidsApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/parents")]
    public class ParentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ParentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/v1/parents
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parents = await _serviceManager.ParentService.GetAllAsync();
            return Ok(new { Success = true, Data = parents });
        }

        // GET: api/v1/parents/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parent = await _serviceManager.ParentService.GetByIdAsync(id);
            if (parent == null)
                return NotFound(new { Success = false });

            return Ok(new { Success = true, Data = parent });
        }

        // POST: api/v1/parents
        [HttpPost]
        public async Task<IActionResult> Create(ParentCreateDto dto)
        {
            var result = await _serviceManager.ParentService.CreateAsync(dto);
            return Ok(new { Success = true, Data = result });
        }

        // PUT: api/v1/parents/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateParentDTO dto)
        {
            var updated = await _serviceManager.ParentService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Success = false });

            return Ok(new { Success = true });
        }

        // DELETE: api/v1/parents/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _serviceManager.ParentService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Success = false });

            return Ok(new { Success = true });
        }

        // ================= Child-style Advanced =================

        // GET: api/v1/parents/5/children
        [HttpGet("{id:int}/children")]
        public async Task<IActionResult> GetChildren(int id)
        {
            var data = await _serviceManager.ParentService.GetChildrenSummaryAsync(id);
            return Ok(new { Success = true, Data = data });
        }

        // GET: api/v1/parents/5/weekly-progress
        [HttpGet("{id:int}/weekly-progress")]
        public async Task<IActionResult> GetWeeklyProgress(int id)
        {
            var data = await _serviceManager.ParentService.GetWeeklyChildReportsAsync(id);
            return Ok(new { Success = true, Data = data });
        }

        // POST: api/v1/parents/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(ParentLoginDTO dto)
        {
            var token = await _serviceManager.ParentService.LoginAsync(dto.Email, dto.Password);
            if (token == null)
                return Unauthorized(new { Success = false });

            return Ok(new { Success = true, Token = token });
        }
    }
}
