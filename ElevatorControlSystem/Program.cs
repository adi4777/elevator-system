using ElevatorControlSystem.Common.Models;
using ElevatorControlSystem.Core.Services;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration configuration = builder.Build();
var floors = int.Parse(configuration.GetSection("Building:Floors").Value ?? ""); // number of floors in the building
var elevators = int.Parse(configuration.GetSection("Building:Elevators").Value ?? ""); // number of elevators in the building
var displayDelay = int.Parse(configuration.GetSection("Building:DisplayStatusDelay").Value ?? "");

var building = new Building(floors, elevators);
var service = new BuildingService(building);
service.StartSimulation(displayDelay);
