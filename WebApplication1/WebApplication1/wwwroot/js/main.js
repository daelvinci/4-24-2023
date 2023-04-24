$(document).ready(function () {
    $(document).on("click", '.open-book-modal', function (e) {

        e.preventDefault();
        var url = $(this).attr("href");

        fetch(url)
            .then(response => response.text())
            .then(modalHtml => {
                $("#quickModal .modal-dialog").html(modalHtml)
            });


        $("#quickModal").modal("show")
    })

    $(document).on("click", '.add-to-basket', function(e){
        e.preventDefault();
        var basketUrl = $(this).attr("href");

        fetch(basketUrl)
            .then(response => response.text())
            .then(html=> 
               $("#basket-cart").html(html))

    })

    $(document).on("click", '.delete-from-basket', function (e) {
        e.preventDefault();
        var url =`https://localhost:7105/book/deletefrombasket/${$(this).attr("data")}`;

        fetch(url)
            .then(response => response.text())
            .then(html =>
                $("#basket-cart").html(html))

    })
})

