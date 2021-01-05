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

//search
function searchAllProducts(inp, products) {
    /*the autocomplete function takes two arguments,
  the text field element and an array of possible autocompleted values:*/
    var currentFocus;
    /*execute a function when someone writes in the text field:*/

    var a, b, i, val = inp.value;
    /*close any already open lists of autocompleted values*/
    closeAllLists();
    if (!val) { return false; }
    currentFocus = -1;
    /*create a DIV element that will contain the items (values):*/
    a = document.createElement("DIV");
    a.setAttribute("id", "myInputautocomplete-list");
    a.setAttribute("class", "autocomplete-items");
    /*append the DIV element as a child of the autocomplete container:*/
    inp.parentNode.appendChild(a);
    /*for each item in the array...*/
    for (i = 0; i < products.length; i++) {
        var product = products[i];
        /*check if the item starts with the same letters as the text field value:*/
        if (product['title'].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
            /*create a DIV element for each matching element:*/
            b = document.createElement("DIV");
            /*make the matching letters bold:*/
            b.innerHTML = "<strong>" + product['title'].substr(0, val.length) + "</strong>";
            b.innerHTML += product['title'].substr(val.length);
            /*insert a input field that will hold the current array item's value:*/
            b.innerHTML += "<input type='hidden' value='" + product['title'] + "'>";

            var imgEl = document.createElement('img');
            imgEl.setAttribute('src', `${product['mainImageUrl']}`);
            imgEl.setAttribute('alt', `${product['title']}`);
            imgEl.style.width = '50px';
            b.appendChild(imgEl);

            var aEl = document.createElement('a');
            aEl.setAttribute('href', `https://localhost:44319/Products/Details/${product['id']}`);
            aEl.appendChild(b);

            /*execute a function when someone clicks on the item value (DIV element):*/
            b.addEventListener("click", function (e) {
                /*insert the value for the autocomplete text field:*/
                inp.value = this.getElementsByTagName("input")[0].value;
                /*close the list of autocompleted values,
                (or any other open lists of autocompleted values:*/
                closeAllLists();
            });
            a.appendChild(aEl);
        }
    }

    
    function addActive(x) {
        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}