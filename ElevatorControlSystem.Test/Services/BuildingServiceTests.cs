using ElevatorControlSystem.Common.Models;
using ElevatorControlSystem.Core.Services;

namespace ElevatorControlSystem.Test.Services
{
    [TestClass]
    public class BuildingServiceTests
    {
        private Building _building;
        private BuildingService _buildingService;

        [TestInitialize]
        public void Setup()
        {
            _building = new Building(5, 2);
            _buildingService = new BuildingService(_building);
        }

        [TestMethod]
        public void FindClosestElevator_ShouldReturnClosestElevator()
        {
            // Arrange
            int requestFloor = 4;

            // Act
            var closestElevator = _buildingService.FindClosestElevator(
                new Request(requestFloor, Direction.Up)
            );

            // Assert
            Assert.IsNotNull(closestElevator);
            Assert.AreEqual(1, closestElevator.CurrentFloor);
        }
    }
}
