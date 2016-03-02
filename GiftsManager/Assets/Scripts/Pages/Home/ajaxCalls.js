$(document).ready(function () {

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
        var url = "/Home/ChangeGroup/";

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
        var url = "Home/LoadEvents/";

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

    function loadGiftsSection() {
        var url = "/Home/LoadGiftsSection/";
        var groupText = $('#dropdownGroupId').text();
        var eventName = $("#dropdownEventId").text();

        $.ajax({
            url: url,
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
        var groupName = $("#dropdownGroupId").text();
        var url = "/Home/ReserveGift";

        $.ajax({
            url: url,
            type: 'POST',
            data: { id: giftId, userId: userId, groupName: groupName }
        });
    }

    function postDataBuying(giftId, userId, price) {
        var groupName = $("#dropdownGroupId").text();
        var url = "/Home/BuyGift";

        $.ajax({
            url: url,
            type: 'POST',
            data: { id: giftId, userId: userId, groupName: groupName, price: price }
        });
    }

    function popupBuying(id, userId) {
        swal({
            title: 'Enter the cost of the gift',
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
                postDataBuying(id, userId, price);

                setTimeout(function() {
                    swal({
                            title: 'Bought !',
                            text: 'You have now bought this gift.',
                            type: 'success'
                        },
                        function() {
                            loadGiftsSection();
                        });
                }, 3000);
            } else {
                $(".error").text("You must enter a valid price");
            }
        });
    }

    $(document).on("click", ".btn.btn-success.btnTakeIt", function () {
        var id = this.id;
        var userId = $(this).data("user");

        swal({
            title: "Take it",
            text: "Please select an option",
            type: "info",
            showCancelButton: true,
            closeOnConfirm: false,
            closeOnCancel: false,
            cancelButtonText: "Reserve it",
            confirmButtonText: "Buy it"
        }, function (isBought) {
            swal.disableButtons();

            if (isBought)
                popupBuying(id, userId);
            else {
                postDataReserving(id, userId);

                setTimeout(function () {
                    swal({
                        title: 'Reserved !',
                        text: 'You have now reserved this gift.',
                        type: 'success'
                    },
                    function () {
                        loadGiftsSection();
                    });
                }, 3000);
            }
        });
    });

    function formatNumber(input) {
        return input.replace(",", ".").replace("$", "").trim();
    }

    function isNumeric(input) {
        return (input - 0) == input && ("" + input).trim().length > 0;
    }
});
