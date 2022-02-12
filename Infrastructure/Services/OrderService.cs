using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;
    
        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            //get basket from basket repository
            var basket = await _basketRepo.GetBasketAsync(basketId);
            //get item from the product repository
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            //get delivery method from repository
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(1);
            //calculate subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var orders= await _unitOfWork.Repository<Order>().ListAllAsync();
            //create order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, items, subtotal);
            _unitOfWork.Repository<Order>().Add(order);
            //save to database
            var result = await _unitOfWork.Complete();

            if(result <= 0) return null;
            // delete basket
            await _basketRepo.DeleteBasketAsync(basketId);
            //return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
        public async Task<IReadOnlyList<Order>> GetAllOrders()
        {
            var spec = new OrdersWithItemsAndOrderingSpecification();
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
        public async Task<bool> IsOrderComplitedOrCanceled(int orderId)
        {
            Order order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if(order.Status==OrderStatus.Completed || order.Status==OrderStatus.Canceled )
                return true;

            return false;
        }
        public async Task<Order> ChangeOrderStatus(int orderId, string status)
        {
            Order order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if(status== "COMPLETED" || status=="Completed")   order.Status=OrderStatus.Completed;
            else if(status =="CANCELED" || status=="Canceled") order.Status=OrderStatus.Canceled;

            _unitOfWork.Repository<Order>().Update(order);
          var result = await _unitOfWork.Complete();

            if(result <= 0) return null;

            return order;

        }

        public async Task<int> GetOrderId()
        {
           var result = await _unitOfWork.Repository<Order>().ListAllAsync();
           return result.Count;
        }
        
        public List<string> GetAllStatus()
        {
            return new List<string>
            {
                OrderStatus.Completed.ToString(),
                OrderStatus.Pending.ToString(),
                OrderStatus.Canceled.ToString()
            };

        }

       
    }
}