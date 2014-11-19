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
