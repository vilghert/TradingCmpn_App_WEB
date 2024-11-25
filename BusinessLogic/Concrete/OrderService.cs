public class OrderService : IOrderService
{
    private readonly IOrderDal _orderDal;

    public OrderService(IOrderDal orderDal)
    {
        _orderDal = orderDal;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        return await _orderDal.GetAllAsync();
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        return await _orderDal.GetByIdAsync(id);
    }

    public void CreateOrder(OrderDto order)
    {
        _orderDal.InsertAsync(order);
    }

    public void UpdateOrder(OrderDto order)
    {
        _orderDal.Update(order);
    }

    public void DeleteOrder(int id)
    {
        _orderDal.Delete(id);
    }

    public List<OrderDto> GetOrderHistory(int userId)
    {
        return _orderDal.GetOrderHistory(userId);
    }

    public async Task<List<OrderDto>> SearchOrdersAsync(decimal minTotalAmount, decimal maxTotalAmount)
    {
        var orders = await _orderDal.GetAllAsync();
        return orders.Where(o => o.TotalAmount >= minTotalAmount && o.TotalAmount <= maxTotalAmount).ToList();
    }

    public async Task<List<OrderDto>> SortOrdersAsync(string sortBy)
    {
        var orders = await _orderDal.GetAllAsync();

        return sortBy switch
        {
            "Total" => orders.OrderBy(o => o.TotalAmount).ToList(),  // Сортуємо за загальною сумою
            _ => orders  // Якщо інший параметр, повертаємо без сортування
        };
    }

    public List<OrderDto> GetOrdersForCurrentUser()
    {
        var currentUserId = 1;
        return _orderDal.GetOrdersForUser(currentUserId);
    }

    public List<OrderItemDto> GetOrderItems(int orderId)
    {
        return _orderDal.GetOrderItems(orderId);
    }
    public async Task RepeatOrderAsync(int orderId)
    {
        try
        {
            // Отримуємо існуюче замовлення
            var existingOrder = await _orderDal.GetByIdAsync(orderId);
            if (existingOrder == null)
            {
                Console.WriteLine($"Order with ID {orderId} not found.");
                return;
            }

            // Створюємо нове замовлення
            var newOrder = new OrderDto
            {
                UserId = existingOrder.UserId,
                TotalAmount = existingOrder.TotalAmount,
                OrderStatusId = existingOrder.OrderStatusId, // Використовуємо статус оригінального замовлення
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = existingOrder.OrderItems?.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    // Інші необхідні поля
                }).ToList() ?? new List<OrderItemDto>()
            };

            // Зберігаємо нове замовлення
            await _orderDal.InsertAsync(newOrder);

            Console.WriteLine($"Order {orderId} successfully repeated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while repeating order: {ex.Message}");
        }
    }
}