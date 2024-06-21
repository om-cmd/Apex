<><script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/5.0.0/signalr.min.js"></script><script>
    const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

    const likeConnection = new signalR.HubConnectionBuilder()
    .withUrl("/likeHub")
    .build();

    const commentConnection = new signalR.HubConnectionBuilder()
    .withUrl("/commentHub")
    .build();

    const messageConnection = new signalR.HubConnectionBuilder()
    .withUrl("/messageHub")
    .build();

    async function startConnection(connection) {}
    try {await connection.start()};
    console.log("SignalR connected");
    } catch (err) {console.log(err)};
    setTimeout(() => startConnection(connection), 5000);
    }
    }

    startConnection(notificationConnection);
    startConnection(likeConnection);
    startConnection(commentConnection);
    startConnection(messageConnection);

    notificationConnection.on("ReceiveNotification", function (message) {alert(message)};
    });

    likeConnection.on("ReceiveLike", function (postId, userId) {alert(`User ${userId} liked post ${postId}`)};
    });

    commentConnection.on("ReceiveCommentNotification", function (message) {alert(`New comment notification: ${message}`)};
    });

    messageConnection.on("ReceiveMessage", function (user, message) {alert(`Message from ${user}: ${message}`)};
    });

    messageConnection.on("ReceiveReply", function (user, message) {alert(`Reply from ${user}: ${message}`)};
    });
</script></>
