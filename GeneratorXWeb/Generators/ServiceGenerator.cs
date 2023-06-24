using GeneratorXWeb.Helpers;
using GeneratorXWeb.Models;

namespace GeneratorXWeb.Generators
{
    public class ServiceGenerator
    {
        private readonly GenericHelper genericHelper;
        private readonly EnumHelper enumHelper;
        public ServiceGenerator(GenericHelper genericHelper, EnumHelper enumHelper)
        {
            this.genericHelper = genericHelper;
            this.enumHelper = enumHelper;
        }


        public bool GenerateService(ProjectViewModel project) {
            
            
            project.FeatureList!.ForEach(feature =>
            {
                CreateService( feature,project.ProjectName!);
                CreateServiceInterface( feature,project.ProjectName!);
                CreateServicePrivates( feature, project.ProjectName!);

            });

            return true;
        }

        //Service
        private bool CreateService( FeatureViewModel feature, string projectName)
        {
            var builder = new ClassBuilder();
            var area = enumHelper.GetEnumDescription(feature.ProjectArea!);
            builder += $"using {projectName}Core.Models.Data.{projectName}DbContext;";
            builder += $"using {projectName}Core.Extensions;";
            builder += $"using {projectName}Core.Models.ViewModels;";
            builder += $"using {projectName}Core.UnitsOfWork.IUnitOfWork;";
            builder += $"using {projectName}Core.Services.IServices;";
            builder += "using Microsoft.AspNetCore.Mvc.Rendering;";
            builder += "using System;";
            builder += "using System.Collections.Generic;";
            builder += "using System.Linq;";

            builder.AddLine();
            builder.AddLine();
            builder += $"namespace {projectName}Core.Services";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder.AddLine();
            builder += $"public partial class {feature.FeatureName}Service : I{feature.FeatureName}Service";
            builder += "{";
            builder.Indent();
            builder += $"private readonly I{area}UnitOfWork _{area.ToLower()}UnitOfWork;";
            builder += "private readonly IApprovalService _approvalService;";
            builder.AddLine();
            builder.AddLine();
            builder += $"public {feature.FeatureName}Service(I{area}UnitOfWork {area.ToLower()}UnitOfWork, IApprovalService approvalService)";
            builder += "{";
            builder.Indent();
            builder += $"{area.ToLower()}UnitOfWork= {area.ToLower()}UnitOfWork;";
            builder += $"_approvalService = approvalService;";
            builder.Dedent();
            builder += "}";
            builder.AddLine();
            builder.AddLine();
            //Approve
            builder.AddLine();
            builder += $"public void Approve{feature.FeatureName}(ApprovalViewModel viewModel)";
            builder += "{";
            builder.Indent();
            builder += $"viewModel.FeatureName = FeatureEnum.{feature.FeatureName}.GetDescription();";
            builder += $"var {genericHelper.ToCamel(feature.FeatureName!)}=_{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Find((int)viewModel.RecordId);";
            builder += $"viewModel.CurrentActiveFlag = {genericHelper.ToCamel(feature.FeatureName!)}.ActiveFlag.Value;";
            builder += "int newActiveFlag = _approvalService.ProcessApproval(viewModel);";
            builder += $"{genericHelper.ToCamel(feature.FeatureName!)}.ActiveFlag = newActiveFlag;";
            builder += $"_{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Update({genericHelper.ToCamel(feature.FeatureName!)});";
            builder += $"var result = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.SaveChanges();";
            builder += $"if (result != (1 + (string.IsNullOrEmpty(viewModel.Comment) ? 0 : 1)))";
            builder += $"throw new BaseException(\"Failed to authorize {feature.FeatureDisplayName}\")";
            builder.Dedent();
            builder += "}";

            builder.AddLine();
            builder.AddLine();
            //SubmitAdd
            builder += $"public void Add{feature.FeatureName}({feature.FeatureName}AddEditViewModel viewModel)";
            builder += "{";
            builder.Indent();
            builder += $"{feature.FeatureName} model = ConvertViewModelTo{feature.FeatureName}(viewModel);";
            builder += "model.ActiveFlag = (int)ActiveFlagEnum.NewRecord;";
            builder += $"_{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Create(model);";
            builder += $"var result = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.SaveChanges();";
            builder += $"if (result != 1)";
            builder += $"throw new BaseException(\"Failed to add {feature.FeatureDisplayName}\");";
            builder.Dedent();
            builder += "}";



            builder.AddLine();
            builder.AddLine();
            //UpdateViewModel
            builder += $"public {feature.FeatureName}AddEditViewModel GetUpdateViewModel(long {genericHelper.ToCamel(feature.FeatureName!)}Id)";
            builder += "{";
            builder.Indent();
            builder += $" var model = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Find({genericHelper.ToCamel(feature.FeatureName!)}Id);";
            builder += $"var viewModel =Convert{feature.FeatureName}ToAddEditViewModel(model);";
            builder += "return viewModel;";
            builder.Dedent();
            builder += "}";


            builder.AddLine();
            builder.AddLine();
            //SubmitUpdate
            builder += $"public void Update{feature.FeatureName}({feature.FeatureName}AddEditViewModel viewModel)";
            builder += "{";
            builder.Indent();
            builder += $" var model = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Find(viewModel.{feature.FeatureName}Id);";
            builder += $"model = ConvertEditViewModelTo{feature.FeatureName}(model, viewModel);";
            builder += "if (model.ActiveFlag != (int)ActiveFlagEnum.NewRecord)";
            builder += "model.ActiveFlag = (int)ActiveFlagEnum.MarkedForReactivation;";
            builder += $"_{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Update(model);";
            builder += $"var result = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.SaveChanges();";
            builder += $"if (result != 1)";
            builder += $"throw new BaseException(\"Failed to update {feature.FeatureDisplayName}\");";
            builder.Dedent();
            builder += "}";


            builder.AddLine();
            builder.AddLine();
            //GetGrid
            builder += $"public List<{feature.FeatureName}GetViewModel> Get{feature.FeatureName}List()";
            builder += "{";
            builder.Indent();
            builder += $" var model = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.GetAll().ToList();";
            builder += $"var viewModel =Convert{feature.FeatureName}ToViewModel(model);";
            builder += "return viewModel;";
            builder.Dedent();
            builder += "}";


            builder.AddLine();
            builder.AddLine();
            //GetDetail
            builder += $"public {feature.FeatureName}DetailViewModel Get{feature.FeatureName}List()";
            builder += "{";
            builder.Indent();
            builder += $" var model = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.GetAll().ToList();";
            builder += $"var viewModel =Convert{feature.FeatureName}ToDetailViewModel(model);";
            builder += "return viewModel;";
            builder.Dedent();
            builder += "}";



            builder.AddLine();
            builder.AddLine();
            //Delete
            builder += $"public void Delete{feature.FeatureName}({feature.FeatureName}DeleteViewModel viewModel)";
            builder += "{";
            builder.Indent();
            builder += $" var model = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Find(viewModel.{feature.FeatureName}Id);";
            builder += "model.ActiveFlag = (int)ActiveFlagEnum.MarkedForDeletion;";
            builder += "model.LastDateUpdated = DateTime.Now;";
            builder += "model.LastUpdateAppUserId = viewModel.AppUserId;";
            builder += $"_{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.Update(model);";
            builder += $"var result = _{area.ToLower()}UnitOfWork.{feature.FeatureName}Repository.SaveChanges();";
            builder += $"if (result != 1)";
            builder += $"   throw new BaseException(\"Failed to update {feature.FeatureDisplayName}\");";
            builder.Dedent();
            builder += "}";

            builder.AddLine();
            builder.AddLine();
            //Selectlist
            feature.FieldList!.ForEach(field =>
            {
                if (field.ValueSource == ValueSource.LookUp)
                {

                    builder += $"public SelectList Get{field.FieldName}SelectList()";
                    builder += "{";
                    builder.Indent();
                    builder += $" return _{area.ToLower()}UnitOfWork.LookupRepository.GetLookUpSelectList((int)LookUpCodeIdEnum.{Enum.GetName((LookUpCodeId)field.LookUpCodeId!)});";
                    builder.Dedent();
                    builder += "}";
                }
            if (field.ValueSource == ValueSource.OtherFeature)
                {
                    builder += $"public SelectList Get{field.FieldName}SelectList()";
                    builder += "{";
                    builder.Indent();
                    builder += $" return _{area.ToLower()}UnitOfWork.{field.SourceFeature}Repository.GetLookUpSelectList((int)LookUpCodeIdEnum.{Enum.GetName((LookUpCodeId)field.LookUpCodeId!)});";
                    builder.Dedent();
                    builder += "}";
                }
            });
            builder.Dedent();
            builder += "}"; 
            builder.Dedent();
            builder += "}";

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\GeneratedCode\\{projectName}Core\\Services";
            Directory.CreateDirectory(docPath);
            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, feature.FeatureName + "Service.cs"), true))
            {
                outputFile.Write(builder.ToString());
            }
            return true;
            
        }

        //Service Interface
        private bool CreateServiceInterface(FeatureViewModel feature, string projectName)
        {
            var builder = new ClassBuilder();
            var area = enumHelper.GetEnumDescription(feature.ProjectArea!);
            builder += $"using {projectName}Core.Models.ViewModels;";
            builder += "using Microsoft.AspNetCore.Mvc.Rendering;";
            builder += "using System.Collections.Generic;";;

            builder.AddLine();
            builder.AddLine();
            builder += $"namespace {projectName}Core.Services";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder.AddLine();
            builder += $"public interface I{feature.FeatureName}Service";
            builder += "{";
            builder.Indent();
            builder += $"void Approve{feature.FeatureName}(ApprovalViewModel viewModel);";
            builder += $"void Add{feature.FeatureName}({feature.FeatureName}AddEditViewModel viewModel);";
            builder += $"{feature.FeatureName}AddEditViewModel GetUpdateViewModel(long {genericHelper.ToCamel(feature.FeatureName!)}Id);";
            builder += $"void Update{feature.FeatureName}({feature.FeatureName}AddEditViewModel viewModel);";
            builder += $"List<{feature.FeatureName}GetViewModel> Get{feature.FeatureName}List();";
            builder += $"{feature.FeatureName}DetailViewModel Get{feature.FeatureName}Detail();";
            builder += $"void Delete{feature.FeatureName}({feature.FeatureName}DeleteViewModel viewModel);";

            feature.FieldList!.ForEach(field =>
            {
                if (field.ValueSource == ValueSource.LookUp|| field.ValueSource == ValueSource.OtherFeature)
                {
                    builder += $"SelectList Get{field.FieldName}SelectList();";
                }
                
            });

            builder.Dedent();
            builder += "}";
            builder.Dedent();
            builder += "}";

            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\GeneratedCode\\{projectName}Core\\Services\\IServices";
            Directory.CreateDirectory(docPath);
            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "I"+feature.FeatureName + "Service.cs"), true))
            {
                outputFile.Write(builder.ToString());
            }
            return true;
        }

        //Service Privates
        private bool CreateServicePrivates( FeatureViewModel feature, string projectName)
        {
            var builder = new ClassBuilder();
            var area = enumHelper.GetEnumDescription(feature.ProjectArea!);
            builder += "using System.Linq;";
            builder += $"using {projectName}Core.Models.Data.{projectName}DbContext;";
            builder += $"using {projectName}Core.Models.ViewModels;";
            builder += "using Microsoft.AspNetCore.Mvc.Rendering;";
            builder += "using System;";
            builder += "using System.Collections.Generic;";

            builder.AddLine();
            builder.AddLine();
            builder += $"namespace {projectName}Core.Services";
            builder += "{";
            builder.Indent();
            builder.AddLine();
            builder.AddLine();
            builder += $"public partial class {feature.FeatureName}Service";
            builder += "{";
            builder.Indent();
            builder.AddLine();



            builder.AddLine();
            builder.AddLine();
            //Model to Grid/Get View Model
            builder += $"private List<{feature.FeatureName}GetViewModel> Convert{feature.FeatureName}ToViewModel(List<{feature.FeatureName}> model)";
            builder += "{";
            builder.Indent();
            builder += $"return model.Select(x => new {feature.FeatureName}GetViewModel";
            builder += "{";
            builder.Indent();
            feature.FieldList!.ForEach(field => {
                
                if (field.ShowOnGrid)
                {

                    builder += $"{field.FieldName}=x.{field.FieldName},";
                    if (field.ValueSource == ValueSource.LookUp)
                        builder += $"_{area.ToLower()}UnitOfWork.LookUpRepository.GetLookUpName(x.{field.FieldName}),";
                    if (field.ValueSource == ValueSource.OtherFeature)
                        builder += $"_{area.ToLower()}UnitOfWork.{field.SourceFeature}Repository.Get{field.SourceFeature}Name(x.{field.SourceFeature}Id),";
                }

            });
            builder.Dedent();
            builder += "}).ToList();";
            builder.Dedent();
            builder += "}";


            builder.AddLine();
            builder.AddLine();
            //Model to Detail View Model
            builder += $"private {feature.FeatureName}DetailViewModel Convert{feature.FeatureName}ToViewModel({feature.FeatureName} model)";
            builder += "{";
            builder.Indent();
            builder += $"return  new {feature.FeatureName}GetViewModel";
            builder += "{";
            builder.Indent();
            builder += $"{feature.FeatureName}Id = model.{feature.FeatureName}Id,";
            feature.FieldList!.ForEach(field => {
                
                if (field.ShowOnDetailScreen)
                {

                    builder += $"{field.FieldName}=model.{field.FieldName},";
                    if (field.ValueSource == ValueSource.LookUp)
                        builder += $"_{area.ToLower()}UnitOfWork.LookUpRepository.GetLookUpName(model.{field.FieldName}),";
                    if (field.ValueSource == ValueSource.OtherFeature)
                        builder += $"_{area.ToLower()}UnitOfWork.{field.SourceFeature}Repository.Get{field.SourceFeature}Name(model.{field.SourceFeature}Id),";
                }

            });
            builder.Dedent();
            builder += "};";
            builder.Dedent();
            builder += "}";


            builder.AddLine();
            builder.AddLine();
            // Model to Add Edit View Model
            builder += $"private {feature.FeatureName}DetailViewModel Convert{feature.FeatureName}ToViewModel({feature.FeatureName} model)";
            builder += "{";
            builder.Indent();
            builder += $"return new {feature.FeatureName}GetViewModel";
            builder += "{";
            builder.Indent();
            builder += $"{feature.FeatureName}Id = model.{feature.FeatureName}Id,";
            feature.FieldList!.ForEach(field => {
                
                
                    builder += $"{field.FieldName}=model.{field.FieldName},";

            });
            builder.Dedent();
            builder += "};";
            builder.Dedent();
            builder += "}";

            builder.Dedent();
            builder += "}";
            builder.Dedent();
            builder += "}";
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\GeneratedCode\\{projectName}Core\\Services";
            Directory.CreateDirectory(docPath);
            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, feature.FeatureName + "ServicePrivates.cs"), true))
            {
                outputFile.Write(builder.ToString());
            }
            return true;
        }

    }
}
