namespace GoodsService.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string GoodsTitle { get; set; }

        public int Count { get; set; }

        public float TotalPrice { get; set; }

        public DateTime OrderTime { get; set; }


    }
}
