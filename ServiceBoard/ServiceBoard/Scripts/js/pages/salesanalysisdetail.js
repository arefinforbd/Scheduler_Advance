$(function () {

    $("#selCategory").css('width', '240px').selectmenu({ position: {} });
    $("#selCharts").css('width', '80px').selectmenu({ position: {} });

    if ($("#selCharts-button .ui-selectmenu-text").html() == "YTD") {
        $("#divYTDDTP").show();
        $("#divMTDDTP").hide();
    }
    else {
        $("#divYTDDTP").hide();
        $("#divMTDDTP").show();
    }

    $("ul#selCharts-menu").bind("click" ,function () {
        var chartType = $("#selCharts-button .ui-selectmenu-text").html();

        if (chartType == "YTD") {
            $("#divYTDDTP").show();
            $("#divMTDDTP").hide();
        }
        else {
            $("#divYTDDTP").hide();
            $("#divMTDDTP").show();
        }
        $("#widget-bar-chart").hide();
        $("#widget-bar-chart2").hide();
    });

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

    $('#id-date-picker-mtd').datepicker({
        dateFormat: "MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
        onClose: function (dateText, inst) {
            $(".ui-datepicker-calendar").hide();
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, month, 1));
        }
    })

    $("#id-date-picker-mtd").focus(function () {
        $(".ui-datepicker-calendar").hide();
        $(".ui-datepicker-close").addClass("btn");
        $(".ui-datepicker-close").addClass("btn-sm");
        $(".ui-datepicker-close").addClass("btn-info");
        $(".ui-datepicker-close").html("Select");
        $(".ui-datepicker-current").hide();
    });
    
    $("#btnPreviewOverall").click(function () {

        if (Validate() == false)
            return;

        $("#divLoading").show();
        $("#divLoading").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
        $("#widget-bar-chart").hide();
        $("#widget-bar-chart2").hide();

        if ($("#selCharts-button .ui-selectmenu-text").html() == "YTD")
            LoadAJAXSalesAnalysisOverallDetailYTD();
        
        if ($("#selCharts-button .ui-selectmenu-text").html() == "MTD")
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
        $("#widget-bar-chart").hide();
        $("#widget-bar-chart2").hide();

        if ($("#selCharts-button .ui-selectmenu-text").html() == "YTD")
            LoadAJAXSalesAnalysisByCategoryDetailYTD();

        if ($("#selCharts-button .ui-selectmenu-text").html() == "MTD")
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

    var FromDate = $("#id-date-picker-1").val();
    var ToDate = $("#id-date-picker-2").val();

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisOverallDetailYTD",
        type: "POST",
        data: { fromDate: FromDate, toDate: ToDate },
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

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisOverallDetailMTD",
        type: "POST",
        data: { mtdDate: $("#id-date-picker-mtd").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#widget-bar-chart2").show();
                LoadBarChart(data, $("#flot-bar-chart2"), false);
                $("#flot-bar-chart2 div.flot-text div.flot-x-axis").css("left", "-240px");
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
        data: { category: Category, mtdDate: $("#id-date-picker-mtd").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#widget-bar-chart2").show();
                LoadBarChart(data, $("#flot-bar-chart2"), false);
                $("#flot-bar-chart2 div.flot-text div.flot-x-axis").css("left", "-240px");
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

    if ($("#selCharts-button .ui-selectmenu-text").html() == "YTD") {
        if ($("#id-date-picker-1").val().trim().length <= 0) {
            alert("Please select From Date.");
            $("#id-date-picker-1").focus();
            return false;
        }

        if ($("#id-date-picker-2").val().trim().length <= 0) {
            alert("Please select To Date.");
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
    }
    else {
        if ($("#id-date-picker-mtd").val().trim().length <= 0) {
            alert("Please select Date.");
            $("#id-date-picker-mtd").focus();
            return false;
        }
    }

    return true;
}