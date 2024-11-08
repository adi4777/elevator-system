using ElevatorControlSystem.Common.Models;
using ElevatorControlSystem.Core.Services;

var floors = 10; // number of floors in the building
var elevators = 4; // number of elevators in the building

var building = new Building(floors, elevators);
var service = new BuildingService(building);
service.StartSimulation();
