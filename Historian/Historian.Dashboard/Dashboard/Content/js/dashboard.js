$(function () {
    $.extend(true, window, {
        "historian": {
            "load": function () { historian.sidebar.load(); }
        }
    })
});