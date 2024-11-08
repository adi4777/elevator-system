using ElevatorControlSystem.Common.Models;

namespace ElevatorControlSystem.Core.Services
{
    public class BuildingService
    {
        private readonly Building _building;

        public BuildingService(Building building)
        {
            _building = building;
            foreach (var item in _building.Elevators)
            {
                var elevatorService = new ElevatorService(item);
                Task.Run(elevatorService.Operate);
            }
        }

        public void StartSimulation(int displayStatusDelay)
        {
            Console.WriteLine(
                $"**Starting simulation with {_building.Floors} floors and {_building.Elevators.Count} elevators**"
            );
            Task.Run(GenerateRandomRequests);
            Task.Run(ProcessRequests);

            while (true)
            {
                DisplayStatus();
                Thread.Sleep(displayStatusDelay); // Display status after specific delay
            }
        }

        private void GenerateRandomRequests()
        {
            var rand = new Random();
            Thread.Sleep(1000);
            while (true)
            {
                int floor = rand.Next(1, _building.Floors + 1);
                Direction direction = rand.Next(2) == 0 ? Direction.Up : Direction.Down;
                direction =
                    floor == 1 && direction == Direction.Down ? Direction.Up
                    : floor == _building.Floors && direction == Direction.Up ? Direction.Down
                    : direction; // no Down and Up request should be generated for lowest and highest floor respectively
                lock (_building.Requests)
                {
                    _building.Requests.Enqueue(new Request(floor, direction));
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"=> {direction} request on floor {floor} received.");
                Console.ResetColor();
                Thread.Sleep(rand.Next(5000, 15000)); // Randomize request interval
            }
        }

        private void ProcessRequests()
        {
            while (true)
            {
                if (_building.Requests.Count > 0)
                {
                    Request request;
                    lock (_building.Requests)
                    {
                        request = _building.Requests.Dequeue();
                    }

                    Elevator closestElevator = FindClosestElevator(request);
                    var elevatorService = new ElevatorService(closestElevator);
                    elevatorService.AddStop(request.Floor);
                }
                Thread.Sleep(1000);
            }
        }

        private void DisplayStatus()
        {
            Console.ResetColor();
            Console.WriteLine("\n##=====================================##");
            foreach (var elevator in _building.Elevators)
            {
                Console.WriteLine(
                    $"Elevator {elevator.Id} - Floor: {elevator.CurrentFloor}, Status: {elevator.Status}\n    Stops: {string.Join(", ", elevator.Stops)}"
                );
            }
            Console.WriteLine("\n");
        }

        public Elevator FindClosestElevator(Request request)
        {
            Elevator? closestElevator = null;
            int minDistance = _building.Floors + 1;

            while (closestElevator == null) // Continue looping until a suitable elevator is found
            {
                foreach (var elevator in _building.Elevators)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - request.Floor);
                    if (
                        distance < minDistance
                        && (
                            elevator.IsIdle
                            || elevator.IsMovingTowards(request.Floor, request.Direction)
                        )
                    )
                    {
                        minDistance = distance;
                        closestElevator = elevator;
                    }
                }

                if (closestElevator == null)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(
                        $"** No available elevator for request on floor {request.Floor} going {request.Direction}. Waiting..."
                    );
                    Thread.Sleep(5000); // Wait for few seconds before rechecking
                }
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(
                $"** Assigned elevator {closestElevator.Id} to request on floor {request.Floor} going {request.Direction}."
            );
            Console.ResetColor();
            return closestElevator;
        }
    }
}
