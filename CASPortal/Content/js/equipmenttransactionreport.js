$(function () {
    $("#chkShowActiveStations").prop("checked", true);
    $("#hdnShowActiveStations").val("true");
    $("#chkJobTimes").prop("checked", true);
    $("#hdnJobTimes").val("true");
});

function Validate() {

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