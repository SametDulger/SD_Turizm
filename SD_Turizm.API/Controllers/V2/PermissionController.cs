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
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILoggingService _loggingService;

        public PermissionController(IPermissionService permissionService, ILoggingService loggingService)
        {
            _permissionService = permissionService;
            _loggingService = loggingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> GetAll()
        {
            try
            {
                var permissions = await _permissionService.GetAllAsync();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all permissions", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> GetById(int id)
        {
            try
            {
                var permission = await _permissionService.GetByIdAsync(id);
                if (permission == null)
                    return NotFound();

                return Ok(permission);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting permission with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<Permission>> GetByName(string name)
        {
            try
            {
                var permission = await _permissionService.GetByNameAsync(name);
                if (permission == null)
                    return NotFound();

                return Ok(permission);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting permission with name {name}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("resource/{resource}")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetByResource(string resource)
        {
            try
            {
                var permissions = await _permissionService.GetByResourceAsync(resource);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting permissions for resource {resource}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("action/{action}")]
        public async Task<ActionResult<IEnumerable<Permission>>> GetByAction(string action)
        {
            try
            {
                var permissions = await _permissionService.GetByActionAsync(action);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting permissions for action {action}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Permission>> Create([FromBody] CreatePermissionRequest request)
        {
            try
            {
                var permission = new Permission
                {
                    Name = request.Name,
                    Description = request.Description,
                    Resource = request.Resource,
                    Action = request.Action,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                var createdPermission = await _permissionService.CreateAsync(permission);
                return CreatedAtAction(nameof(GetById), new { id = createdPermission.Id }, createdPermission);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error creating permission", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Permission>> Update(int id, [FromBody] UpdatePermissionRequest request)
        {
            try
            {
                var existingPermission = await _permissionService.GetByIdAsync(id);
                if (existingPermission == null)
                    return NotFound();

                existingPermission.Name = request.Name;
                existingPermission.Description = request.Description;
                existingPermission.Resource = request.Resource;
                existingPermission.Action = request.Action;
                existingPermission.UpdatedDate = DateTime.UtcNow;

                var updatedPermission = await _permissionService.UpdateAsync(existingPermission);
                return Ok(updatedPermission);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error updating permission with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var permission = await _permissionService.GetByIdAsync(id);
                if (permission == null)
                    return NotFound();

                await _permissionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error deleting permission with id {id}", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("resources")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllResources()
        {
            try
            {
                var resources = await _permissionService.GetAllResourcesAsync();
                return Ok(resources);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all resources", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("actions")]
        public async Task<ActionResult<IEnumerable<string>>> GetAllActions()
        {
            try
            {
                var actions = await _permissionService.GetAllActionsAsync();
                return Ok(actions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting all actions", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<Permission>>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            try
            {
                var permissions = await _permissionService.GetPagedAsync(page, pageSize, searchTerm);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError("Error getting paged permissions", ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class CreatePermissionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }

    public class UpdatePermissionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
