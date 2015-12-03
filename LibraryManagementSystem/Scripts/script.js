$(document).ready(function () {
    //Global container for alerts and flash messages
    var pageAlerts = $("#page-alerts");

    //The container of the "Collect fees" message in the reservation model
    var reservationNotice = $("#empty-reservation-notice");

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
        return "<div class=\"alert alert-" + type + "\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\">" +
                "<span aria-hidden=\"true\">&times;</span>" +
            "</button>"
            + message +
            "</div>";
    }


    /**
     * Generates the HTML for one or more Twitter Bootstrap error alerts.
     *
     * @param error     - (string || array) A single string or array of errors to display.
     * @returns {string} - HTML for one or more twitter bootstrap alerts.
     */
    function makeAlertErrors(error) {
        if ($.isArray(error)) {
            var errorMessages = "<ul>";

            $.each(error, function (k, v) {
                errorMessages += "<li>" + v + "</li>";
            });
            errorMessages += "</ul>";

            return makeAlert(errorMessages, "danger");
        }
        return makeAlert(error, "danger");
    }


    /**
     * Removes all alerts on the page.
     */
    function clearAlerts() {
        $(".alert").remove();
    }



    /** ****************************************
     *  Check Out/Reserve Item Modal
     ** ****************************************/
    var reservationModal        = $("#reservation-modal");
    var reservationItemId       = reservationModal.find("input[name=\"item_id\"]");
    var reservationIsResvered   = reservationModal.find("input[name=\"is_reserved\"]");
    var reservationSubmit       = reservationModal.find("button[data-action=\"reserve_item\"]");
    var reservationAlerts       = reservationModal.find(".alerts-container");
    var reservationItems        = $("#reserved-items");

    /**
     * Click handler for checkout/reservation confirm button.
     * Sends request to server to save a checkout.
     */
    reservationSubmit.click(function () {
        //Remove alerts from page
        clearAlerts();

        //If an item ID hasn't been provided, prompt the user
        if(reservationItemId.val() === "") {
            reservationAlerts.html(makeAlertErrors("Please enter an item ID."));
            return false;
        }

        //If the inputted item ID isn't numeric, prompt the user to correct the input
        if (!$.isNumeric(reservationItemId.val())) {
            reservationAlerts.html(makeAlertErrors("Invalid ID: Item ID should be numeric."));
            return false;
        }

        //Prepare payload for ajax request
        var payload = {
            "CustomerId": $(this).attr("data-customer-id"),
            "IsReserved": (reservationIsResvered.prop("checked")) ? true : false,
            "LibraryItemId": reservationItemId.val()
        };

        //Send request to server to create the checkout
        $.ajax({
            url: "/library/reservations/CheckOut",
            type: "post",
            dataType: "json",
            data: payload,
            before: function() {
                
            },
            success: function(request) {
                if (request.status) {
                    pageAlerts.html(makeAlert(request.response.message, "success"));
                    reservationNotice.hide();
                    var html = "" +
                        "<div class=\"item\" data-content=\"item\" data-reservation-id=\"" + request.response.reservation.Id + "\">" +
                            "<div class=\"row\">" +
                                    "<div class=\"col-xs-6\">" +
                                        "<p>" + request.response.item.Title + "</p>" +
                                    "</div>" +
                                "<div class=\"col-xs-6 col-sm-3\">" +
                                    "<p><b>Return By " + request.response.reservation.DueDate + "</b></p>" +
                                "</div>" +
                                "<div class=\"col-xs-12 col-sm-3 text-right\">" +
                                    "<button class=\"action-btn btn btn-danger\" data-trigger=\"check-in\" " +
                                        "data-reservation-id=\"" + request.response.reservation.Id +"\">Check In</button>" +
                                "</div>" +
                            "</div>" +
                        "</div>";
                    reservationItems.append(html);
                    reservationModal.modal("hide");
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


    /** ****************************************
     *  Check-In Item Modal
     ** ****************************************/
    var checkInModal = $("#check-in-model");
    var checkInItemName = checkInModal.find("[data-content=\"check-in-item-name\"]");
    var checkInAction = checkInModal.find("[data-action=\"check_in_item\"]");
    var checkInAlerts = checkInModal.find(".alerts-container");
    var checkInLoader = checkInModal.find(".loader-container");
    var checkInItemType = checkInModal.find("[data-content=\"item-type\"]");
    var checkInFeeNotice = checkInModal.find("[data-content=\"fee-notice\"]");
    var checkInDueDate = checkInModal.find("[data-content=\"due-date\"]");
    var checkInLateFee = checkInModal.find("[data-content=\"late-fee\"]");


    /**
     * Click handler for "Check In" buttons on customer details page.
     * Loads reservation data into modal.
     */
    reservationItems.on("click", "[data-trigger=\"check-in\"]", function () {
        //Get the reservation ID from the button's attributes
        var reservationId = $(this).attr("data-reservation-id");

        //Store the parent container DOM object
        var lateFeeParent = checkInLateFee.parent();

        //Clear alerts from the page
        clearAlerts();

        //Reset the color of the fee text in the modal
        lateFeeParent.attr("class", "");

        //Show the modal
        checkInModal.modal("show");

        //Send the request to the server
        $.ajax({
            url: "/reservations/Details",
            type: "post",
            dataType: "json",
            data: {
                "reservation_id": reservationId
            },
            before: function () {
                //Show loading icon while the request is processing
                checkInLoader.show();
            },
            success: function(request) {
                if (request.status) {
                    //Fill in the modal data
                    checkInItemName.text(request.response.item.title);
                    checkInItemType.text(request.response.item.type);
                    checkInDueDate.text(request.response.dueDate);
                    checkInAction.attr("data-reservation-id", request.response.id);

                    if (parseFloat(request.response.lateFee) === 0) {
                        checkInLateFee.text(0);
                        checkInLateFee.parent().addClass("text-success");
                        checkInFeeNotice.hide();
                    } else {
                        checkInLateFee.text(request.response.lateFee);
                        checkInLateFee.parent().addClass("text-danger");
                        checkInFeeNotice.show();
                    }
                } else {
                    checkInAlerts.html(makeAlertErrors(request.response));
                }
            },
            error: function () {
                reservationAlerts.html(makeAlertErrors("Oops! Something when wrong. Please refresh and try again."));
            },
            complete: function () {
                checkInLoader.fadeOut(300);
            }
        });

    });


    /**
     * Check-In confirmation button click handler.
     * Removes a reservation/checkout from the system.
     */
    checkInAction.click(function () {
        //Remove all alerts on the page
        clearAlerts();
        
        //Get the reservation's ID from the button's attributes.
        var reservationId = $(this).attr("data-reservation-id");

        //Send request to the server
        $.ajax({
            url: "/reservations/CheckIn",
            type: "post",
            dataType: "json",
            data: { "id": reservationId },
            before: function() {
                checkInLoader.fadeIn(100);
            },
            success: function (request) {
                if (request.status) {
                    pageAlerts.html(makeAlert(request.response, "success"));
                    var numItems = reservationItems.find(".item").length;
                    if (numItems === 1) {
                        reservationNotice.show();
                    }

                    var itemContainer = $("[data-content=\"item\"][data-reservation-id=\"" + reservationId + "\"]");
                    itemContainer.fadeOut(300);
                    setTimeout(function() {
                        itemContainer.remove();
                    }, 300);

                    checkInModal.modal("hide");
                } else {
                   checkInAlerts.html(makeAlertErrors(request.response));
                }
            },
            error: function () {
                reservationAlerts.html(makeAlertErrors("Oops! Something when wrong. Please refresh and try again."));
            },
            complete: function () {
                checkInLoader.fadeOut(300);
            }
        });
    });


    /** ****************************************
     *  Delete Customer Modal
     ** ****************************************/
    var customersTable = $("#customers-list");
    var customersEmptyNotice = $("#customers-empty");
    var customerDeleteTrigger = $("[data-trigger=\"delete-customer\"]");
    var customerDeleteConfirm = $("[data-action=\"delete-customer\"]");
    var customerDeleteModal = $("#customer-delete-modal");
    var customerModalLoader = customerDeleteModal.find(".loader-container");
    var customerNameField = customerDeleteModal.find("[data-content=\"customer-name\"]");


    /**
     * Click handler for the "Delete" button on the customer listing page.
     * Displays a confirmation modal so the user may confirm that they want to delete the customer.
     */
    customerDeleteTrigger.click(function () {
        //Store the customer's ID
        var customerId = $(this).attr("data-customer-id");

        //Store the customer's name
        var customerName = $(this).attr("data-customer-name");

        //Hide alerts on the page
        clearAlerts();

        //Show the modal
        customerDeleteModal.modal("show");

        //Set the value of the modal's customer-id input to the customer's ID
        customerDeleteConfirm.attr("data-customer-id", customerId);

        //Place the customer's name in the modal
        customerNameField.text(customerName);

        //Hide the loading icon
        customerModalLoader.fadeOut(1000);
        return false;
    });


    /**
     * Click handler for "Confirm Delete" button in the delete customer modal.
     * Sends a request to the server to delete the give customer.
     */
    customerDeleteConfirm.click(function () {
        //Store the customer's ID
        var customerId = $(this).attr("data-customer-id");
        
        //Store the DOM element of the row that contains the custoemr's information in the table.
        //This will be used to remove the customer's record from the GUI if the deletion is successful.
        var recordRow = $("[data-content=\"customer\"][data-customer-id=\"" + customerId + "\"]");

        //Send the request to the server
        $.ajax({
            url: "/customers/AjaxDelete",
            type: "post",
            dataType: "json",
            data: { "id": customerId },
            before: function() {
                customerModalLoader.show();
            },
            success: function(request) {
                if (request.status) {
                    pageAlerts.html(makeAlert(request.response, "success"));
                    recordRow.remove();

                    if (customersTable.find("tbody tr").length === 0) {
                        customersTable.hide();
                        customersEmptyNotice.show();
                    }
                } else {
                    pageAlerts.html(makeAlertErrors(request.response));
                }
            },
            error: function() {
                reservationAlerts.html(makeAlertErrors("Oops! Something when wrong. Please refresh and try again."));
            },
            complete: function() {
                customerModalLoader.fadeOut(300);
                customerDeleteModal.modal("hide");
            }
        });
    });
});