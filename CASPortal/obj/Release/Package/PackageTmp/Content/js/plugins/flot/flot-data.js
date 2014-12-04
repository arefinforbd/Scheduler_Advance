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
    //[\"j1_31\",\"3#INTERNAL#3#EFK#10#ACTIVITY\"]
    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/TrendAnalysis",
        type: "POST",
        data: { siteNo: $("#hdnSite").val(), contractNo: $("#hdnContract").val(), selectedNodes: JSON.stringify(_selectedNodes), area: $("#spanArea").html(), frequency: $("#hdnFrequency").val(), fromDate: $("#dtpFrom").val(), toDate: $("#dtpTo").val(), groupBy: $("#txtGroup").val(), chartType: "PIE" },
        dataType: "JSON",
    })
    .done(function (result) {
        $.plot($("#flot-pie-chart"), result.Pies, options);
    })
    .fail(function () {
        alert("error occured");
    });
});
