$(function () {
    $.extend(true, window, {
        "historian": {
            "sidebar": new hsSidebar()
        }
    });

    function hsSidebar () {
        var baseUrl = $('body').attr('baseUrl');
        var historianUrl = $('body').attr('historianUrl');

        this.load = function () {
            // get sidebar content url
            var sidebarHtmlUrl = baseUrl + '/content/html/dashboard.sidebar.html';

            // load
            $('.sidebar').load(sidebarHtmlUrl, function () {
                loadChannels();
            });
        };

        var loadChannels = function () {
            // get channels url and pass-through url
            var channelsUrl = historianUrl + '/api/channels/all';
            var requestUrl = baseUrl + '/ws-passthrough?uri=' + channelsUrl;

            // make request for data
            $.getJSON(requestUrl, function (data) {
                var sidebar = $('.historian-sidebar-channels');
                sidebar.html('');

                // check if we have data
                if (data.length == 0) {
                    // if we don't, show message
                    var item = $("<li />");
                    var link = $('<a href="#" />');
                    link.html('No Channels');
                    link.appendTo(item);

                    item.appendTo(sidebar);
                } else {
                    // if we do, loop through channels
                    for (var i = 0; i < data.length; i++) {
                        // create sidebar item
                        var item = $("<li />");
                        var link = $('<a href="#" />');
                        link.html('&#35;' + data[i].Name);
                        link.appendTo(item);

                        // add click handler
                        item.click(selectChannel);

                        // add attributes
                        item.attr('channel_name', data[i].Name);

                        // add to sidebar
                        item.appendTo(sidebar);
                    }
                }
            });
        };

        var selectChannel = function () {
            // get the current li (clicked on)
            var current = $(this);

            // get the channel name
            var channelName = current.attr('channel_name');

            // remove any active flags
            $('.nav-sidebar li').each(function (i, o) {
                $(o).removeClass('active');
            });

            // make this one active
            current.addClass('active');
            current.find('a').html('&#35;' + channelName + ' - Loading...');

            historian.channel.loadOverview(channelName, function() {
                current.find('a').html('&#35;' + channelName);
            });
        };
    };
});