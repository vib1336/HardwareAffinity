// Send vote ajax

function sendVote(productId, rate) {
    var token = $("#votesForm input[name=__RequestVerificationToken]").val();
    var json = { productId: productId, rate: rate };
    $.ajax({
        url: "/api/votes",
        method: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            var average = data['average'];
            $('#votesInfo').html(`${average} / 5`);
        }
    });
}
