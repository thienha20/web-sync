using Npgsql;
using web_sync.Repositories.cb;
using web_sync.Repositories.ob;
using web_sync.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var settings = builder.Configuration.GetRequiredSection("ConnectionStrings");
var connectCb = new NpgsqlConnection(settings["CbSQLConnection"]);
var connectOb = new NpgsqlConnection(settings["ObSQLConnection"]);

// Đăng ký PostgreSQL Connection vào Service Collection
builder.Services.AddScoped<NpgsqlConnection>(_ => connectCb);
builder.Services.AddScoped<NpgsqlConnection>(_ => connectOb);

// Đăng ký cho từng Repository và Service
builder.Services.AddScoped<CategoryObRepository>(_ => new CategoryObRepository(connectOb));
builder.Services.AddScoped<CountryObRepository>(_ => new CountryObRepository(connectOb));
builder.Services.AddScoped<LogObRepository>(_ => new LogObRepository(connectOb));
builder.Services.AddScoped<UserObRepository>(_ => new UserObRepository(connectOb));
builder.Services.AddScoped<RegionObRepository>(_ => new RegionObRepository(connectOb));
builder.Services.AddScoped<PostObRepository>(_ => new PostObRepository(connectOb));

builder.Services.AddScoped<CategoryCbRepository>(_ => new CategoryCbRepository(connectCb));
builder.Services.AddScoped<CountryCbRepository>(_ => new CountryCbRepository(connectCb));
builder.Services.AddScoped<LogCbRepository>(_ => new LogCbRepository(connectCb));
builder.Services.AddScoped<UserCbRepository>(_ => new UserCbRepository(connectCb));
builder.Services.AddScoped<RegionCbRepository>(_ => new RegionCbRepository(connectCb));
builder.Services.AddScoped<PostCbRepository>(_ => new PostCbRepository(connectCb));

builder.Services.AddScoped<FileLogService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<RegionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
