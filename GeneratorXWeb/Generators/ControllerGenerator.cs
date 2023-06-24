public class ControllerGenerator
{
    private readonly ClassBuilder _classBuilder;
    private readonly string _projectName;
    private readonly string _featureName;

    public ControllerGenerator(string projectName, string featureName)
    {
        _classBuilder = new ClassBuilder();
        _projectName = projectName;
        _featureName = featureName;
    }

    public string Generate()
    {
        _classBuilder
            .AddUsingDirective("System")
            .AddUsingDirective("System.Collections.Generic")
            .AddUsingDirective("System.Linq")
            .AddUsingDirective("System.Threading.Tasks")
            .AddUsingDirective($"{_projectName}.Extensions")
            .AddUsingDirective($"{_projectName}.Models.ModuleClaims")
            .AddUsingDirective($"{_projectName}.Models.ViewModels")
            .AddUsingDirective($"{_projectName}.Services.IServices")
            .AddUsingDirective("Microsoft.AspNetCore.Authorization")
            .AddUsingDirective("Microsoft.AspNetCore.Http")
            .AddUsingDirective("Microsoft.AspNetCore.Mvc")
            .StartNamespace($"{_projectName}.Areas.Masters.Controllers")
            .StartClassDeclaration($"{_featureName}Controller", "Controller")
            .AddPrivateReadonlyField($"I{_featureName}Service", $"_{_featureName.ToLower()}Service")
            .StartConstructor($"{_featureName}Controller", $"I{_featureName}Service {_featureName.ToLower()}Service")
            .AssignFieldFromParameter($"{_featureName.ToLower()}Service")
            .EndAction()
            .AddAttribute("HttpGet")
            .AddMethod("IActionResult", "Index", $"ViewData[\"{_featureName.ToLower()}\"] = \"active\";\n\nreturn View();")
            .AddAttribute("HttpGet")
            .AddMethod("IActionResult", "GetGridList", $"var viewModel = _{_featureName.ToLower()}Service.Get{_featureName}List();\n\nreturn PartialView(viewModel);")
            .AddAttribute("HttpGet")
            .AddAttribute("Authorize", $"(Permissions.{_featureName}.Save)")
            .AddMethod("IActionResult", "Add", $"ViewBag.detailTitle = \"Add {_featureName}\";\n\nreturn PartialView(new {_featureName}AddEditViewModel {{ Guid = UniqueKeys.GUIDEncryptionMask()}});")
            .AddAttribute("HttpPost")
            .AddAttribute("Authorize", $"(Permissions.{_featureName}.Save)")
            .AddMethod("IActionResult", "Add", $"{_featureName}AddEditViewModel viewModel", $"if(ModelState.IsValid)\n{{\n    viewModel.AppUserId = HttpContext.Session.Get<long>(SessionValueKeys.appUserId);\n    _{_featureName.ToLower()}Service.Add{_featureName}(viewModel);\n\n    return Content(\"Successfully added {_featureName.ToLower()}\");\n}}\n\nthrow new BaseException(GlobalFunctions.GetModelStateErrors(ModelState));")
            .AddAttribute("HttpGet")
            .AddAttribute("Authorize", $"(Permissions.{_featureName}.Edit)")
            .AddMethod("IActionResult", "Update", "int sectorId", $"ViewBag.detailTitle = \"Edit {_featureName}\";\n\nvar viewModel = _{_featureName.ToLower()}Service.GetUpdateViewModel(sectorId);\n\nreturn PartialView(viewModel);")
            .AddAttribute("HttpPost")
            .AddAttribute("Authorize", $"(Permissions.{_featureName}.Edit)")
            .AddMethod("IActionResult", "Update", $"{_featureName}AddEditViewModel viewModel", $"if (ModelState.IsValid)\n{{\n    viewModel.AppUserId = HttpContext.Session.Get<long>(SessionValueKeys.appUserId);\n    _{_featureName.ToLower()}Service.Update{_featureName}(viewModel);\n\n    return Content(\"Successfully edited {_featureName.ToLower()}\");\n}}\n\nthrow new BaseException(GlobalFunctions.GetModelStateErrors(ModelState));")
            .AddAttribute("HttpDelete")
            .AddAttribute("Authorize", $"(Permissions.{_featureName}.DeleteAction)")
            .AddMethod("IActionResult", "Delete", "int sectorId", $"{_featureName}DeleteViewModel viewModel = new {_featureName}DeleteViewModel\n{{\n    SectorId = sectorId,\n    AppUserId = HttpContext.Session.Get<long>(SessionValueKeys.appUserId)\n}};\n\n_{_featureName.ToLower()}Service.Delete{_featureName}(viewModel);\n\nreturn Content(\"Successfully deleted {_featureName.ToLower()}\");")
            .AddAttribute("HttpGet")
            .AddAttribute("Authorize", $"(Permissions.{_featureName}.Authorize)")
            .AddMethod("IActionResult", "ApproveRecord", "int recordId", "ApprovalViewModel viewModel = new ApprovalViewModel\n{\n    RecordId = recordId,\n};\nViewBag.detailTitle = \"Authorize Record\";\n\nreturn PartialView(viewModel);")
            .AddAttribute("HttpPost")
            .AddAttribute("Authorize", $"(Permissions.{_featureName}.Authorize)")
            .AddMethod("IActionResult", "ApproveRecord", "ApprovalViewModel viewModel", "if (ModelState.IsValid)\n{\n    viewModel.AppUserId = HttpContext.Session.Get<long>(SessionValueKeys.appUserId);\n    _{_featureName.ToLower()}Service.Approve{_featureName}(viewModel);\n\n    return Content($\"Successfully {(viewModel.IsApproved ? \"approved\" : \"disapproved\")} record\");\n}\n\nthrow new BaseException(GlobalFunctions.GetModelStateErrors(ModelState));")
            .EndAction()
            .EndAction();

        return _classBuilder.ToString();
    }
}
