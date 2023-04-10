using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YFS.Core.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int RootId { get; set; }
        public string? UserId { get; set; }
        public string? Name_UA { get; set; }
        public string? Name_ENG { get; set; }
        public string? Name_RU { get; set; }
        public string? Note { get; set; }
    }
}
