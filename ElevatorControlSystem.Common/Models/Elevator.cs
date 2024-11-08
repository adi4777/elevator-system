namespace ElevatorControlSystem.Common.Models
{
    public class Elevator
    {
        public int Id { get; }
        public int CurrentFloor { get; set; }
        public ElevatorStatus Status { get; set; }
        public HashSet<int> Stops { get; }

        private const int FloorTravelTime = 10000; // 10 seconds
        private const int StopTime = 10000; // 10 seconds

        public Elevator(int id)
        {
            Id = id;
            CurrentFloor = 1;
            Status = ElevatorStatus.Idle;
            Stops = [];
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
    }
}
