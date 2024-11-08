using ElevatorControlSystem.Common.Models;
using ElevatorControlSystem.Core.Services;

namespace ElevatorControlSystem.Test.Services
{
    [TestClass]
    public class ElevatorServiceTests
    {
        private Elevator _elevator;
        private ElevatorService _elevatorService;

        [TestInitialize]
        public void Setup()
        {
            _elevator = new Elevator(1);
            _elevatorService = new ElevatorService(_elevator);
        }

        [TestMethod]
        public void AddStop_ShouldAddStopToQueue()
        {
            // Arrange
            int targetFloor = 5;

            // Act
            _elevatorService.AddStop(targetFloor);

            // Assert
            Assert.AreEqual(1, _elevator.Stops.Count);
            Assert.AreEqual(targetFloor, _elevator.Stops.First());
            Assert.IsFalse(_elevator.IsIdle);
        }

        [TestMethod]
        public async Task MoveAsync_ShouldMoveElevatorToFloor()
        {
            // Arrange
            int targetFloor = 3;
            _elevatorService.AddStop(targetFloor);

            // Act
            await _elevatorService.MoveToFloor(targetFloor);

            // Assert
            Assert.AreEqual(targetFloor, _elevator.CurrentFloor);
        }
    }
}
