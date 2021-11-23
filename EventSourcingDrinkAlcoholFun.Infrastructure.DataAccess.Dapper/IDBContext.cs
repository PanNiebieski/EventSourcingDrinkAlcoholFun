namespace EventSourcingDrinkAlcoholFun.Infrastructure.DataAccess.Dapper.Repositories
{
    public interface IDBContext
    {
        string ConnectionString { get; }
    }

    public class DBContext : IDBContext
    {
        public DBContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}