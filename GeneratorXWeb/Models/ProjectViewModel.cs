using System.ComponentModel.DataAnnotations;

namespace GeneratorXWeb.Models
{
    public class ProjectViewModel
    {
        public string? ProjectName { get; set; }
        public string? ProjectDisplayName { get; set; }
        public bool HasMakerChecker { get; set; }
        public List<FeatureViewModel>? FeatureList { get; set; }

    }
}
