﻿/* ****************************** Image Upload Preview ***************************************** */
function imageUploadPreview(targetId) {
    var e = window.event;
    for (var i = 0; i < e.srcElement.files.length; i++) {
        var file = e.srcElement.files[i];
        var $targetImg = $(targetId);
        var reader = new FileReader();
        reader.onloadend = function () {
            $targetImg.attr("src", reader.result);
        };
        reader.readAsDataURL(file);
    }
}

// Uploads multiple images to the server
function imageUpload(url, productId, container) {
    // global variable to keep track of pending image uploads
    imageUploadPendingCount = 0;
    showLightboxMessage("Please wait while image upload finishes", "ImageUploadPendingLightboxMessage");

    var e = window.event;
    var $container = $(container);
    for (var i = 0; i < e.srcElement.files.length; i++) {
        var file = e.srcElement.files[i];
        imageUploadSingle(file, url, productId, $container);
        imageUploadPendingCount++;
    }
}

// Uploads a single image to the server
function imageUploadSingle(file, url, productId, $container) {
    var reader = new FileReader();
    reader.onloadend = function () {
        var myFormData = new FormData();
        myFormData.append('file', file);
        $.ajax({
            type: 'POST',
            url: url + productId,
            data: myFormData,
            processData: false, // important
            contentType: false, // important
            success: function (response) {
                imageUploadFinished();
                var $newPicture = $(`
                            <picture id="image-${response.imageId}">
                                <img src="${reader.result}" class="thumbnail" />
                                <button onclick="imageDelete('/Edit/Products/DeleteImage/', ${productId}, ${response.imageId});return false;">
                                    Delete Image
                                </button>
                            </picture>
                    `);
                $container.append($newPicture);
            },
            error: function (response) {
                imageUploadFinished();
                alert("Failed to upload image");
            }
        });

    };
    reader.readAsDataURL(file);
}

// Deletes an image from the server
function imageDelete(url, productId, imageId) {
    $.ajax({
        type: 'POST',
        url: url + productId,
        data: { imageId: imageId },
        success: function (response) {
            $('#image-' + imageId).remove();
        },
        error: function (response) {
            alert("Failed to delete image");
        }
    });
}

function imageUploadFinished() {
    imageUploadPendingCount--;
    if (imageUploadPendingCount === 0) {
        hideLightboxMessage("ImageUploadPendingLightboxMessage");
    }
}