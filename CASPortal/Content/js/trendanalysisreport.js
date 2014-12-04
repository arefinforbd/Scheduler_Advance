var _selectedNodes = [];
var _listSite = $("#ulSites > :first-child");
var _listContract = $("#ulContracts > :first-child");
var _listArea = $("#ulArea > :first-child");

var labels = [];
var yaxisvalues = [];
var dataset = [];
var lineColor = "";
var randomColorFactor = function () { return Math.round(Math.random() * 250) };

function LineColor() {
    lineColor = randomColorFactor() + "," + randomColorFactor() + "," + randomColorFactor();
}

function Validate() {

    if ($("a.dropdown-Site span[data-bind='label']").text() == "Select Site") {
        alert("Please select Site.");
        $("a.dropdown-Site").fadeOut();
        $("a.dropdown-Site").fadeIn();
        return false;
    }

    if ($("a.dropdown-Contract span[data-bind='label']").text() == "Select Contract") {
        alert("Please select Contract.");
        $("a.dropdown-Contract").fadeOut();
        $("a.dropdown-Contract").fadeIn();
        return false;
    }

    if ($("#j1_1_anchor").find(".jstree-checkbox").hasClass("jstree-undetermined") == false) {
        alert("Please select any item.");
        return false;
    }

    if ($("#dtpFrom").val().trim().length <= 0) {
        alert("Please enter From Date.");
        $("#dtpFrom").focus();
        return false;
    }

    if ($("#dtpTo").val().trim().length <= 0) {
        alert("Please enter To Date.");
        $("#dtpTo").focus();
        return false;
    }

    var dayDiff = 0;
    dayDiff = new Date($("#dtpTo").datepicker('getDate')) - new Date($("#dtpFrom").datepicker('getDate'));

    if (dayDiff < 0) {
        alert("To date cannot be smaller than From date.");
        return false;
    }

    if ($("#txtGroup").val() > 12) {
        alert("Please enter less than 12 weeks.");
        $("#txtGroup").focus();
        return false;
    }

    return true;
}

$(function () {

    var ulSite = $("#ulSites > :first-child").text();
    $(".dropdown-Site").find('[data-bind="label"]').text(ulSite);

    var ulContract = $("#ulContracts > :first-child").text();
    $(".dropdown-Contract").find('[data-bind="label"]').text(ulContract);

    var ulArea = $("#ulArea > :first-child").text();
    $(".dropdown-Area").find('[data-bind="label"]').text(ulArea);

    $("#ddlConracts").hide();
    $("#jstree").hide();
    $("#divRightSide").hide();

    $("#dtpFrom").datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        onSelect: function (date) {
            var dayDiff = 0;
            if ($("#dtpTo").val().trim().length > 0) {
                dayDiff = new Date($("#dtpTo").datepicker('getDate')) - new Date($("#dtpFrom").datepicker('getDate'));

                if (dayDiff < 0) {
                    alert("From date cannot be bigger than To date.");
                    $("#dtpFrom").val("");
                    return;
                }
            }
        }
    });

    $("#dtpTo").datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        onSelect: function (date) {
            var dayDiff = 0;
            dayDiff = new Date($("#dtpTo").datepicker('getDate')) - new Date($("#dtpFrom").datepicker('getDate'));

            if (dayDiff < 0) {
                alert("To date cannot be smaller than From date.");
                $("#dtpTo").val("");
                return;
            }
        }
    });

    $("#rdoWeeks").click(function () {
        $("#lblGroup").html("Group By No of Weeks: ");
        $("#txtGroup").removeAttr("disabled");
        $("#txtGroup").css("background-color", "#FFFFFF");
        $("#txtGroup").val("1");
        $("#hdnFrequency").val("1");
    });

    $("#rdoMonths").click(function () {
        $("#lblGroup").html("Group By Months: ");
        $("#txtGroup").attr("disabled", "disabled");
        $("#txtGroup").css("background-color", "#CCCCCC");
        $("#txtGroup").val("1");
        $("#hdnFrequency").val("2");
    });

    $("#rdoJobDate").click(function () {
        $("#lblGroup").html("Group By Job Date: ");
        $("#txtGroup").attr("disabled", "disabled");
        $("#txtGroup").css("background-color", "#CCCCCC");
        $("#txtGroup").val("1");
        $("#hdnFrequency").val("3");
    });

    $("#txtGroup").focusout(function () {
        if ($("#txtGroup").val() > 12) {
            alert("Please enter less than 12 weeks.");
            $("#txtGroup").focus();
        }
    });

    $(document.body).on('click', '#ulSites li', function (event) {
        var $target = $(event.currentTarget);
        $("#ulSites > :first-child").show();
        _listSite.css("background-color", "#FFFFFF");
        _listSite.removeClass("selected");
        $(this).addClass("selected");

        if (_listSite != $(this)) {
            _listSite.show();
        }

        $(this).css("background-color", "#f9f9c0");

        if ($target.text() == $(this).text()) {
            if (_listSite.text() == $(this).text()) {
                return;
            }
            _listSite = $(this);
        }

        $target.closest('.btn-group')
           .find('[data-bind="label"]').text($target.text())
              .end()
           .children('.dropdown-Site').dropdown('toggle');

        if ($target.text() != "Select Site") {
            $("#hdnSite").val($(this).attr("id"));
            $("#ddlConracts").show();
        }
        else {
            $("#ddlConracts").hide();
            $("#jstree").hide();
            $("#divRightSide").hide();
        }

        return false;
    });

    $(document.body).on('click', '#ulContracts li', function (event) {
        var $target = $(event.currentTarget);
        _listContract.css("background-color", "#FFFFFF");

        if (_listContract != $(this)) {
            _listContract.show();
        }

        $(this).css("background-color", "#f9f9c0");

        if ($target.text() == $(this).text()) {
            if (_listContract.text() == $(this).text()) {
                return;
            }
            _listContract = $(this);
        }

        $target.closest('.btn-group')
           .find('[data-bind="label"]').text($target.text())
              .end()
           .children('.dropdown-Contract').dropdown('toggle');

        if ($target.text() != "Select Contract") {
            $("#hdnContract").val($(this).attr("id"));
            $("#jstree").show();
            $("#divRightSide").show();
            $("#divLeftSide").css("border-right", "1px solid #CCCCCC");
        }
        else {
            $("#jstree").hide();
            $("#divRightSide").hide();
        }

        return false;
    });

    $('#jstree').jstree({
        "checkbox": {
            "keep_selected_style": false
        },
        "plugins": ["checkbox"]
    });

    //$('#jstree').jstree('open_all');
    $('#jstree').jstree("open_node", "#j1_1");

    $('#jstree').on('changed.jstree', function (e, data) {
        var i, j, r = [];
        _selectedNodes = data.selected;
    })
      .jstree();

    $(document.body).on('click', '#ulArea li', function (event) {
        var $target = $(event.currentTarget);
        $("#ulArea > :first-child").show();
        _listArea.css("background-color", "#FFFFFF");
        _listArea.removeClass("selected");
        $(this).addClass("selected");

        if (_listArea != $(this)) {
            _listArea.show();
        }

        $(this).css("background-color", "#f9f9c0");

        if ($target.text() == $(this).text()) {
            if (_listArea.text() == $(this).text()) {
                return;
            }
            _listArea = $(this);
        }

        $target.closest('.btn-group')
           .find('[data-bind="label"]').text($target.text())
              .end()
           .children('.dropdown-Area').dropdown('toggle');

        if ($target.text() != "[ALL]") {
            $("#hdnArea").val($(this).attr("id"));
        }
        else {
        }

        return false;
    });
});

function Loading() {
    $("#divLoading").html("<img alt= title= src=" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loading.gif width='10%' />");
    $("#divTrendReportFields").css("background-color", "#EEEEEE");
    $("#dtpFrom").css("background-color", "#EEEEEE");
    $("#dtpTo").css("background-color", "#EEEEEE");
    //$("#btnPreview").attr("disabled", "disabled");
    $("#btnReset").attr("disabled", "disabled");
}

function LoadingComplete() {
    $("#divLoading").html("");
    $("#divTrendReportFields").css("background-color", "#FFFFFF");
    $("#dtpFrom").css("background-color", "#FFFFFF");
    $("#dtpTo").css("background-color", "#FFFFFF");
    $("#btnPreview").removeAttr("disabled");
    $("#btnReset").removeAttr("disabled");
}

$("#btnPreview").click(function () {

    var yKeys = [];
    var labels = [];

    if (Validate() == false)
        return;

    Loading();
    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/TrendAnalysis",
        type: "POST",
        data: { siteNo: $("#hdnSite").val(), contractNo: $("#hdnContract").val(), selectedNodes: JSON.stringify(_selectedNodes), area: $("#spanArea").html(), frequency: $("#hdnFrequency").val(), fromDate: $("#dtpFrom").val(), toDate: $("#dtpTo").val(), groupBy: $("#txtGroup").val(), chartType: "ALL" },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                //$('#jstree').jstree("deselect_all");

                //BAR Chart
                var html = "";
                yKeys = [];
                labels = [];
                yaxisvalues = [];
                dataset = [];

                var parsedData = data.Bars;

                $.each(parsedData[0], function (colName, value) {
                    if (colName != "DateLabel") {
                        yKeys.push(colName);
                        colName = colName.replace("__", " (");
                        colName = colName.replace("_", ")");
                        labels.push(colName);
                    }
                });

                $("#morris-bar-chart").html("");
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
                $("#morris-bar-chart svg").css("height", "375px");

                //LINE Chart
                labels = [];
                dataset = [];
                html = "";
                for (var index = 0; index < data.Lines.length; index++) {
                    if (index % data.Lines[0].Count == 0 && index > 0) {
                        LineColor();
                        dataset.push({
                            fillColor: "rgba(255,255,255,0)",
                            strokeColor: "rgba(" + lineColor + ",0.6)",
                            pointColor: "rgba(" + lineColor + ",0.6)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(" + lineColor + ",1)",
                            //label: data.Lines[index - 1].label,
                            data: yaxisvalues
                        });
                        yaxisvalues = [];

                        html += '<div style="background-color: rgba(' + lineColor + ',0.8);width: 12px; height: 12px; float: left; margin-top: 2px;"></div>';
                        html += '<div style="font-size: 12px; margin-left: 5px; float: left; margin-right: 5px;">' + data.Lines[index - 1].label + '</div>';
                    }
                    yaxisvalues.push([data.Lines[index].lineValue]);
                }

                LineColor();
                dataset.push({
                    fillColor: "rgba(255,255,255,0)",
                    strokeColor: "rgba(" + lineColor + ",0.6)",
                    pointColor: "rgba(" + lineColor + ",0.6)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(" + lineColor + ",1)",
                    //label: data.Lines[index - 1].label,
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

                var ctx = document.getElementById("canvas").getContext("2d");
                window.myLine = new Chart(ctx).Line(lineChartData, {
                    responsive: false
                });
                $("#canvas").height($("#divLinePanel").height() - 80);

                //PIE Chart
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

                $.plot($("#flot-pie-chart"), data.Pies, options);

                LoadingComplete();
                //alert(data);
            }
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
});

$("#btnReset").click(function () {
    $('#jstree').jstree("deselect_all");
    $("#dtpFrom").val("");
    $("#dtpTo").val("");

    var ulSite = $("#ulSites li").eq(0).text();
    $(".dropdown-Site").find('[data-bind="label"]').text(ulSite);
    _listSite.css("background-color", "#FFFFFF");

    var ulContract = $("#ulContracts li").eq(0).text();
    $(".dropdown-Contract").find('[data-bind="label"]').text(ulContract);
    _listContract.css("background-color", "#FFFFFF");

    var ulArea = $("#ulArea li").eq(0).text();
    $(".dropdown-Area").find('[data-bind="label"]').text(ulArea);
    _listArea.css("background-color", "#FFFFFF");


    $("#rdoWeeks").prop("checked", true);
    $("#lblGroup").html("Group By No of Weeks: ");
    $("#txtGroup").removeAttr("disabled");
    $("#txtGroup").css("background-color", "#FFFFFF");
    $("#txtGroup").val("1");
    $("#hdnFrequency").val("1");

    $("#ddlConracts").hide();
    $("#jstree").hide();
    $("#divRightSide").hide();

});