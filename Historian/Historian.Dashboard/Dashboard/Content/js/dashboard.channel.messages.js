$(function () {
    var baseUrl = $('body').attr('baseUrl');
    var historianUrl = $('body').attr('historianUrl');
    var numberMostRecentMessages = 10;

    $.extend(true, window, {
        "historian": {
            "channel": {
                "loadMessages": loadMessages
            }
        }
    });

    function loadMessages(channelName, callback) {
        var overviewHtmlUrl = baseUrl + '/content/html/dashboard.channel.messages.html';

        $('.main').load(overviewHtmlUrl, function () {
            $('.hs-channel-name').html(channelName);
            loadLastDay(channelName, callback);

            $("#channel_overview").click(function () { historian.channel.loadOverview(channelName, callback); });
        });
    };

    function loadLastDay(channel, callback) {
        // create urls
        var messagesUrl = historianUrl + '/api/dashboard/channels/' + channel + '/messages/last-day';
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

                    var timestampJson = data[i].Timestamp;
                    var timestamp = new Date(Date.parse(timestampJson));
                    var td = $("<td />");
                    td.html(timestamp.toDateString() + '<br />' + timestamp.toTimeString());
                    td.appendTo(row);

                    // create title cell
                    td = $("<td />");
                    td.html(data[i].Title);
                    td.appendTo(row);

                    // create tags cell
                    td = $("<td />");
                    $.each(data[i].Tags, function (i, tag) {
                        $("<button type='button' class='btn btn-default btn-xs' />").html(tag).appendTo(td);
                    });
                    td.appendTo(row);

                    // create kind cell
                    td = $("<td />");
                    td.html(data[i].KindName);
                    td.appendTo(row);

                    // create contents cell
                    td = $("<td />");
                    td.html(data[i].Contents);
                    td.appendTo(row);

                    // append row
                    row.appendTo($('.historian-messages tbody'));

                    kinds.push(data[i].KindName);
                }
            }

            if (callback != null) {
                callback();
            }
        });
    }

    function loadMostRecentMessages(channel, callback) {
        // create urls
        var messagesUrl = historianUrl + '/api/dashboard/channels/' + channel + '/messages/mostRecent/' + numberMostRecentMessages;
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

                    var timestampJson = data[i].Timestamp;
                    var timestamp = new Date(Date.parse(timestampJson));
                    var td = $("<td />");
                    td.html(timestamp.toDateString() + '<br />' + timestamp.toTimeString());
                    td.appendTo(row);

                    // create title cell
                    td = $("<td />");
                    td.html(data[i].Title);
                    td.appendTo(row);

                    // create tags cell
                    td = $("<td />");
                    $.each(data[i].Tags, function (i, tag) {
                        $("<button type='button' class='btn btn-default btn-xs' />").html(tag).appendTo(td);
                    });
                    td.appendTo(row);

                    // create kind cell
                    td = $("<td />");
                    td.html(data[i].KindName);
                    td.appendTo(row);

                    // create contents cell
                    td = $("<td />");
                    td.html(data[i].Contents);
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

    function countOccurances(arr) {
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
});