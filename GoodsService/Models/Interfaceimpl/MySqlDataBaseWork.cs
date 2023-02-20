using GoodsService.Models.Interfaces;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data.SqlClient;
using System.Transactions;

namespace GoodsService.Models.Interfaceimpl
{
    public class MySqlDataBaseWork : IDataBase
    {
        private string _connectionstring;
        private string _database;

        public MySqlDataBaseWork(string server, string user, string password, string database)
        {
            _connectionstring = $"server={server};user={user};password={password};";
            _database = database;
        }

        public async Task<bool> AddGoods(string title, int count, float price)
        {
            int result = 0;
            using (MySqlConnection connection = new MySqlConnection(_connectionstring))
            {
                await connection.OpenAsync();
                MySqlCommand addgoodscommand = new MySqlCommand($"USE {_database};INSERT INTO Goods(Title,Count,Price) VALUES(@title,@count,@price)", connection);
                MySqlParameter[] goodsparameters = new MySqlParameter[] { new MySqlParameter("@title", title), new MySqlParameter("@count", count), new MySqlParameter("@price", price) };
                addgoodscommand.Parameters.AddRange(goodsparameters);
                result = await addgoodscommand.ExecuteNonQueryAsync();

            }
            return result > 0;
        }

        public async Task<List<Goods>> GetGoods()
        {

            List<Goods> goods=new List<Goods>();
            using (MySqlConnection connection =new MySqlConnection(_connectionstring))
            {
                connection.Open();

                MySqlCommand getgoodscommand = new MySqlCommand($"USE {_database};SELECT * FROM Goods",connection);

                using ( MySqlDataReader reader = getgoodscommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while(await reader.ReadAsync())
                        {

                            goods.Add(new Goods { Id = reader.GetInt32(0), Title = reader.GetString(1), Price = reader.GetFloat(2), Count = reader.GetInt32(3) });
                        }
                    }
                }

            }
            return goods;
        }

        public async Task<List<Order>> GetOrders()
        {
            List<Order> orders = new List<Order>();
            using (MySqlConnection connection = new MySqlConnection(_connectionstring))
            {
                await connection.OpenAsync();
                MySqlCommand getgoodscommand = new MySqlCommand($"USE {_database};SELECT Orders.Id, Goods.Title, Orders.Count, Goods.Price*Orders.Count, Ordertime FROM Orders  JOIN Goods WHERE Orders.Idgoods =Goods.Id", connection);

                using (MySqlDataReader reader = getgoodscommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {

                            orders.Add(new Order { Id = reader.GetInt32(0), GoodsTitle=reader.GetString(1), Count=reader.GetInt32(2), TotalPrice=reader.GetFloat(3), OrderTime=reader.GetDateTime(4) });
                        }
                    }
                }

            }
            return orders;
        }

        public async Task<bool> Order(int count, int idgood)
        {
            int result = 0;
            using (MySqlConnection connection =new MySqlConnection(_connectionstring))
            {
                MySqlTransaction transaction = null ;
                try
                {
                    await connection.OpenAsync();
                    bool IsAbleToAdd = false;
                
                    MySqlCommand desirablegoodscommand = new MySqlCommand($"USE {_database}; SELECT Goods.Count, COUNT( Goods.Count >={count}) FROM Goods where Goods.Id={idgood}", connection);
                    using (MySqlDataReader reader = desirablegoodscommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                          
                               IsAbleToAdd = reader.GetInt32(0) > 0;

                            }
                        }
                    }
           
                    if (IsAbleToAdd)
                    {
                       transaction = connection.BeginTransaction();

                        MySqlCommand ordercommand = new MySqlCommand($"USE {_database};INSERT INTO Orders(Idgoods,Count,Ordertime)  VALUES(@idgoods, @count, @ordertime)", connection);
                        MySqlParameter[] orderparameters = new MySqlParameter[] { new MySqlParameter("@idgoods", idgood), new MySqlParameter("@count", count), new MySqlParameter("@OrderTime", DateTime.UtcNow) };
                        ordercommand.Transaction = transaction;
                        ordercommand.Parameters.AddRange(orderparameters);
                        await ordercommand.ExecuteNonQueryAsync();
                        MySqlCommand goodschangecommand = new MySqlCommand($"USE {_database};UPDATE Goods SET Goods.Count = Goods.Count - {count} ", connection);
                        goodschangecommand.Transaction = transaction;
                        result = await goodschangecommand.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                    }
                }catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;

                }
            }
            return result > 0;
        }

        public async Task<bool> RemoveGoods(int id)
        {
            int result = 0;
            using (MySqlConnection connection = new MySqlConnection(_connectionstring))
            {
                await connection.OpenAsync();
                MySqlCommand removegoodscommand = new MySqlCommand($"USE {_database};DELETE FROM Goods WHERE Goods.Id = {id}", connection);
                result = await removegoodscommand.ExecuteNonQueryAsync();

            }
            return result > 0;
        }

        public async Task<bool> RemoveOrder(int id)
        {
            int result = 0;
            MySqlTransaction transaction = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionstring))
            {
                try
                {
                    await connection.OpenAsync();
                    int count = 0;
                   transaction = connection.BeginTransaction();
                
                  
                    MySqlCommand getgoodscommand = new MySqlCommand($"USE {_database};SELECT Orders.Count FROM Orders JOIN Goods WHERE Orders.Idgoods =Goods.Id", connection);
                    using (MySqlDataReader reader = getgoodscommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {

                                count = reader.GetInt32(0);

                            }
                        }
                    }

                    MySqlCommand removeordercommand = new MySqlCommand($"USE {_database};DELETE FROM Orders WHERE Orders.Id = {id}", connection);
                    await removeordercommand.ExecuteNonQueryAsync();
                    MySqlCommand goodschangecommand = new MySqlCommand($"USE {_database};UPDATE Goods SET Goods.Count = Goods.Count + {count} ", connection);
                    goodschangecommand.Transaction = transaction;
                    result = await goodschangecommand.ExecuteNonQueryAsync();
                    transaction.Commit();
                }
                catch 
                {
                    await transaction.RollbackAsync();
                    return false;

                }
            }
            return result > 0;
        }

      
    }
}
