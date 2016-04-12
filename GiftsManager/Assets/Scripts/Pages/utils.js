window.Utils = window.Utils || {};

Utils.FormatNumber = function(input) {
    return input.replace(",", ".").replace("$", "").trim();
};

Utils.IsEmpty = function (input) {
    return (input.length === 0 || !input.trim());
}

Utils.IsNumeric = function(input) {
    return (input - 0) == input && ("" + input).trim().length > 0;
};

Utils.GetGroup = function() {
    return $('#dropdownGroupId').text();
}

Utils.GetEvent = function() {
    return $("#dropdownEventId").text();
}

Utils.ValidateEmail = function (email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

Utils.ReloadPage = function() {
    window.location.reload();
}

Utils.SimplePostRequest = function(url, dataDictionnary) {
    $.ajax({
        url: url,
        type: 'POST',
        data: dataDictionnary
    });

    return false;
}