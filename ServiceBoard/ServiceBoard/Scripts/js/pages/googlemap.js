function MapMaker(map, address, infowindow) {

    $.getJSON('http://maps.googleapis.com/maps/api/geocode/json?address=' + address + '&sensor=true', null, function (data) {

        var p = data.results[0].geometry.location
        var latlng = new google.maps.LatLng(p.lat, p.lng);
        var tooltipText = "Address: <span style='font-weight: bold;'>" + address + "</span>";

        infowindow.close();

        var marker = new google.maps.Marker({
            position: latlng,
            map: map,
            title: 'Click to get info'/*,
                icon: 'http://www.edmonton.ca/icon_environment_32x32.png'*/
        });
        markers.push(marker);

        var labelText = "<div style='background-color: yellow;'>" + numbersOfJobs[index] + "</div>";

        var labelOption = {
            content: labelText
                    , boxStyle: {
                        border: "1px solid black"
                        , textAlign: "center"
                        , fontSize: "8pt"
                        , width: "20px"
                    }
            , disableAutoPan: true
            , pixelOffset: new google.maps.Size(-10, 0)
            , position: latlng
            , closeBoxURL: ""
            , isHidden: false
            , pane: "mapPane"
            , enableEventPropagation: true
        };

        var ibLabel = new InfoBox(labelOption);
        ibLabel.open(map, marker);
        ibLabels.push(ibLabel);

        var getMapClickListener = function (m) {
            return function () {
                infowindow.setContent(tooltipText);
                infowindow.open(m, marker);
            };
        };

        google.maps.event.addListener(marker, 'click', getMapClickListener(map));
        index++;
    });
}

function GetGoogleMapLocation(googlemap, zoomLevel, showTooltip, showLabel) {

    myOptions = {
        zoom: zoomLevel,
        center: new google.maps.LatLng(mapLatitude, mapLongitude),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(googlemap[0], myOptions);
    var infowindow = new google.maps.InfoWindow();

    for (var x = 0; x < address.length; x++) {
        setTimeout(MapMaker(map, address[x], infowindow), 5000);
    }
    index = 0;
}
