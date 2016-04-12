$(document).ready(function () {

    var lang = window.tradKeys;

    // Accordions

    $(document).on("click", ".userAnchor", function (e) {
        e.preventDefault();

        var email = $(this).find('.userAnchorEmail').val();

        $('.giftsAccordionsUserEmail').filter(function() {
            return $(this).val() === email;
        }).parent().find('h3').click();
    });

    $(document).on("click", ".akordeon h3", function (e) {
        e.preventDefault();

        var element = $(this);

        $('html, body').animate({
            scrollTop: $(element).offset().top
        }, 1000);
    });

    // Groups

    $(document).on("click", ".groups.dropdown-menu li a", function () {
        var groupText = $(this).text();
        $(this).parents('.dropdown').find('.dropdown-toggle').html(groupText);

        $.ajax({
            url: Constants.Url.Home.ChangeGroup,
            type: 'POST',
            data: { groupName: groupText },
            success: function (data) {
                $("#GroupUsersDiv").html(data);

                setMargin();
                loadEvents(groupText);
            }
        });
    });

    function setMargin() {
        var height = $('#usersList').height();
        var heightToSet = 112 + height;
        $("#DropDownEventDiv").css('margin-top', heightToSet);
    }

    // Events

    $(document).on("click", ".events.dropdown-menu li a", function () {
        var text = $(this).text();
        $(this).parents('.dropdown').find('.dropdown-toggle').html(text);
        loadGiftsSection();
        document.body.click();
        return false;
    });

    function loadEvents(groupText) {
        $.ajax({
            url: Constants.Url.Home.LoadEvents,
            type: 'POST',
            data: { groupName: groupText },
            success: function (data) {
                $("#DropDownEventDiv").html(data);
            }
        });

        return false;
    }

    function loadGiftsSection() {
        var groupText = Utils.GetGroup();
        var eventName = Utils.GetEvent();

        $.ajax({
            url: Constants.Url.Home.LoadGiftsSection,
            type: 'POST',
            data: { eventName: eventName, groupName: groupText },
            success: function (data) {
                $("#GiftsDiv").html(data);

                $(".akordeon").akordeon({
                    toggle: true,
                    isExpandedItem: false
                });
            }
        });
    }

    // SweetAlert Section

    function postDataReserving(giftId, userId) {
        var groupName = Utils.GetGroup();
        var data = { id: giftId, userId: userId, groupName: groupName };
        Utils.SimplePostRequest(Constants.Url.Home.ReserveGift, data);
    }

    function postDataBuying(giftId, userId, price) {
        var groupName = Utils.GetGroup();
        var data = { id: giftId, userId: userId, groupName: groupName, price: price };
        Utils.SimplePostRequest(Constants.Url.Home.BuyGift, data);
    }

    function popupBuying(id, userId) {
        Popup.showPricePopup(lang.enterCost, popupBuyingCallback.bind(this, id, userId));
    }

    function popupBuyingCallback(id, userId) {
        var price = $("#input-field-price").val();
        price = Utils.FormatNumber(price);
        $(".error").text("");

        if (Utils.IsNumeric(price)) {
            swal.disableButtons();
            postDataBuying(id, userId, price);
            Popup.showSuccess(lang.bought, lang.giftBought, loadGiftsSection);
        } else {
            $(".error").text(lang.priceError);
        }
    }

    $(document).on("click", ".btn.btn-success.btnTakeIt", function () {
        var id = this.id;
        var userId = $(this).data("user");

        Popup.showInfoPopup(lang.takeIt, lang.select, lang.reserveIt, lang.buyIt, false, popupInfoCallBack.bind(this, id, userId));
    });

    function popupInfoCallBack(id, userId, isBought) {
        swal.disableButtons();

        if (isBought)
            popupBuying(id, userId);
        else {
            postDataReserving(id, userId);
            Popup.showSuccess(lang.reserved, lang.giftReserved, loadGiftsSection);
        }
    }
});