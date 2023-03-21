using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFS.Core.Models;

namespace YFS.Core.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        //add userid
        public int Favorites { get; set; }
        public int AccountGroupId { get; set; }
        public int CurrencyId { get; set; }
        public int BankId { get; set; }
        public string? Name { get; set; }
        public DateTime OpeningDate { get; set; }
        public string? Note { get; set; }
        public decimal Balance { get; set; }
    }
}
