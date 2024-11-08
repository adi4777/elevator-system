namespace ElevatorControlSystem.Common.Models
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
    }
}
