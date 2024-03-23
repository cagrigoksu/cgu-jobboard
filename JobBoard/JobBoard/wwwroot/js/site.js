// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showPop = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            $('#formModal .modal-body').html(result)
            $('#formModal .modal-title').html(title)
            
            $('#formModal').modal('show');
        }
    });
}

modalClose = () => {
    $('#formModal').modal('hide')   
}