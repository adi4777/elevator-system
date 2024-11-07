namespace ElevatorControlSystem.Common.Classes
{
    public class Elevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        public ElevatorStatus Status { get; private set; }
        public HashSet<int> Stops { get; }

        private const int FloorTravelTime = 10000; // 10 seconds
        private const int StopTime = 10000; // 10 seconds

        public Elevator(int id)
        {
            Id = id;
            CurrentFloor = 1;
            Status = ElevatorStatus.Idle;
            Stops = [];

            Task.Run(Operate);
        }

        public void AddStop(int floor)
        {
            Stops.Add(floor);
            if (Status == ElevatorStatus.Idle)
            {
                Status = floor > CurrentFloor ? ElevatorStatus.GoingUp : ElevatorStatus.GoingDown;
            }
        }

        public bool IsIdle => Status == ElevatorStatus.Idle;

        public bool IsMovingTowards(int floor, Direction direction)
        {
            return (
                    Status == ElevatorStatus.GoingUp
                    && direction == Direction.Up
                    && floor >= CurrentFloor
                )
                || (
                    Status == ElevatorStatus.GoingDown
                    && direction == Direction.Down
                    && floor <= CurrentFloor
                );
        }

        private async Task Operate()
        {
            while (true)
            {
                if (Stops.Count == 0)
                {
                    Status = ElevatorStatus.Idle;
                }
                else
                {
                    int nextFloor =
                        Status == ElevatorStatus.GoingUp
                            ? Stops.Where(f => f > CurrentFloor).DefaultIfEmpty(Stops.Min()).Min()
                            : Stops.Where(f => f < CurrentFloor).DefaultIfEmpty(Stops.Max()).Max();

                    await MoveToFloor(nextFloor);

                    if (Stops.Contains(CurrentFloor))
                    {
                        Stops.Remove(CurrentFloor);
                        Console.WriteLine(
                            $"** Elevator {Id} stopped at floor {CurrentFloor} for passengers."
                        );
                        await Task.Delay(StopTime); // Wait for passengers to get in/out
                    }

                    Status =
                        Stops.Count == 0 ? ElevatorStatus.Idle
                        : nextFloor > CurrentFloor ? ElevatorStatus.GoingUp
                        : ElevatorStatus.GoingDown;
                }

                await Task.Delay(500);
            }
        }

        private async Task MoveToFloor(int floor)
        {
            while (CurrentFloor != floor)
            {
                CurrentFloor = CurrentFloor < floor ? CurrentFloor + 1 : CurrentFloor - 1;

                Console.WriteLine(
                    $"** Elevator {Id} moving to floor {floor} - Current floor: {CurrentFloor}"
                );
                await Task.Delay(FloorTravelTime); // floor to floor travel time
            }
        }
    }
}
