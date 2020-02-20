using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

namespace Dashboard.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.SaveTokens = true;
                options.Events = new OpenIdConnectEvents
                {
                    OnTokenResponseReceived = ctx =>
                    {

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx => {
                        var token = ctx.SecurityToken.RawData;
                        var claims = new List<Claim>
                        {
                            new Claim("token", token)
                        };
                        var appIdentity = new ClaimsIdentity(claims);
                        ctx.Principal.AddIdentity(appIdentity);
                                                
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = ctx =>
                    {
                        if (!ctx.Request.Query.TryGetValue("access_token", out StringValues values))
                        {
                            return Task.CompletedTask;
                        }

                        var token = values.Single();

                        ctx.Token = token;

                        return Task.CompletedTask;
                    }
                };
            });


                //services.Configure<JwtBearerOptions>(AzureADDefaults.AuthenticationScheme, options =>
                //{
                //    options.Events = new JwtBearerEvents
                //    {
                //        OnTokenValidated = ctx =>
                //        {

                //            return Task.CompletedTask;
                //        }
                //    };
                //    //options.TokenValidationParameters.ValidateIssuer = false;
                //    //options.Events = new OpenIdConnectEvents
                //    //{
                //    //    OnAuthorizationCodeReceived = ctx =>
                //    //    {

                //    //        return Task.CompletedTask;
                //    //    }
                //    //};
                //});

                services.AddRazorPages().AddMvcOptions(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddRazorPagesOptions(options =>
            {
                options.Conventions.AllowAnonymousToPage("/First");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}