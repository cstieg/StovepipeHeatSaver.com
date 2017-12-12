﻿/* ****************************** Image Upload ***************************************** */
// REQUIRES: JQuery, Cstieg.LightboxMessage
/* Usage:
    <div id="product-images" class="row">
        @if (ViewBag.Title == "Edit")
        {
            if (Model.WebImages != null)
            {
                foreach (var webImage in Model.WebImages)
                {
                    @Html.Partial("_ProductImagePartial", webImage)
                }
            }
        }
    </div>
    <div class="clearfix"></div>
    <hr />
    <div class="form-horizontal">
        <label for="imageFile" class="btn btn-default">Choose image file to add</label>
        @{
            int? ModelId = null;
            if (Model != null)
            {
                ModelId = Model.Id;
            }
        }
        <input id="imageFile" type="file" accept=".jpg, .jpeg, .png, .gif" multiple name="imageFile" class="hidden"
                    onchange="ImageUpload.uploadImages('/Edit/Products/AddImage/@ModelId', '#product-images')" />
    </div>
*/


// Module to upload image files to the server, and add them to a collection in HTML
var ImageUploader = function () {
    "use strict";

    // module-level variable to keep track of pending image uploads
    var imageUploadPendingCount = 0;
    var lightboxMessage = null;

    // Uploads multiple images to the server
    // url: relative URL to which to post the image
    // container: the HTML element in which to display the uploaded image
    this.uploadImages = function(url, container) {
        var e = window.event;
        var $container = $(container);
        var imageUploadCount = e.srcElement.files.length;
        imageUploadPendingCount = imageUploadCount;
        if (imageUploadCount > 0) {
            lightboxMessage = new LightboxMessage("Please wait while image upload finishes");
            lightboxMessage.display();
        }

        // post each image to server
        for (var i = 0; i < imageUploadCount; i++) {
            var file = e.srcElement.files[i];
            this.uploadSingleImage(file, url, $container);
        }
    };


    // Uploads a single image to the server
    // file: the file object of the image that has been uploaded into the browser
    // url: relative URL to which to post the image
    // $container: the JQuery object referencing the HTML element in which to display the uploaded image
    this.uploadSingleImage = function (file, url, $container) {
        var reader = new FileReader();
        var that = this;

        // Post image data to server when finished reading with FileReader
        reader.onloadend = function () {
            var myFormData = new FormData();
            myFormData.append('file', file);

            $.ajax({
                context: that,
                type: 'POST',
                url: url,
                data: myFormData,
                processData: false, // important
                contentType: false, // important
                success: function (response) {
                    imageUploadFinished();
                    var $newPicture = $(response);
                    $container.append($newPicture);
                },
                error: function (response) {
                    imageUploadFinished();
                    alert("Failed to upload image: \n" + response.responseJSON.message);
                }
            });

        };
        reader.readAsDataURL(file);
    };

    // Deletes an image from the server
    // url: The url at which to delete the image
    // imageId: the id of the image to be deleted
    this.deleteImage = function (url, imageId) {
        $.ajax({
            type: 'POST',
            url: url,
            data: { imageId: imageId },
            success: function (response) {
                $('#image-' + imageId).remove();
            },
            error: function (response) {
                alert("Failed to delete image: \n" + response.responseJSON.message);
            }
        });
    };

    // Decrement pending upload count and cancel wait message when finished all uploads
    function imageUploadFinished() {
        imageUploadPendingCount--;
        if (imageUploadPendingCount === 0) {
            lightboxMessage.destroy();
        }
    }


    // Previews an image file uploaded to form before form is saved
    // targetId: the element in which to display the image preview
    this.previewImage = function (targetId) {
        var e = window.event;
        if (e.srcElement.files.length === 1) {
            var file = e.srcElement.files[0];
            var $targetImg = $(targetId);
            var reader = new FileReader();
            reader.onloadend = function () {
                $targetImg.attr("src", reader.result);
            };
            reader.readAsDataURL(file);
        }
    };



    return {
        uploadImages: this.uploadImages,
        uploadSingleImage: this.uploadSingleImage,
        deleteImage: this.deleteImage,
        previewImage: this.previewImage
    };
};

var ImageUpload = new ImageUploader();
