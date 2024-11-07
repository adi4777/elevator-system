namespace ElevatorControlSystem.Common.Classes
{
    public class Building
    {
        public int Floors { get; }
        public List<Elevator> Elevators { get; }
        public Queue<Request> Requests { get; }

        public Building(int floors, int elevatorCount)
        {
            Floors = floors;
            Elevators = [];
            Requests = new Queue<Request>();

            for (int i = 0; i < elevatorCount; i++)
            {
                Elevators.Add(new Elevator(i + 1));
            }
        }

        public void StartSimulation()
        {
            Task.Run(GenerateRandomRequests);
            Task.Run(ProcessRequests);

            while (true)
            {
                DisplayStatus();
                Thread.Sleep(10000); // Display status after sepcific delay
            }
        }

        private void GenerateRandomRequests()
        {
            var rand = new Random();
            while (true)
            {
                int floor = rand.Next(1, Floors + 1);
                Direction direction = rand.Next(2) == 0 ? Direction.Up : Direction.Down;
                lock (Requests)
                {
                    Requests.Enqueue(new Request(floor, direction));
                }
                Console.WriteLine($"=> {direction} request on floor {floor} received.");
                Thread.Sleep(rand.Next(5000, 15000)); // Randomize request interval
            }
        }

        private void ProcessRequests()
        {
            while (true)
            {
                if (Requests.Count > 0)
                {
                    Request request;
                    lock (Requests)
                    {
                        request = Requests.Dequeue();
                    }

                    Elevator closestElevator = FindClosestElevator(request);
                    closestElevator.AddStop(request.Floor);
                }
                Thread.Sleep(1000);
            }
        }

        private void DisplayStatus()
        {
            Console.WriteLine("\n##=====================================##");
            foreach (var elevator in Elevators)
            {
                Console.WriteLine(
                    $"Elevator {elevator.Id} - Floor: {elevator.CurrentFloor}, Status: {elevator.Status}\n    Stops: {string.Join(", ", elevator.Stops)}"
                );
            }
            Console.WriteLine("\n");
        }

        private Elevator FindClosestElevator(Request request)
        {
            Elevator? closestElevator = null;
            int minDistance = Floors + 1;

            while (closestElevator == null) // Continue looping until a suitable elevator is found
            {
                foreach (var elevator in Elevators)
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
                    Console.WriteLine(
                        $"** No available elevator for request on floor {request.Floor} going {request.Direction}. Waiting..."
                    );
                    Thread.Sleep(5000); // Wait for few seconds before rechecking
                }
            }

            Console.WriteLine(
                $"** Assigned elevator {closestElevator.Id} to request on floor {request.Floor} going {request.Direction}."
            );
            return closestElevator;
        }
    }
}
