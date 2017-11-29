﻿(function toggleVideo() {
    $('#watch-video-tab').on('click', { toggleTarget: '#watch-video' }, toggleHidden);
})();

async function toggleHidden(e) {
    $(e.data.toggleTarget).toggleClass('no-height');
    if ($(e.data.toggleTarget).hasClass('no-height') == false) {
        // sleep to wait for slide down transition
        await sleep(1000);
        var bottom = getCoords($(e.data.toggleTarget)[0]).top + $(e.data.toggleTarget)[0].offsetHeight;
        var screenBottom = window.pageYOffset + window.innerHeight;
        var scrollDown = Math.max(bottom - screenBottom, 0);
        $('html, body').animate({
            scrollTop: '+=' + scrollDown
        }, 1000);

    }
}


// stackoverflow.com
function getCoords(elem) {
    var box = elem.getBoundingClientRect();
    var body = document.body;
    var docEl = document.documentElement;

    var scrollTop = window.pageYOffset;
    var scrollLeft = window.pageXOffset;

    var clientTop = docEl.clientTop || body.clientTop || 0;
    var clientLeft = docEl.clientLeft || body.clientLeft || 0;

    var top = box.top + scrollTop - clientTop;
    var left = box.left + scrollLeft - clientLeft;

    return { top: Math.round(top), left: Math.round(left) };
}

function sleep(time) {
    return new Promise((resolve) => setTimeout(resolve, time));
}

(function loadSideNav() {
    var $sideNav = $('.side-nav');
    if ($sideNav.length === 0) {
        return;
    }
    var $sections = $('section');
    $.each($sections, function (index, element) {
        var sectionName = element.getAttribute('sidenav-name');
        if (!sectionName) {
            sectionName = $('#' + element.id).find('h2').text();
        }
        var onclickText = "window.location = '#" + element.id + "';compensateForHeader()";
        $sideNav.append('<a onclick="' + onclickText + '">' + sectionName + '</a>');
    });
})();

function compensateForHeader() {
    $('html, body').animate({
        scrollTop: '-=' + 120
    }, 400);    
}


/* ****************************** Sortable Product Images **************************************** */
var productImages = document.getElementById('product-images');
if (productImages != null) {
    var sortable = Sortable.create(productImages, {
        onEnd: function (/**Event*/evt) {
            var $productImages = $(productImages);
            var productId = $('#Id').val();
            var imageOrder = [];
            $($productImages.children()).each(function (index, element) {
                imageOrder[index] = element.id.replace('image-', '');
            });
            var data = {
                imageOrder: JSON.stringify(imageOrder)
            };
            $.post('/edit/products/orderWebImages/' + productId, data);
        }
    });
}
