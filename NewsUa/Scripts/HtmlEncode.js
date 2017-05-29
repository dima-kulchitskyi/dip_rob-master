function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}