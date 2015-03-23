$(function () {

    $("#selCategory").css('width', '240px').selectmenu({ position: {} });
    $("#selCharts").css('width', '80px').selectmenu({ position: {} });

    $('#id-date-picker-1').datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
        onSelect: function (date) {
            if ($("#id-date-picker-2").datepicker('getDate') == null) {
                $('#id-date-picker-2').datepicker("setDate", $("#id-date-picker-1").datepicker('getDate'));
            }
        }
    })
    .next().on(ace.click_event, function () {
        $(this).prev().focus();
    });

    $('#id-date-picker-2').datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
        onSelect: function (date) {
            var dayDiff = 0;
            dayDiff = new Date($("#id-date-picker-2").datepicker('getDate')) - new Date($("#id-date-picker-1").datepicker('getDate'));

            if (dayDiff < 0) {
                alert("To date cannot be smaller than From date.");
                $("#id-date-picker-2").val("");
                return;
            }
        }
    })
    .next().on(ace.click_event, function () {
        $(this).prev().focus();
    });
    
    $("#btnPreview").click(function () {

        if (Validate() == false)
            return;

        $("#divLoading").show();
        $("#divLoading").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
        LoadAJAXSalesAnalysisDetail();
    });

    $("#btnReset").click(function () {
        alert("OK");
    });
});

function LoadAJAXSalesAnalysisDetail() {

    var Category = $("#selCategory-button .ui-selectmenu-text").html();
    var FromDate = $("#id-date-picker-1").val();
    var ToDate = $("#id-date-picker-2").val();

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisByCategoryDetail",
        type: "POST",
        data: { category: Category, fromDate: FromDate, toDate: ToDate },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#widget-bar-chart").show();
                LoadBarChart(data, $("#flot-bar-chart"), false);
                LoadingComplete();
            }
            else {
                LoadingComplete();
                alert("There is no data to show.");
            }
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
}

function LoadingComplete() {
    $("#divLoading").html("");
    $("#divLoading").hide();
}

function Validate() {

    if ($("#selCategory-button .ui-selectmenu-text").html() == "Select Category") {
        alert("Please select Category.");
        $("#selCategory-button").fadeOut();
        $("#selCategory-button").fadeIn();
        return false;
    }

    if ($("#id-date-picker-1").val().trim().length <= 0) {
        alert("Please enter From Date.");
        $("#id-date-picker-1").focus();
        return false;
    }

    if ($("#id-date-picker-2").val().trim().length <= 0) {
        alert("Please enter To Date.");
        $("#id-date-picker-2").focus();
        return false;
    }

    var dayDiff = 0;
    dayDiff = new Date($("#id-date-picker-2").datepicker('getDate')) - new Date($("#id-date-picker-1").datepicker('getDate'));

    if (dayDiff < 0) {
        alert("To date cannot be smaller than From date.");
        return false;
    }

    return true;
}