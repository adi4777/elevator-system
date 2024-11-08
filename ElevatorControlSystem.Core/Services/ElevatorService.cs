using ElevatorControlSystem.Common.Models;

namespace ElevatorControlSystem.Core.Services
{
    public class ElevatorService
    {
        private readonly Elevator _elevator;
        private const int FloorTravelTime = 10000; // 10 seconds
        private const int StopTime = 10000; // 10 seconds

        public ElevatorService(Elevator elevator)
        {
            _elevator = elevator;
        }

        public void AddStop(int floor)
        {
            _elevator.Stops.Add(floor);
            if (_elevator.Status == ElevatorStatus.Idle)
            {
                _elevator.Status =
                    floor > _elevator.CurrentFloor
                        ? ElevatorStatus.GoingUp
                        : ElevatorStatus.GoingDown;
            }
        }

        public async Task MoveToFloor(int floor)
        {
            while (_elevator.CurrentFloor != floor)
            {
                _elevator.CurrentFloor =
                    _elevator.CurrentFloor < floor
                        ? _elevator.CurrentFloor + 1
                        : _elevator.CurrentFloor - 1;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(
                    $"** Elevator {_elevator.Id} moving to floor {floor} - Current floor: {_elevator.CurrentFloor}"
                );
                Console.ResetColor();
                await Task.Delay(FloorTravelTime); // floor to floor travel time
            }
        }

        public async Task Operate()
        {
            while (true)
            {
                if (_elevator.Stops.Count == 0)
                {
                    _elevator.Status = ElevatorStatus.Idle;
                }
                else
                {
                    int nextFloor =
                        _elevator.Status == ElevatorStatus.GoingUp
                            ? _elevator
                                .Stops.Where(f => f > _elevator.CurrentFloor)
                                .DefaultIfEmpty(_elevator.Stops.Min())
                                .Min()
                            : _elevator
                                .Stops.Where(f => f < _elevator.CurrentFloor)
                                .DefaultIfEmpty(_elevator.Stops.Max())
                                .Max();
                    await MoveToFloor(nextFloor);

                    if (_elevator.Stops.Contains(_elevator.CurrentFloor))
                    {
                        _elevator.Stops.Remove(_elevator.CurrentFloor);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(
                            $"** Elevator {_elevator.Id} stopped at floor {_elevator.CurrentFloor} for passengers."
                        );
                        Console.ResetColor();
                        await Task.Delay(StopTime); // Wait for passengers to get in/out
                    }
                    _elevator.Status =
                        _elevator.Stops.Count == 0 ? ElevatorStatus.Idle
                        : nextFloor > _elevator.CurrentFloor ? ElevatorStatus.GoingUp
                        : ElevatorStatus.GoingDown;
                }

                await Task.Delay(500);
            }
        }
    }
}
