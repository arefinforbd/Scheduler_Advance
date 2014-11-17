//Flot Line Chart
$(function () {
    var lineData = [];

    var options = {
        series: {
            lines: {
                show: true
            },
            points: {
                show: true
            }
        },
        grid: {
            hoverable: true,
            borderWidth: 2,
            borderColor: "#633200",
            backgroundColor: { colors: ["#ffffff", "#EDF5FF"] }
        },
        yaxis: {
            min: 0
        },
        xaxis: {
            mode: "time",
            timeformat: "%d/%m/%y"
        },
        tooltip: true,
        tooltipOpts: {
            content: "'%s' of %x.1 is %y.4"
        }
    };

    function DateFormat(dateval){
        return gd(new Date(dateval).getFullYear(), new Date(dateval).getMonth(), new Date(dateval).getDate());
    }

    function gd(year, month, day) {
        return new Date(year, month, day).getTime();
    }

    $.ajax({
        type: "GET",
        dataType: 'json',
        url: "/Report/GetLineData",
        data: {}
    })
    .done(function (result) {
        for (var index = 0; index < result.length; index++) {
            lineData.push([DateFormat(result[index].x), result[index].y]);
        }

        var dataset = [{ label: "EXTERN (MOUSE)", data: lineData }];
        $.plot($("#flot-line-chart"), dataset, options);
        $("#flot-line-chart").UseTooltip();
    })
    .fail(function () {
        alert("error occured");
    });

    /*var previousPoint = null, previousLabel = null;
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    $.fn.UseTooltip = function () {
        $(this).bind("plothover", function (event, pos, item) {
            if (item) {
                if ((previousLabel != item.series.label) || (previousPoint != item.dataIndex)) {
                    previousPoint = item.dataIndex;
                    previousLabel = item.series.label;
                    $("#tooltip").remove();

                    var x = item.datapoint[0];
                    var y = item.datapoint[1];

                    var color = item.series.color;
                    var month = new Date(x).getMonth();

                    if (item.seriesIndex == 0) {
                        showTooltip(item.pageX,
                        item.pageY,
                        color,
                        "<strong>" + item.series.label + "</strong><br>x = " + monthNames[month] + ", y = " + y);
                    } else {
                        showTooltip(item.pageX,
                        item.pageY,
                        color,
                        "<strong>" + item.series.label + "</strong><br>x = " + monthNames[month] + ", y = " + y);
                    }
                }
            } else {
                $("#tooltip").remove();
                previousPoint = null;
            }
        });
    };

    function showTooltip(x, y, color, contents) {
        $('<div id="tooltip">' + contents + '</div>').css({
            position: 'absolute',
            display: 'none',
            top: y - 40,
            left: x - 80,
            border: '2px solid ' + color,
            padding: '3px',
            'font-size': '9px',
            'border-radius': '5px',
            'background-color': '#fff',
            'font-family': 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
            opacity: 0.9
        }).appendTo("body").fadeIn(200);
    }*/
});

//Flot Pie Chart
$(function () {
    var options = {
        series: {
            pie: {
                show: true,
                radius: 0.9
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

    $.ajax({
        type: "GET",
        dataType: 'json',
        url: "/Report/GetPieData",
        data: {}
    })
    .done(function (result) {
        $.plot($("#flot-pie-chart"), result, options);
    })
    .fail(function () {
        alert("error occured");
    });
});
