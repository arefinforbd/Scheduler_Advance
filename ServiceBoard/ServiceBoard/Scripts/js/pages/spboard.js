function LineColor(colourIndex) {

    var colours = ["235,181,16", "238,13,47", "89,201,52", "221,30,188", "23,227,217", "111,124,140", "128,64,128", "64,128,128", "0,64,128", "39,73,233"];

    if (colourIndex > 9)
        lineColor = randomColorFactor() + "," + randomColorFactor() + "," + randomColorFactor();
    else
        lineColor = colours[colourIndex];
}

function LoadLineChart(data) {

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

    for (var index = 0; index < data.Lines[0].Count; index++) {
        labels.push([data.Lines[index].DateLabel]);
    }

    var lineChartData = {
        labels: labels,
        datasets: dataset
    }
    $("#tooltip").remove();

    var ctx = document.getElementById("flot-line-chart").getContext("2d");

    //If mobile or tablet
    //if (/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini/i.test(navigator.userAgent.toLowerCase())) {
    //    $("#flot-line-chart").removeAttr("height");
    //    $("#flot-line-chart").removeAttr("width");
    //    window.myLine = new Chart(ctx).Line(lineChartData, {
    //        responsive: true
    //    });
    //}
    //else {
    //    window.myLine = new Chart(ctx).Line(lineChartData, {
    //        responsive: false
    //    });
    //}
    window.myLine = new Chart(ctx).Line(lineChartData, {
        responsive: false
    });

    //var divDebtorWidth = $("#divDebtor").width() - ($("#divDebtor").width() * 0.1);
    //$("#flot-line-chart").width(divDebtorWidth);
}

function LoadBarChart(data, chartControl) {

    var yaxisvalues = [];
    var dataset = [];

    var innerindex = 0;
    var xvalues = [];
    var parsedData = data.Bars;
    var colCount = 0;

    //Creating labels for x-axis
    for (var loopindex = 0; loopindex < data.Bars.length; loopindex++) {
        $.each(parsedData[loopindex], function (colName, value) {
            if (colName == "BarXLabel") {
                xvalues.push([loopindex, value]);
            }
        });
    }

    //Counting the number of columns returned from server
    $.each(parsedData[0], function (colName, value) {
        if (colName == "BarXLabel") {
            colCount++;
        }
    });

    innerindex = 0;
    for (var loopindex = 0; loopindex < colCount; loopindex++) {
        $.each(data.Bars[loopindex], function (colName, value) {
            if (colName != "BarXLabel") {

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

    $.plot(chartControl, dataset, {
        series: {stack: 0,
            bars: {
                show: true,
                barWidth: 0.2,
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