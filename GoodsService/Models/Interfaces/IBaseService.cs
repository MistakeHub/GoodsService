using GoodsService.Helpers;

namespace GoodsService.Models.Interfaces
{
    public interface IBaseService
    {
        public Task<ReturnResult> GetGoods();

        public Task<ReturnResult> GetOrders();

        public Task<ReturnResult> Order(int count, int idgood);

        public Task<ReturnResult> RemoveOrder(int id);

        public Task<ReturnResult> RemoveGoods(int id);

        public Task<ReturnResult> AddGoods(string title, int count, float price);
    }
}
