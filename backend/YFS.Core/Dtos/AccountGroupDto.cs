using System.ComponentModel.DataAnnotations;

namespace YFS.Core.Dtos
{
    public class AccountGroupDto
    {
        public int AccountGroupId { get; set; }
        public string? UserId { get; set; }
        public string? AccountGroupNameRu { get; set; }

        public string? AccountGroupNameEn { get; set; }

        public string? AccountGroupNameUa { get; set; }

        public int GroupOrederBy { get; set; }
    }

    /*
    public abstract class AccountGroupUpdateDto
    {
        [Required(ErrorMessage = "AccountGroupNameEn is a required field.")]
        public string? AccountGroupNameEn { get; set; }
    }*/
}
