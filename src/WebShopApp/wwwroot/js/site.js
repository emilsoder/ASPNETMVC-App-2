
$(window).resize(function () {
    panelBodyFontSize.call();
    panelBodySize.call();

});
$(document).ready(function () {
    panelBodyFontSize.call();
    panelBodySize.call();

});
function panelBodySize() {
    if ($(window).width() < 768) {
        $('div.panel-body').css({
            'min-height': '0px',
            'max-height': 'auto',
            'height': 'auto'
        });
    }
    else if ($(window).width() > 768) {
        $('div.panel-body').css({
            'min-height': '110px',
            'max-height': '400px',
            'height': '100px'
        });
    }
};

function panelBodyFontSize() {
    if ($(window).width() < 680) {
        $('span.bodyText').css({
            'font-size': '12px'
        });
    }
    else if ($(window).width() > 680) {
        $('span.bodyText').css({
            'font-size': '15px'
        });
    };
};

$(document).ready(function () {
    $("#NOTLOGGEDIN").click(function () {
        $("#LoginModal").modal();
    });
});

function openLoginModal() {
    $("#LoginModal").modal();
};
