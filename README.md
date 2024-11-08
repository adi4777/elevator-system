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

- Floors: the number of floors in the building
- Elevators: list of elevators with its properties
- Requests: list of requests with its properties

The **Elevator** class has below properties:

- Id: the unique id of the elevator
- CurrentFloor: the current floor of the elevator
- Status: current status of the elevator- Idle/GoingUp/GoingDown
- Stops: the stops that the elevator currently is assigned to

The **Request** class has below properties:

- Floor: the number of floor from which a request is raised
- Direction: the direction of the request- Up/Down


The application flow is controlled by two services: BuidlingService and ElevatorService.

The **BuildingService** starts simulation and runs two tasks: GenerateRandomRequests and ProcessRequests.

- GenerateRandomRequests: this method generate random requests from a floor with a direction which is then added to the Request queue in the Building object.
- ProcessRequests: this method reads the Request queue, finds the closest elevator to the request, and add a stop in the closest elevator.

The **ElevatorService** controls each elevator by operating it continuosly, adding a stop, and moving to a floor.

- AddStop: this method add a stop in the elevator.
- MoveToFloor: this method moves the elevator to the requested floor.
- Operate: this method operated the elevator and updates its status based on the stops & current floor.

