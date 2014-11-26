var _selectedNodes = [];

$(function () {
    $('#jstree').jstree({
        "checkbox": {
            "keep_selected_style": false
        },
        "plugins": ["checkbox"]
    });

    //$('#jstree').jstree('open_all');

    $('#jstree').on('changed.jstree', function (e, data) {
        var i, j, r = [];
        for (i = 0, j = data.selected.length; i < j; i++) {
            _selectedNodes.push(data.node.id);
        }
        //alert('Selected: ' + r.join(', '));
        //alert(data.node.id + " " + data.node.text);
    })
      .jstree();
});

$("#btnPreview").click(function () {

    $.ajax({
        url: $("#hdnSiteURL").val() + "/Report/TrendAnalysis",
        type: "POST",
        data: { selectedNodes: JSON.stringify(_selectedNodes) },
        dataType: "JSON",
        success: function (data) {
            if (data != null && data == "successfull") {
                _selectedNodes = [];
                //ClearTrendAnalysisTree();
                alert("OK");
            }
        },
        error: function (request) {
            alert("Please try again. Something went wrong.");
        }
    });
});


function ClearTrendAnalysisTree() {
    var CurrentSelectedNodes = jQuery("#jstree").jstree("get_selected");
    for (i = 0; i < CurrentSelectedNodes.length; i++) {
        var CurrentSelectedNodeID = CurrentSelectedNodes[i];
        if (CurrentSelectedNodeID) {
            jQuery("#jstree").jstree("deselect_node", $("#" + CurrentSelectedNodeID));
        }
    }
}