using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Misaka.Config;
using Misaka.DependencyInject.Autofac;
using Misaka.DependencyInjection;
using Misaka.EntityFrameworkCore;
using Misaka.MessageQueue;
using Misaka.MessageQueue.InMemory;
using Misaka.Sample.Web.Application;

namespace Misaka.Sample.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Config.Configuration.Instance
                  .UseAutofac()
                  .UseConfiguration(configuration)
                  .LoadComponent(nameof(Misaka))
                  .UseEfCore()
                  .UseEfCoreMessageStore<SampleDbContext>()
                  .UserDbContextPool<SampleDbContext>(option =>
                                                     {
                                                         option.UseMySQL("server=47.101.138.31;database=misaka_framework;user=autyan;password=demo87#yq");
                                                     })
                  .UseInMemoryQueue(option =>
                                    {
                                        option.Topics = new[] {"Test"};
                                    });
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
                            {
                                options.InputFormatters.Insert(0, new RawRequestBodyInputFormatter());
                            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMemoryCache();

            return ObjectProviderFactory.Instance.Populate(services).Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            StartMessageQueue(applicationLifetime);
        }

        public void StartMessageQueue(IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStarted.Register(async () => {
                                                                await MessageQueueFactory.Instance.StartAsync();
                                                            });
        }
    }
}
