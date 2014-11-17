$(function () {
    var chart = Morris.Bar({
        element: 'morris-bar-chart',
        data: [0, 0],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        hideHover: 'auto',
        resize: true
    });

    $.ajax({
        type: "GET",
        dataType: 'json',
        url: "/Report/GetBarData",
        data: {}
    })
    .done(function (data) {
        chart.setData(data);
    })
    .fail(function () {
        alert("error occured");
    });
});