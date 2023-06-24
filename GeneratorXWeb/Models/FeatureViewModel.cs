using System.ComponentModel.DataAnnotations;

namespace GeneratorXWeb.Models
{
    public class FeatureViewModel
    {
        public string? FeatureName { get; set; }
        public string? FeatureDisplayName { get; set; }
        public bool WaiveMakerChecker { get; set; }
        [EnumDataType(typeof(ProjectArea))]
        public ProjectArea? ProjectArea { get; set; }
        public List<FieldViewModel>? FieldList { get; set; }
        public List<string>? FeatureNameList { get; set; }

    }
}
