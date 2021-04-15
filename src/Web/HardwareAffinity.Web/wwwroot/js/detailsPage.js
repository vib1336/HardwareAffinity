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
            if (!data['hasUserVotedBefore']) {
                alert('Thank you for your vote.');
                let stars = document.getElementsByClassName('active-stars');
                for (const el of stars) {
                    el.removeAttribute('class'); // doesn't work correctly on all <a> star elements
                    el.setAttribute('class', 'disabled-stars');
                }
            }
            var average = data['average'];
            var votesCount = data['count'];
            $('#votesInfo').html(`${average.toFixed(2)} / ${votesCount} votes`);
        }
    });
}

// Show comment form

function showCommentBox(parentId) {
    $("#commentForm input[name='ParentId']").val(parentId);
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

$(document).ready(function () {
    setTimeout(function () {
        $('#tempData-success').hide();
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
    var price = $('#Price').val();

    var json = { id: id, title: title, description: description, price: price };

    $.ajax({
        url: "/productsapi/updateproduct",
        method: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            $('#ProductTitle').html(data['title']);
            $('#ProductDescription').html(`<i class="text-success fa fa-check-square-o" aria-hidden="true"></i> <strong>Description</strong> ${data['description']}`);
            $('#ProductPrice').html(`<i class="fas fa-euro-sign"></i> ${data['price'].toFixed(2)}`);
            placeHolderElement.find('.modal').modal('hide');
        }
    });
}

// show confirm delete modal

function showConfirmDeleteModal(url, productId, trId) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            $('#ConfirmDeletePlaceholder').html(data);
            $('#ConfirmDeletePlaceholder .modal').modal('show');

            $('#ConfirmDeleteBtn').click(function (event) {

                var token = $('#ConfirmDeletePlaceholder form input[name=__RequestVerificationToken]').val();

                $.ajax({
                    type: "POST",
                    url: "/productsapi/deleteproduct",
                    data: JSON.stringify({ productId: productId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    headers: { 'X-CSRF-TOKEN': token },
                    success: function (res) {
                        if (res.isDeleted) {
                            $(`#${trId}`).remove();
                            $('#ConfirmDeletePlaceholder .modal').modal('hide');
                        }
                    }
                })
            })
        }
    })
}

// delete product comment

function deleteProductComment(commentId, commentContentId) {
    if (confirm("Delete this comment?")) {
        var token = $('#commentDeleteForm input[name=__RequestVerificationToken]').val();
        var json = { commentId };

        $.ajax({
            url: "/commentsapi/deletecomment",
            type: "POST",
            data: JSON.stringify(json),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'X-CSRF-TOKEN': token },
            success: function (res) {
                if (res['isDeleted']) {
                    $(`#${commentContentId}`).html('<em>The comment was deleted by admin.</em>');
                }
            }
        })
    }
}
