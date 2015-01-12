function Validate() {

    var statusVal = "";
    var sortVal = "";

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

    if ($("#rdoAll").prop("checked") == true)
        statusVal = "All";
    if ($("#rdoActive").prop("checked") == true)
        statusVal = "Active";
    if ($("#rdoInActive").prop("checked") == true)
        statusVal = "InActive";

    if ($("#rdoLocation").prop("checked") == true)
        sortVal = "Location";
    if ($("#rdoArea").prop("checked") == true)
        sortVal = "Area";

    var url = $("#hdnSiteURL").val() + "/Report/EquipmentReport";
    $.ajax({
        url: url,
        type: "POST",
        data: {
            hdnSite: $("#hdnSite").val(), hdnContract: $("#hdnContract").val(), rdoStatus: statusVal, rdoSort: sortVal
        },
        dataType: "json",
        success: function (resp) {
            if (resp == "No BLOB")
                alert("There is no data to show.");
            else {
                $("#hdnFormSubmit").val("BLOB");
                $('#eqrepoform').submit();
            }

            return false;
        }
    })

    return true;
}