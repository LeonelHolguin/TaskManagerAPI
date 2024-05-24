using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Application.Interfaces.Services;
using TaskManager.Core.Application.ViewModels.Task;

namespace WebAPI.TaskManager.Controllers
{
    [Route("api/v1/task")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("{userId}/get_all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaskViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            List<TaskViewModel> taskListVm = new();

            try
            {
                taskListVm = await _taskService.GetAllByUserId(userId);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(taskListVm);
        }

        [HttpGet]
        [Route("{taskId}/get_task")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOneById(int taskId)
        {
            TaskViewModel taskVm = new();

            try
            {
                taskVm = await _taskService.GetById(taskId);

                if (taskVm.Id is 0)
                   return NotFound("Task not found");

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(taskVm);
        }

        [HttpPost]
        [Route("create_task")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TaskViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewTask([FromBody] TaskViewModel taskToCreate)
        {
            TaskViewModel taskCreatedVm = new();

            try
            {
                taskCreatedVm = await _taskService.Add(taskToCreate);

            } catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

            return Created("", taskCreatedVm);
        }

        [HttpPut]
        [Route("update_task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] TaskViewModel taskToUpdate)
        {
            try
            {
                await _taskService.Update(taskToUpdate);

            } catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("{taskId}/delete_task")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int taskId)
        {
            try
            {
                await _taskService.Delete(taskId);

            } catch (Exception ex) 
            { 
                return BadRequest(ex);
            }

            return NoContent();
        }
    }
}
