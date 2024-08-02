$(function () {

    const VISIBILE = 'visible';
    const INVISIBLE = 'invisible';

    var binaryElements = $('.binary').find('span');
    var textElements = $('.text').find('span');

    var selectedText = null;

    if (!binaryElements || !textElements) {
        return;
    };

    $(binaryElements).on('mousedown touchstart', function (event) {
        onselect(this);
    })

    $(binaryElements).on('mouseup touchend', function (event) {
        onunselect();
    })

    function onselect(element) {
        selectedText = textElements.filter('.' + $(element).attr('class'));
        selectedText
            .removeClass(INVISIBLE)
            .addClass(VISIBILE);
    }

    function onunselect() {
        selectedText
            .removeClass(VISIBILE)
            .addClass(INVISIBLE);
    }
});


$(function () {
    const GDPR_ACCEPTED = 'gdpr_accepted';

    var gdprAccepted = Cookies.get(GDPR_ACCEPTED);

    if (gdprAccepted) {
        return;
    }

    var gdprElement = $('#gdpr-consent');
    if (!gdprElement) {
        return;
    }

    gdprElement.slideDown();

    $('#accept-btn').on('click', function () {
        gdprElement.slideUp();
        Cookies.set(GDPR_ACCEPTED, 'true', { expires: 30 });
    });

    $('#reject-btn').on('click', function () {
        gdprElement.slideUp();
    });
});