//wwwroot/js/signalr-connection.js

document.addEventListener("DOMContentLoaded", function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/messageHub")
        .build();

    connection.on("ReceiveMessage", (user, message) => {
        console.log(`User ${user} says: ${message}`);
        // Display the message in the UI
    });

    connection.on("ReceiveReply", (user, message) => {
        console.log(`Reply from ${user}: ${message}`);
        // Display the reply in the UI
    });

    connection.start().catch(err => console.error(err.toString()));

    window.sendMessage = async function (user, message) {
        try {
            await connection.invoke("SendMessage", user, message);
        } catch (err) {
            console.error(err.toString());
        }
    }

    window.replyMessage = async function (user, message, replyToUser) {
        try {
            await connection.invoke("ReplyMessage", user, message, replyToUser);
        } catch (err) {
            console.error(err.toString());
        }
    }
});
