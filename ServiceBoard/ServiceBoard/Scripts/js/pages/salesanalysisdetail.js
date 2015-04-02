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
    
    $("#btnPreviewOverall").click(function () {

        if (Validate() == false)
            return;

        $("#divLoading").show();
        $("#divLoading").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
        LoadAJAXSalesAnalysisOverallDetailYTD();
        LoadAJAXSalesAnalysisOverallDetailMTD();
    });

    $("#btnResetOverall").click(function () {
        $("#id-date-picker-1").val("");
        $("#id-date-picker-2").val("");
        $("#widget-bar-chart").hide();
        $("#widget-bar-chart2").hide();
    });

    $("#btnPreviewByCategory").click(function () {

        if (Validate() == false)
            return;

        legend = false;
        $("#divLoading").show();
        $("#divLoading").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
        LoadAJAXSalesAnalysisByCategoryDetailYTD();
        LoadAJAXSalesAnalysisByCategoryDetailMTD();
    });

    $("#btnResetByCategory").click(function () {
        var category = $('#selCategory :nth-child(1)').html();
        $('#selCategory :nth-child(1)').prop('selected', 'selected');
        $("#selCategory-button .ui-selectmenu-text").html(category);
        $("#id-date-picker-1").val("");
        $("#id-date-picker-2").val("");
        $("#widget-bar-chart").hide();
        $("#widget-bar-chart2").hide();
    });
});

function LoadAJAXSalesAnalysisOverallDetailYTD() {

    var Category = $("#selCategory-button .ui-selectmenu-text").html();
    var FromDate = $("#id-date-picker-1").val();
    var ToDate = $("#id-date-picker-2").val();

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisOverallDetailYTD",
        type: "POST",
        data: { category: Category, fromDate: FromDate, toDate: ToDate },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#widget-bar-chart").show();
                LoadBarChart(data, $("#flot-bar-chart"), false);
                $("#flot-bar-chart div.flot-text div.flot-x-axis").css("left", "-12px");
                LoadingComplete();
            }
            else {
                alert("There is no data to show.");
            }
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
}

function LoadAJAXSalesAnalysisOverallDetailMTD() {

    var Category = $("#selCategory-button .ui-selectmenu-text").html();
    var FromDate = $("#id-date-picker-1").val();
    var ToDate = $("#id-date-picker-2").val();

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisOverallDetailMTD",
        type: "POST",
        data: { category: Category, fromDate: FromDate, toDate: ToDate },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#widget-bar-chart2").show();
                LoadBarChart(data, $("#flot-bar-chart2"), false);
                $("#flot-bar-chart2 div.flot-text div.flot-x-axis").css("left", "-12px");
                //$("#flot-bar-chart2 div.flot-text div.flot-x-axis").css("font-size", "11px");
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

function LoadAJAXSalesAnalysisByCategoryDetailYTD() {

    var Category = $("#selCategory-button .ui-selectmenu-text").html();
    var FromDate = $("#id-date-picker-1").val();
    var ToDate = $("#id-date-picker-2").val();

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisByCategoryDetailYTD",
        type: "POST",
        data: { category: Category, fromDate: FromDate, toDate: ToDate },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#widget-bar-chart").show();
                LoadBarChart(data, $("#flot-bar-chart"), false);
                $("#flot-bar-chart div.flot-text div.flot-x-axis").css("left", "-12px");
                LoadingComplete();
            }
            else {
                alert("There is no data to show.");
            }
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
}

function LoadAJAXSalesAnalysisByCategoryDetailMTD() {

    var Category = $("#selCategory-button .ui-selectmenu-text").html();
    var FromDate = $("#id-date-picker-1").val();
    var ToDate = $("#id-date-picker-2").val();

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisByCategoryDetailMTD",
        type: "POST",
        data: { category: Category, fromDate: FromDate, toDate: ToDate },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#widget-bar-chart2").show();
                LoadBarChart(data, $("#flot-bar-chart2"), false);
                $("#flot-bar-chart2 div.flot-text div.flot-x-axis").css("left", "-12px");
                //$("#flot-bar-chart2 div.flot-text div.flot-x-axis").css("font-size", "11px");
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

    if (dayDiff > 31449600000) {
        alert("Please keep the date range less than or equal to 1 year.");
        return false;
    }

    return true;
}