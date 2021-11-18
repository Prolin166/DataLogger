using Business.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DataLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        [HttpGet("[action]")]
        public IActionResult GetOperationTimer()
        {
            return Ok(_managementService.GetOperationTimerValue());
        }

        [HttpPost("[action]")]
        public IActionResult ResetOperationTimer()
        {
            _managementService.ResetOperationTimer();
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult GetDeviceId()
        {
            return Ok(_managementService.GetDeviceId());
        }

        [HttpPost("[action]")]
        public IActionResult SetDeviceId(string id)
        {
            return Ok(_managementService.SetDeviceId(id));
        }

        [HttpGet("[action]")]
        public IActionResult GetIPAdress()
        {
            return Ok(_managementService.GetIPAdress());
        }

        [HttpGet("[action]")]
        public IActionResult GetHostName()
        {
            return Ok(_managementService.GetHostName());
        }

        [HttpGet("[action]")]
        public IActionResult GetManagementOptions()
        {
            return Ok(_managementService.GetDeviceProperties());
        }

        [HttpGet("[action]")]
        public IActionResult GetMacAdress()
        {
            return Ok(_managementService.GetMacAddr());
        }

        [HttpGet("[action]")]
        public IActionResult GetDaysToSave()
        {
            return Ok(_managementService.GetDaysToSave());
        }

        [HttpPost("[action]")]
        public IActionResult SetDaysToSave(int days)
        {
            return Ok(_managementService.SetDaysToSave(days));
        }
    }
}
