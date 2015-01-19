var _listSite = $("#ulSites > :first-child");
var _listContract = $("#ulContracts > :first-child");

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

    return true;
}

$(function () {

    $("#reportSubMenu").addClass("in");
    $("#reportSubMenu").css("height", "auto");
    $("#reportMenu").parent().addClass("active");

    $("#ddlConracts").hide();

    var ulSite = $("#ulSites > :first-child").text();
    $(".dropdown-Site").find('[data-bind="label"]').text(ulSite);

    var ulSite = $("#ulSites > :first-child").text();
    $(".dropdown-Site").find('[data-bind="label"]').text(ulSite);

    var ulContract = $("#ulContracts > :first-child").text();
    $(".dropdown-Contract").find('[data-bind="label"]').text(ulContract);

    $("#dtpFrom").datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
        onSelect: function (date) {
            var dayDiff = 0;
            if ($("#dtpTo").val().trim().length > 0) {
                dayDiff = new Date($("#dtpTo").datepicker('getDate')) - new Date($("#dtpFrom").datepicker('getDate'));

                if (dayDiff < 0) {
                    alert("From date cannot be bigger than To date.");
                    $("#dtpFrom").val("");
                    return;
                }
            }
            if ($("#dtpTo").datepicker('getDate') == null) {
                $('#dtpTo').datepicker("setDate", $("#dtpFrom").datepicker('getDate'));
            }
        }
    });

    $("#dtpTo").datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
        onSelect: function (date) {
            var dayDiff = 0;
            dayDiff = new Date($("#dtpTo").datepicker('getDate')) - new Date($("#dtpFrom").datepicker('getDate'));

            if (dayDiff < 0) {
                alert("To date cannot be smaller than From date.");
                $("#dtpTo").val("");
                return;
            }
        }
    });

    $(document.body).on('click', '#ulSites li', function (event) {
        var $target = $(event.currentTarget);
        $("#ulSites > :first-child").show();
        _listSite.css("background-color", "#FFFFFF");
        _listSite.removeClass("selected");
        $(this).addClass("selected");

        if (_listSite != $(this)) {
            _listSite.show();
        }

        $(this).css("background-color", "#f9f9c0");

        if ($target.text() == $(this).text()) {
            if (_listSite.text() == $(this).text()) {
                return;
            }
            _listSite = $(this);
        }

        $target.closest('.btn-group')
           .find('[data-bind="label"]').text($target.text())
              .end()
           .children('.dropdown-Site').dropdown('toggle');

        if ($target.text() != "Select Site") {
            _listContract = $("#ulContracts > :first-child");
            $("#hdnSite").val($(this).attr("id"));
            $("#divContractLoading").show();
            $("#divContractLoading").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='25px' />");
            LoadContracts();
            $("#ddlConracts").show();
        }
        else {
            if (location.href.indexOf("Report/EquipmentTransaction") > 0 || location.href.indexOf("Report/EquipmentReport") > 0) {
                $("#ddlConracts").hide();
                $("#divLower").hide();
            }
            if (location.href.indexOf("Report/TrendAnalysis") > 0) {
                ResetSite();
                ResetTrendAnalysisFields();
            }
            if (location.href.indexOf("Report/InstalledEquipment") > 0) {
                $("#ddlConracts").hide();
                ResetSite();
                ResetInstalledEquipmentFields();
            }
        }

        return false;
    });

    $(document.body).on('click', '#ulContracts li', function (event) {
        var $target = $(event.currentTarget);
        _listContract.css("background-color", "#FFFFFF");

        if (_listContract != $(this)) {
            _listContract.show();
        }

        $(this).css("background-color", "#f9f9c0");

        if ($target.text() == $(this).text()) {
            if (_listContract.text() == $(this).text()) {
                return;
            }
            _listContract = $(this);
        }

        $target.closest('.btn-group')
           .find('[data-bind="label"]').text($target.text())
              .end()
           .children('.dropdown-Contract').dropdown('toggle');

        if ($target.text() != "Select Contract") {
            $("#hdnContract").val($(this).attr("id"));
            if (location.href.indexOf("Report/EquipmentTransaction") > 0 || location.href.indexOf("Report/EquipmentReport") > 0) {
                $("#divLower").show();
            }
            if (location.href.indexOf("Report/TrendAnalysis") > 0) {
                $("#jstree").show();
                $("#divRightSide").show();
                $("#divLeftSide").css("border-right", "1px solid #CCCCCC");
            }
            if (location.href.indexOf("Report/InstalledEquipment") > 0) {
                $("#divLoading").html("<img alt= title= src=" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loading.gif width='10%' />");
                LoadInstalledLocations();
            }
        }
        else {
            if (location.href.indexOf("Report/EquipmentTransaction") > 0 || location.href.indexOf("Report/EquipmentReport") > 0) {
                $("#divLower").hide();
            }
            if (location.href.indexOf("Report/TrendAnalysis") > 0) {
                ResetTrendAnalysisFields();
                $("#ddlConracts").show();
            }
            if (location.href.indexOf("Report/InstalledEquipment") > 0) {
                ResetInstalledEquipmentFields();
            }
        }

        return false;
    });
});

function LoadContracts() {

    var html = "";

    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/GetContracts",
        type: "POST",
        data: { siteNo: $("#hdnSite").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#ulContracts").html(data);
                $(".dropdown-Contract").find('[data-bind="label"]').text($("#ulContracts > :first-child").text());
                $("#divContractLoading").hide();
                $("#divContractLoading").html("");
            }
            if (data == "Session timed out") {
                $("#divContractLoading").hide();
                $("#divContractLoading").html("");
                alert("Session timed out. Please log in again.");
                window.location.href = $("#hdnSiteURL").val();
            }
        },
        error: function (request) {
            $("#divContractLoading").hide();
            $("#divContractLoading").html("");
            alert("Please try again. Something went wrong.");
        }
    });
}

function ResetSite() {
    var ulSite = $("#ulSites li").eq(0).text();
    $(".dropdown-Site").find('[data-bind="label"]').text(ulSite);
    _listSite.css("background-color", "#FFFFFF");
    _listSite = $("#ulSites > :first-child");
}

function ResetTrendAnalysisFields() {
    $('#jstree').jstree("deselect_all");
    $("#dtpFrom").val("");
    $("#dtpTo").val("");

    var ulContract = $("#ulContracts li").eq(0).text();
    $(".dropdown-Contract").find('[data-bind="label"]').text(ulContract);
    _listContract.css("background-color", "#FFFFFF");
    _listContract = $("#ulContracts > :first-child");

    var ulArea = $("#ulArea li").eq(0).text();
    $(".dropdown-Area").find('[data-bind="label"]').text(ulArea);
    _listArea.css("background-color", "#FFFFFF");

    $("#divLinePanel").hide();
    $("#divPiePanel").hide();
    $("#divBarPanel").hide();

    $("#rdoWeeks").prop("checked", true);
    $("#lblGroup").html("Group By No of Weeks: ");
    $("#txtGroup").removeAttr("disabled");
    $("#txtGroup").css("background-color", "#FFFFFF");
    $("#txtGroup").val("1");
    $("#hdnFrequency").val("1");

    $("#rdoSerialNo").prop("checked", true);
    $("#chkExclude").prop("checked", true);
    $("#hdnExclude").val("true");

    $("#ddlConracts").hide();
    $("#jstree").hide();
    $("#divRightSide").hide();
    $("#divLeftSide").css("border-right", "");
}

function ResetInstalledEquipmentFields() {

    var ulContract = $("#ulContracts li").eq(0).text();
    $(".dropdown-Contract").find('[data-bind="label"]').text(ulContract);
    _listContract.css("background-color", "#FFFFFF");
    _listContract = $("#ulContracts > :first-child");

    $("#divEquipmentLocation").html("");
    $("#divEquipmentLocation").hide();
    $("#divEquipmentLocationFooter").hide();
    $("#divLocationDetail").hide();
}