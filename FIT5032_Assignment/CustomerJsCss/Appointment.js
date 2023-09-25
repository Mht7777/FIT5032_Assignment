$(document).ready(function () {
    //$.getJSON("/YourControllerName/GetAddresses", function (data) {
    //    var addressList = data;
    //});
    var addresses = $('#clinic-address-list .address');



    mapboxgl.accessToken = "pk.eyJ1IjoidG1oOTk5IiwiYSI6ImNsbXFldjU0NjAycGkydW5oODNoN3Q2cTcifQ.wMGs3hNb_VoMdMVFNN6ECw"; // Replace with your Mapbox token

    // Initialize the map
    var map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v10',
        center: [144.9631, -37.8136], // Melbourne default coordinates
        zoom: 10
    });

    // Loop through each address
    addresses.each(function () {
        // Get the address text
        var addressText = $(this).text();

        // Display the address on the map
        geocodeAndDisplayMarker(addressText);
    });

    function geocodeAndDisplayMarker(address) {
        var geocodingApi = `https://api.mapbox.com/geocoding/v5/mapbox.places/${encodeURIComponent(address)}.json?access_token=${mapboxgl.accessToken}`;

        $.get(geocodingApi, function (data) {
            if (data.features && data.features.length > 0) {
                var coordinates = data.features[0].center;

                // Set marker at the coordinates of the address
                var marker = new mapboxgl.Marker()
                    .setLngLat(coordinates)
                    .addTo(map);

                marker.getElement().addEventListener('click', function () {
                    new mapboxgl.Popup()
                        .setLngLat(coordinates)
                        .setHTML(address)
                        .addTo(map);

                    // Zoom in when the marker is clicked
                    map.flyTo({
                        center: coordinates,
                        zoom: 15  // Adjust this value as needed
                    });
                });
            } else {
                console.log(`Address ${address} not found!`);
            }
        });
    }




    let currentStep = 1;
    // Show only the first step initially
    $('.booking-step').hide();
    $('#step-1').show();

    $('.next-btn').click(function () {
        let nextStep = $(this).data('next');
        $('#step-' + currentStep).hide();
        $('#step-' + nextStep).show();
        currentStep = nextStep;
        updateProgressBar(currentStep);
    });

    $('.prev-btn').click(function () {
        let prevStep = $(this).data('prev');
        $('#step-' + currentStep).hide();
        $('#step-' + prevStep).show();
        currentStep = prevStep;
        updateProgressBar(currentStep);
    });


    function updateProgressBar(stepNumber) {
        let percentage = (stepNumber / 4) * 100;
        $('.progress-bar')
            .css('width', percentage + '%')
            .attr('aria-valuenow', percentage)
            .attr('aria-label', 'Booking progress: Step ' + stepNumber + ' of 4')  // Update this line for the aria-label
            .text('Step ' + stepNumber);
    }

    $('.next-btn').click(function (event) {
        event.preventDefault(); // prevent form submission

        // rest of your code...
    });

    $('.prev-btn').click(function (event) {
        event.preventDefault(); // prevent form submission

        // rest of your code...
    });



});




