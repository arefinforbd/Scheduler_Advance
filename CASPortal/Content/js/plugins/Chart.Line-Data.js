﻿var labels = [];
var yaxisvalues = [];
var dataset = [];
var lineColor = "";
var randomColorFactor = function () { return Math.round(Math.random() * 250) };

function LineColor() {
    lineColor = randomColorFactor() + "," + randomColorFactor() + "," + randomColorFactor();
}

$(function () {

    //$("div#divLineLegend").css("-moz-transform", "rotate(-90deg)")

    $('#jstree').jstree({
        "checkbox": {
            "keep_selected_style": false
        },
        "plugins": ["checkbox"]
    });

    $('#jstree').on('changed.jstree', function (e, data) {
        var i, j, r = [];
        for (i = 0, j = data.selected.length; i < j; i++) {
            r.push(data.instance.get_node(data.selected[i]).text);
        }
        alert('Selected: ' + r.join(', '));
        alert(data.node.id + " " + data.node.text);
    })
      .jstree();

    $.ajax({
        type: "GET",
        datatype: "json",
        url: "/Report/GetLineData",
        data: {}
    })
    .done(function (result) {
        var html = "";
        for (var index = 0; index < result.length; index++) {
            if (index % 12 == 0 && index > 0) {
                LineColor();
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
                html += '<div style="font-size: 12px; margin-left: 5px; float: left; margin-right: 5px;">' + result[index-1].label + '</div>';
            }
            yaxisvalues.push([result[index].lineValue]);
        }

        LineColor();
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
        html += '<div style="font-size: 12px; margin-left: 5px; float: left; margin-right: 5px;">' + result[index-1].label + '</div>';
        $("#divLineLegend").html(html);

        for (var index = 0; index < 12; index++) {
            labels.push([result[index].DateLabel]);
        }

        var lineChartData = {
            labels: labels,
            datasets: dataset
        }

        var ctx = document.getElementById("canvas").getContext("2d");
        window.myLine = new Chart(ctx).Line(lineChartData, {
            responsive: true
        });
    })
    .fail(function () {
        alrt("error occured");
    });
});