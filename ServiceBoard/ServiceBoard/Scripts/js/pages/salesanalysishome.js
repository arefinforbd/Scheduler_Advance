$(function () {

    //flot chart resize plugin, somehow manipulates default browser resize event to optimize it!
    //but sometimes it brings up errors with normal resize event handlers

    //var placeholder = $('#piechart-placeholder').css({ 'margin-top': '15px', 'width': '380px', 'min-height': '220px' });
    //var data = [
    //    { label: "social networks", data: 38.7, color: "#68BC31" },
    //    { label: "search engines", data: 24.5, color: "#2091CF" },
    //    { label: "ad campaigns", data: 8.2, color: "#AF4E96" },
    //    { label: "direct traffic", data: 18.6, color: "#DA5430" },
    //    { label: "other", data: 10, color: "#FEE074" }
    //]

    //function drawPieChart(placeholder, data, position) {
    //    $.plot(placeholder, data, {
    //        series: {
    //            pie: {
    //                show: true,
    //                tilt: 0.8,
    //                highlight: {
    //                    opacity: 0.25
    //                },
    //                stroke: {
    //                    color: '#fff',
    //                    width: 2
    //                },
    //                startAngle: 2
    //            }
    //        },
    //        legend: {
    //            show: true,
    //            position: position || "ne",
    //            labelBoxBorderColor: null,
    //            margin: [-30, 15]
    //        }
    //        ,
    //        grid: {
    //            hoverable: true,
    //            clickable: true
    //        }
    //    })
    //}
    //drawPieChart(placeholder, data);

    /**
    we saved the drawing function and the data to redraw with different position later when switching to RTL mode dynamically
    so that's not needed actually.
    */
    //placeholder.data('chart', data);
    //placeholder.data('draw', drawPieChart);

    //pie chart tooltip example
    //var $tooltip = $("<div class='tooltip top in'><div class='tooltip-inner'></div></div>").hide().appendTo('body');
    //var previousPoint = null;

    //placeholder.on('plothover', function (event, pos, item) {
    //    if (item) {
    //        if (previousPoint != item.seriesIndex) {
    //            previousPoint = item.seriesIndex;
    //            var tip = item.series['label'] + " : " + item.series['percent'] + '%';
    //            $tooltip.show().children(0).text(tip);
    //        }
    //        $tooltip.css({ top: pos.pageY + 10, left: pos.pageX + 10 });
    //    } else {
    //        $tooltip.hide();
    //        previousPoint = null;
    //    }

    //});

    //var d1 = [];
    //for (var i = 0; i < Math.PI * 2; i += 0.5) {
    //    d1.push([i, Math.sin(i)]);
    //}

    //var d2 = [];
    //for (var i = 0; i < Math.PI * 2; i += 0.5) {
    //    d2.push([i, Math.cos(i)]);
    //}

    //var d3 = [];
    //for (var i = 0; i < Math.PI * 2; i += 0.2) {
    //    d3.push([i, Math.tan(i)]);
    //}

    //var sales_charts = $('#sales-charts').css({ 'width': '100%', 'height': '220px' });
    //$.plot("#sales-charts", [
    //    { label: "Domains", data: d1 },
    //    { label: "Hosting", data: d2 },
    //    { label: "Services", data: d3 }
    //], {
    //    hoverable: true,
    //    shadowSize: 0,
    //    series: {
    //        lines: { show: true },
    //        points: { show: true }
    //    },
    //    xaxis: {
    //        tickLength: 0
    //    },
    //    yaxis: {
    //        ticks: 10,
    //        min: -2,
    //        max: 2,
    //        tickDecimals: 3
    //    },
    //    grid: {
    //        backgroundColor: { colors: ["#fff", "#fff"] },
    //        borderWidth: 1,
    //        borderColor: '#555'
    //    }
    //});

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
                LoadBarChart(data, $("#flot-bar-chart"));
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
                LoadBarChart(data, $("#flot-bar-chart-cat"));
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
                LoadLineChart(data);
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