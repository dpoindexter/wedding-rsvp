var t;

var ddTriggers = $("a.ddtrigger")
    .mouseover(function (e) {
        var el = this;
        ddTriggers.each(function () {
            if (el === this) {
                clearTimeout(t);
                var current = $(this).addClass("active");
                var dd = current.next().show();
                position(dd, current);
            } else {
                $(this).removeClass("active").next().hide();
            }
        });
    })
    .mouseout(function (e) {
        var el = $(this);
        t = setTimeout(function () {
            el.removeClass("active").next().hide();
        }, 250);
    });

    var ddList = $("div.ddlist")
    .mouseover(function (e) {
        clearTimeout(t);
    })
    .mouseout(function (e) {
        var el = $(this).prev();
        t = setTimeout(function () {
            el.removeClass("active").next().hide();
        }, 250);
    });

function position(el, of) {
    el.position({
        of: of,
        my: "left top",
        at: "left bottom"
    });
}