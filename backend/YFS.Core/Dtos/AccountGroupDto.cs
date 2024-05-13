namespace YFS.Core.Dtos
{
    public class AccountGroupDto
    {
        public int AccountGroupId { get; set; }
        public string UserId { get; set; } = null!;
        public string? AccountGroupNameRu { get; set; }

        public string? AccountGroupNameEn { get; set; }

        public string? AccountGroupNameUa { get; set; }

        public int GroupOrderBy { get; set; }
    }
}
