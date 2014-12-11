var _selectedNodes = [];
var _listSite = $("#ulSites > :first-child");
var _listContract = $("#ulContracts > :first-child");
var _listArea = $("#ulArea > :first-child");
var _listChartType = $("#ulChartType > :first-child");

var labels = [];
var yaxisvalues = [];
var dataset = [];
var lineColor = "";
var randomColorFactor = function () { return Math.round(Math.random() * 275) };

function LineColor() {
    var r = 
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

    if ($("#j1_1_anchor").find(".jstree-checkbox").hasClass("jstree-undetermined") == false && $("#j1_1_anchor").hasClass("jstree-clicked") == false) {
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

    //if ((!!navigator.userAgent.match(/Trident.*rv\:11\./)) || (/msie/.test(navigator.userAgent.toLowerCase())))
    //    alert("IE");
    //else
    //    alert("NOT IE");

    var ulSite = $("#ulSites > :first-child").text();
    $(".dropdown-Site").find('[data-bind="label"]').text(ulSite);

    var ulContract = $("#ulContracts > :first-child").text();
    $(".dropdown-Contract").find('[data-bind="label"]').text(ulContract);

    var ulArea = $("#ulArea > :first-child").text();
    $(".dropdown-Area").find('[data-bind="label"]').text(ulArea);

    var ulChartType = $("#ulChartType > :first-child").text();
    $(".dropdown-ChartType").find('[data-bind="label"]').text(ulChartType);

    $("#ddlConracts").hide();
    $("#jstree").hide();
    $("#divRightSide").hide();

    $("#divLinePanel").hide();
    $("#divPiePanel").hide();
    $("#divBarPanel").hide();

    $("#divLineCompanyInfo").hide();
    $("#divPieCompanyInfo").hide();
    $("#divBarCompanyInfo").hide();

    $("#dtpFrom").datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
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
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
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

    $(document.body).on('click', '#ulChartType li', function (event) {
        var $target = $(event.currentTarget);
        $("#ulChartType > :first-child").show();
        _listChartType.css("background-color", "#FFFFFF");
        _listChartType.removeClass("selected");
        $(this).addClass("selected");

        if (_listChartType != $(this)) {
            _listChartType.show();
        }

        $(this).css("background-color", "#f9f9c0");

        if ($target.text() == $(this).text()) {
            if (_listChartType.text() == $(this).text()) {
                return;
            }
            _listChartType = $(this);
        }

        $target.closest('.btn-group')
           .find('[data-bind="label"]').text($target.text())
              .end()
           .children('.dropdown-ChartType').dropdown('toggle');

        if ($target.text() != "All Charts") {
            $("#hdnChartType").val($(this).attr("id"));
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
    $("#btnPreview").attr("disabled", "disabled");
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

function LoadLineChart(data) {

    var html = "";
    var labels = [];
    dataset = [];
    yaxisvalues = [];

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
    else
    {
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
        url: $("#hdnSiteURL").val() + "/Report/TrendAnalysis",
        type: "POST",
        data: { siteNo: $("#hdnSite").val(), contractNo: $("#hdnContract").val(), selectedNodes: JSON.stringify(_selectedNodes), area: $("#spanArea").html(), frequency: $("#hdnFrequency").val(), fromDate: $("#dtpFrom").val(), toDate: $("#dtpTo").val(), groupBy: $("#txtGroup").val(), chartType: "ALL" },
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

                LoadingComplete();
            }
            else {
                LoadingComplete();
                alert("There is no data to show.");
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

    $("#divLinePanel").hide();
    $("#divPiePanel").hide();
    $("#divBarPanel").hide();

    $("#rdoWeeks").prop("checked", true);
    $("#lblGroup").html("Group By No of Weeks: ");
    $("#txtGroup").removeAttr("disabled");
    $("#txtGroup").css("background-color", "#FFFFFF");
    $("#txtGroup").val("1");
    $("#hdnFrequency").val("1");

    $("#ddlConracts").hide();
    $("#jstree").hide();
    $("#divRightSide").hide();
    $("#divLeftSide").css("border-right", "");
});

function DownloadChart(canvas) {
    //if ((!!navigator.userAgent.match(/Trident.*rv\:11\./)) || (/msie/.test(navigator.userAgent.toLowerCase()))) {
    var postURL = $("#hdnSiteURL").val() + "/Report/DownloadChartImage";

    $("#frmDownloadChart").attr("action", postURL);
    $("#base64Data").val(canvas.toDataURL("image/png").replace("data:image/png;base64,", ""));
    $("#frmDownloadChart").submit();
    $("#base64Data").val("");
    //}
    //else {
        //document.location.href = canvas.toDataURL("image/png", 1.0).replace("image/png", "image/octet-stream");
    //}
}

$("#btnLineDownload").click(function () {
    $("#divLineCompanyInfo").show();
    $('#divPanelBodyLine').html2canvas({
        onrendered: function (canvas) {
            $("#chartType").val("Line");
            DownloadChart(canvas);            
            $("#divLineCompanyInfo").hide();
        }
    });
});

$("#btnPieDownload").click(function () {
    $("#divPieCompanyInfo").show();
    $('#divPanelBodyPie').html2canvas({
        onrendered: function (canvas) {
            $("#chartType").val("Pie");
            DownloadChart(canvas);
            $("#divPieCompanyInfo").hide();
        }
    });
});

$("#btnBarDownload").click(function () {
    $("#divBarCompanyInfo").show();
    $('#divPanelBodyBar').html2canvas({
        onrendered: function (canvas) {
            $("#chartType").val("Bar");
            DownloadChart(canvas);
            $("#divBarCompanyInfo").hide();
        }
    });
});