$(document).ready(function () {
    const $stars = $("#rating-star .star");

    $stars.click(function () {
        let rating = $(this).data("rating");
        $("#ratingValue").val(rating);
        updateStars(rating);
    });

    function updateStars(rating) {
        $stars.each(function () {
            if ($(this).data("rating") <= rating) {
                $(this).text('\u2605'); // Filled star
            } else {
                $(this).text('\u2606'); // Empty star
            }
        });
    }
});