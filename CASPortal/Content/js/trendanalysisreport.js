var _selectedNodes = [];
var _listArea = $("#ulArea > :first-child");
var _listTech = $("#ulTechs > :first-child");
var _listChartType = $("#ulChartType > :first-child");

var labels = [];
var yaxisvalues = [];
var dataset = [];
var lineColor = "";
var randomColorFactor = function () { return Math.round(Math.random() * 275) };

function LineColor(colourIndex) {

    var colours = ["235,181,16", "238,13,47", "89,201,52", "221,30,188", "23,227,217", "111,124,140", "128,64,128", "64,128,128", "0,64,128", "39,73,233"];

    if (colourIndex > 9)
        lineColor = randomColorFactor() + "," + randomColorFactor() + "," + randomColorFactor();
    else
        lineColor = colours[colourIndex];
}

function ShowChartLegend(data) {
    $(".spanLine1").html(data.Legend.Line1);
    $(".spanLine2").html(data.Legend.Line2);
    $(".spanLine3").html(data.Legend.Line3);
    $(".spanLine4").html(data.Legend.Line4);
    $(".spanChartFooter").html(data.Legend.Footer);
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

    var ulArea = $("#ulArea > :first-child").text();
    $(".dropdown-Area").find('[data-bind="label"]').text(ulArea);

    var ulChartType = $("#ulChartType > :first-child").text();
    $(".dropdown-ChartType").find('[data-bind="label"]').text(ulChartType);

    $("#jstree").hide();
    $("#divRightSide").hide();

    $("#divLineCompanyInfo").hide();
    $("#divPieCompanyInfo").hide();
    $("#divBarCompanyInfo").hide();

    $("#divLineFooter").hide();
    $("#divPieFooter").hide();
    $("#divBarFooter").hide();

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

$("#btnReset").click(function () {
    ResetSite();
    ResetTrendAnalysisFields();
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
    $("#divLineFooter").show();
    $('#divPanelBodyLine').html2canvas({
        onrendered: function (canvas) {
            $("#chartType").val("Line");
            DownloadChart(canvas);            
            $("#divLineCompanyInfo").hide();
            $("#divLineFooter").hide();
        }
    });
});

$("#btnPieDownload").click(function () {
    $("#divPieCompanyInfo").show();
    $("#divPieFooter").show();
    $('#divPanelBodyPie').html2canvas({
        onrendered: function (canvas) {
            $("#chartType").val("Pie");
            DownloadChart(canvas);
            $("#divPieCompanyInfo").hide();
            $("#divPieFooter").hide();
        }
    });
});

$("#btnBarDownload").click(function () {
    $("#divBarCompanyInfo").show();
    $("#divBarFooter").show();
    $('#divPanelBodyBar').html2canvas({
        onrendered: function (canvas) {
            $("#chartType").val("Bar");
            DownloadChart(canvas);
            $("#divBarCompanyInfo").hide();
            $("#divBarFooter").hide();
        }
    });
});