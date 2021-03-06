﻿var _listTech = $("#ulTechs > :first-child");

$(function () {
    $("#hdnFormSubmit").val("");
    $("#chkShowActiveStations").prop("checked", true);
    $("#hdnShowActiveStations").val("true");
    $("#chkJobTimes").prop("checked", true);
    $("#hdnJobTimes").val("true");

    var ulTech = $("#ulTechs > :first-child").text();
    $(".dropdown-Tech").find('[data-bind="label"]').text(ulTech);

    $(document.body).on('click', '#ulTechs li', function (event) {
        var $target = $(event.currentTarget);
        $("#ulTechs > :first-child").show();
        _listTech.css("background-color", "#FFFFFF");
        _listTech.removeClass("selected");
        $(this).addClass("selected");

        if (_listTech != $(this)) {
            _listTech.show();
        }

        $(this).css("background-color", "#f9f9c0");

        if ($target.text() == $(this).text()) {
            if (_listTech.text() == $(this).text()) {
                return;
            }
            _listTech = $(this);
        }

        $target.closest('.btn-group')
           .find('[data-bind="label"]').text($target.text())
              .end()
           .children('.dropdown-Tech').dropdown('toggle');

        if ($target.text() != "[ALL]") {
            $("#hdnTech").val($(this).attr("id"));
        }

        return false;
    });
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

    $("#process").val("Please wait...");
    $("#process").addClass("btn-disabled");
    $("#process").removeClass("btn-info");
    $("#process").attr("disabled", "disabled");
    $("#divLoading").html("<img alt='' title='' src=" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loading.gif width='10%'>");
    $("#process").attr("disabled", "disabled");

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
            if (resp == "No BLOB"){
                setTimeout(function ()
                {
                    $("#divLoading").html("");
                    $("#process").val("Preview");
                    $("#process").addClass("btn-info");
                    $("#process").removeClass("btn-disabled");
                    $("#process").removeAttr("disabled");
                }, 1000);
                alert("There is no data to show.");
            }
            else {
                $("#hdnFormSubmit").val("BLOB");
                $('#eqtransrepoform').submit();

                setTimeout(function ()
                {
                    $("#divLoading").html("");
                    $("#process").val("Preview");
                    $("#process").addClass("btn-info");
                    $("#process").removeClass("btn-disabled");
                    $("#process").removeAttr("disabled");
                }, 1000);

                return true;
            }
            return false;
        }
    })
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