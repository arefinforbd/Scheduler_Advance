$(function () {

    $("#rdoYTD").prop("checked", true);
    LoadingSalesYTDChart(1);
    LoadAJAXDebtorAnalysis();

    $("#liYTD").click(function () {
        $("#divYTDLoading2").show();
        $("#divYTDLoading2").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
    });

    $("#liCatComp").click(function () {
        if ($("#rdoMTDCat").prop("checked")) {
            $("#rdoMTDCat").prop("checked", true);
            LoadingSalesCatSum(1, "m");
        }
        else {
            $("#rdoYTDCat").prop("checked", true);
            LoadingSalesCatSum(1, "y");
        }
    });

    $("#rdoYTD").click(function () {
        LoadingSalesYTDChart(1);
    });

    $("#rdoMTD").click(function () {
        LoadingSalesYTDChart(2);
    });

    $("#rdoYTDCat").click(function () {
        LoadingSalesCatSum(1, "y");
    });

    $("#rdoMTDCat").click(function () {
        LoadingSalesCatSum(2, "m");
    });
});

function LoadingSalesYTDChart(ReportType) {
    $("#flot-bar-chart").hide();
    $("#divYTDLoading").show();
    $("#divYTDLoading").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
    $("#hdnReportType").val(ReportType);
    LoadAJAXSalesAnalysis($("#hdnReportType").val());
    $("#divYTDLoading").html("");
    $("#divYTDLoading").hide();
    $("#flot-bar-chart").show();
}

function LoadingSalesCatSum(ReportType, YearOrMonth) {
    $("#flot-bar-chart-cat").hide();
    $("#divYTDLoading3").show();
    $("#divYTDLoading3").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
    $("#hdnReportTypeCat").val(ReportType);
    LoadAJAXSalesAnalysisByCategorySum($("#hdnReportTypeCat").val(), YearOrMonth);
    $("#divYTDLoading3").html("");
    $("#divYTDLoading3").hide();
    $("#flot-bar-chart-cat").show();
}

function LoadAJAXSalesAnalysis(ReportType) {
    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysis",
        type: "POST",
        data: { reportType: ReportType },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                LoadBarChart(data, $("#flot-bar-chart"), false);
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

function LoadAJAXSalesAnalysisByCategorySum(ReportType, YearOrMonth) {
    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisByCategorySum",
        type: "POST",
        data: { reportType: ReportType, yearOrMonth: YearOrMonth },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                LoadBarChart(data, $("#flot-bar-chart-cat"), false);
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

function LoadAJAXDebtorAnalysis() {
    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/DebtorAnalysis",
        type: "POST",
        data: { },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                LoadLineChart(data, "flot-line-chart");
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