/// <reference path="../jquery-1.9.1.intellisense.js" />

$(function () {

    hub = $.connection.liveHub;
    hub.alert = function (message) {
        alert(message);
    };
    $.connection.hub
        .start()
        .done(function (response) {
            if (response.state == 1) {
                hub.server
                    .defaultView()
                    .done(function (result) {
                        editor.setValue(result);
                    });
            }
        });


    $('#mdl-source')
        .on('shown.bs.modal', function () {
            PrepareSource();

            source.setValue("Loading...");

            hub.server
                .getSourceCode(editor.getValue())
                .done(function (result) {
                    source.setValue(result);
                });

        });


    $('#btn-compile').click(function () {

    });

    $('#btn-source').click(function (e) {
        $('#mdl-source')
            .modal('show');

        e.preventDefault();
    });

    $('#btn-server').click(function () {

    });

    $('#btn-client').click(function () {

    });

});