using ENSEK.Models;
using ENSEK.Services.CSVServices;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace ENSEK.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ENSEKController : Controller
    {
        private readonly ICSVService _csvService;

        public ENSEKController(ICSVService cSVService)
        {
            _csvService = cSVService;
        }


        [HttpPost("accounts-upload")]
        public ActionResult AddAccountsToDb([FromForm] IFormFileCollection formFiles)
        {
            try
            {
                var accounts = _csvService.ReadCSV<Account>(formFiles[0].OpenReadStream());
                _csvService.AddToAccount(accounts);
                
                return Ok("Accounts added to Database successfully!");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                return BadRequest(ex.Message);
                
            }
            
        }
        [HttpPost("meter-reading-uploads")]
        public async Task<IActionResult> AddMeterReading([FromForm] IFormFileCollection formFiles)
        {
            try
            {
                var reads = _csvService.ReadCSV<MeterReading>(formFiles[0].OpenReadStream());
                var results =_csvService.AddToMeterReading(reads);

                return Ok(results);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return BadRequest(ex.InnerException.Message);
                }
                return BadRequest(ex.Message);

            }

        }



    }
}
