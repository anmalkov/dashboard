using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;

namespace Dashboard.Ui.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;

        public string AccessToken { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task OnGet()
        {
            //var accessToken = HttpContext.GetTokenAsync(AzureADDefaults.AuthenticationScheme, "access_token").Result;
            //var idToken = HttpContext.GetTokenAsync(AzureADDefaults.AuthenticationScheme, "id_token").Result;
            //var claimsToken = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
            //Redirect($"https://localhost:/#id_token={claimsToken}");

            AccessToken = await _tokenAcquisition.GetAccessTokenOnBehalfOfUserAsync(new[] { "api://a1378021-3598-476b-bfc6-80fdd56275c9/Weather.Read" });
        }
    }
}
