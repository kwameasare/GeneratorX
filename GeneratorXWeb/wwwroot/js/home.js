var fieldList = [];
var featureList = [];
var featureNameList = [];

$(document).ready(function () {
    GetGridList(JSON.stringify(featureList));

});

function getFormData(data) {
    var unindexed_array = data;
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function AddField() {
    var lookUpCodeId;
    var fieldName = $("#fieldName").val();
    var displayName = $("#displayName").val();
    if (valueSource == 3) {
        lookUpCodeId = parseInt($("#lookUpCodeId").val());
    }
    
    if (lookUpCodeId == null) {
        lookUpCodeId=999
    }
    var sourceFeature = $("#sourceFeature").val();
    var dataType = parseInt($("#dataType").val());
    var valueSource = parseInt($("#valueSource").val());
    var showOnGrid = $("#showOnGrid").is(":checked");
    var showOnDetailScreen = $("#showOnDetailScreen").is(":checked");
    var hidden = $("#hidden").is(":checked");
    var mField = {};
    mField = {
        FieldName: fieldName,
        DisplayName: displayName,
        DataType: dataType,
        ValueSource: valueSource,
        SourceFeature: sourceFeature,
        LookUpCodeId: lookUpCodeId,
        ShowOnGrid: showOnGrid,
        ShowOnDetailScreen: showOnDetailScreen,
        Hidden: hidden
    };
    fieldList.push(mField);
    var markup = "<tr><td><input type='checkbox' name='record'></td><td>" + fieldName + "</td><td>" + displayName + "</td><td>" + dataType + "</td><td>" + valueSource + "</td><td>" + showOnGrid + "</td><td>" + showOnDetailScreen + "</td><td>" + hidden + "</td></tr>";
    $("table tbody").append(markup);
}

// Find and remove selected table rows
$(".delete-row").click(function () {
    $("table tbody").find('input[name="record"]').each(function (index) {
        if ($(this).is(":checked")) {

            fieldList.splice(index, 1);
            $(this).parents("tr").remove();
        }
    });
});
function CheckSource() {
    var selectedSource = $("#valueSource").val();

    if (selectedSource == "2") {
        $("#sFeature").css("display", "block");
        $("#lookUp").css("display", "none");
    }
    else if (selectedSource == "3") {
        $("#sFeature").css("display", "none");
        $("#lookUp").css("display", "block");
    }
    else {
        $("#sFeature").css("display", "none");
        $("#lookUp").css("display", "none");
    }
}

function AddFeature() {
    var nameListJson = JSON.stringify(featureNameList);
    $.ajax({
        url: `/Home/AddFeature`,
        type: 'Get',
        data: { "featureNameList": nameListJson },
        beforeSend: function () {

        },
        complete: function () {

        },
        error: function (xhr) {

            AjaxErrorHandler(xhr);
        },
        success: function (response) {
            fieldList = [];
            $("#modalBodyFull").html(response);
            var title = $("#FeatureDetailTitle").html();
            $("#modalTitleFull").html(title);
            $("#partialModalFull").modal('show');
        }
    });

}

function GetGridList(list) {

    $.ajax({
        url: `/Home/GetGridList`,
        type: 'POST',
        data: { "featureList": list },
        beforeSend: function () {

        },
        complete: function () {

        },
        error: function (xhr) {

            AjaxErrorHandler(xhr);
        },
        success: function (response) {
            $("#dataTable").html(response);

            $("#newGrid").DataTable();
        }
    });
    //Ajax Call Here//
}

function SubmitFeature() {
    var waiveMakerChecker = $("#waiveMakerChecker").is(":checked");
    var projectArea = parseInt($("#projectArea").val());
    var feature = {
        FeatureName: $("#featureName").val(),
        FeatureDisplayName: $("#featureDisplayName").val(),
        WaiveMakerChecker: waiveMakerChecker,
        ProjectArea: projectArea,
        FieldList: fieldList
    }
    featureList.push(feature);
    featureNameList.push(feature.FeatureName);
    $("#closeModal").click();
    $('#partialModalFull').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();

    GetGridList(JSON.stringify(featureList));
}

function SubmitProject() {
    var hasMakerChecker = $("#hasMakerChecker").is(":checked");
    var project = {
        ProjectName: $("#projectName").val(),
        ProjectDisplayName: $("#projectDisplayName").val(),
        HasMakerChecker: hasMakerChecker,
        FeatureList: featureList
    }

    var projectJson = JSON.stringify(project);

    $.ajax({
        url: `/Home/AddProject`,
        type: 'POST',
        data: { "project": projectJson },
        beforeSend: function () {

        },
        complete: function () {

        },
        error: function (xhr) {

            AjaxErrorHandler(xhr);
        },
        success: function (response) {
            toastr.success(response, 'Successfull');
            //location.reload();
        }
    });
    //Ajax Call Here//

}