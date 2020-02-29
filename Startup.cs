using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PieShop.Models;
using Endpoint = Microsoft.AspNetCore.Http.Endpoint;

namespace PieShop
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

			services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();

			//services.AddScoped<IPieRepository, MockPieRepository>();
			//services.AddScoped<ICategoryRepository, MockCategoryRepository>();

			services.AddScoped<IPieRepository, PieRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();

			services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));

			services.AddHttpContextAccessor();

			services.AddSession();

			services.AddControllersWithViews();

			services.AddRazorPages();

			services.AddAuthentication()
				.AddGoogle(options =>
				{
					options.ClientId = _configuration["Authentication.Google.ClientId"];
					options.ClientSecret = _configuration["Authentication.Google.ClientSecret"];
				})
				.AddFacebook(options =>
				{
					options.AppId = _configuration["Authentication.Facebook.AppId"];
					options.AppSecret = _configuration["Authentication.Facebook.AppSecret"];
				})
				.AddTwitter(options =>
				{
					options.ConsumerKey = _configuration["Authentication.Twitter.Key"];
					options.ConsumerSecret = _configuration["Authentication.Twitter.Secret"];
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseSession();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				//endpoints.MapGet("/", async context =>
				//{
				//	await context.Response.WriteAsync("Hello World!");
				//});

				endpoints.MapControllerRoute(
					name:"default",
					pattern:"{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
