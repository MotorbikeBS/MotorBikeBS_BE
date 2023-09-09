using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotorBikeBS_API.AutoMapper;
using Repository.Implement;
using Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Automapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Repository
builder.Services.AddScoped<IMotorBikeRepository, MotorBikeRepository>();

//CORS
builder.Services.AddCors();

//Newtonsoft Json
builder.Services.AddControllers().AddNewtonsoftJson(options =>
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); ;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(builder =>
{
	builder.AllowAnyOrigin()
	.AllowAnyHeader()
	.AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
