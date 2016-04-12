window.Popup = window.Popup || {};
var lang = window.tradKeys;

Popup.showPricePopup = function(title, func) {
    swal({
        title: title,
        html: '<p><input id="input-field-price"></p>' +
            '<p class="error"></p>',
        showCancelButton: true,
        closeOnConfirm: false
    }, function() {
        func();
    });
};

Popup.showAddGroupEventPopup = function(title, func) {
    swal({
        title: title,
        html: '<p><input id="input-field-name"></p>' +
            '<p class="error"></p>',
        type: "info",
        showCancelButton: true,
        closeOnConfirm: false,
        closeOnCancel: true,
        cancelButtonText: lang.no,
        confirmButtonText: lang.yes
    }, function (confirmation) {
        func(confirmation);
    });
}

Popup.showInfoPopup = function(title, text, cancelText, confirmText, closeOnCancel, func) {
    swal({
        title: title,
        text: text,
        type: "info",
        showCancelButton: true,
        closeOnConfirm: false,
        closeOnCancel: closeOnCancel,
        cancelButtonText: cancelText,
        confirmButtonText: confirmText
    }, function (confirmation) {
        func(confirmation);
    });
}

Popup.showSuccess = function(title, text, func) {
    setTimeout(function () {
        swal({
            title: title,
            text: text,
            type: 'success'
        }, function () {
            if (func) {
                func();
            }
        });
    }, 2000);
};