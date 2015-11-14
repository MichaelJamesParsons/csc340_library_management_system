$(document).ready(function () {

    /** ****************************************
     *  Add Reservation Page
     ** ****************************************/
    var reservationModal        = $('#reservation-modal');
    var reservationItemId       = reservationModal.find('input[name="item_id"]');
    var reservationIsResvered   = reservationModal.find('input[name="is_reserved"]');
    var reservationSubmit       = reservationModal.find('button[data-action="reserve_item"]');
    var reservationAlerts       = reservationModal.find('.alerts-container');

    reservationSubmit.click(function () {
        if(reservationItemId.val() == "") {
            reservationAlerts.html(makeAlertErrors("Please enter an item ID."));
            return false;
        }

        if (!$.isNumeric(reservationItemId.val())) {
            reservationAlerts.html(makeAlertErrors("Invalid ID: Item ID should be numeric."));
            return false;
        }

        var payload = {
            "CustomerId": $(this).attr('data-customer-id'),
            "IsReserved": (reservationIsResvered.prop("checked")) ? true : false,
            "LibraryItemId": reservationItemId.val()
        };

        $.ajax({
            url: '/library/reservations/AjaxCreate',
            type: 'post',
            dataType: 'json',
            data: payload,
            before: function() {
                
            },
            success: function(request) {
                if (request.status) {
                    reservationAlerts.html(makeAlert("Success!", "success"));
                } else {
                    reservationAlerts.html(makeAlertErrors(request.response));
                }
            },
            error: function() {
                reservationAlerts.html(makeAlertErrors("Oops! Something when wrong. Please refresh and try again."));
            }
        });

        return false;
    });


    /**
     * Generates the HTML for a Twitter Bootstrap alert.
     *
     * @param message - The message to place inside of the alert.
     * @param type - The type of bootstrap alert.
     *                  Options:
     *                      * Success
     *                      * Info
     *                      * Warning
     *                      * Danger
     * @returns {string} - HTML for a bootstrap alert
     */
    function makeAlert(message, type) {
        return '<div class="alert alert-' + type + '">' + message + '</div>';
    }


    /**
     * Generates the HTML for one or more Twitter Bootstrap error alerts.
     *
     * @param error     - (string || array) A single string or array of errors to display.
     * @returns {string} - HTML for one or more twitter bootstrap alerts.
     */
    function makeAlertErrors(error) {
        if ($.isArray(error)) {
            var errorMessages = '<ul>';

            $.each(error, function(k, v) {
                errorMessages += '<li>' + v + '</li>';
            });
            errorMessages += '</ul>';

            return makeAlert(errorMessages, 'danger');
        }
        return makeAlert(error, 'danger');
    }
});