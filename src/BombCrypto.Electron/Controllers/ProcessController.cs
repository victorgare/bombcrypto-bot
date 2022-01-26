using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ApplicationCore.Interfaces.Services;
using BombCrypto.CrossCutting.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BombCrypto.Electron.Controllers
{
    public class ProcessController : Controller
    {
        private readonly IProcessService _processService;

        public ProcessController(IProcessService processService)
        {
            _processService = processService;
        }


        [HttpPost]
        public IActionResult Index()
        {
            var config = new Config();
            _ = Task.Run(() => _processService.Process(config));

            return Ok();
        }

        [HttpPost]
        public IActionResult Stop()
        {
            CencelationTokenHelper.CancellationTokenSource.Cancel();
            return Ok();
        }
    }
}
