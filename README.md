# Elevator Control System

The Elevator Control System in a console app developed in .Net 8 to simulate an elevator system in a building.

## Folder Structure

  - /ElevatorControlSystem: Contains the startup class and logic.
  - /ElevatorControlSystem.Common: Contains the shared data model classes.
  - /ElevatorControlSystem.Core: Contains the business logic services.
  - /ElevatorControlSystem.Test: Contains unit tests for the application.

## Logic Explanation

It has three main models: Building, Elevator, Request

The **Building** class has below properties:

- Floors: number of floors in the building, its value is picked from appsettings.json `Building:Floors` property
- Elevators: list of elevators with its properties, its value is picked from appsettings.json `Building:Elevators` property
- Requests: queue of elevator requests.

The **Elevator** class has below properties:

- Id: the unique id of the elevator.
- CurrentFloor: current floor of the elevator.
- Status: current status of the elevator- `Idle / GoingUp / GoingDown`.
- Stops: set of stops that the elevator currently is assigned to.

The **Request** class has below properties:

- Floor: floor from which the request is raised.
- Direction: direction of the request- `Up / Down`.


The application flow is controlled by two services: BuildingService and ElevatorService.

The **BuildingService** starts simulation and runs two tasks: GenerateRandomRequests and ProcessRequests.

- GenerateRandomRequests: this method generate random requests from a floor with a direction which is then added to the Request queue in the Building object.
- ProcessRequests: this method reads the Request queue, finds the closest elevator to the request, and adds a stop in the closest elevator.

The **ElevatorService** controls each elevator by operating it continuosly, adding a stop, and moving to a floor.

- AddStop: this method adds a stop in the elevator.
- MoveToFloor: this method moves the elevator to the requested floor.
- Operate: this method operates the elevator and updates its status based on the stops & current floor.

## App Screenshot
![app-running](https://github.com/user-attachments/assets/6fa12ddd-93bb-4108-a026-2d6e78c28130)
