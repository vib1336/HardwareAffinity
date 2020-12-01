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

// Show comment form

function showCommentBox() {
    $('#commentForm').show();
    $([document.documentElement, document.body]).animate({
        scrollTop: $("#commentForm").offset().top
    }, 2000);
}

$(document).ready(function () {
    setTimeout(function () {
        $('#tempData').hide();
    }, 3000);
});

// show modal pop-up

$(function () {

    var placeHolderElement = $('#PlaceHolderHere');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeHolderElement.html(data);
            placeHolderElement.find('.modal').modal('show');
        })
    })
})

// edit product ajax

function editProduct(id) {
    var placeHolderElement = $('#PlaceHolderHere');

    var token = $('#editProductForm input[name=__RequestVerificationToken]').val();
    var title = $('#Title').val();
    var description = $('#Description').val();

    var json = { id: id, title: title, description: description };

    $.ajax({
        url: "/api/productsapi",
        method: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            $('#ProductTitle').html(data['title']);
            $('#ProductDescription').html(data['description']);
            placeHolderElement.find('.modal').modal('hide');
        }
    });
}
