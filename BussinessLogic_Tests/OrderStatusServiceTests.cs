using Moq;
using DTO;

[TestFixture]
public class OrderStatusServiceTests
{
    private Mock<IOrderStatusDal> _mockOrderStatusDal;
    private OrderStatusService _orderStatusService;

    [SetUp]
    public void SetUp()
    {
        _mockOrderStatusDal = new Mock<IOrderStatusDal>();
        _orderStatusService = new OrderStatusService(_mockOrderStatusDal.Object);
    }

    [Test]
    public void GetAllOrderStatuses_ReturnsAllOrderStatuses()
    {
        // Arrange
        var orderStatuses = new List<OrderStatusDto>
        {
            new OrderStatusDto { OrderStatusId = 1, OrderStatusName = "Pending" },
            new OrderStatusDto { OrderStatusId = 2, OrderStatusName = "Completed" }
        };
        _mockOrderStatusDal.Setup(dal => dal.GetAll()).Returns(orderStatuses);

        // Act
        var result = _orderStatusService.GetAllOrderStatuses();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Pending", result[0].OrderStatusName);
    }

    [Test]
    public void GetOrderStatusById_ReturnsOrderStatus_WhenExists()
    {
        // Arrange
        var orderStatus = new OrderStatusDto { OrderStatusId = 1, OrderStatusName = "Pending" };
        _mockOrderStatusDal.Setup(dal => dal.GetById(1)).Returns(orderStatus);

        // Act
        var result = _orderStatusService.GetOrderStatusById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Pending", result.OrderStatusName);
    }

    [Test]
    public void GetOrderStatusById_ReturnsNull_WhenDoesNotExist()
    {
        // Arrange
        _mockOrderStatusDal.Setup(dal => dal.GetById(1)).Returns((OrderStatusDto)null);

        // Act
        var result = _orderStatusService.GetOrderStatusById(1);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task CreateOrderStatus_CallsInsertAsyncOnDal()
    {
        // Arrange
        var orderStatus = new OrderStatusDto { OrderStatusName = "New Status" };
        _mockOrderStatusDal.Setup(dal => dal.Insert(orderStatus)).Verifiable();

        // Act
        await Task.Run(() => _orderStatusService.CreateOrderStatus(orderStatus));

        // Assert
        _mockOrderStatusDal.Verify(dal => dal.Insert(orderStatus), Times.Once);
    }

    [Test]
    public async Task UpdateOrderStatus_CallsUpdateOnDal()
    {
        // Arrange
        var orderStatus = new OrderStatusDto { OrderStatusId = 1, OrderStatusName = "Updated Status" };
        _mockOrderStatusDal.Setup(dal => dal.Update(orderStatus)).Verifiable();

        // Act
        await Task.Run(() => _orderStatusService.UpdateOrderStatus(orderStatus));

        // Assert
        _mockOrderStatusDal.Verify(dal => dal.Update(orderStatus), Times.Once);
    }

    [Test]
    public async Task DeleteOrderStatus_CallsDeleteOnDal()
    {
        // Arrange
        int orderStatusId = 1;
        _mockOrderStatusDal.Setup(dal => dal.Delete(orderStatusId)).Verifiable();

        // Act
        await Task.Run(() => _orderStatusService.DeleteOrderStatus(orderStatusId));

        // Assert
        _mockOrderStatusDal.Verify(dal => dal.Delete(orderStatusId), Times.Once);
    }
}