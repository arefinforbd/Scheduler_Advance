//This is a scheduler. This scheduler has starting and endind hour.
//No task can be assigned before the starting and after the ending hours.
//It has dynamic interval multiplicable by 30 minutes.
//Booked time slot cannot be set for another task.

var _listItem = $("#ulItems > :first-child");
var _listSite = $("#ulSites > :first-child");

var _minhours = "7:00";
var _maxhours = "18:00";
var _interval = "30";
var _duration = 0;
var _businessStartHour = 0;
var _thresholdDay = 0;
var _timeSlots = [];
var _combid = "";
var _calenderDate = new Date("01/Jan/1990");

//Populate Date and Time table
$(function () {

    $("#ddlItems").hide();
    HideForPublicSite();
    $("#divDatePicker").hide();
    $("#tblDate").hide();
    $("#tblTime").hide();
    $("#tblDesc").hide();
    $("#divContinueButton").hide();

    var ulSite = $("#ulSites > :first-child").text();
    $(".dropdown-site").find('[data-bind="label"]').text(ulSite);

    var ulItem = $("#ulItems > :first-child").text();
    $(".dropdown-item").find('[data-bind="label"]').text(ulItem);

    $(document).tooltip();
    $("#datepicker").val("");

    $("#datepicker").datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
        onSelect: function (date) {
            if (new Date(FormatDateA(_calenderDate)).getDate() == new Date($("#datepicker").datepicker('getDate')).getDate()
                && new Date(FormatDateA(_calenderDate)).getMonth() == new Date($("#datepicker").datepicker('getDate')).getMonth()
                && new Date(FormatDateA(_calenderDate)).getYear() == new Date($("#datepicker").datepicker('getDate')).getYear())
                return;

            _calenderDate = $("#datepicker").datepicker('getDate');
            if (CheckDate()) {
                $("#divContinueButton").hide();
                $("#tblDate").show();
                $("#tblTime").show();
                $("#tblDesc").show();

                generateDays($(this).datepicker('getDate'));
                generateTimes();
            }
            else {
                alert("Back date cannot be selected.");
                $("#datepicker").val("");
            }
        }
    });
});

function HideForPublicSite() {
    if (location.href.indexOf("?customerid") > 0) {
        $("#ddlSites").hide();
        $("#ddlItems").show();
        $(".navbar-static-top").hide();
        $("#page-wrapper").css("margin", "20px");
        $("#page-wrapper").css("border", "1px solid #DDDDDD");
    }
}

function ResetMain() {
    Reset();

    var ulSite = $("#ulSites li").eq(0).text();
    $(".dropdown-site").find('[data-bind="label"]').text(ulSite);

    var ulItem = $("#ulItems li").eq(0).text();
    $(".dropdown-item").find('[data-bind="label"]').text(ulItem);

    _listSite.css("background-color", "#FFFFFF");
    _listItem.css("background-color", "#FFFFFF");

    $("#itemDescription").html("");
    $("#ddlItems").hide();
    $("#divDatePicker").hide();
}

function Reset() {
    $("#divDatePicker").show();
    $("#datepicker").val("");
    $("#tblDate").html("");
    $("#tblTime").html("");
    $("#tblDate").hide();
    $("#tblTime").hide();
    $("#tblDesc").hide();
    $("#divContinueButton").hide();
    $("#itemDescription").show();
}

$(document.body).on('click', '#ulSites li', function (event) {
    var $target = $(event.currentTarget);
    $("#ulSites > :first-child").show();
    _listSite.css("background-color", "#FFFFFF");
    _listSite.removeClass("selected");
    $(this).addClass("selected");
    $("#ddlItems").show();
    $("#datepicker").val("");

    if (_listSite != $(this)) {
        _listSite.show();
    }

    $(this).css("background-color", "#f9f9c0");

    if ($target.text() == $(this).text()) {
        _listSite = $(this);
    }

    $target.closest('.btn-group')
       .find('[data-bind="label"]').text($target.text())
          .end()
       .children('.dropdown-site').dropdown('toggle');

    if ($target.text() == "Select Site") {
        ResetMain();
    }

    return false;
});

$(document.body).on('click', '#ulItems li', function (event) {
    var $target = $(event.currentTarget);
    $("#ulItems > :first-child").show();
    _listItem.css("background-color", "#FFFFFF");
    $("#datepicker").val("");

    if (_listItem != $(this)) {
        _listItem.show();
    }

    $(this).css("background-color", "#f9f9c0");

    if ($target.text() == $(this).text()) {
        _listItem = $(this);
    }

    $target.closest('.btn-group')
       .find('[data-bind="label"]').text($target.text())
          .end()
       .children('.dropdown-item').dropdown('toggle');

    if ($target.text() != "Select Item") {
        Reset();
        $("#hdnItemID").val($(this).attr("id"));
        $("#itemDescription").html($(this).attr("desc"));
        $("#hdnDuration").val($(this).attr("duration"));
    }
    else {
        $("#divDatePicker").hide();
        $("#tblDate").hide();
        $("#tblTime").hide();
        $("#tblDesc").hide();
        $("#divContinueButton").hide();
        $("#itemDescription").html("");
        $("#itemDescription").hide();
    }

    return false;
});

function CheckDate() {
    var dateDiff = 86400000;
    var dayDiff = 0;
    var calDate = $("#datepicker").datepicker('getDate')

    dayDiff = (new Date(FormatDateA(calDate)) - new Date(FormatDateA(new Date()))) / dateDiff;

    if (dayDiff < 0) {
        return false;
    }
    return true;
}

//This method generates hour in the popup based upon the interval
function AddHour() {
    var chkHour = 0;
    var hour = 0;
    var ttime = $("#txtEndTime").val();

    if (ttime.indexOf(":30") > 0) {
        ttime = ttime.replace(":30", ":50");
    }

    _duration = parseInt($("#hdnDuration").val()) / 60;
    ttime = parseInt(ttime.replace(":", "")) + (_duration * 100);

    if (ttime > parseInt(_maxhours.replace(":", ""))) {
        hour = _maxhours;
        ttime = ttime - (_duration * 100);
        alert("Schedule cannot be set after " + (_maxhours.indexOf(":50") > 0 ? _maxhours.replace(":50", ":30") : _maxhours));
    }

    ttime = ttime.toString().substring(0, ttime.toString().length - 2) + ":" + ttime.toString().substring(ttime.toString().length - 2);

    if (ttime.indexOf(":50") > 0) {
        ttime = ttime.replace(":50", ":30");
    }

    return ttime;
}

//Add button of popup
$("#btnAddHour").click(function () {
    var combidend = $("#hdncombidend").val();

    combidend = parseInt(combidend.replace("id", "")) + 10;
    combidend = "id" + combidend;

    if (CheckAvailableTime(combidend)) {
        alert("There is no available time. Please select another time.");
        return;
    }
    else {
        var startindex = 0;
        var endindex = 0;
        var ttime = AddHour();

        combidend = parseInt(combidend.replace("id", "")) - 10;
        combidend = "id" + combidend;

        if (ttime != $("#txtEndTime").val()) {
            startindex = parseInt(combidend.replace("id", ""));
            endindex = startindex + (parseInt($("#hdnDuration").val() / 30) * 10);
            $("#txtEndTime").val(ttime);
            $("#hdncombidend").val("id" + endindex);
        }
    }
});

//Save button selects and sets the occupied time slots
$("#btnSave").click(function () {
    if (window.confirm("Are you sure to set schedule from " + $("#txtStartTime").val() + " to " + $("#txtEndTime").val() + " on " + $("#txtDate").val() + "?")) {
        var combid = $("#hdncombid").val();
        var combidend = $("#hdncombidend").val();

        var startindex = parseInt(combid.replace("id", ""));
        var endindex = parseInt(combidend.replace("id", ""));

        endindex = isNaN(endindex) ? startindex : endindex;

        for (var index = startindex; index <= endindex; index += 10) {
            $("#tblTime").find('[combid="id' + index + '"]').css("background-color", "#32e09b")
            $("#tblTime").find('[combid="id' + index + '"]').removeAttr("data-target");
            $("#tblTime").find('[combid="id' + index + '"]').removeAttr("data-toggle");
            $("#tblTime").find('[combid="id' + index + '"]').removeAttr("class");
            $("#tblTime").find('[combid="id' + index + '"]').html("N/A");
            $("#tblTime").find('[combid="id' + index + '"]').addClass("occupied");
            $("#tblTime").find('[combid="id' + index + '"]').addClass("selectedtimeslot");
            $("#tblTime").find('[combid="id' + index + '"]').css("vertical-align", "middle");
            $("#tblTime").find('[combid="id' + index + '"]').css("padding", "0px");
        }
        $("#hdncombid").val("");
        $("#hdncombidend").val("");
        $("#divContinueButton").show();

        var scheduledDate = $("#txtDate").val().replace(/-/g, '/');

        _timeSlots.push({
            ItemID: $("#hdnItemID").val().trim(),
            Date: scheduledDate,
            StartTime: $("#txtStartTime").val(),
            EndTime: $("#txtEndTime").val(),
            SpecialInstruction: $("#txtSpecialInstruction").val(),
            IsPublicHoliDay: false
        });
    }
    else {
        return false;
    }
});

function SelectedDate(row) {
    var dateval;
    var caldate = $("#datepicker").datepicker('getDate');
    if (caldate == null)
        caldate = new Date();

    var dayparam = caldate;
    var time = parseInt(row);

    dateval = new Date(dayparam.setDate(caldate.getDate() + (_thresholdDay - 1) + time));
    return FormatDate(dateval);
}

function CheckAvailableTime(combid) {

    var startindex = parseInt(combid.replace("id", ""));
    var endindex = startindex + (parseInt($("#hdnDuration").val() / 30) * 10);

    for (var index = startindex; index < endindex; index += 10) {
        if ($("#tblTime").find('[combid="id' + index + '"]').hasClass("occupied")
            || $("#tblTime").find('[combid="id' + index + '"]').hasClass("afterhour")) {
            return true;
        }
    }

    return false;
}

function CheckMaximumTime(combid) {

    var startHour = $("#tblTime").find('[combid="' + combid + '"]').attr("starthour");
    startHour = startHour.replace(":30", "50");
    startHour = parseFloat(startHour.replace(":", "")) / 100 + ($("#hdnDuration").val() / 60);
    startHour = parseFloat(startHour) * 100;

    if (parseFloat(startHour) > parseFloat(_maxhours.replace(":", ""))) {
        alert("Schedule cannot be set after " + (_maxhours.indexOf(":50") > 0 ? _maxhours.replace(":50", ":30") : _maxhours));
        return true;
    }
    return false;
}

//Yellow cell click event. Popup opens and set date, starting and ending time.
$(document.body).on('click', '#tblTime td.td00', function (event) {

    _combid = "";
    var combid = $(this).attr("combid");
    var combidend = "";

    if (CheckAvailableTime(combid)) {
        alert("There is no available time. Please select another time.");
        $(this).removeAttr("data-target");
        event.preventDefault();
        return;
    }

    if (CheckMaximumTime(combid)) {
        $(this).removeAttr("data-target");
        event.preventDefault();
        return;
    }

    $("#hdncombidend").val("");

    var idval = $(this).attr("id");
    var caldate = $("#datepicker").datepicker('getDate');
    idval = idval.replace("id", "");

    var startHRStr = "";
    var endHRStr = "";

    _duration = parseFloat($("#hdnDuration").val()) / 60;

    startHRStr = $(this).attr("starthour").replace(":00", "");
    endHRStr = parseFloat(startHRStr) + _duration;
    startHRStr = startHRStr + ":00";

    if (endHRStr.toString().indexOf(".5") > 0)
        endHRStr = endHRStr.toString().replace(".5", ":30");
    else
        endHRStr = endHRStr.toString() + ":00";

    var scheduledDate = SelectedDate($(this).attr("row"));
    scheduledDate = scheduledDate.substring(0, scheduledDate.indexOf("<br/>"));
    combidend = parseInt(combid.replace("id", "")) + (_duration * 20) - 10;
    combidend = "id" + combidend;

    $("#txtDate").val(scheduledDate);
    $("#txtStartTime").val(startHRStr);
    $("#txtEndTime").val(endHRStr);
    $("#hdncombid").val(combid);
    $("#hdncombidend").val(combidend);
});

//Blue cell click event. Popup opens and set date, starting and ending time.
$(document.body).on('click', '#tblTime td.td30', function (event) {

    _combid = "";
    var combid = $(this).attr("combid");
    var combidend = "";

    if (CheckAvailableTime(combid)) {
        alert("There is no available time. Please select another time.");
        $(this).removeAttr("data-target");
        event.preventDefault();
        return;
    }

    if (CheckMaximumTime(combid)) {
        $(this).removeAttr("data-target");
        event.preventDefault();
        return;
    }

    $("#hdncombidend").val("");

    var idval = $(this).attr("id");
    var caldate = $("#datepicker").datepicker('getDate');
    idval = idval.replace("id", "");

    var startHRStr = "";
    var endHRStr = "";

    _duration = parseFloat($("#hdnDuration").val()) / 60;

    startHRStr = $(this).attr("starthour").replace(":30", ".5");
    endHRStr = parseFloat(startHRStr) + _duration;
    startHRStr = startHRStr.replace(".5", ":30");

    if (endHRStr.toString().indexOf(".5") > 0)
        endHRStr = endHRStr.toString().replace(".5", ":30");
    else
        endHRStr = endHRStr.toString() + ":00";

    var scheduledDate = SelectedDate($(this).attr("row"));
    scheduledDate = scheduledDate.substring(0, scheduledDate.indexOf("<br/>"));
    combidend = parseInt(combid.replace("id", "")) + (_duration * 20) - 10;
    combidend = "id" + combidend;

    $("#txtDate").val(scheduledDate);
    $("#txtStartTime").val(startHRStr);
    $("#txtEndTime").val(endHRStr);
    $("#hdncombid").val($(this).attr("combid"));
    $("#hdncombidend").val(combidend);
});

function GetStartingHour(idval) {

    var result = "";
    var startHR = 0;
    var endHR = 0;

    endHR = _businessStartHour + (parseInt(idval) * _duration);
    startHR = endHR - _duration;

    if (startHR.toString().indexOf(".5") > 0)
        startHR = startHR.toString().replace(".5", ":30");
    else
        startHR = startHR.toString() + ":00";

    result = startHR;

    return result;
}

function GetEndingHour(idval) {

    var result = "";
    var startHR = 0;
    var endHR = 0;

    endHR = _businessStartHour + (parseInt(idval) * _duration);
    startHR = endHR - _duration;

    if (endHR.toString().indexOf(".5") > 0)
        endHR = endHR.toString().replace(".5", ":30");
    else
        endHR = endHR.toString() + ":00";

    result = endHR;

    return result;
}

//Method for populating Time. This method draws cells of timeslots.
//It draws the cells grey color which are before starting and after ending hours.
//It draws the occupied cells deep grey color.
function generateTimes() {

    var startingHr = "";
    var endingHr = "";
    var timeval;
    var index = 0;
    var urlVal = "";
    var startHR = "";
    var endHR = "";

    urlVal = $("#hdnSiteURL").val() + "/Scheduler/GetTimeSlots";
    $("#tblTime").html("<img alt= title= src=" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loading.gif width='10%' />");

    var calDate = $("#datepicker").datepicker('getDate');
    calDate = new Date(calDate.setDate(calDate.getDate() + _thresholdDay));

    if (calDate == null)
        calDate = new Date();

    $.ajaxSetup({ cache: false });
    $.ajax({
        url: urlVal,
        type: "GET",
        data: { dateStartedFrom: calDate, itemID: $("#hdnItemID").val() },
        dataType: "json",
        success: function (data) {
            if (data != null) {
                var html = "";
                var html_tbl_row = "";
                var hourHeader = "";

                _businessStartHour = parseInt(data.MinStartHour);
                var businessEndHour = parseInt(data.MaxEndHour);

                //_businessStartHour = 6;
                //businessEndHour = 19;

                //_duration = parseInt(data.Duration);
                _duration = 30;
                _duration = (_duration / 60);

                if (businessEndHour.toString().indexOf(".5") > 0) {
                    _maxhours = businessEndHour.toString().replace(".5", ":50");
                }
                else
                    _maxhours = businessEndHour + ":00";

                html += '<thead><tr class="timeheader">';

                for (index = _businessStartHour; index < businessEndHour; index++) {
                    hourHeader = index.toString().indexOf(".5") > 0 ? index.toString().replace(".5", ":30") : index + ":00";
                    html += '<th colspan="2">' + hourHeader + '</th>';
                }

                //Generating cells for timeslots
                for (var tr = 1; tr <= 7; tr++) {
                    html += '<tr>';
                    for (var td = 1; td <= ((businessEndHour - _businessStartHour) / _duration) ; td++) {

                        startHR = parseInt(_businessStartHour) + parseInt(_duration);
                        endHR = parseInt(_businessStartHour) + parseInt(_duration) + 0;
                        startingHr = GetStartingHour(td);
                        endingHr = GetEndingHour(td);

                        if (td % 2 > 0) {
                            html += '<td starthour="' + startingHr + '" colspan="' + (_duration * 2) + '" title="' + (startingHr + "-" + endingHr) + '" combid="id' + td + tr + '" id="id' + td + '" row="' + tr + '" data-target="#myModal" data-toggle="modal" class="tdTime td00"></td>';
                        }
                        else {
                            html += '<td starthour="' + startingHr + '" colspan="' + (_duration * 2) + '" title="' + (startingHr + "-" + endingHr) + '" combid="id' + td + tr + '" id="id' + td + '" row="' + tr + '" data-target="#myModal" data-toggle="modal" class="tdTime td30"></td>';
                        }
                    }
                    html += '</tr>';
                }

                html += '</tbody>';
                $("#tblTime").html("");
                $("#tblTime").html(html);

                if (/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini/i.test(navigator.userAgent.toLowerCase())) {
                    $("#tblTime").css("margin-left", "19px");
                    $("#tblTime td").css("height", "51.5px");
                }

                var dateDiff = 86400000;
                var dayDiff = 0;
                var startindex = 0;
                var endindex = 0;
                var diff = 0;

                //Setting color for the cells before starting hours of everyday
                $.each(data.BusinessHours, function (i, bsHour) {

                    dayDiff = (new Date(bsHour.Date) - new Date(FormatDateA(calDate))) / dateDiff;
                    dayDiff = dayDiff + 1;

                    //Color will be set for next seven days
                    if (dayDiff >= 0 && dayDiff <= 7) {
                        endindex = (parseFloat(bsHour.BusinessStartHour) - parseFloat(_businessStartHour)) * 2

                        startindex = parseInt(1 + "" + dayDiff);
                        endindex = parseInt(endindex + "" + dayDiff);

                        for (var lIndex = startindex; lIndex <= endindex; lIndex += 10) {
                            $("#tblTime").find('[combid="id' + lIndex + '"]').css("background-color", "#CCCCCC")
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-target");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-toggle");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("class");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("title");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').addClass("afterhour");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').css("padding", "0px");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').css("border-right", "1px solid #CCCCCC");
                        }
                    }

                    //If scheduler starts from the current date, starting time will be after current time.
                    if (new Date(FormatDateA(new Date())).getDate() == new Date(bsHour.Date).getDate()
                        && new Date(FormatDateA(new Date())).getMonth() == new Date(bsHour.Date).getMonth()
                        && new Date(FormatDateA(new Date())).getYear() == new Date(bsHour.Date).getYear()) {

                        var currentHour = new Date().getHours();
                        var currentMinute = new Date().getMinutes();

                        if (currentMinute > 30)
                            currentHour++;
                        else
                            currentHour += 0.5;

                        endindex = (parseFloat(currentHour) - parseFloat(_businessStartHour)) * 2

                        startindex = parseInt(1 + "" + dayDiff);
                        endindex = parseInt(endindex + "" + dayDiff);

                        for (var lIndex = startindex; lIndex <= endindex; lIndex += 10) {
                            $("#tblTime").find('[combid="id' + lIndex + '"]').css("background-color", "#CCCCCC")
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-target");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-toggle");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("class");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("title");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').addClass("afterhour");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').css("padding", "0px");
                            $("#tblTime").find('[combid="id' + lIndex + '"]').css("border-right", "1px solid #CCCCCC");
                        }
                    }
                });

                //Setting color for the cells after ending hours of everyday
                $.each(data.BusinessHours, function (i, bsHour) {

                    dayDiff = (new Date(bsHour.Date) - new Date(FormatDateA(calDate))) / dateDiff;
                    dayDiff = dayDiff + 1;

                    //Color will be set for next seven days
                    if (dayDiff >= 0 && dayDiff <= 7) {
                        startindex = (((parseFloat(bsHour.BusinessEndHour) - parseFloat(_businessStartHour)) * 2) + 1);
                        endindex = ((businessEndHour - parseFloat(_businessStartHour))) * 2;

                        if (bsHour.IsWorkingDay) {
                            startindex = parseInt(startindex + "" + dayDiff);
                            endindex = parseInt(endindex + "" + dayDiff);

                            for (var lIndex = startindex; lIndex <= endindex; lIndex += 10) {
                                $("#tblTime").find('[combid="id' + lIndex + '"]').css("background-color", "#CCCCCC")
                                $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-target");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-toggle");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("class");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("title");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').addClass("afterhour");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').css("padding", "0px");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').css("border-right", "1px solid #CCCCCC");
                            }
                        }
                    }
                });

                //Setting occupied/booked hours for days
                $.each(data.TimeSlots, function (i, occupied) {

                    dayDiff = (new Date(occupied.Date) - new Date(FormatDateA(calDate))) / dateDiff;
                    dayDiff = dayDiff + 1;

                    //Color will be set for next seven days
                    if (dayDiff >= 0 && dayDiff <= 7) {
                        startindex = (((parseFloat(occupied.StartTime) - parseFloat(_businessStartHour)) * 2) + 1);

                        if (startindex >= 0) {
                            endindex = ((parseFloat(occupied.EndTime) - parseFloat(_businessStartHour)) * 2);

                            startindex = parseInt(startindex + "" + dayDiff);
                            endindex = parseInt(endindex + "" + dayDiff);

                            for (var lIndex = startindex; lIndex <= endindex; lIndex += 10) {
                                $("#tblTime").find('[combid="id' + lIndex + '"]').css("background-color", "#808080")
                                $("#tblTime").find('[combid="id' + lIndex + '"]').css("vertical-align", "middle");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').css("padding", "0px");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-target");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("data-toggle");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').removeAttr("class");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').html("N/A");
                                $("#tblTime").find('[combid="id' + lIndex + '"]').addClass("occupied");
                            }
                        }

                        if (occupied.IsPublicHoliDay) {

                            endindex = ((businessEndHour - parseFloat(_businessStartHour))) * 2;
                            startindex = parseInt("1" + dayDiff);
                            endindex = parseInt(endindex + "" + dayDiff);

                            var incIndex = startindex + 10;
                            var colSpan = ((endindex - startindex) + 10) / 10;

                            for (var lIndex = incIndex; lIndex <= endindex; lIndex += 10) {
                                $("#tblTime").find('[combid="id' + lIndex + '"]').remove();
                            }

                            $("#tblTime").find('[combid="id' + startindex + '"]').css("background-color", "#CCCCCC")
                            $("#tblTime").find('[combid="id' + startindex + '"]').css("font-size", "16px");
                            $("#tblTime").find('[combid="id' + startindex + '"]').css("vertical-align", "middle");
                            $("#tblTime").find('[combid="id' + startindex + '"]').css("padding", "0px");
                            $("#tblTime").find('[combid="id' + startindex + '"]').attr("colspan", colSpan);
                            $("#tblTime").find('[combid="id' + startindex + '"]').removeAttr("data-target");
                            $("#tblTime").find('[combid="id' + startindex + '"]').removeAttr("data-toggle");
                            $("#tblTime").find('[combid="id' + startindex + '"]').removeAttr("class");
                            $("#tblTime").find('[combid="id' + startindex + '"]').addClass("afterhour");
                            $("#tblTime").find('[combid="id' + startindex + '"]').html("Public Holiday");
                        }
                    }
                });

                //Setting color for the weekend
                $.each(data.BusinessHours, function (i, bsHour) {

                    dayDiff = (new Date(bsHour.Date) - new Date(FormatDateA(calDate))) / dateDiff;
                    dayDiff = dayDiff + 1;

                    //Color will be set for next seven days
                    if (dayDiff >= 0 && dayDiff <= 7) {
                        startindex = (((parseFloat(bsHour.BusinessEndHour) - parseFloat(_businessStartHour)) * 2) + 1);
                        endindex = ((businessEndHour - parseFloat(_businessStartHour))) * 2;

                        if (!bsHour.IsWorkingDay) {
                            //If weekend
                            endindex = ((businessEndHour - parseFloat(_businessStartHour))) * 2;
                            startindex = parseInt("1" + dayDiff);
                            endindex = parseInt(endindex + "" + dayDiff);

                            var incIndex = startindex + 10;
                            var colSpan = ((endindex - startindex) + 10) / 10;

                            for (var lIndex = incIndex; lIndex <= endindex; lIndex += 10) {
                                $("#tblTime").find('[combid="id' + lIndex + '"]').remove();
                            }

                            $("#tblTime").find('[combid="id' + startindex + '"]').css("background-color", "#CCCCCC")
                            $("#tblTime").find('[combid="id' + startindex + '"]').css("font-size", "16px");
                            $("#tblTime").find('[combid="id' + startindex + '"]').css("vertical-align", "middle");
                            $("#tblTime").find('[combid="id' + startindex + '"]').css("padding", "0px");
                            $("#tblTime").find('[combid="id' + startindex + '"]').attr("colspan", colSpan);
                            $("#tblTime").find('[combid="id' + startindex + '"]').removeAttr("data-target");
                            $("#tblTime").find('[combid="id' + startindex + '"]').removeAttr("data-toggle");
                            $("#tblTime").find('[combid="id' + startindex + '"]').removeAttr("class");
                            $("#tblTime").find('[combid="id' + startindex + '"]').addClass("afterhour");
                            $("#tblTime").find('[combid="id' + startindex + '"]').html("Weekend");
                        }
                    }
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("An error occurred while retrieving schedule information, please try again.");
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}

//Method for populating Dates
function generateDays(day) {

    var dateval;
    var index = 0;
    var dayparam = day;
    var html = '<thead><tr class="timeheader"><th id="dateheader"></th></tr>';
    _timeSlots = [];

    /*$.ajaxSetup({ cache: false });
    $.ajax({
        url: $("#hdnSiteURL").val() + "/Scheduler/GetThresholdDay",
        type: "GET",
        data: { },
        dataType: "JSON",
        success: function (data) {
            if (data != null && data == "successfull") {
                _thresholdDay = data.ThresholdDay;    
            }
            else
                alert("Please try again. Something went wrong.");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Please try again. Something went wrong.");
        },
        complete: function (jqXHR, textStatus) {

        }
    });*/

    _thresholdDay = 0;

    //Scheduler will start from few days after as per thresholdDay value.
    day = new Date(day.setDate(day.getDate() + (_thresholdDay - 1)));

    for (index = 1; index <= 7; index++) {
        day = new Date(dayparam.setDate(day.getDate() + 1));
        html += '<tr><th>' + FormatDate(day) + '</th></tr>';
    }
    html += '</thead>';

    $("#tblDate").html(html);
}

$("#btnContinue").click(function () {

    var customerid = "";
    var siteID = $('#ulSites li.selected').attr('id');

    $.ajax({
        url: $("#hdnSiteURL").val() + "/Scheduler/PostTimeSlot",
        type: "POST",
        data: { siteID: siteID, itemID: $("#hdnItemID").val().trim(), timeSlots: JSON.stringify(_timeSlots) },
        dataType: "JSON",
        success: function (data) {
            if (data != null && data == "successfull") {

                if (location.href.indexOf("?customerid") > 0) {
                    customerid = "/?customerid=" + window.location.href.slice(window.location.href.indexOf('=') + 1);
                }
                window.location.href = $("#hdnSiteURL").val() + "/Scheduler/CustomerInformation" + customerid;
            }
        },
        error: function (request) {
            alert("Please try again. Something went wrong.");
        }
    });
});

$("#btnReset").click(function () {
    ResetMain();
});

function FormatDate(datevalue) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

    var yyyy = datevalue.getFullYear().toString();
    var mm = monthNames[datevalue.getMonth()];
    var dd = datevalue.getDate().toString();

    var finalDate = (dd[1] ? dd : "0" + dd[0]) + "-" + mm + "-" + yyyy + "<br/>" + dayNames[datevalue.getDay()];

    return finalDate;
}

function FormatDateA(datevalue) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var finalDate = "";
    var yyyy = datevalue.getFullYear().toString();
    var mm = monthNames[datevalue.getMonth()];
    var dd = datevalue.getDate().toString();

    //IE 11 and below are being checked here
    /*if ((!!navigator.userAgent.match(/Trident.*rv\:11\./)) || (/msie/.test(navigator.userAgent.toLowerCase())))
        finalDate = mm + " " + dd + ", " + yyyy;
    else
        finalDate = (dd[1] ? dd : "0" + dd[0]) + "/" + mm + "/" + yyyy;*/

    finalDate = mm + " " + dd + ", " + yyyy;

    return finalDate;
}