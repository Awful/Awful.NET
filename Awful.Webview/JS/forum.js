function showPostMenu(postId) {
    if (window.invokeCSharpAction) {
        var postItem = {
            type: "showPostMenu",
            id: postId
        };
        window.invokeCSharpAction(JSON.stringify(postItem));
    }
}

function hidePosts() {
    var start = document.querySelector("#start");
    var seenPosts = document.querySelectorAll("post.seen");
    var unseenPosts = document.querySelectorAll("post:not(.seen)");

    if (seenPosts.length <= 0) {
        return;
    }

    if (seenPosts.length > 0 && unseenPosts.length <= 0) {
        return;
    }

    start.classList.remove("hidden");

    for (var i = 0; i < seenPosts.length; ++i) {
        seenPosts[i].classList.add("hidden");
    }
}

function showPosts() {
    var start = document.querySelector("#start");
    start.classList.add("hidden");
    var seenPosts = document.querySelectorAll("post.seen");
    for (var i = 0; i < seenPosts.length; ++i) {
        seenPosts[i].classList.remove("hidden");
    }
}

hidePosts();