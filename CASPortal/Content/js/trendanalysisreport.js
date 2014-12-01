var _selectedNodes = [];
var _listArea = $("#ulArea > :first-child");

function Validate() {

    if ($("#j1_1_anchor").find(".jstree-checkbox").hasClass("jstree-undetermined") == false) {
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

    return true;
}

$(function () {

    var ulArea = $("#ulArea > :first-child").text();
    $(".dropdown-Area").find('[data-bind="label"]').text(ulArea);

    $("#dtpFrom").datepicker({
        dateFormat: "dd MM, yy",
        showOtherMonths: true,
        selectOtherMonths: true,
        altFormat: "yy-mm-dd",
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
});

$("#btnPreview").click(function () {

    if (Validate() == false)
        return;

    $("#divLoading").html("<img alt= title= src=" + $("#hdnSiteURL").val() + "/Content/Images/ajax-loading.gif width='10%' />");

    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/TrendAnalysis",
        type: "POST",
        data: { selectedNodes: JSON.stringify(_selectedNodes) },
        dataType: "JSON",
        success: function (data) {
            if (data != null) {
                $('#jstree').jstree("deselect_all");
                alert(data);
                $("#divLoading").html("");
            }
        },
        error: function (request) {
            alert("Please try again. Something went wrong.");
        }
    });

    $("#divLoading").html("");
});

$("#btnReset").click(function () {
    $('#jstree').jstree("deselect_all");
});