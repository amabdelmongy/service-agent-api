using Data;
using Data.MessageBus;
using Domain;
using Domain.MessageBus;
using Domain.ServiceAgent;
using Domain.ServiceAgent.CommandHandlers;
using Domain.ServiceAgent.InputValidator;
using Domain.ServiceAgent.Projection;
using Domain.ServiceAgentExecution;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 

namespace WebApi
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
            //InMemoryServiceBus
            services.AddSingleton<IServiceBusPublisher, InMemoryServiceBus>();
            var inMemoryServiceBus = new InMemoryServiceBus();

            //Repositories
            new RunDBScriptsRepository(Configuration.GetConnectionString("CreateConnection")).Run(@".\DBScript\DbScript.SQL");
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var projectionRepository = new ProjectionRepository(connectionString);
            var dispatchRepository = new DispatchRepository(inMemoryServiceBus, connectionString);
            var eventRepository = new EventRepository(connectionString, dispatchRepository);

            services.AddSingleton<IEventRepository, EventRepository>(
                (ctx) => eventRepository
            );
            services.AddSingleton<IProjectionRepository, ProjectionRepository>(
                (ctx) => projectionRepository
            );

            //Command Handlers
            services.AddTransient<IServiceAgentCommandHandler, ServiceAgentCommandHandler>(); 
            services.AddTransient<IScheduleServiceAgentCommandHandler, ScheduleServiceAgentCommandHandler>();
            services.AddTransient<IFailServiceAgentCommandHandler, FailServiceAgentCommandHandler>();
            services.AddTransient<ICompleteServiceAgentCommandHandler, CompleteServiceAgentCommandHandler>();
            

            //Services
            services.AddTransient<IInputValidator, InputValidator>();
            services.AddTransient<IServiceAgentService, ServiceAgentService>();
            services.AddTransient<IServiceAgentWorkflow, ServiceAgentWorkflow>();
            services.AddTransient<IExecutionFacade, ExecutionFacade>();

            services.AddControllers();

            //AddSubscriptions to ServiceBus
            new MessageSubscription(
                inMemoryServiceBus,
                new MessageBusHandler(projectionRepository)
            ).AddSubscriptions();


            // ********************
            // Setup CORS
            // ********************
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader()
                    .AllowAnyMethod());
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            // Shows UseCors with named policy.
            app.UseCors("AllowSpecificOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
