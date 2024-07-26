
using FluentValidation;
using TournamentSimulator.Core.Entities;
using TournamentSimulator.Core.Interfaces;
using TournamentSimulator.Core.Services;
using TournamentSimulator.Core.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register dependencies
builder.Services.AddScoped<ITournamentSimulatorService, TournamentSimulatorService>();
builder.Services.AddScoped<ITeamGenerator, TeamGenerator>();
builder.Services.AddScoped<IGroupGenerator, GroupGenerator>();
builder.Services.AddScoped<IGroupSimulator, GroupSimulator>();
builder.Services.AddScoped<IMatchSimulator, MatchSimulator>();
builder.Services.AddScoped<IMatchGenerator, MatchGenerator>();

// Register validators
builder.Services.AddScoped<IValidator<Group>, GroupValidator>();
builder.Services.AddScoped<IValidator<Team>, TeamValidator>();

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