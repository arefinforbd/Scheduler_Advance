function LoadInstalledLocations() {
    var html = "";
    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/GetInstalledLocations",
        type: "POST",
        data: { contractNo: $("#hdnContract").val() },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {

                var locObject = data[0].Location;
                html += '<table class="table table-striped table-bordered table-hover" id="datatablefiles" style="font-size:12px;cursor:pointer;"><thead><tr><th>Seq</th><th>Location</th></tr></thead><tbody>';
                LoadInstalledLocationDetail(locObject);

                $.each(data, function (i, loc) {
                    html += '<tr>'
                        + '<td>' + loc.SequenceNo + '</td>'
                        + '<td location=' + loc.Location + '>' + loc.Location + '</td>'
                        + '</tr>';
                    i++;
                });
                $("#divEquipmentLocation").html(html);
            }
        },
        error: function (request) {
            alert("Please try again. Something went wrong.");
        }
    });
}

$(document.body).on('click', '#divEquipmentLocation #datatablefiles td', function (event) {
    
    LoadInstalledLocationDetail($(this).attr("location"));

    //$(this).css("background-color", "#f9f9c0");

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