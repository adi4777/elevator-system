namespace ElevatorControlSystem.Common.Classes
{
    public class Request(int floor, Direction direction)
    {
        public int Floor { get; } = floor;
        public Direction Direction { get; } = direction;
    }
}
