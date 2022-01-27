using BombCrypto.ApplicationCore.Domain;
using BombCrypto.ApplicationCore.Interfaces.Services;
using BombCrypto.CrossCutting.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
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
        public IActionResult Index([FromBody] Config config)
        {

            config.CancellationTokenSource = CencelationTokenHelper.CancellationTokenSource;
            Console.WriteLine(JsonSerializer.Serialize(config));
            _ = Task.Run(() => _processService.Process(config));
            //config.CancellationTokenSource.Token.ThrowIfCancellationRequested();
            return Ok();
        }

        [HttpPost]
        public IActionResult Stop()
        {
            CencelationTokenHelper.CancellationTokenSource.Cancel(true);
            return Ok();
        }
    }
}
