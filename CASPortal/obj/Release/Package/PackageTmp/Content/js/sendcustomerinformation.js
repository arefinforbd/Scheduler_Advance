$("#btnSubmit").click(function () {

    if (Validate()) {

        if (confirm("Are you sure to send your information?")) {

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
                        window.location.href = $("#hdnSiteURL").val() + "/Scheduler";
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

    if ($("#firstname").val().trim().length <= 0) {
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

    if ($("#mobileno").val().trim().length <= 0) {
        alert("Please enter Mobile No.");
        $("#mobileno").focus();
        return false;
    }

    return true;
}