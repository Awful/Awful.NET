function showPostMenu(postId) {
    if (window.invokeCSharpAction) {
        var postItem = {
            type: "showPostMenu",
            id: postId
        };
        window.invokeCSharpAction(JSON.stringify(postItem));
    }
}