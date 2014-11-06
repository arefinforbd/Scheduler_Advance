$(function () {

    HideForPublicSite();

    if ($("#lastname").val().trim().length > 0) {
        $("#firstname").removeClass("mandatory");
        $("#mobileno").removeClass("mandatory")
    }

    if ($("#firstname").val() != "")
        $("#firstname").attr("readonly", true);
    if ($("#lastname").val() != "")
        $("#lastname").attr("readonly", true);
    if ($("#email").val() != "")
        $("#email").attr("readonly", true);
    if ($("#houseno").val() != "")
        $("#houseno").attr("readonly", true);
    if ($("#streetname").val() != "")
        $("#streetname").attr("readonly", true);
    if ($("#address").val() != "")
        $("#address").attr("readonly", true);
    if ($("#city").val() != "")
        $("#city").attr("readonly", true);
    if ($("#state").val() != "")
        $("#state").attr("readonly", true);
    if ($("#postcode").val() != "")
        $("#postcode").attr("readonly", true);
    if ($("#phoneno").val() != "")
        $("#phoneno").attr("readonly", true);
    if ($("#mobileno").val() != "")
        $("#mobileno").attr("readonly", true);
});

function HideForPublicSite() {
    if (location.href.indexOf("?customerid") > 0) {
        $(".navbar-static-top").hide();
        $("#page-wrapper").css("margin", "20px");
        $("#page-wrapper").css("border", "1px solid #DDDDDD");
    }
}

$("#btnSubmit").click(function () {

    if (Validate()) {

        if (confirm("Are you sure to send your information?")) {
            var customerid = "";
            var firstName = $("#firstname").val().trim();
            var lastName = $("#lastname").val().trim();
            var email = $("#email").val().trim();
            var houseNo = $("#houseno").val().trim();
            var streetName = $("#streetname").val().trim();
            var address = $("#address").val().trim();
            var city = $("#city").val().trim();
            var state = $("#state").val().trim();
            var postCode = $("#postcode").val().trim();
            var phoneNo = $("#phoneno").val().trim();
            var mobileNo = $("#mobileno").val().trim();

            $.ajaxSetup({ cache: false });
            $.ajax({
                url: $("#hdnSiteURL").val() + "/Scheduler/SendCustomerInformation",
                type: "POST",
                data: { firstname: firstName, lastname: lastName, email: email, houseno: houseNo, streetname: streetName, address: address, city: city, state: state, postcode: postCode, phoneno: phoneNo, mobileno: mobileNo },
                dataType: "JSON",
                success: function (data) {
                    if (data != null && data == "successfull") {
                        alert("Thank you. Your information has been sent.");
                        if (location.href.indexOf("?customerid") > 0) {
                            customerid = "/?customerid=" + window.location.href.slice(window.location.href.indexOf('=') + 1);
                        }
                        window.location.href = $("#hdnSiteURL").val() + "/Scheduler" + customerid;
                    }
                    else
                        alert("Please try again. Something went wrong.");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("Please try again. Something went wrong.");
                },
                complete: function (jqXHR, textStatus) {

                }
            });
        }
    }
});

function Validate() {

    if ($("#firstname").val().trim().length <= 0 && $("#lastname").prop("readonly") == false) {
        alert("Please enter First Name.");
        $("#firstname").focus();
        return false;
    }

    if ($("#lastname").val().trim().length <= 0) {
        alert("Please enter Last Name.");
        $("#lastname").focus();
        return false;
    }

    if ($("#email").val().trim().length <= 0) {
        alert("Please enter Email.");
        $("#email").focus();
        return false;
    }

    if ($("#houseno").val().trim().length <= 0) {
        alert("Please enter House No.");
        $("#houseno").focus();
        return false;
    }

    if ($("#streetname").val().trim().length <= 0) {
        alert("Please enter Street Name.");
        $("#streetname").focus();
        return false;
    }

    if ($("#city").val().trim().length <= 0) {
        alert("Please enter City.");
        $("#city").focus();
        return false;
    }

    if ($("#state").val().trim().length <= 0) {
        alert("Please enter State.");
        $("#state").focus();
        return false;
    }

    if ($("#postcode").val().trim().length <= 0) {
        alert("Please enter Post Code.");
        $("#postcode").val("");
        $("#postcode").focus();
        return false;
    }

    if ($("#mobileno").val().trim().length <= 0 && $("#lastname").prop("readonly") == false) {
        alert("Please enter Mobile No.");
        $("#mobileno").focus();
        return false;
    }

    return true;
}