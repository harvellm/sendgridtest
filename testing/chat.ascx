﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="chat.ascx.cs" Inherits="testing.chat" %>

<div class="container">
    <input type="text" id="message" />
    <input type="button" id="sendmessage" value="Broadcast" />
    <input type="button" id="testmessage" value="Send" />
    <input type="button" id="proofmessage" value="GenerateProof" />
    <input type="hidden" id="displayname" />
    <input type="file" id="fileUpload" />
    <input type="button" id="uploadtest" />
    <ul id="discussion"></ul>
</div>
<!--Script references. -->
<!--Reference the jQuery library. -->
<script src="Scripts/jquery-3.1.1.min.js"></script>
<!--Reference the SignalR library. -->
<script src="Scripts/jquery.signalR-2.2.2.min.js"></script>
<!--Reference the autogenerated SignalR hub script. -->
<script src="signalr/hubs"></script>
<!--Add script to update the page and send messages.-->
<script type="text/javascript">
    var fileData
    function getBuffer(resolve) {
        var reader = new FileReader();
        //reader.readAsArrayBuffer(fileData);
        reader.readAsDataURL(fileData)
        reader.onload = function () {
            var arrayBuffer = reader.result
            //var bytes = new Uint8Array(arrayBuffer);
            resolve(arrayBuffer);
        }
    }

    $(function () {
        // Declare a proxy to reference the hub.
        var chat = $.connection.chatHub;
        var file = $.connection.fileHub;
        // Create a function that the hub can call to broadcast messages.
        chat.client.broadcastMessage = function (name, message) {
            // Html encode display name and message.
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(message).html();
            // Add the message to the page.
            $('#discussion').append('<li><strong>' + encodedName
                + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };
        file.client.writeFile = function (name, message) {
            // Html encode display name and message.
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(message).html();
            // Add the message to the page.
            $('#proofMessages').append('<li><strong>' + encodedName
                + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };
        // Get the user name and store it to prepend to messages.
        $('#displayname').val(prompt('Enter your name:', ''));
        // Set initial focus to message input box.
        $('#message').focus();
        //Set the querystring
        $.connection.hub.qs = { 'name': $('#displayname').val() }
        // Start the connection.
        $.connection.hub.start().done(function () {
            $('#sendmessage').click(function () {
                // Call the Send method on the hub.
                chat.server.send($('#displayname').val(), $('#message').val());
                // Clear text box and reset focus for next comment.
                $('#message').val('').focus();
            });

            $('#testmessage').click(function () {
                chat.server.send($('#displayname').val(), $('#message').val(), $('#displayname').val());
            });

            $('#proofmessage').click(function () {
                chat.server.getCartStatus($('#displayname').val());
            });

            $('#uploadtest').click(function () {
                var files = $('#fileUpload')[0].files
                // Pass the file to the blob, not the input[0].
                fileData = new Blob([files[0]]);
                // Pass getBuffer to promise.
                var promise = new Promise(getBuffer);
                // Wait for promise to be resolved, or log error.
                promise.then(function (data) {
                    // Here you can pass the bytes to another function.
                    $.ajax
                    file.server.saveFile("test.pdf",data.split(',')[1]);
                }).catch(function (err) {
                    console.log('Error: ', err);
                });
            });
        });
    });

</script>
