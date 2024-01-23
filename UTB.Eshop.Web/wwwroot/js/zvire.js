function Buy(zvireId, urlAction, outElementId, locale) {
    $.ajax({
        type: "POST",
        url: urlAction,
        data: { zvireId: zvireId },
        dataType: "text",
        success: function (totalPrice) {
            ChangeTotalPriceInformation(outElementId, locale, totalPrice);
        },
        error: function (req, status, error) {
            $(outElementId).text('error during buying!');
        }
    });
}

function ChangeTotalPriceInformation(outElementId, locale, totalPrice) {
    $(outElementId).text(parseFloat(totalPrice).toLocaleString(locale,
        {
            style: "currency",
            currency: "CZK",
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        }));
}