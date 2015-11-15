$(document).ready(function () {
    var pageAlerts = $("#page-alerts");
    var reservationNotice = $("#empty-reservation-notice");

    /** ****************************************
     *  Check Out/Reserve Item Modal
     ** ****************************************/
    var reservationModal        = $("#reservation-modal");
    var reservationItemId       = reservationModal.find("input[name=\"item_id\"]");
    var reservationIsResvered   = reservationModal.find("input[name=\"is_reserved\"]");
    var reservationSubmit       = reservationModal.find("button[data-action=\"reserve_item\"]");
    var reservationAlerts       = reservationModal.find(".alerts-container");
    var reservationItems        = $("#reserved-items");

    reservationSubmit.click(function () {
        clearAlerts();
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
    var checkInItemName = checkInModal.find("[data-content=\"check-in-item-name\"]")
    var checkInAction = checkInModal.find("[data-action=\"check_in_item\"]")
    var checkInAlerts = checkInModal.find(".alerts-container");
    var checkInLoader = checkInModal.find(".loader-container");
    var checkInItemType = checkInModal.find("[data-content=\"item-type\"]");
    var checkInFeeNotice = checkInModal.find("[data-content=\"fee-notice\"]");
    var checkInDueDate = checkInModal.find("[data-content=\"due-date\"]");
    var checkInLateFee = checkInModal.find("[data-content=\"late-fee\"]");

    reservationItems.on("click", "[data-trigger=\"check-in\"]", function () {
        var reservationId = $(this).attr("data-reservation-id");
        var lateFeeParent = checkInLateFee.parent();
        clearAlerts();
        lateFeeParent.attr("class", "");

        checkInModal.modal("show");

        $.ajax({
            url: "/reservations/Details",
            type: "post",
            dataType: "json",
            data: {
                "reservation_id": reservationId
            },
            before: function() {
                checkInLoader.show();
            },
            success: function(request) {
                if (request.status) {
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


    checkInAction.click(function () {
        clearAlerts();
        var reservationId = $(this).attr("data-reservation-id");

        $.ajax({
            url: "/reservations/AjaxDelete",
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

    customerDeleteTrigger.click(function() {
        var customerId = $(this).attr("data-customer-id");
        var customerName = $(this).attr("data-customer-name");

        clearAlerts();
        customerDeleteModal.modal("show");
        customerDeleteConfirm.attr("data-customer-id", customerId);
        customerNameField.text(customerName);
        customerModalLoader.fadeOut(1000);
        return false;
    });

    customerDeleteConfirm.click(function() {
        var customerId = $(this).attr("data-customer-id");
        var recordRow = $("[data-content=\"customer\"][data-customer-id=\"" + customerId + "\"]");

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

            $.each(error, function(k, v) {
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
});