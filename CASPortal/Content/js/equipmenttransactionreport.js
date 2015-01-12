$(function () {
    $("#hdnFormSubmit").val("");
    $("#chkShowActiveStations").prop("checked", true);
    $("#hdnShowActiveStations").val("true");
    $("#chkJobTimes").prop("checked", true);
    $("#hdnJobTimes").val("true");
});

function Validate() {

    var locationVal = "";
    var advanceVal = "";

    if ($("a.dropdown-Site span[data-bind='label']").text() == "Select Site") {
        alert("Please select Site.");
        $("a.dropdown-Site").fadeOut();
        $("a.dropdown-Site").fadeIn();
        return false;
    }

    if ($("a.dropdown-Contract span[data-bind='label']").text() == "Select Contract") {
        alert("Please select Contract.");
        $("a.dropdown-Contract").fadeOut();
        $("a.dropdown-Contract").fadeIn();
        return false;
    }

    if ($("#dtpFrom").val().trim().length <= 0) {
        alert("Please enter From Date.");
        $("#dtpFrom").focus();
        return false;
    }

    if ($("#dtpTo").val().trim().length <= 0) {
        alert("Please enter To Date.");
        $("#dtpTo").focus();
        return false;
    }

    var dayDiff = 0;
    dayDiff = new Date($("#dtpTo").datepicker('getDate')) - new Date($("#dtpFrom").datepicker('getDate'));

    if (dayDiff < 0) {
        alert("To date cannot be smaller than From date.");
        return false;
    }

    $("#divLoading").html("<img alt= title= src=" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loading.gif width='10%' />");

    if ($("#rdoLocation").prop("checked") == true)
        locationVal = "Location";
    if ($("#rdoSection").prop("checked") == true)
        locationVal = "Section";
    if ($("#rdoArea").prop("checked") == true)
        locationVal = "Area";
    if ($("#rdoDateTime").prop("checked") == true)
        locationVal = "DateTime";
    if ($("#rdoSingleSection").prop("checked") == true)
        locationVal = "Single Section";

    if ($("#rdoNone").prop("checked") == true)
        advanceVal = "None";
    if ($("#rdoScanned").prop("checked") == true)
        advanceVal = "Scanned";
    if ($("#rdoUnScanned").prop("checked") == true)
        advanceVal = "Un Scanned";
    if ($("#rdoNewItem").prop("checked") == true)
        advanceVal = "New Item";
    if ($("#rdoReplaced").prop("checked") == true)
        advanceVal = "Single Replaced";

    var url = $("#hdnSiteURL").val() + "/Report/EquipmentTransaction";
    $.ajax({
        url: url,
        type: "POST",
        data: {
            hdnSite: $("#hdnSite").val(), hdnContract: $("#hdnContract").val(), dtpFrom: $("#dtpFrom").val(),
            dtpTo: $("#dtpTo").val(), rdoLocation: locationVal, hdnPrintDetails: $("#hdnPrintDetails").val(),
            hdnPrintMaterials: $("#hdnPrintMaterials").val(), rdoAdvance: advanceVal, hdnTech: $("#hdnTech").val(),
            hdnShowActiveStations: $("#hdnShowActiveStations").val(), hdnJobTimes: $("#hdnJobTimes").val()
        },
        dataType: "json",
        success: function (resp) {
            if (resp == "No BLOB")
                alert("There is no data to show.");
            else {
                $("#hdnFormSubmit").val("BLOB");
                $('#eqtransrepoform').submit();
            }
            
            $("#divLoading").html("");
            return false;
        }
    })

    $("#divLoading").html("");
    return true;
}

$("#chkPrintDetails").click(function () {
    $("#hdnPrintDetails").val($(this).prop("checked"));
});

$("#chkPrintMaterials").click(function () {
    $("#hdnPrintMaterials").val($(this).prop("checked"));
});

$("#chkShowActiveStations").click(function () {
    $("#hdnShowActiveStations").val($(this).prop("checked"));
});

$("#chkJobTimes").click(function () {
    $("#hdnJobTimes").val($(this).prop("checked"));
});