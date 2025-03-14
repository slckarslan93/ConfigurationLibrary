using ConfigurationLibrary.UI.Models.Pagination;

namespace ConfigurationLibrary.UI.Models.Configuration
{
    public class ConfigurationPaginationModel : BasePaginationModel
    {
        public List<ConfigurationModel> Records { get; set; } = new();
        public int TotalPages { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
