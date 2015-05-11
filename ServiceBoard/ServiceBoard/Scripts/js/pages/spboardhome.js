var map;
var index = 0;
var markers = [];
var ibLabels = [];
var statePostCode = [];
var numbersOfJobs = [];
var leftJobsDivWidth = 0;
var mapLatitude = -27.908451;
var mapLongitude = 133.150296;

var myOptions = {
    zoom: 4,
    mapTypeId: google.maps.MapTypeId.ROADMAP
};

map = new google.maps.Map(document.getElementById("googleMap-dialog"), myOptions);

function Validate() {
    if ($("#txtGoogleMapFromDate").val().trim().length <= 0) {
        alert("Please select From Date.");
        $("#txtGoogleMapFromDate").focus();
        return false;
    }

    if ($("#txtGoogleMapToDate").val().trim().length <= 0) {
        alert("Please select To Date.");
        $("#txtGoogleMapToDate").focus();
        return false;
    }

    var dayDiff = 0;
    dayDiff = new Date($("#txtGoogleMapToDate").datepicker('getDate')) - new Date($("#txtGoogleMapFromDate").datepicker('getDate'));

    if (dayDiff < 0) {
        alert("To date cannot be smaller than From date.");
        return false;
    }

    if (dayDiff > 5184000000) {
        alert("Please keep the date range less than or equal to 60 days.");
        return false;
    }

    return true;
}

function LoadTabularData() {
    var html = "";
    var FromDate = $("#txtGoogleMapFromDate").val();
    var ToDate = $("#txtGoogleMapToDate").val();

    $("#divJobList").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loader-bar.gif' />");

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/GetBookedJobList",
        type: "POST",
        data: { fromDate: FromDate, toDate: ToDate, area: $("#txtArea").val(), tech: $("#txtTech").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#spanNumberOfJobs").html(data.length);
                $("#bookedJobsList").show();

                html += '<table class="table table-striped table-bordered table-hover" id="datatablefiles" style="font-size: 12px;"><thead><tr><th>Address</th><th>Customer Name</th><th>Tech</th><th>Job date</th><th>Status</th></tr></thead><tbody>';

                $.each(data, function (i, job) {
                    html += '<tr>';
                    html += '<td>' + job.Address + '</td>';
                    html += '<td>' + job.CustomerName + '</td>';
                    html += '<td>' + job.Tech + '</td>';
                    html += '<td>' + ToDateTimeString(job.JobDate) + '</td>';
                    html += '<td>' + job.Status + '</td>';
                    html += '</tr>';
                });

                $("#divJobList").html(html);
            }
            else {
                $("#spanNumberOfJobs").html("0");
                $("#divJobList").html("");
            }
        },
        error: function (request) {
            $("#spanNumberOfJobs").html("0");
            $("#divJobList").html("");
            alert("No job found. Try again with other criteria.");
        }
    });
}

$("#btnTableSlide").click(function () {

    if (Validate() == false)
        return;

    $("#rightMap").fadeOut(300);
    $("#leftJobs .widget-header .widget-title").fadeIn(200);
    if ($("#leftJobs").is(':visible') == false)
        $("#leftJobs").delay(350).fadeIn(300);

    $("#leftJobs").delay(200).animate(
        { width: "100%" },
        {
            duration: 1500,
            specialEasings: 'linear'
        }
    );

    LoadTabularData();
    $("#divJobList").delay(2000).fadeIn(500);
});

$("#btnMapSlide").click(function () {

    if (Validate() == false)
        return;

    var FromDate = $("#txtGoogleMapFromDate").val();
    var ToDate = $("#txtGoogleMapToDate").val();

    $("#leftJobs .widget-header .widget-title").fadeOut(300);
    $("#leftJobs").animate({ width: '0px' }, 850);
    $("#leftJobs").fadeOut(300);

    LoadJobNumbersByPostCode(FromDate, ToDate, $('#googleMap-dialog'));

    if ($("#rightMap").is(':visible') == false)
        $("#rightMap").delay(1150).fadeIn(200);

    $("#googleMap-dialog").delay(500).animate(
        { width: "100%" },
        {
            duration: 1500,
            specialEasings: 'linear'
        }
    );

    $('#rightMap').delay(500).animate({ width: "100%" },
    {
        progress: function () {
            google.maps.event.trigger(map, 'resize');
            var centerMap = new google.maps.LatLng(mapLatitude, mapLongitude);
            map.panTo(centerMap);
        },
        duration: 1500
    });
});

$("#btnSearchBookedJobs").click(function () {

    if (Validate() == false)
        return;

    var mapDelay = 0;
    if ($("#leftJobs").width() > 1000) {
        mapDelay = 1000;
    }

    $("#leftJobs").animate(
        { width: "25%" },
        {
            duration: 1000,
            specialEasings: 'linear'
        }
    );
    $("#rightMap").delay(mapDelay).fadeIn(300);

    var html = "";
    var FromDate = $("#txtGoogleMapFromDate").val();
    var ToDate = $("#txtGoogleMapToDate").val();

    $("#divJobList").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loader-bar.gif' />");

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/GetBookedJobList",
        type: "POST",
        data: { fromDate: FromDate, toDate: ToDate, area: $("#txtArea").val(), tech: $("#txtTech").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $("#spanNumberOfJobs").html(data.length);
                $("#bookedJobsList").show();

                $.each(data, function (i, job) {
                    html += '<div class="jobListItem">';
                    html += '<a style="cursor: pointer;">' + job.Address + '</a><br />';
                    html += '<b>Client: </b><span class="spanClient">' + job.CustomerName + '</span><br/>';
                    html += '<b>Tech: </b><span class="spanTech">' + job.Tech + '</span><br/>';
                    html += '<b>Job Date: </b><span class="spanJobDate">' + ToDateTimeString(job.JobDate) + '</span><br/>';
                    html += '<b>Job Status: </b><span class="spanJobStatus">' + job.Status + '</span></div>';
                });

                $("#divJobList").html(html);
            }
            else {
                $("#spanNumberOfJobs").html("0");
                $("#divJobList").html("");
                //alert("No job found. Try again with other criteria.");
            }
        },
        error: function (request) {
            $("#spanNumberOfJobs").html("0");
            $("#divJobList").html("");
            alert("No job found. Try again with other criteria.");
        }
    });

    $("#leftJobs").delay(0).fadeIn(300);
    $("#leftJobs .widget-header .widget-title").delay(0).fadeIn(300);

    var leftJobsWidth = $("#leftJobs").width();
    if ($("#leftJobs").width() == 0 || $("#leftJobs").width() > 1000) {
        leftJobsWidth = leftJobsDivWidth;
    }

    LoadJobNumbersByPostCode(FromDate, ToDate, $('#googleMap-dialog'));
    LoadJobNumbersByPostCode(FromDate, ToDate, $('#googleMap-dialog'));

    $("#googleMap-dialog").delay(mapDelay).animate(
        { width: (leftJobsWidth * 3.08) },
        {
            duration: 1000,
            specialEasings: 'linear'
        }
    );

    $('#rightMap').delay(mapDelay).animate({ width: (leftJobsWidth * 3.08) },
    {
        progress: function () {
            google.maps.event.trigger(map, 'resize');
            var centerMap = new google.maps.LatLng(mapLatitude, mapLongitude);
            map.panTo(centerMap);
        }
    });
});

$(document.body).on('click', '#divJobList div.jobListItem a', function () {

    var address = $(this).html();
    myOptions = {
        zoom: 17,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("googleMap-dialog"), myOptions);
    var infowindow = new google.maps.InfoWindow();
    var pinIconUrl = 'http://maps.google.com/mapfiles/ms/icons/';
    var pinColors = ['green-dot.png', 'red-dot.png', 'blue-dot.png', 'red-dot.png', 'green-dot.png'];
    var statuses = ["Checked", "Booked", "Assign", "Complete", "Quote"];

    $("#divJobList .jobListItem").each(function () {
        $(this).css("background-color", "#FFFFFF");
    });

    var divJobItem = $(this).parent();
    divJobItem.css("background-color", "#EEEEEE");

    $.getJSON('http://maps.googleapis.com/maps/api/geocode/json?address=' + address + '&sensor=true', null, function (data) {
        if (data.status == "OK") {
            var statusText = divJobItem.find(".spanJobStatus").text();
            var p = data.results[0].geometry.location
            var latlng = new google.maps.LatLng(p.lat, p.lng);
            var bookedDate = (new Date().getDate()) + "/" + (new Date().getMonth() + 1) + "/" + new Date().getFullYear();

            var tooltipText = "Address: <span style='font-weight: bold;'>" + address + "</span><br/>";
            tooltipText += "Tech: <span style='font-weight: bold;'>" + divJobItem.find(".spanTech").text() + "</span><br/>";
            tooltipText += "Client: <span style='font-weight: bold;'>" + divJobItem.find(".spanClient").text() + "</span><br/>";
            tooltipText += "Job Date: <span style='font-weight: bold;'>" + divJobItem.find(".spanJobDate").text() + "</span><br/>";
            tooltipText += "Status: <span style='font-weight: bold;'>" + statusText + "</span>";

            infowindow.close();
            map.setCenter(new google.maps.LatLng(p.lat, p.lng));

            var marker = new google.maps.Marker({
                position: latlng,
                map: map,
                icon: pinIconUrl + pinColors[statuses.indexOf(statusText)]
            });

            infowindow.setContent(tooltipText);
            infowindow.open(map, marker);
        }
        else
            alert("Address Not Found. Please check again.");
    })
});

function LoadJobNumbersByPostCode(FromDate, ToDate, mapControl) {

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/GetJobNumberByPostCode",
        type: "POST",
        data: { fromDate: FromDate, toDate: ToDate, area: $("#txtArea").val(), tech: $("#txtTech").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $.each(data, function (i, job) {
                    statePostCode.push(job.StatePostCode);
                    numbersOfJobs.push(job.NumbersOfJob);
                });
                GetGoogleMapLocation(mapControl, 4, true, true);
            }
        },
        error: function (request) {
        }
    });

    markers = [];
    ibLabels = [];
    statePostCode = [];
    numbersOfJobs = [];
}

$(function () {

    $("#rdoYTD").prop("checked", true);
    LoadAJAXResourceUtilizationOneDayPerTech();
    LoadingSalesYTDChart(1);
    LoadAJAXDebtorAnalysis();

    $("#liYTD").click(function () {
        $("#divYTDLoading2").show();
        $("#divYTDLoading2").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
    });

    $("#liCatComp").click(function () {
        if ($("#rdoMTDCat").prop("checked")) {
            $("#rdoMTDCat").prop("checked", true);
            LoadingSalesCatSum(1, "m");
        }
        else {
            $("#rdoYTDCat").prop("checked", true);
            LoadingSalesCatSum(1, "y");
        }
    });

    $("#rdoYTD").click(function () {
        LoadingSalesYTDChart(1);
    });

    $("#rdoMTD").click(function () {
        LoadingSalesYTDChart(2);
    });

    $("#rdoYTDCat").click(function () {
        LoadingSalesCatSum(1, "y");
    });

    $("#rdoMTDCat").click(function () {
        LoadingSalesCatSum(2, "m");
    });

    $("#fullScreenMap").animatedModal();

    var date = new Date();
    var newdate = new Date(date);
    newdate.setDate(newdate.getDate() - 30);
    var pd = new Date(newdate);

    LoadJobNumbersByPostCode(FormatCustomDate(pd), FormatCustomDate(date), $('#googleMap'));

    $("#vmore-googlemap").on('click', function (e) {

        $("#txtGoogleMapFromDate").val("");
        $("#txtGoogleMapToDate").val("");
        $("#divJobList").html("");
        $("#spanNumberOfJobs").html("0");

        var dialog = $("#dialog-googlemap").removeClass('hide').dialog({
            modal: true,
            width: "100%",
            show: {
                effect: "blind",
                duration: 500
            },
            buttons: [
                {
                    text: "Close",
                    "class": "btn btn-primary btn-minier",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]
        });

        GetGoogleMapLocation($('#googleMap-dialog'), 4, true, true);
        $("#ui-id-3").html("<i class='ace-icon fa fa-star orange'></i> Booked Jobs");
        leftJobsDivWidth = $("#leftJobs").width() + 2;
    });

    $("#fullScreenMap").on('click', function (e) {
        $("#googleMap-fullscreen").height($("#animatedModal").height() - $(".close-animatedModal").height() - 21);
        GetGoogleMapLocation($('#googleMap-fullscreen'), 5, true, true);
    });

    $('#txtGoogleMapFromDate').datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
        onSelect: function (date) {
            if ($("#txtGoogleMapToDate").datepicker('getDate') == null) {
                $('#txtGoogleMapToDate').datepicker("setDate", $("#txtGoogleMapFromDate").datepicker('getDate'));
            }
        }
    })

    $('#txtGoogleMapToDate').datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        changeMonth: true,
        changeYear: true,
        yearRange: (new Date().getFullYear() - 5) + ':' + (new Date().getFullYear() + 2),
        onSelect: function (date) {
            var dayDiff = 0;
            dayDiff = new Date($("#txtGoogleMapToDate").datepicker('getDate')) - new Date($("#txtGoogleMapFromDate").datepicker('getDate'));

            if (dayDiff < 0) {
                alert("To date cannot be smaller than From date.");
                $("#txtGoogleMapToDate").val("");
                return;
            }
        }
    })
});

function LoadingSalesYTDChart(ReportType) {
    $("#flot-bar-chart").hide();
    $("#divYTDLoading").show();
    $("#divYTDLoading").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
    $("#hdnReportType").val(ReportType);
    LoadAJAXSalesAnalysis($("#hdnReportType").val());
    $("#divYTDLoading").html("");
    $("#divYTDLoading").hide();
    $("#flot-bar-chart").show();
}

function LoadingSalesCatSum(ReportType, YearOrMonth) {
    $("#flot-bar-chart-cat").hide();
    $("#divYTDLoading3").show();
    $("#divYTDLoading3").html("<img alt='' src='" + $("#hdnSiteURL").val() + "/Content/Images/loading.gif' width='75px' />");
    $("#hdnReportTypeCat").val(ReportType);
    LoadAJAXSalesAnalysisByCategorySum($("#hdnReportTypeCat").val(), YearOrMonth);
    $("#divYTDLoading3").html("");
    $("#divYTDLoading3").hide();
    $("#flot-bar-chart-cat").show();
}

function LoadAJAXSalesAnalysis(ReportType) {
    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysis",
        type: "POST",
        data: { reportType: ReportType },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                LoadBarChart(data, $("#flot-bar-chart"), true);
            }
            else {
                alert("There is no data to show.");
            }
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
}

function LoadAJAXSalesAnalysisByCategorySum(ReportType, YearOrMonth) {
    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/SalesAnalysisByCategorySum",
        type: "POST",
        data: { reportType: ReportType, yearOrMonth: YearOrMonth },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                LoadBarChart(data, $("#flot-bar-chart-cat"), true);
            }
            else {
                alert("There is no data to show.");
            }
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
}

function LoadAJAXDebtorAnalysis() {
    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/DebtorAnalysis",
        type: "POST",
        data: { },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                LoadLineChart(data, "flot-line-chart");
            }
            else {
                alert("There is no data to show.");
            }
        },
        error: function (request) {
            LoadingComplete();
            alert("Please try again. Something went wrong.");
        }
    });
}

function LoadAJAXResourceUtilizationOneDayPerTech() {

    var FromDate = FormatCustomDate(new Date());

    $.ajax({
        url: $("#hdnSiteURL").val() + "/SPBoard/ResourceUtilizationOneDayPerTech",
        type: "POST",
        data: { fromDate: FromDate },
        dataType: "JSON",
        success: function (data) {
            if (data != null && data.Bars != null) {
                LoadBarChartHorizontal(data, $("#flot-bar-chart3"), false);
                $("#flot-bar-chart3 div.legend table").css("opacity", "0.85");
                $("#flot-bar-chart3 div.legend table").css("background-color", "#FFFFFF");
                $("#flot-bar-chart3 div.legend table").css("right", "-25px");
            }
            else {
                $("#flot-bar-chart3").css("text-align", "center");
                $("#flot-bar-chart3").css("margin", "0 auto");
                $("#flot-bar-chart3").css("padding-top", "100px");
                $("#flot-bar-chart3").css("color", "#BBBBBB");
                $("#flot-bar-chart3").html("There is no data for today. Click View More for details.");
            }
        },
        error: function (request) {
        }
    });
}