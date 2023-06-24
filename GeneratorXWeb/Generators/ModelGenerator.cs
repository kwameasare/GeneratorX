using GeneratorXWeb.Helpers;
using GeneratorXWeb.Models;

namespace GeneratorXWeb.Generators
{
    public class ModelGenerator
    {
        private readonly EnumHelper enumHelper;
        public ModelGenerator(GenericHelper genericHelper, EnumHelper enumHelper)
        {

            this.enumHelper = enumHelper;
        }



        public bool GenerateModel(ProjectViewModel project)
        {

            project.FeatureList!.ForEach(feature =>
            {
                var builder = new ClassBuilder();
                var enumHelper = new EnumHelper();

                builder += "using System.ComponentModel.DataAnnotations;";
                builder.AddLine();
                CreateGetViewModel(builder, feature);

                builder.AddLine();
                builder.AddLine();
                CreateDeatilViewModel(builder, feature);

                builder.AddLine();
                builder.AddLine();
                CreateAddEditViewModel(builder, feature);

                builder.AddLine();
                builder.AddLine();
                CreateDeleteViewModel(builder, feature.FeatureName!);

                // Set a variable to the Documents path.
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\GeneratedCode\\{project.ProjectName}Core\\Models\\ViewModels";
                Directory.CreateDirectory(docPath);
                // Append text to an existing file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, feature.FeatureName + "ViewModel.cs"), true))
                {
                    outputFile.Write(builder.ToString());
                }
            });
            return true;
        }
        private void CreateGetViewModel(ClassBuilder classBuilder, FeatureViewModel feature)
        {
            classBuilder += $"public class {feature.FeatureName}GetViewModel {{";
            classBuilder.AddLine();
            classBuilder.Indent();
            classBuilder += $"public int {feature.FeatureName}Id {{get; set;}}";
            if (feature.FieldList != null) feature.FieldList.ForEach(field =>
            {
                if (field.ShowOnGrid)
                {
                    classBuilder += $"public {enumHelper.GetEnumDescription(field.DataType!)} {field.FieldName} {{get; set;}}";
                    if (field.ValueSource == ValueSource.LookUp || field.ValueSource == ValueSource.OtherFeature) classBuilder += $"public string {field.FieldName}Name {{get; set;}}";
                }
            });

            classBuilder += "public int ActiveFlag { get; set; }";
            classBuilder += "public string ActiveFlagColorCode { get; set; }";
            classBuilder.Dedent();
            classBuilder += "}";

        }
        private void CreateDeatilViewModel(ClassBuilder classBuilder, FeatureViewModel feature)
        {
            classBuilder += $"public class {feature.FeatureName}DetailViewModel {{";
            classBuilder.AddLine();
            classBuilder.Indent();
            classBuilder += $"public int {feature.FeatureName}Id {{get; set;}}";
            if (feature.FieldList != null) feature.FieldList.ForEach(field =>
            {
                if (field.ShowOnDetailScreen)
                {
                    classBuilder += $"public {enumHelper.GetEnumDescription(field.DataType!)} {field.FieldName} {{get; set;}}";
                    if (field.ValueSource == ValueSource.LookUp || field.ValueSource == ValueSource.OtherFeature) classBuilder += $"public string {field.FieldName}Name {{get; set;}}";
                }
            });

            classBuilder += "public int ActiveFlag { get; set; }";
            classBuilder += "public string ActiveFlagColorCode { get; set; }";
            classBuilder.Dedent();
            classBuilder += "}";

        }

        private void CreateAddEditViewModel(ClassBuilder classBuilder, FeatureViewModel feature)
        {
            classBuilder += $"public class {feature.FeatureName}AddEditViewModel {{";
            classBuilder.AddLine();
            classBuilder.Indent();
            classBuilder += $"public int {feature.FeatureName}Id {{get; set;}}";
            if (feature.FieldList != null) feature.FieldList.ForEach(field =>
            {
                if (field.Required) classBuilder += $"[Required(ErrorMessage = \"{field.DisplayName} name is required\")]";
                classBuilder += $"public {enumHelper.GetEnumDescription(field.DataType!)} {field.FieldName} {{get; set;}}";
                if (field.ValueSource == ValueSource.LookUp || field.ValueSource == ValueSource.OtherFeature) classBuilder += $"public SelectList {field.FieldName}SelectList {{get; set;}}";
            });
            classBuilder += "public long AppUserId { get; set; }";
            classBuilder += "public string Guid { get; set; }";
            classBuilder += "public int ActiveFlag { get; set; }";
            classBuilder += "public long AppUserId { get; set; }";
            classBuilder.Dedent();
            classBuilder += "}";

        }

        private void CreateDeleteViewModel(ClassBuilder classBuilder, string featureName)
        {
            classBuilder += $"public class {featureName}DeleteGetViewModel {{";
            classBuilder.AddLine();
            classBuilder.Indent();
            classBuilder += $"public int {featureName}Id {{get; set;}}";
            classBuilder += "public long AppUserId { get; set; }";
            classBuilder.Dedent();
            classBuilder += "}";

        }
    }
}
