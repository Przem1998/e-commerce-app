using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.PayuModels;

namespace Core.Interfaces
{
    public interface IPayuService
    {
        Task<CustomerBasket> UpdateBasketPrice(string basketId);
        Task<string> GetBearer();
        Task<OrderPayu> CreateOrder(Order order, string basketId);
        string GetOrderUrlPayu();
        string GetSignature(string input, string algorithm);

     

    }
}