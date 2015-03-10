//It's for temporary testing purpose.
//Thanks to Olly Smith

$(function () {

    var yKeys = [];
    var labels = [];

    $.ajax({
        type: "GET",
        dataType: 'json',
        url: $("#hdnSiteURL").val() + "/Report/GetBarData",
        data: {}
    })
    .done(function (data) {
        if (data != null) {
            var html = "";
            var parsedData = $.parseJSON(data);

            $.each(parsedData[0], function (colName, value) {
                if (colName != "DateLabel") {
                    yKeys.push(colName);
                    colName = colName.replace("__", " (");
                    colName = colName.replace("_", ")");
                    labels.push(colName);
                }
            });

            var chart = Morris.Bar({
                element: 'morris-bar-chart',
                data: [0, 0],
                xkey: 'DateLabel',
                ykeys: yKeys,
                labels: labels,
                hideHover: 'auto',
                resize: true,
                xLabelAngle: 50
            });

            chart.setData(parsedData);

            chart.options.labels.forEach(function (label, i) {
                html += '<div style="background-color: ' + chart.options.barColors[i] + ';width: 12px; height: 12px; float: left; margin-top: 2px;"></div>';
                html += '<div style="font-size: 12px; margin-left: 5px; float: left; margin-right: 5px;">' + label + '</div>';
            })
            $("#divBarLegend").html(html);
        }
    })
    .fail(function () {
        alert("error occured");
    });
});