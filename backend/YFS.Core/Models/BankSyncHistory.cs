using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace YFS.Core.Models
{
    public class BankSyncHistory
    {
        public int Id { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Country { get; set; }  // Added to specify the country of the synchronization
    }
}
