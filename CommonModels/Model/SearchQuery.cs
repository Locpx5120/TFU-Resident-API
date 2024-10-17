using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class SearchQuery
    {
        public SearchQuery()
        {
            Keyword = "";
            PageNumber = 1;
            PageSize = 10;
            IsActive = true;
            IsDeleted = false;
        }
        [DefaultValue("")]
        public string Keyword { get; set; } = string.Empty;
        [Required]
        [DefaultValue(1)]
        public int PageNumber { get; set; } = 1;
        [Required]
        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;

        [DefaultValue(null)]
        public bool? IsActive { get; set; }

        [DefaultValue(null)]
        public bool? IsDeleted { get; set; }

        [DefaultValue(null)]
        [JsonProperty("filter")]
        public SearchFilter? Filter { get; set; }
    }

    public class SearchQueryComboSetting : SearchQuery
    {
        [DefaultValue(null)]
        public bool? IsPublic { get; set; }
        public bool? IsViewAll { get; set; }
    }


    public class SearchFilter
    {
        [JsonProperty("field")]
        public string? Field { get; set; }
        [JsonProperty("value")]
        public string? Value { get; set; }
    }
}
