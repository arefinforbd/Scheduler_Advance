﻿function LoadInstalledLocations() {
    var html = "";
    var locationStyle = "";

    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/GetInstalledLocations",
        type: "POST",
        data: { contractNo: $("#hdnContract").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {

                var locObject = data[0].Location;
                html += '<table class="table table-striped table-bordered table-hover" id="datatablefiles" style="font-size:12px;cursor:pointer;"><thead><tr><th>Seq.</th><th>Location</th></tr></thead><tbody>';
                LoadInstalledLocationDetail(locObject);

                $.each(data, function (i, loc) {

                    if (loc.Status == false)
                        locationStyle = "background-color: rgb(240,128,128);";
                    else
                        locationStyle = "";

                    if (i < 1) {
                        html += '<tr>'
                            + '<td style="background-color: #f9f9c0; font-weight: bold;"' + locationStyle + '" location=' + loc.Location + '>' + loc.SequenceNo + '</td>'
                            + '<td style="background-color: #f9f9c0; font-weight: bold;"' + locationStyle + '" location=' + loc.Location + '>' + loc.Location + '</td>'
                            + '</tr>';
                    }
                    else {
                        html += '<tr>'
                            + '<td style="' + locationStyle + '" location=' + loc.Location + '>' + loc.SequenceNo + '</td>'
                            + '<td style="' + locationStyle + '" location=' + loc.Location + '>' + loc.Location + '</td>'
                            + '</tr>';
                    }
                    i++;
                });
                html += "</tbody></table>";
                $("#divEquipmentLocation").html(html);
                $("#divEquipmentLocationFooter").html('<span style="font-size:12px;color: #CCCCCC;margin-top:-10px;float:left;">Click on the above location list to see details.</span>');
                $("#divLoading").html("");
                $("#divEquipmentLocation").show();
                $("#divEquipmentLocationFooter").show();
                $("#divLocationDetail").show();
            }
        },
        error: function (request) {
            alert("Please try again. Something went wrong.");
        }
    });
}

$(document.body).on('click', '#divEquipmentLocation #datatablefiles td', function (event) {
    var i = 0;
    
    $("#divEquipmentLocation #datatablefiles tr").each(function () {
        $(this).css("background-color", "#ffffff");
        if (i == 0 || i % 2 > 0) {
            $(this).children('td').each(function () {
                if ($(this).css("background-color") != "rgb(240, 128, 128)") {
                    $(this).css("background-color", "#f9f9f9");
                }
                $(this).css("font-weight", "normal");
            });
        }
        else {
            $(this).children('td').each(function () {
                if ($(this).css("background-color") != "rgb(240, 128, 128)") {
                    $(this).css("background-color", "#ffffff");
                }
                $(this).css("font-weight", "normal");
            });
        }
        i++;
    });

    LoadInstalledLocationDetail($(this).attr("location"));
    if ($(this).css("background-color") != "rgb(240,128,128)") {
        $($(this).parent()).children('td').each(function () {
            if ($(this).css("background-color") != "rgb(240, 128, 128)") {
                $(this).css("background-color", "#f9f9c0");
            }
            $(this).css("font-weight", "bold");
        });
    }
    return false;
});

function LoadInstalledLocationDetail(location) {
    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/GetInstalledLocationDetail",
        type: "POST",
        data: { location:  location},
        dataType: "JSON",
        success: function (data) {
            if (data != null) {

                var installedDate = data.DateInstalled.replace("/Date(", "");
                installedDate = installedDate.replace(")/", "");;

                $("#lblDateInstalled").text(getMMDDYY(parseInt(installedDate)));
                if (data.Status == true) {
                    $("#lblStatus").text("Active");
                    $("#lblStatus").css("color", "#00CC00");
                }
                else {
                    $("#lblStatus").text("InActive");
                    $("#lblStatus").css("color", "#FF0000");
                }
                $("#lblSeq").text(data.SequenceNo);
                $("#lblSerialNo").text(data.Serial);
                $("#lblReportName").text(data.ReportName);
                $("#lblLocation").text(data.Location);
                $("#lblSectionID").text(data.SectionID);
                $("#lblType").text(data.EquipmentType);
                $("#lblQuestionID").text(data.QuestionID);
                $("#lblArea").text(data.Area);
            }
        },
        error: function (request) {
            alert("Please try again. Something went wrong.");
        }
    });
}

function getMMDDYY(ticks) {
    var date = new Date(ticks);
    var mm = date.getMonth()+1;
    var dd = date.getDate();
    var yy = new String(date.getFullYear()).substring(2);
    if (mm < 10) mm = "0"+mm;
    if (dd < 10) dd = "0"+dd;
    return dd + "/" + mm + "/" + yy;
}