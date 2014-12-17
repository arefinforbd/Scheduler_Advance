var _listSite = $("#ulSites > :first-child");
var _listContract = $("#ulContracts > :first-child");

$(function () {

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
            $("#hdnSite").val($(this).attr("id"));
            LoadContracts();
            $("#ddlConracts").show();
        }
        else {
            $("#ddlConracts").hide();
            $("#jstree").hide();
            $("#divRightSide").hide();
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
            if (location.href.indexOf("Report/EquipmentTransaction") > 0) {
                $("#divLower").show();
            }
            if (location.href.indexOf("Report/TrendAnalysis") > 0) {
                $("#jstree").show();
                $("#divRightSide").show();
                $("#divLeftSide").css("border-right", "1px solid #CCCCCC");
            }
        }
        else {
            $("#jstree").hide();
            $("#divRightSide").hide();
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
            }
            else {
            }
        },
        error: function (request) {
            alert("Please try again. Something went wrong.");
        }
    });
}