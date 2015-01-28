$(function () {
    $("#divSortBy").hide();
    $("#divFrequency").css("margin-bottom", "10px");
    $("#ddlArea").hide();
});

function LoadLineChart(data) {

    var html = "";
    var labels = [];
    var colourIndex = 0;
    dataset = [];
    yaxisvalues = [];

    for (var index = 0; index < data.Lines.length; index++) {
        if (index % data.Lines[0].Count == 0 && index > 0) {
            LineColor(colourIndex);
            dataset.push({
                fillColor: "rgba(255,255,255,0)",
                strokeColor: "rgba(" + lineColor + ",0.6)",
                pointColor: "rgba(" + lineColor + ",0.6)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(" + lineColor + ",1)",
                data: yaxisvalues
            });
            yaxisvalues = [];

            html += '<div style="background-color: rgba(' + lineColor + ',0.8);width: 12px; height: 12px; float: left; margin-top: 2px;"></div>';
            html += '<div style="font-size: 12px; margin-left: 5px; float: left; margin-right: 5px;">' + data.Lines[index - 1].label + '</div>';
            colourIndex++;
        }
        yaxisvalues.push([data.Lines[index].lineValue]);
    }

    LineColor(colourIndex);
    dataset.push({
        fillColor: "rgba(255,255,255,0)",
        strokeColor: "rgba(" + lineColor + ",0.6)",
        pointColor: "rgba(" + lineColor + ",0.6)",
        pointStrokeColor: "#fff",
        pointHighlightFill: "#fff",
        pointHighlightStroke: "rgba(" + lineColor + ",1)",
        data: yaxisvalues
    });

    html += '<div style="background-color: rgba(' + lineColor + ',0.8);width: 12px; height: 12px; float: left; margin-top: 2px;"></div>';
    html += '<div style="font-size: 12px; margin-left: 5px; float: left; margin-right: 5px;">' + data.Lines[index - 1].label + '</div>';
    $("#divLineLegend").html(html);

    for (var index = 0; index < data.Lines[0].Count; index++) {
        labels.push([data.Lines[index].DateLabel]);
    }

    var lineChartData = {
        labels: labels,
        datasets: dataset
    }
    $("#tooltip").remove();

    $("#divLinePanel").show();
    var ctx = document.getElementById("flot-line-chart").getContext("2d");

    //If mobile or tablet
    if (/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini/i.test(navigator.userAgent.toLowerCase())) {
        $("#flot-line-chart").removeAttr("height");
        $("#flot-line-chart").removeAttr("width");
        window.myLine = new Chart(ctx).Line(lineChartData, {
            responsive: true
        });
    }
    else {
        window.myLine = new Chart(ctx).Line(lineChartData, {
            responsive: false
        });
    }
    $("#flot-line-chart").height($("#divLinePanel").height() - 80);
}

function LoadPieChart(data) {

    var options = {
        series: {
            pie: {
                show: true,
                radius: 0.7,
                combine: {
                    color: '#999'
                }
            }
        },
        legend: {
            show: false
        },
        grid: {
            hoverable: true
        },
        tooltip: true,
        tooltipOpts: {
            content: "%p.0%, %s", // show percentages, rounding to 2 decimal places
            shifts: {
                x: 20,
                y: 0
            },
            defaultTheme: false
        }
    };

    $("#divPiePanel").show();
    $.plot($("#flot-pie-chart"), data.Pies, options);
}

function LoadBarChart(data) {

    dataset = [];
    yaxisvalues = [];

    var innerindex = 0;
    var xvalues = [];
    var parsedData = data.Bars;
    var colCount = 0;

    //Creating labels for x-axis
    for (var loopindex = 0; loopindex < data.Bars.length; loopindex++) {
        $.each(parsedData[loopindex], function (colName, value) {
            if (colName == "DateLabel") {
                xvalues.push([loopindex, value]);
            }
        });
    }

    //Counting the number of columns returned from server
    $.each(parsedData[0], function (colName, value) {
        if (colName == "DateLabel") {
            colCount++;
        }
    });

    innerindex = 0;
    for (var loopindex = 0; loopindex < colCount; loopindex++) {
        $.each(data.Bars[loopindex], function (colName, value) {
            if (colName != "DateLabel") {

                for (var index = 0; index < data.Bars.length; index++) {
                    yaxisvalues.push([index, data.Bars[index][colName]]);
                }

                dataset.push({
                    label: colName,
                    bars: { order: innerindex },
                    data: yaxisvalues
                });
                yaxisvalues = [];
                innerindex++;
            }
        });
    }

    $("#divBarPanel").show();
    $.plot($("#flot-bar-chart"), dataset, {
        series: {
            bars: {
                show: true,
                barWidth: 0.13,
                align: 'center'
            }
        },
        grid: {
            hoverable: true
        },
        valueLabels: {
            show: true
        },
        xaxis: {
            ticks: xvalues
        }
    });
}

$("#btnPreview").click(function () {

    var labels = [];
    var strChartType = "All Charts";

    if (Validate() == false)
        return;

    $("#divLinePanel").hide();
    $("#divPiePanel").hide();
    $("#divBarPanel").hide();

    Loading();
    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/TrendAnalysisByQuestion",
        type: "POST",
        data: { siteNo: $("#hdnSite").val(), contractNo: $("#hdnContract").val(), selectedNodes: JSON.stringify(_selectedNodes), area: $("#spanArea").html(), frequency: $("#hdnFrequency").val(), fromDate: $("#dtpFrom").val(), toDate: $("#dtpTo").val(), groupBy: $("#txtGroup").val(), chartType: $("#spanChartType").html() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                //$('#jstree').jstree("deselect_all");

                //LINE Chart
                if (($("#spanChartType").html() == strChartType || $("#spanChartType").html() == "LINE")) {
                    LoadLineChart(data);
                }

                //PIE Chart
                if (($("#spanChartType").html() == strChartType || $("#spanChartType").html() == "PIE")) {
                    LoadPieChart(data);
                }

                //BAR Chart
                if (($("#spanChartType").html() == strChartType || $("#spanChartType").html() == "BAR")) {
                    LoadBarChart(data);
                }

                if (/iphone|ipad|ipod/i.test(navigator.userAgent.toLowerCase())) {
                    $("#btnLineDownload").hide();
                    $("#btnPieDownload").hide();
                    $("#btnBarDownload").hide();
                }
            }
            else {
                alert("There is no data to show.");
            }
            LoadingComplete();
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
});