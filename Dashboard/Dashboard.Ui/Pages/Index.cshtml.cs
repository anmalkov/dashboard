using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Dashboard.Ui.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var accessToken = HttpContext.GetTokenAsync(AzureADDefaults.AuthenticationScheme, "access_token").Result;
            var idToken = HttpContext.GetTokenAsync(AzureADDefaults.AuthenticationScheme, "id_token").Result;
            var claimsToken = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
            //Redirect($"https://localhost:44396/#id_token={claimsToken}");
        }
    }
}
