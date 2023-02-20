namespace GoodsService.Models.Interfaces
{
    public interface IDataBase
    {
        public Task<List<Goods>> GetGoods();

        public Task<List<Order>> GetOrders();

        public Task<bool> Order( int count, int idgood);

        public Task<bool> RemoveOrder(int id);

        public Task<bool> RemoveGoods(int id);

        public Task<bool> AddGoods(string title, int count, float price);

      

    }
}
