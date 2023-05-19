namespace YFS.Core.Dtos
{
    public class AccountTypeDto
    {
        public int AccountTypeId { get; set; }
        public string? NameUa { get; set; }
        public string? NameRu { get; set; }
        public string? NameEn { get; set; }
        public string? NoteUa { get; set; }
        public string? NoteEn { get; set; }
        public int TypeOrederBy { get; set; }
    }
}
