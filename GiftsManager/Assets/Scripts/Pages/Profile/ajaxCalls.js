$(document).ready(function () {

    var lang = window.tradKeys;

    // Groups

    function isAdmin(groupName) {
        $.ajax({
            url: Constants.Url.IsAdmin,
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

        isAdmin(groupText);

        $.ajax({
            url: Constants.Url.ChangeGroup,
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
        var groupText = Utils.GetGroup();
        Popup.showInfoPopup(lang.removeGroup + " " + groupText,
            lang.removeGroupConfirmation, lang.no, lang.yes, true,
            openPopupDeleteGroupCallBack.bind(this, groupText));
    });

    function openPopupDeleteGroupCallBack(groupName, confirmation) {
        if (confirmation) {
            swal.disableButtons();
            deleteGroup(groupName);

            Popup.showSuccess(lang.complete, groupName + ' ' + lang.hasBeenRemoved,
                Utils.ReloadPage.bind(this));
        }
    }

    $(document).on("click", ".addGroupDiv i", function () {
        Popup.showAddGroupEventPopup(lang.groupName,
            createGroupPopupCallBack.bind(this));
    });

    function createGroupPopupCallBack(confirmation) {
        if (confirmation) {
            var groupName = $("#input-field-name").val();
            isGroupExist(groupName);
        }
    }

    function isGroupExist(groupName) {
        $.ajax({
            url: Constants.Url.IsGroupExist,
            type: 'POST',
            data: { groupName: groupName },
            success: function (value) {
                if (value === "False" && !Utils.IsEmpty(groupName)) {
                    swal.disableButtons();
                    createGroup(groupName);

                    Popup.showSuccess(lang.complete, groupName + ' ' + lang.hasBeenAdded,
                        Utils.ReloadPage.bind(this));
                } else {
                    $(".error").text(lang.groupAlreadyExist);
                }
            }
        });

        return false;
    }

    function createGroup(groupName) {
        var data = { groupName: groupName };
        Utils.SimplePostRequest(Constants.Url.CreateGroup, data);
    }

    function deleteGroup(groupName) {
        var data = { groupName: groupName };
        Utils.SimplePostRequest(Constants.Url.DeleteGroup, data);
    }

    // Events

    function createEvent(eventName) {
        var groupName = Utils.GetGroup();
        var data = { eventName: eventName, groupName: groupName };
        Utils.SimplePostRequest(Constants.Url.CreateEvent, data);
    }

    function isEventExist(eventName, groupName) {
        $.ajax({
            url: Constants.Url.IsEventExist,
            type: 'POST',
            data: { eventName: eventName, groupName: groupName },
            success: function (value) {
                if (value === "False" && !Utils.IsEmpty(eventName)) {
                    swal.disableButtons();
                    createEvent(eventName);

                    Popup.showSuccess(lang.complete, eventName + ' ' + lang.hasBeenAdded,
                        Utils.ReloadPage.bind(this));
                } else {
                    $(".error").text(lang.eventAlreadyExist);
                }
            }
        });

        return false;
    }

    $(document).on("click", ".addEventDiv i, #addEventLink", function () {
        Popup.showAddGroupEventPopup(lang.eventName,
            createEventPopupCallBack.bind(this));
    });

    function createEventPopupCallBack(confirmation) {
        if (confirmation) {
            var eventName = $("#input-field-name").val();
            var groupName = Utils.GetGroup();

            isEventExist(eventName, groupName);
        }
    }

    $(document).on("click", ".events.dropdown-menu li a", function () {
        var text = $(this).text();
        $(this).parents('.dropdown').find('.dropdown-toggle').html(text);

        $(".deleteGroupEvent.deleteEvent").css('visibility', 'visible');
        loadGiftsSection(text);

        document.body.click();
        return false;
    });

    function loadEvents(groupText) {
        $.ajax({
            url: Constants.Url.LoadEvents,
            type: 'POST',
            data: { groupName: groupText },
            success: function (data) {
                $("#DropDownEventDiv").html(data);
            }
        });

        return false;
    }

    $(document).on("click", ".deleteEvent i", function () {
        var eventText = Utils.GetEvent();
        var groupText = Utils.GetGroup();

        Popup.showInfoPopup(lang.removeEvent + " " + eventText,
            lang.removeEventConfirmation, lang.no, lang.yes, true,
            openPopupDeleteEventCallBack.bind(this, eventText, groupText));
    });

    function openPopupDeleteEventCallBack(eventName, groupName, confirmation) {
        if (confirmation) {
            swal.disableButtons();
            deleteEvent(eventName, groupName);

            Popup.showSuccess(lang.complete, eventName + ' ' + lang.hasBeenRemoved,
                Utils.ReloadPage.bind(this));
        }
    }

    function deleteEvent(eventName, groupName) {
        var data = { eventName: eventName, groupName: groupName };
        Utils.SimplePostRequest(Constants.Url.DeleteEvent, data);
    }

    // Remove User

    $(document).on("click", "#usersList ol li i", function () {
        var groupName = Utils.GetGroup();
        var userEmail = $(this).parent().find("strong").text();

        if ($("#userEmail").val() !== userEmail) {
            Popup.showInfoPopup(lang.removeUser + " " + userEmail,
                lang.removeUserConfirmation, lang.no, lang.yes, true,
                openPopupRemoveUserCallBack.bind(this, userEmail, groupName));
        }

        return false;
    });

    function openPopupRemoveUserCallBack(userEmail, groupName, confirmation) {
        if (confirmation) {
            swal.disableButtons();
            removeUserFromGroup(groupName, userEmail);

            Popup.showSuccess(lang.complete, userEmail + ' ' + lang.hasBeenRemoved, null);
        }
    }

    function removeUserFromGroup(groupName, userEmail) {
        $.ajax({
            url: Constants.Url.RemoveUserFromGroup,
            type: 'POST',
            data: { groupName: groupName, email: userEmail },
            success: function (data) {
                $("#GroupUsersDiv").html(data);
            }
        });
    }

    // Remove gift from wishlist

    $(document).on("click", ".wishlistTable tr td i", function () {
        var giftName = $(this).parent().parent().find("strong").text();
        var eventName = Utils.GetEvent();

        Popup.showInfoPopup(lang.removeGift + " " + giftName,
                lang.removeGiftConfirmation, lang.no, lang.yes, true,
                openPopupRemoveFromWishlistCallBack.bind(this, giftName, eventName));

        return false;
    });

    function openPopupRemoveFromWishlistCallBack(giftName, eventName, confirmation) {
        if (confirmation) {
            swal.disableButtons();
            removeFromWishList(giftName, eventName);

            Popup.showSuccess(lang.complete, giftName + ' ' + lang.hasBeenRemoved, null);
        }
    }

    function removeFromWishList(giftName, eventName) {
        $.ajax({
            url: Constants.Url.RemoveGiftFromWishlist,
            type: 'POST',
            data: { giftName: giftName, eventName: eventName },
            success: function () {
                loadGiftsSection(eventName);
            }
        });
    }

    // Remove gift from reservation

    $(document).on("click", ".reservedGitfsTable tr td i.trash", function () {
        var giftName = $(this).parent().parent().find("strong").first().text();
        var eventName = Utils.GetEvent();
        var groupName = Utils.GetGroup();

        Popup.showInfoPopup(lang.removeGift + " " + giftName,
                lang.removeGiftReservation, lang.no, lang.yes, true,
                openPopupRemoveFromReservationCallBack.bind(this, giftName, eventName, groupName));

        return false;
    });

    function openPopupRemoveFromReservationCallBack(giftName, eventName, groupName, confirmation) {
        if (confirmation) {
            swal.disableButtons();
            removeFromReservation(giftName, eventName, groupName);

            Popup.showSuccess(lang.complete, giftName + ' ' + lang.hasBeenRemoved, null);
        }
    }

    function removeFromReservation(giftName, eventName, groupName) {
        $.ajax({
            url: Constants.Url.RemoveGiftFromReservation,
            type: 'POST',
            data: { giftName: giftName, eventName: eventName, groupName: groupName },
            success: function () {
                loadGiftsSection(eventName);
            }
        });
    }

    // Buy gift from reservation

    $(document).on("click", ".reservedGitfsTable tr td i.cart", function () {
        var giftName = $(this).parent().parent().find("strong").first().text();
        var eventName = Utils.GetEvent();
        var groupName = Utils.GetGroup();
        var userId = $(this).parent().find(".userId").first().val();
        var giftId = $(this).parent().find(".giftId").first().val();

        Popup.showPricePopup(lang.enterCost,
            openPopupBuyGiftFromReservationCallBack.bind(this, giftName, eventName, groupName, userId, giftId));

        return false;
    });

    function openPopupBuyGiftFromReservationCallBack(giftName, eventName, groupName, userId, giftId) {
        var price = $("#input-field-price").val();
        price = Utils.FormatNumber(price);
        $(".error").text("");

        if (Utils.IsNumeric(price)) {
            swal.disableButtons();
            buyFromReservation(giftName, eventName, groupName, userId, giftId, price);

            Popup.showSuccess(lang.complete, giftName + ' ' + lang.hasBeenBought, null);
        } else {
            $(".error").text(lang.priceError);
        }
    }

    function buyFromReservation(giftName, eventName, groupName, userId, giftId, price) {
        $.ajax({
            url: Constants.Url.BuyGiftFromReservation,
            type: 'POST',
            data: {
                giftName: giftName,
                eventName: eventName,
                groupName: groupName,
                userId: userId,
                giftId: giftId,
                price: price
            },
            success: function () {
                loadGiftsSection(eventName);
            }
        });
    }

    // Add Gift

    function createGift(giftName) {
        var eventName = Utils.GetEvent();

        $.ajax({
            url: Constants.Url.AddGiftInWishlist,
            type: 'POST',
            data: { newGift: giftName, eventName: eventName },
            success: function () {
                loadGiftsSection(eventName);
            }
        });

        return false;
    }

    function isGiftExist(name, eventName, groupName) {
        $.ajax({
            url: Constants.Url.IsGiftExist,
            type: 'POST',
            data: { name: name, eventName: eventName, groupName: groupName },
            success: function(value) {
                if (value === "False" && !Utils.IsEmpty(name)) {
                    swal.disableButtons();
                    createGift(name);

                    Popup.showSuccess(lang.complete, name + ' ' + lang.hasBeenCreated, null);
                } else {
                    $(".error").text(lang.giftAlreadyExist);
                }
            }
        });

        return false;
    }

    $(document).on("click", "#addGiftLink", function () {
        Popup.showAddGroupEventPopup(lang.giftName,
            createGiftPopupCallBack.bind(this));

        return false;
    });

    function createGiftPopupCallBack(confirmation) {
        if (confirmation) {
            var giftName = $("#input-field-name").val();
            var eventName = Utils.GetEvent();
            var groupName = Utils.GetGroup();

            isGiftExist(giftName, eventName, groupName);
        }
    }

    // Load Gift section

    function loadGiftsSection(eventName) {
        $.ajax({
            url: Constants.Url.LoadGiftsSection,
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
        Popup.showAddGroupEventPopup(lang.enterEmail,
            addUserPopupCallBack.bind(this));

        return false;
    });

    function addUserPopupCallBack(confirmation) {
        var email = $("#input-field-name").val();

        if (confirmation) {
            if (Utils.ValidateEmail(email)) {
                createUserIfExist(email);
            } else {
                $(".error").text(lang.invalidEmail);
            }
        }    
    }

    function createUser(email) {
        var group = $('.dropdown:first').find('.dropdown-toggle').html();

        $.ajax({
            url: Constants.Url.AddUserToGroup,
            type: 'POST',
            data: { groupName: group, email: email },
            success: function (data) {
                $("#GroupUsersDiv").html(data);
            }
        });

        return false;
    }

    function createUserIfExist(userEmail) {
        $.ajax({
            url: Constants.Url.IsUserExist,
            type: 'POST',
            data: { userEmail: userEmail },
            success: function(value) {
                if (value) {
                    $(".cancel").click();
                    createUser(userEmail);
                } else {
                    $(".error").text(lang.userNotExist);
                }
            }
        });

        return false;
    }
});