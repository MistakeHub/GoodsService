using GoodsService.Models;
using GoodsService.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoodsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsOrderController : ControllerBase
    {
        private IBaseService _service;
        public GoodsOrderController(IBaseService service)
        {
            _service = service;
        }
        [HttpGet("/orders")]
        public IActionResult GetOrders()
        {
            var result = _service.GetOrders().Result;
            return new ObjectResult(result.Data) { StatusCode = result.StatuseCode };
        }


        [HttpGet("/goods")]
        public IActionResult GetGoods()
        {
            var result = _service.GetGoods().Result;
            return new ObjectResult(result.Data) { StatusCode = result.StatuseCode };
        }

     
        [HttpPost("/makeorder")]
        public IActionResult Post(int count, int idgoods)
        {
            var result = _service.Order(count,idgoods).Result;
            return new ObjectResult(result.Data) { StatusCode = result.StatuseCode };
        }
        [HttpPost("/addgoods")]
        public IActionResult Post(string title, int count, float price)
        {
            var result = _service.AddGoods(title,count,price).Result;
            return new ObjectResult(result.Data) { StatusCode = result.StatuseCode };
        }


        [HttpDelete("removeorder/{id}")]
        public IActionResult Put(int id)
        {
            var result = _service.RemoveOrder(id).Result;
            return new ObjectResult(result.Data) { StatusCode = result.StatuseCode };
        }

     
        [HttpDelete("removegoods/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.RemoveGoods(id).Result;
            return new ObjectResult(result.Data) { StatusCode = result.StatuseCode };
        }
    }
}
