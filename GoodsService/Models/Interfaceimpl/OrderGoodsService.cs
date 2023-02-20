using GoodsService.Helpers;
using GoodsService.Helpers.Enums;
using GoodsService.Models.Interfaces;

namespace GoodsService.Models.Interfaceimpl
{
    public class OrderGoodsService : IBaseService
    {
        private IDataBase _context;

        public OrderGoodsService(IDataBase context)
        {
            _context = context;
        }
        public Task<ReturnResult> GetGoods()
        {
            return Task.FromResult(new ReturnResult { Data = _context.GetGoods(), StatuseCode = (int)Statuse.Success });
        }

        public Task<ReturnResult> GetOrders()
        {
            return Task.FromResult(new ReturnResult { Data = _context.GetOrders(), StatuseCode = (int)Statuse.Success });
        }

        public Task<ReturnResult> Order(int count, int idgood)
        {
            ReturnResult result=new ReturnResult();
            if (_context.Order(count,idgood).Result)
            {
                result.Data = $"The order has been added";
                result.StatuseCode = (int)Statuse.Success;
                
            }
            else
            {
                result.Data = $"Unable to add order, please try later";
                result.StatuseCode = (int)Statuse.Conflicted;
            }
            return Task.FromResult(result);
        }

        public Task<ReturnResult> RemoveGoods(int id)
        {
            ReturnResult result = new ReturnResult();
            if (_context.RemoveGoods(id).Result)
            {
                result.Data = $"The Goods has been removed";
                result.StatuseCode = (int)Statuse.Success;

            }
            else
            {
                result.Data = $"Unable to remove goods, please try later";
                result.StatuseCode = (int)Statuse.Conflicted;
            }
            return Task.FromResult(result);
        }

        public Task<ReturnResult> RemoveOrder(int id)
        {
            ReturnResult result = new ReturnResult();
            if (_context.RemoveOrder(id).Result)
            {
                result.Data = $"The order has been removed";
                result.StatuseCode = (int)Statuse.Success;

            }
            else
            {
                result.Data = $"Unable to remove order, please try later ";
                result.StatuseCode = (int)Statuse.Conflicted;
            }
            return Task.FromResult(result);
        }
        public Task<ReturnResult> AddGoods(string title, int count, float price)
        {
            ReturnResult result = new ReturnResult();
            if (_context.AddGoods(title, count, price).Result)
            {
                result.Data = $"The goods has been added";
                result.StatuseCode = (int)Statuse.Success;

            }
            else
            {
                result.Data = $"Unable to add goods, please try later";
                result.StatuseCode = (int)Statuse.Conflicted;
            }
            return Task.FromResult(result);

        }
    }
}
