using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace YFS.Data.Models
{
    public class AccountGroupRepository : IAccountGroupRepository
    {
        private readonly string _connectionString;
        public AccountGroupRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
        public Task<AccountGroup> Add(AccountGroup entity)
        {
            throw new NotImplementedException();
        }

        public Task<AccountGroup> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountGroup> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccountGroup> GetAllByUser(int userid)
        {
            var sql = "SELECT * FROM AccountGroup WHERE UserId = @userid";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<AccountGroup>(sql, new { userid });
            }
        }

        public Task<AccountGroup> Update(AccountGroup entity)
        {
            throw new NotImplementedException();
        }


        /*   

      public AccountGroup Get(int accgrpid)
      {
          using (var connection = new SqlConnection(_connectionString))
          {
              connection.Open();
              return null;
              //return connection.Query<AccountGroup>("SELECT * FROM AccountGroup WHERE AccountGroupId = @ACCGRPID", new { accgrpid });
          }
      }

      public IEnumerable<AccountGroup> GetAccountGroups(int userid)
      {
          using (var connection = new SqlConnection(_connectionString))
          {
              connection.Open();
              return connection.Query<AccountGroup>("SELECT * FROM AccountGroup WHERE UserId = @userid", new { userid });
          }
      }

      public void Update(AccountGroup accgrp)
      {
          throw new NotImplementedException();
      }
*/
    }
}
