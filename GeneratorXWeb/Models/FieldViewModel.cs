using System.ComponentModel.DataAnnotations;

namespace GeneratorXWeb.Models
{
    public class FieldViewModel
    {
        public string? FieldName { get; set; }
        public string? DisplayName { get; set; }
        [EnumDataType(typeof(DataType))]
        public DataType DataType { get; set; }
        [EnumDataType(typeof(ValueSource))]
        public ValueSource ValueSource { get; set; }
        [EnumDataType(typeof(LookUpCodeId))]
        public LookUpCodeId LookUpCodeId { get; set; }
        public string? SourceFeature { get; set; }
        public bool Required { get; set; }
        public bool ShowOnGrid { get; set; }
        public bool ShowOnDetailScreen { get; set; }
        public bool Hidden { get; set; }

    }
}
