/// <reference path="../jquery-2.0.3.intellisense.js" />

function formatUrl(url) {
    if (appRelative == null) {
        return "/" + url;
    }

    if (appRelative.charAt(appRelative.length - 1) == '/') {
        return appRelative + url;
    } else {
        return appRelative + '/' + url;
    }
}

$(function () {

    hub = $.connection.liveHub;
    hub.alert = function (message) {
        alert(message);
    };
    hub.client.errors = function (errors) {
        var container = $('#errors tbody');
        
        for (var i = 0; i < errors.length; i++) {
            container.append("<tr class='error'><td>" + errors[i].ErrorNumber + "</td><td>" + errors[i].ErrorText + "</td><td>" + errors[i].Line + "</td><td>" + errors[i].Column + "</td></tr>");
        }
        container.parent().parent().addClass("highlight");
        setTimeout(function () { 
            container.parent().parent().removeClass("highlight");
        }, 2000);

        $('#mdl-source').modal('hide');
    };
    $.connection.hub
        .start()
        .done(function (response) {
            if (response.state == 1) {
                hub.server
                    .defaultView(identifier)
                    .done(function (result) {
                        editor.setValue(result);
                    });
            }
        });


    $('#mdl-source')
        .on('shown.bs.modal', function () {
            PrepareSource();
            source.setValue("Loading...");
            $('#errors tbody').html("");

            hub.server
                .getSourceCode(editor.getValue())
                .done(function (result) {
                    source.setValue(result);
                });

        });


    $('#btn-source').click(function (e) {
        $('#mdl-source').modal('show');
        e.preventDefault();
    });

    $('#btn-server').click(function (e) {
        var iframe = $('#ifr-server')[0];
        iframe.url = 'about:blank';
        hub.server
            .runServer(identifier, editor.getValue())
            .done(function (newId) {
                if (typeof (newId) != 'undefined') {
                    var url = formatUrl("server/" + newId);
                    //window.location.href = url;

                    iframe.src = url;
                    $('#mdl-server').modal('show').find('.modal-url').html(iframe.src).attr('href', iframe.src);
                    $('#errors tbody').html("");
                }
            });
        e.preventDefault();
    });

    $('#btn-client').click(function (e) {
        var iframe = $('#ifr-client')[0];
        iframe.url = 'about:blank';
        hub.server
            .runServer(identifier, editor.getValue())
            .done(function (newId) {
                if (typeof (newId) != 'undefined') {
                    var url = formatUrl("client/" + newId);
                    //window.location.href = url;

                    iframe.src = url;
                    $('#mdl-client').modal('show').find('.modal-url').html(iframe.src).attr('href', iframe.src);
                    $('#errors tbody').html("");
                }
            });
        e.preventDefault();
    });

    $('#btn-save').click(function (e) {
        hub.server
            .save(identifier, editor.getValue())
            .done(function (newId) {
                if (identifier != newId) {
                    var url = formatUrl(newId);
                    window.location.href = url;
                }
            });
        e.preventDefault();
    });

});