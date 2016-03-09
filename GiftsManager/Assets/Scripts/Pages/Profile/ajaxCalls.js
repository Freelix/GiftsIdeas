$(document).ready(function () {

    var lang = window.tradKeys;

    // Groups

    function isAdmin(groupName) {
        var url = "IsAdmin/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: groupName },
            success: function (data) {
                if (data === "True") {
                    $(".deleteGroupEvent.deleteGroup").css('visibility', 'visible');
                    $(".groupDiv").css('visibility', 'visible');
                } 
                else {
                    $(".deleteGroupEvent.deleteGroup").css('visibility', 'collapse');
                    $(".groupDiv").css('visibility', 'collapse');
                }
                       
            }
        });
    }

    $(document).on("click", ".groups.dropdown-menu li a", function () {
        var groupText = $(this).text();
        $(this).parents('.dropdown').find('.dropdown-toggle').html(groupText);
        var url = "ChangeGroup/";

        isAdmin(groupText);

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: groupText },
            success: function (data) {
                $("#GroupUsersDiv").html(data);

                setMargin();
                loadEvents(groupText);
            }
        });
        
        // Lose focus
        document.body.click();
        return false;
    });

    function setMargin() {
        var height = $('#usersList').height();
        var heightToSet = 112 + height;
        $("#DropDownEventDiv").css('margin-top', heightToSet);
    }

    $(document).on("click", ".deleteGroup i", function () {
        var groupText = $("#dropdownGroupId").text();
        openPopupDeleteGroup(groupText);
    });

    function openPopupDeleteGroup(groupName) {
        swal({
            title: lang.removeGroup + " " + groupName,
            text: lang.removeGroupConfirmation,
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.no,
            confirmButtonText: lang.yes
        }, function (confirmation) {
            if (confirmation) {
                swal.disableButtons();
                deleteGroup(groupName);

                setTimeout(function () {
                    swal({
                        title: lang.complete,
                        text: groupName + ' ' + lang.hasBeenRemoved,
                        type: 'success'
                    },
                    function() {
                        window.location.reload();
                    });
                }, 2000);
            }
        });
    }

    $(document).on("click", ".addGroupDiv i", function () {
        createGroupPopup();
    });

    function createGroupPopup() {
        swal({
            title: lang.groupName,
            html: '<p><input id="input-field-name"></p>' +
                '<p class="error"></p>',
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.no,
            confirmButtonText: lang.yes
        }, function (confirmation) {
            if (confirmation) {
                var groupName = $("#input-field-name").val();
                isGroupExist(groupName);
            }
        });
    }

    function isGroupExist(groupName) {
        var url = "IsGroupExist/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: groupName },
            success: function (value) {
                if (value === "False") {
                    swal.disableButtons();
                    createGroup(groupName);

                    setTimeout(function () {
                        swal({
                            title: lang.complete,
                            text: groupName + ' ' + lang.hasBeenAdded,
                            type: 'success'
                        },
                            function () {
                                window.location.reload();
                            });
                    }, 2000);
                } else {
                    $(".error").text(lang.groupAlreadyExist);
                }
            }
        });

        return false;
    }

    function createGroup(groupName) {
        var url = "CreateGroup/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: groupName }
        });

        return false;
    }

    function deleteGroup(groupName) {
        var url = "DeleteGroup/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: groupName }
        });
    }

    // Events

    function createEventPopup() {
        swal({
            title: lang.eventName,
            html: '<p><input id="input-field-name"></p>' +
                '<p class="error"></p>',
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.No,
            confirmButtonText: lang.Yes
        }, function (confirmation) {
            if (confirmation) {
                var eventName = $("#input-field-name").val();
                var groupName = $("#dropdownGroupId").text();

                isEventExist(eventName, groupName);
            }
        });
    }

    function createEvent(eventName) {
        var url = "CreateEvent/";
        var groupName = $("#dropdownGroupId").text();

        $.ajax({
            url: url,
            type: 'POST',
            data: { eventName: eventName, groupName: groupName }
        });

        return false;
    }

    function isEventExist(eventName, groupName) {
        var url = "IsEventExist/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { eventName: eventName, groupName: groupName },
            success: function (value) {
                if (value === "False") {
                    swal.disableButtons();
                    createEvent(eventName);

                    setTimeout(function () {
                        swal({
                            title: lang.complete,
                            text: eventName + lang.hasBeenAdded,
                            type: 'success'
                        },
                            function () {
                                window.location.reload();
                            });
                    }, 2000);
                } else {
                    $(".error").text(lang.eventAlreadyExist);
                }
            }
        });

        return false;
    }

    $(document).on("click", ".addEventDiv i, #addEventLink", function () {
        createEventPopup();
    });

    $(document).on("click", ".events.dropdown-menu li a", function () {
        var text = $(this).text();
        $(this).parents('.dropdown').find('.dropdown-toggle').html(text);

        $(".deleteGroupEvent.deleteEvent").css('visibility', 'visible');
        loadGiftsSection(text);

        document.body.click();
        return false;
    });

    function loadEvents(groupText) {
        var url = "LoadEvents/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: groupText },
            success: function (data) {
                $("#DropDownEventDiv").html(data);
            }
        });

        return false;
    }

    $(document).on("click", ".deleteEvent i", function () {
        var eventText = $("#dropdownEventId").text();
        var groupText = $("#dropdownGroupId").text();
        openPopupDeleteEvent(eventText, groupText);
    });

    function openPopupDeleteEvent(eventName, groupName) {
        swal({
            title: lang.removeEvent + " " + eventName,
            text: lang.removeEventConfirmation,
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.no,
            confirmButtonText: lang.yes
        }, function (confirmation) {
            if (confirmation) {
                swal.disableButtons();
                deleteEvent(eventName, groupName);

                setTimeout(function () {
                    swal({
                        title: lang.complete,
                        text: eventName + ' ' + lang.hasBeenRemoved,
                        type: 'success'
                    },
                    function () {
                        window.location.reload();
                    });
                }, 2000);
            }
        });
    }

    function deleteEvent(eventName, groupName) {
        var url = "DeleteEvent/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { eventName: eventName, groupName: groupName }
        });
    }

    // Remove User

    $(document).on("click", "#usersList ol li i", function () {
        var groupName = $("#dropdownGroupId").text();
        var userEmail = $(this).parent().find("strong").text();

        if ($("#userEmail").val() !== userEmail)
            openPopupRemoveUser(groupName, userEmail);

        return false;
    });

    function removeUserFromGroup(groupName, userEmail) {
        var url = "RemoveUserFromGroup/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: groupName, email: userEmail },
            success: function (data) {
                $("#GroupUsersDiv").html(data);
            }
        });
    }

    function openPopupRemoveUser(groupName, userEmail) {
        swal({
            title: lang.removeUser + " " + userEmail,
            text: lang.removeUserConfirmation,
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.no,
            confirmButtonText: lang.yes
        }, function (confirmation) {
            if (confirmation) {
                swal.disableButtons();
                removeUserFromGroup(groupName, userEmail);

                setTimeout(function() {
                    swal({
                        title: lang.complete,
                        text: userEmail + '' + lang.hasBeenRemoved,
                        type: 'success'
                    });
                }, 2000);
            }
        });
    }

    // Remove gift from wishlist

    $(document).on("click", ".wishlistTable tr td i", function () {
        var giftName = $(this).parent().parent().find("strong").text();
        var eventName = $("#dropdownEventId").text();
        openPopupRemoveFromWishlist(giftName, eventName);

        return false;
    });

    function removeFromWishList(giftName, eventName) {
        var url = "RemoveGiftFromWishlist/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { giftName: giftName, eventName: eventName },
            success: function (data) {
                loadGiftsSection(eventName);
            }
        });
    }

    function openPopupRemoveFromWishlist(giftName, eventName) {
        swal({
            title: lang.RemoveGift + " " + giftName,
            text: lang.removeGiftConfirmation,
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.no,
            confirmButtonText: lang.yes
        }, function (confirmation) {
            if (confirmation) {
                swal.disableButtons();
                removeFromWishList(giftName, eventName);

                setTimeout(function () {
                    swal({
                        title: lang.complete,
                        text: giftName + ' ' + lang.hasBeenRemoved,
                        type: 'success'
                    });
                }, 2000);
            }
        });
    }

    // Remove gift from reservation

    $(document).on("click", ".reservedGitfsTable tr td i.trash", function () {
        var giftName = $(this).parent().parent().find("strong").first().text();
        var eventName = $("#dropdownEventId").text();
        var groupName = $("#dropdownGroupId").text();
        openPopupRemoveFromReservation(giftName, eventName, groupName);

        return false;
    });

    function removeFromReservation(giftName, eventName, groupName) {
        var url = "RemoveGiftFromReservation/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { giftName: giftName, eventName: eventName, groupName: groupName },
            success: function (data) {
                loadGiftsSection(eventName);
            }
        });
    }

    function openPopupRemoveFromReservation(giftName, eventName, groupName) {
        swal({
            title: lang.removeGift + " " + giftName,
            text: lang.removeGiftReservation,
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.no,
            confirmButtonText: lang.yes
        }, function (confirmation) {
            if (confirmation) {
                swal.disableButtons();
                removeFromReservation(giftName, eventName, groupName);

                setTimeout(function () {
                    swal({
                        title: lang.complete,
                        text: giftName + ' ' + lang.hasBeenRemoved,
                        type: 'success'
                    });
                }, 2000);
            }
        });
    }

    // Buy gift from reservation

    $(document).on("click", ".reservedGitfsTable tr td i.cart", function () {
        var giftName = $(this).parent().parent().find("strong").first().text();
        var eventName = $("#dropdownEventId").text();
        var groupName = $("#dropdownGroupId").text();
        var userId = $(this).parent().find(".userId").first().val();
        var giftId = $(this).parent().find(".giftId").first().val();
        openPopupBuyGiftFromReservation(giftName, eventName, groupName, userId, giftId);

        return false;
    });

    function buyFromReservation(giftName, eventName, groupName, userId, giftId, price) {
        var url = "BuyGiftFromReservation/";

        $.ajax({
            url: url,
            type: 'POST',
            data: {
                giftName: giftName,
                eventName: eventName,
                groupName: groupName,
                userId: userId,
                giftId: giftId,
                price: price
            },
            success: function (data) {
                loadGiftsSection(eventName);
            }
        });
    }

    function openPopupBuyGiftFromReservation(giftName, eventName, groupName, userId, giftId) {
        swal({
            title: lang.enterCost,
            html: '<p><input id="input-field-price"></p>' +
                '<p class="error"></p>',
            showCancelButton: true,
            closeOnConfirm: false
        }, function () {
            var price = $("#input-field-price").val();
            price = formatNumber(price);
            $(".error").text("");

            if (isNumeric(price)) {
                swal.disableButtons();
                buyFromReservation(giftName, eventName, groupName, userId, giftId, price);

                setTimeout(function() {
                    swal({
                        title: lang.complete,
                        text: giftName + ' ' + lang.hasBeenBought,
                        type: 'success'
                    });
                }, 2000);
            } else {
                $(".error").text(lang.priceError);
            }
        });
    }

    // Add Gift

    function createGiftPopup() {
        swal({
            title: lang.giftName,
            html: '<p><input id="input-field-name"></p>' +
                '<p class="error"></p>',
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: true,
            cancelButtonText: lang.no,
            confirmButtonText: lang.yes
        }, function (confirmation) {
            if (confirmation) {
                var giftName = $("#input-field-name").val();
                var eventName = $("#dropdownEventId").text();
                var groupName = $("#dropdownGroupId").text();

                isGiftExist(giftName, eventName, groupName);
            }
        });
    }

    function createGift(giftName) {
        var eventName = $("#dropdownEventId").text();
        var url = "AddGiftInWishlist/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { newGift: giftName, eventName: eventName },
            success: function () {
                loadGiftsSection(eventName);
            }
        });

        return false;
    }

    function isGiftExist(name, eventName, groupName) {
        var url = "IsGiftExist/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { name: name, eventName: eventName, groupName: groupName },
            success: function(value) {
                if (value === "False") {
                    swal.disableButtons();
                    createGift(name);

                    setTimeout(function () {
                        swal({
                            title: lang.complete,
                            text: name + ' ' + lang.hasBeenCreated,
                            type: 'success'
                        });
                    }, 2000);
                } else {
                    $(".error").text(lang.giftAlreadyExist);
                }
            }
        });

        return false;
    }

    $(document).on("click", "#addGiftLink", function () {
        createGiftPopup();
        return false;
    });

    // Load Gift section

    function loadGiftsSection(eventName) {
        var url = "LoadGiftsSection/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { eventName: eventName},
            success: function(data) {
                $("#GiftsDiv").html(data);
            }
        });

        return false;
    }

    // Add user

    $(document).on("click", "#addUserLink", function () {
        addUserPopup();
        return false;
    });

    function addUserPopup() {
        swal({
            title: lang.enterEmail,
            html: '<p><input id="input-field-name"></p>' +
                '<p class="error"></p>',
            showCancelButton: true,
            closeOnConfirm: false
        }, function () {
            var email = $("#input-field-name").val();
            if (validateEmail(email)) {
                isUserExist(email);
            } else {
                $(".error").text(lang.invalidEmail);
            }
        });
    }

    function createUser(email) {
        var url = "AddUserToGroup/";
        var group = $('.dropdown:first').find('.dropdown-toggle').html();

        $.ajax({
            url: url,
            type: 'POST',
            data: { groupName: group, email: email },
            success: function (data) {
                $("#GroupUsersDiv").html(data);
            }
        });

        return false;
    }

    function isUserExist(userEmail) {
        var url = "IsUserExist/";

        $.ajax({
            url: url,
            type: 'POST',
            data: { userEmail: userEmail },
            success: function(value) {
                if (value) {
                    createUser(userEmail);
                    $(".cancel").click();
                } else {
                    $(".error").text(lang.userNotExist);
                }
            }
        });

        return false;
    }

    function formatNumber(input) {
        return input.replace(",", ".").replace("$", "").trim();
    }

    function isNumeric(input) {
        if (!input)
            return false;
        return (input - 0) == input && ("" + input).trim().length > 0;
    }

    function validateEmail(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }
});