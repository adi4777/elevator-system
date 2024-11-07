using ElevatorControlSystem.Common.Classes;

var floors = 10; // number of floors in the building
var elevators = 4; // number of elevators in the building

var building = new Building(floors, elevators);
Console.WriteLine($"**Starting simulation with {floors} floors and {elevators} elevators**");
building.StartSimulation();
