var listobjectpvt = $("#pvtfolder > :first-child");
var listobjectpub = $("#pubfolder > :first-child");

$(document).ready(function () {
    hideSbtBtn();
    var pvtfirstval = $("#pvtfolder > :first-child").text();
    var pubfirstval = $("#pubfolder > :first-child").text();
    $("#pvtfolder > :first-child").hide();
    $("#pubfolder > :first-child").hide();
    $(".dropdown-pvt").find('[data-bind="label"]').text(pvtfirstval);
    $(".dropdown-pub").find('[data-bind="label"]').text(pubfirstval);

    var datetime = {
        format: '%A %B %d, %Y %I:%M:%S' // 12-hour
    };
    $('#livedateTime').jclock(datetime);
});

function GetAdvertiseStatus() {

    var urlVal = $("#hdnSiteURL").val() + "/CustomerInformation/GetAdvertiseStatus";

    $.ajaxSetup({ cache: false });
    $.ajax({
        url: urlVal,
        type: "GET",
        data: { },
        dataType: "json",
        success: function (data) {
            if (data != null) {

                if (data == false) {
                    $("#maintable").removeClass("col-lg-8");
                    $("#maintable").addClass("col-lg-12");
                    $("#advertisements").hide();
                    $("#applogo").removeClass("pull-right");
                    $("#ddlprivatefolder").removeClass("col-lg-6");
                    $("#ddlprivatefolder").addClass("col-lg-4");
                    $("#ddlpublicfolder").removeClass("col-lg-6");
                    $("#ddlpublicfolder").addClass("col-lg-8");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("An error occurred while retrieving internal value.");
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}

$(document.body).on('click', '#pvtfolder li', function (event) {
    var $target = $(event.currentTarget);
    $("#pvtfolder > :first-child").show();
    listobjectpvt.css("background-color", "#FFFFFF");

    if (listobjectpvt != $(this)) {
        listobjectpvt.show();
    }

    $(this).css("background-color", "#f9f9c0");

    if ($target.text() == $(this).text()) {
        listobjectpvt = $(this);
    }

    $("#folderHeading").html("Private Folder :: ");
    $("#folderName").html($target.text());

    $target.closest('.btn-group')
       .find('[data-bind="label"]').text($target.text())
          .end()
       .children('.dropdown-pvt').dropdown('toggle');

    listobjectpub.show();
    listobjectpub.css("background-color", "#FFFFFF");
    $(".dropdown-pub").find('[data-bind="label"]').text($("#pubfolder > :first-child").text());

    onFolderSelection('pvt');

    return false;
});

$(document.body).on('click', '#pubfolder li', function (event) {
    var $target = $(event.currentTarget);
    $("#pubfolder > :first-child").show();
    listobjectpub.css("background-color", "#FFFFFF");

    if (listobjectpub != $(this)) {
        listobjectpub.show();
    }

    $(this).css("background-color", "#f9f9c0");

    if ($target.text() == $(this).text()) {
        listobjectpub = $(this);
    }

    $("#folderHeading").html("Public Folder :: ");
    $("#folderName").html($target.text());

    $target.closest('.btn-group')
       .find('[data-bind="label"]').text($target.text())
          .end()
       .children('.dropdown-pub').dropdown('toggle');

    listobjectpvt.show();
    listobjectpvt.css("background-color", "#FFFFFF");
    $(".dropdown-pvt").find('[data-bind="label"]').text($("#pvtfolder > :first-child").text());

    onFolderSelection('pub');

    return false;
});

function onFolderSelection(folderType) {

    var fileLabel = "";
    var folderSelected = "";
    var urlVal = "";
    var pathName = "";

    hideSbtBtn();

    urlVal = $("#hdnSiteURL").val() + "/CustomerInformation/GetFolderFiles";
    var clientTimeZoneOffSet = (new Date().getTimezoneOffset()) * (-1);

    //private folder
    if (folderType == 'pvt') {
        fileLabel = "Private File(s) :";
        folderSelected = $(".dropdown-pvt").find('[data-bind="label"]').text();
    }

    //public folder
    if (folderType == 'pub') {
        fileLabel = "Public File(s) :";
        folderSelected = $(".dropdown-pub").find('[data-bind="label"]').text();
    }

    $.ajaxSetup({ cache: false });
    $.ajax({
        url: urlVal,
        type: "GET",
        data: { folderName: folderSelected, folderType: folderType, clientTzOffset: clientTimeZoneOffSet },
        dataType: "json",
        success: function (data) {
            if (data != null) {
                var html = "";
                var html_tbl_row = "";

                if (data.length <= 0) {
                    $("#datatablefiles_paginate .pagination").hide();
                }

                var i = 0;
                html += '<table class="table table-striped table-bordered table-hover" id="datatablefiles" style="font-size: 12px;"><thead><tr><th>File Name</th><th>File Description</th><th>Date Uploaded</th></tr></thead><tbody>';
                var pathName = "fileSelected";

                $.each(data, function (i, folder) {
                    html_tbl_row += '<tr class="odd gradeX">'
                        + '<td><input type="radio" id="rb' + i + '" name="rb" value="">'
                        + '<label style="font-weight:normal;" class="fileclass" for="rb' + i + '">' + folder.fileName + '</label></td>'
                        + '<td>' + folder.fileDescription + '</td>'
                        + '<td>' + folder.fileUploadedDate + '</td>'
                        + '<td>' + dateTimeString(folder.fileUploadedDate) + '</td>'
                        + '</tr>';
                    i++;
                });
                html += html_tbl_row + '</tbody></table>';
                $('#divTableFiles').html(html);
                if (data.length > 0) {
                    $('#datatablefiles').dataTable({
                        "order": [[3, "desc"]],
                        "columnDefs": [
                        {
                            "targets": [3],
                            "visible": false,
                            "searchable": false
                        }]
                    });
                    $("#datatablefiles_wrapper .row .col-sm-6").addClass("col-lg-6 col-md-6 col-xs-6");
                }
            } else {
                $("#datatablefiles_paginate").hide();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("An error occurred while retrieving file information, please try again.");
        },
        complete: function (jqXHR, textStatus) {

        }
    });
}

$(document.body).on('click', '.fileclass', function (event) {
    $("#hiddenfoldertype").val($("#folderHeading").html());
    $("#hiddenfilename").val($(this).html().trim());
    $("#hiddenfoldername").val($("#folderName").html());
    showSbtBtn();
});

function showSbtBtn() {
    $("#div_btnSbmt").show();
}

function hideSbtBtn() {
    $("#div_btnSbmt").hide();
}

function dateTimeString(dateTime) {

    var splitedDateTime = dateTime.split(" ");
    var splitedDate = splitedDateTime[0].split("/");
    var dateformated = splitedDate[2] + splitedDate[1] + splitedDate[0];

    var splitedTimeZone = splitedDateTime[1].split("+");
    var splitedTimeS = splitedTimeZone[0].split(".");
    var splitedTime = splitedTimeS[0].split(":");
    var splitedS = splitedTimeS[1];

    var timeformated = splitedTime[0] + splitedTime[1] + splitedTime[2] + splitedS;

    return (dateformated + timeformated);
}

$(function () {
    $("#mainDiv").show();
});