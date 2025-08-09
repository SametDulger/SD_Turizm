using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SD_Turizm.Application.Services;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.API.Controllers.V2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILoggingService _loggingService;

        public RoleController(IRoleService roleService, ILoggingService loggingService)
        {
            _roleService = roleService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAllAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all roles", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetById(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                if (role == null)
                    return NotFound();

                return Ok(role);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting role with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<Role>> GetByName(string name)
        {
            try
            {
                var role = await _roleService.GetByNameAsync(name);
                if (role == null)
                    return NotFound();

                return Ok(role);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting role with name {name}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Role>> Create([FromBody] CreateRoleRequest request)
        {
            try
            {
                var role = new Role
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                var createdRole = await _roleService.CreateAsync(role);
                return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating role", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Role>> Update(int id, [FromBody] UpdateRoleRequest request)
        {
            try
            {
                var existingRole = await _roleService.GetByIdAsync(id);
                if (existingRole == null)
                    return NotFound();

                existingRole.Name = request.Name;
                existingRole.Description = request.Description;
                existingRole.UpdatedDate = DateTime.UtcNow;

                var updatedRole = await _roleService.UpdateAsync(existingRole);
                return Ok(updatedRole);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating role with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                if (role == null)
                    return NotFound();

                await _roleService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting role with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetRolePermissions(int id)
        {
            try
            {
                var permissions = await _roleService.GetRolePermissionsAsync(id);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting permissions for role {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/permissions/{permissionId}")]
        public async Task<ActionResult> AssignPermission(int id, int permissionId)
        {
            try
            {
                var success = await _roleService.AssignPermissionAsync(id, permissionId);
                if (!success)
                    return BadRequest("Failed to assign permission");

                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error assigning permission {permissionId} to role {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}/permissions/{permissionId}")]
        public async Task<ActionResult> RemovePermission(int id, int permissionId)
        {
            try
            {
                var success = await _roleService.RemovePermissionAsync(id, permissionId);
                if (!success)
                    return BadRequest("Failed to remove permission");

                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error removing permission {permissionId} from role {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<Role>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            try
            {
                var roles = await _roleService.GetPagedAsync(page, pageSize, searchTerm);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting paged roles", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class CreateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
