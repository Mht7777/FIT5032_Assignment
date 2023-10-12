$(document).ready(function () {

    //map start



    //map end




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
    });

    $('.prev-btn').click(function (event) {
        event.preventDefault(); // prevent form submission
    });



});




