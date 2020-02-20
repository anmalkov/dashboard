using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;

namespace Dashboard.Ui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;

        public TokenController(ILogger<TokenController> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenOnBehalfOfUserAsync(new[] { "api://a1378021-3598-476b-bfc6-80fdd56275c9/Weather.Read" });
            return Ok(new { token = accessToken });
        }
    }
}