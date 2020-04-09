using BlogService.DataContexts;
using BlogService.Models;
using BlogService.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlogService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var _serializerSettings = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter> { new ObjectIdConverter() }
            };

            //BsonClassMap.RegisterClassMap<ArticlesInfo>();

            //var config = new ServerConfig();
            //Configuration.Bind(config);

            //var artcileContext = new ArticleContext(config.MongoDB);

            //var repo = new ArticlesInfoRepository(artcileContext);
            //services.AddSingleton<IArticlesInfoRepository>(repo);

            services.Configure<Settings>(
              options =>
              {
                  options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                  options.Database = Configuration.GetSection("MongoDb:Database").Value;
                  options.Container = Configuration.GetSection("MongoDb:Container").Value;
                  options.IsContained = Configuration["DOTNET_RUNNING_IN_CONTAINER"] != null;
                  options.Host = Configuration.GetSection("MongoDb:Host").Value;
                  options.Port = int.Parse(Configuration.GetSection("MongoDb:Port").Value);
                  options.User = Configuration.GetSection("MongoDb:User").Value;
                  options.Password = Configuration.GetSection("MongoDb:Password").Value;
              });

            services.AddTransient<IApplicationDbContext<ArticlesInfo>, ApplicationDbContext<ArticlesInfo>>();
            services.AddTransient<IArticlesInfoRepository, ArticlesInfoRepository>();


            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000",
                                        "https://localhost:3000")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
