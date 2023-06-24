using GeneratorXWeb.Helpers;
using GeneratorXWeb.Models;
using PluralizeService.Core;

namespace GeneratorXWeb.Generators
{
    public class RepositoryGenerator
    {
        private readonly GenericHelper genericHelper;
        private readonly EnumHelper enumHelper;
        public RepositoryGenerator(GenericHelper genericHelper, EnumHelper enumHelper)
        {
            this.genericHelper = genericHelper;
            this.enumHelper = enumHelper;
        }


        public bool GenerateRepository(ProjectViewModel project)
        {


            project.FeatureList!.ForEach(feature =>
            {
                CreateRepository(feature, project.ProjectName!);
                CreateRepositoryInterface(feature, project.ProjectName!);

            });

            return true;
        }

        //Repository
        private bool CreateRepository(FeatureViewModel feature, string projectName)
        {
            var builder = new ClassBuilder();
            var area = enumHelper.GetEnumDescription(feature.ProjectArea!);
            builder += $"using {projectName}Core.Models.Data.{projectName}DbContext;";
            builder += $"using {projectName}Core.Repositories.IRepositories;";
            builder += $"using {projectName}Core.Models.ViewModels;";
            builder += "using Microsoft.AspNetCore.Mvc.Rendering;";
            builder += "using System;";
            builder += "using System.Collections.Generic;";
            builder += "using System.Linq;";

            builder.AddLine();
            builder.AddLine();
            builder += $"namespace {projectName}Core.Repositories";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder.AddLine();
            builder += $"public class {feature.FeatureName}Repository :Repository<{feature.FeatureName}>, I{feature.FeatureName}Repository";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder.AddLine();
            builder += $"public {feature.FeatureName}Repository({projectName}DbContext context) : base(context)";
            builder += "{";
            builder.AddLine();
            builder += "}";
            builder.AddLine();
            builder.AddLine();


            builder.AddLine();
            builder.AddLine();
            //Selectlist


            builder += $"public SelectList Get{feature.FeatureName}SelectList()";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder += $"var model =_appContext.{PluralizationProvider.Pluralize(feature.FeatureName)}";
            builder += ".Where(x => x.ActiveFlag != (int)ActiveFlagEnum.Deleted).";
            builder += $"Select(x => new {{ x.{feature.FeatureName}Id, x.{feature.FeatureName}Name }})";
            builder += $".OrderBy(x => x.{feature.FeatureName}Name).ToList();";
            builder.AddLine();
            builder.AddLine();
            builder += $" return new SelectList(model, \"{feature.FeatureName}Id\", \"{feature.FeatureName}Name\");";
            builder.Dedent();
            builder += "}";


            builder.Dedent();
            builder += "}";
            builder.Dedent();
            builder += "}";

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\GeneratedCode\\{projectName}Core\\Repositories";
            Directory.CreateDirectory(docPath);
            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, feature.FeatureName + "Repository.cs"), true))
            {
                outputFile.Write(builder.ToString());
            }
            return true;

        }

        //Repository Interface
        private bool CreateRepositoryInterface(FeatureViewModel feature, string projectName)
        {
            var builder = new ClassBuilder();
            builder += $"using {projectName}Core.Models.ViewModels;";
            builder += "using Microsoft.AspNetCore.Mvc.Rendering;";
            builder += "using System.Collections.Generic;";

            builder.AddLine();
            builder.AddLine();
            builder += $"namespace {projectName}Core.Repositories";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder.AddLine();
            builder += $"public interface I{feature.FeatureName}Repository";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder.AddLine();
            builder += $"SelectList Get{feature.FeatureName}SelectList();";
            builder.AddLine();
            builder.AddLine();



            builder.Dedent();
            builder += "}";
            builder.Dedent();
            builder += "}";

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\GeneratedCode\\{projectName}Core\\Repositories\\IRepositories";
            Directory.CreateDirectory(docPath);
            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "I" + feature.FeatureName + "Repository.cs"), true))
            {
                outputFile.Write(builder.ToString());
            }
            return true;
        }



    }
}
