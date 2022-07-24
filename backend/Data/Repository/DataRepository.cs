using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YFS.Data.Models;

namespace YFS.Data
{
    public class DataRepository //: IDataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IEnumerable<UserGetManyResponse> GetUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<UserGetManyResponse>(@"EXEC dbo.Users.....");
            }
        }

        public IEnumerable<UserGetManyResponse> GetUsersBySearch(string search)
        {
           using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<UserGetManyResponse>("EXEC dbo.Users");
            }
        }

       /* IEnumerable<UserGetManyResponse> IDataRepository.GetUsers()
        {
            throw new NotImplementedException();
        }

        IEnumerable<UserGetManyResponse> IDataRepository.GetUsersBySearch(string search)
        {
            throw new NotImplementedException();
        }*/
    }
}
