﻿$(document).on('click', '[data-toggle="lightbox"]', function (event) {
    event.preventDefault();
    $(this).ekkoLightbox();
});

$(document).on('click', '[data-toggle="lightboxOpen"]', function (event) {
    event.preventDefault();
    $("#FirstGalleryItem").ekkoLightbox();
});

//Make Facebook Feed Responsive 100% Height
$(window).on('resize', function () {
    setTimeout(function () { changeFBPagePlugin(); }, 500);
});

$(window).on('load', function () {
    setTimeout(function () { changeFBPagePlugin(); }, 1500);
});

changeFBPagePlugin = function () {
    var container_width = Number($('.fb-container').width()).toFixed(0);
    var container_height = Number($('.fb-container').height()).toFixed(0);
    if (!isNaN(container_width) && !isNaN(container_height)) {
        $(".fb-page").attr("data-width", container_width).attr("data-height", container_height);
    }
    if (typeof FB !== 'undefined') {
        FB.XFBML.parse();
    }
};

function doModal(title, text) {
    console.log(text);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-info-circle"></i> ' + title + '</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>' + text + '</p>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });
}

function doErrorModal(title, text) {
    console.log(text);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-exclamation-triangle"></i> ' + title + '</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>An unexpected error has occurred which could indicate a defect with the system</p>';
    html += '<div class="alert alert-danger" role="alert">';
    html += '<i class="fas fa-bug"></i> ' + text;
    html += '</div>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });

    var audio = new Audio("/sounds/error.wav");
    audio.play();
}

function doCrashModal(error) {
    var stackError = $(error.responseText).find(".stackerror").html() || "Unknown error";
    var stackTrace = $(error.responseText).find(".rawExceptionStackTrace").html() || "";
    console.log(stackTrace);

    html = '<div id="dynamicModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="confirm-modal" aria-hidden="true">';
    html += '<div class="modal-dialog">';
    html += '<div class="modal-content">';
    html += '<div class="modal-header">';
    html += '<h5 class="modal-title" id="customerModalLabel"><i class="fas fa-exclamation-triangle"></i> An application error has occurred</h5>';
    html += '<button type="button" class="close" data-dismiss="modal" aria-label="Close">';
    html += '<span aria-hidden="true">&times;</span>';
    html += '</button>';
    html += '</div>';
    html += '<div class="modal-body">';
    html += '<p>An unexpected error has occurred which could indicate a defect with the system</p>';
    html += '<div class="alert alert-danger" role="alert">';
    html += '<i class="fas fa-bug"></i> ' + stackError;
    html += '</div>';
    html += '<div class="pre-scrollable small">';
    html += '<p><code>' + stackTrace + '</code></p>';
    html += '</div>';
    html += '</div>';
    html += '<div class="modal-footer">';
    html += '<span class="btn btn-primary" data-dismiss="modal">Close</span>';
    html += '</div>';  // content
    html += '</div>';  // dialog
    html += '</div>';  // footer
    html += '</div>';  // modalWindow
    $('body').append(html);
    $("#dynamicModal").modal();
    $("#dynamicModal").modal('show');

    $('#dynamicModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });

    var audio = new Audio("/sounds/error.wav");
    audio.play();
}