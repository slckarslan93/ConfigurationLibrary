namespace ConfigurationLibrary.UI.Models.Pagination
{
    public class BasePaginationModel
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? OrderBy { get; set; }
        public bool IsDesc { get; set; }
    }
}
