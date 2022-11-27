namespace Api;

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

        services.AddDbContext<ArchiveContext>(x => x.UseInMemoryDatabase("InMemoryDb"));

        services.AddControllers();

        services.AddMvc(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddFluentValidationAutoValidation();

        services.AddSingleton<IValidator<Models.Note>, NoteValidator>();
        services.AddSingleton<IValidator<Models.Report>, ReportValidator>();

        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IReportsRepository<Report>, ReportsRepository>();
        services.AddScoped<IReportsService, ReportsService>();
        services.AddScoped<INotesRepository<Note>, NotesRepository>();
        services.AddScoped<INotesService, NotesService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
