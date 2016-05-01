var hsDashboard = function () {
    var baseUrl = $('body').attr('baseUrl');
    var historianUrl = $('body').attr('historianUrl');
    var sidebar = $('.historian-sidebar-channels');

    var loadChannels = function () {
        // get channels url and pass-through url
        var channelsUrl = historianUrl + '/api/channels/all';
        var requestUrl = baseUrl + '/ws-passthrough?uri=' + channelsUrl;

        // make request for data
        $.getJSON(requestUrl, function (data) {
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
    }

    var selectChannel = function() {
        // get the current li (clicked on)
        var current = $(this);

        // get the channel name
        var channelName = current.attr('channel_name');

        // remove any active flags
        $('.historian-sidebar-channels li').each(function(i, o) {
            $(o).removeClass('active');
        });

        // make this one active
        current.addClass('active');
        current.find('a').html('&#35;' + channelName + ' - Loading...');

        $('.historian-dashboard-title').html('&#35;' + channelName);
        
        var currentDate = new Date();
        currentDate.setMonth(currentDate.getMonth() - 1);

        loadMessages(channelName, currentDate, function () {
            current.find('a').html('&#35;' + channelName);
        });
    }

    var loadMessages = function(channel, from, callback) {
        // create urls
        var messagesUrl = historianUrl + '/api/channels/' + channel + '/messages/startingAt/' + from.toJSON();
        var requestUrl = baseUrl + '/ws-passthrough?uri=' + messagesUrl;

        // make request
        $.getJSON(requestUrl, function (data) {
            $('.historian-messages tbody').html('');
            if (data.length == 0) {
                $('.historian-messages tbody').html('<tr><td colspan="6">There are no messages for &#35;' + channel + '</td></tr>');
            } else {
                var kinds = new Array();

                for (var i = 0; i < data.length; i++) {
                    // create row
                    var row = $("<tr />");

                    // create id cell
                    var td = $("<td />");
                    td.html(i + 1);
                    td.appendTo(row);

                    // create title cell
                    td = $("<td />");
                    td.html(data[i].Title);
                    td.appendTo(row);

                    // create tags cell
                    td = $("<td />");
                    td.appendTo(row);

                    // create kind cell
                    td = $("<td />");
                    td.html(data[i].KindName);
                    td.appendTo(row);

                    // create contents cell
                    td = $("<td />");
                    td.html(data[i].Contents);
                    td.appendTo(row);

                    var timestampJson = data[i].Timestamp;
                    var timestamp = new Date(Date.parse(timestampJson));
                    td = $("<td />");
                    td.html(timestamp.toDateString() + '<br />' + timestamp.toTimeString());
                    td.appendTo(row);

                    // append row
                    row.appendTo($('.historian-messages tbody'));

                    kinds.push(data[i].KindName);
                }

                var stats = countOccurances(kinds);

                var data = {
                    labels: stats[0],
                    datasets: [
                        {
                            data: stats[1],
                            backgroundColor: [
                                "#2aabd2",
                                "#265a88",
                                "#e0e0e0",
                                "#eb9316",
                                "#c12e2a"
                            ]
                        }]
                };

                var ctx = $('.historian-messages-kindsChart')[0];
                var myDoughnutChart = new Chart(ctx, {
                    type: 'doughnut',
                    data: data,
                    //options: options
                });
            }

            if (callback != null) {
                callback();
            }
        });
    }

    var countOccurances = function(arr) {
        var a = [], b = [], prev;

        arr.sort();
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] !== prev) {
                a.push(arr[i]);
                b.push(1);
            } else {
                b[b.length - 1]++;
            }
            prev = arr[i];
        }

        return [a, b];
    }

    this.load = function() {
        loadChannels();
    }
}