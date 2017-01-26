var ForumCommand = function(command, id) {
    var forumCommandObject = {
        "Command": command,
        "Id": id
    };
    try {
        window.external.notify(JSON.stringify(forumCommandObject));
    } catch (e) {

    } 
};

var ScrollToBottom = function() {
    $('html, body').animate({ scrollTop: $(document).height() }, 0);
};

var ScrollToDiv = function (pti) {
    var test = $(pti);
    if (test != null) {
        try {
            $('html, body').animate({
                scrollTop: $(pti).offset().top
            }, 0);
        }
        catch (err) {
            // Ignore, we're probably at the bottom of the page.
            // Besides, if it fails, it just won't scroll.
        }
    }
};


var QuotePreviousPost = function (postId) {

    var quoteObject = {};
    quoteObject.post_id = postId;
    quoteObject.thread_id = $('body').attr('data-thread-id');
    quoteObject.thread_name = $('body').attr('data-thread-name');
    ForumCommand('previous', JSON.stringify(quoteObject));
}

var MarkAsLastRead = function (postId) {

    var quoteObject = {};
    quoteObject.post_id = postId;
    quoteObject.thread_id = $('body').attr('data-thread-id');
    quoteObject.thread_name = $('body').attr('data-thread-name');
    ForumCommand('markAsLastRead', JSON.stringify(quoteObject));
}


var QuotePost = function (postId) {
    
    var quoteObject = {};
    quoteObject.post_id = postId;
    quoteObject.thread_id = $('body').attr('data-thread-id');
    quoteObject.thread_name = $('body').attr('data-thread-name');
    ForumCommand('quote', JSON.stringify(quoteObject));
}

var EditPost = function (postId) {

    var quoteObject = {};
    quoteObject.post_id = postId;
    quoteObject.thread_id = $('body').attr('data-thread-id');
    quoteObject.thread_name = $('body').attr('data-thread-name');
    ForumCommand('edit', JSON.stringify(quoteObject));
}

var ShowHiddenPosts = function() {
    $('#showPosts').fadeOut();
    $('.hiddenpost').fadeIn();
};

var ScrollToTable = function (pti) {
    var table = $('table[data-idx=' + "'" + pti + "'" + ']');
    if (table.length > 0) {
        $('html, body').animate({
            scrollTop: $('table[data-idx=' + "'" + pti + "'" + ']').offset().top
        }, 0);
    }
};

var AddPostToThread = function(postId, postHtml) {
    $('#' + postId).html(postHtml);
};

var OpenLink = function(link) {
    var hostname = $.url('hostname', link);
    if (hostname === "about") {
        ForumCommand('openPost', link);
        return false;
    }
    // If the link is not for another SA thread, open it in IE.
    if (hostname !== 'forums.somethingawful.com' && hostname !== "") {
        ForumCommand('openLink', link);
        return false;
    }
    var file = $.url('file', link);
    switch(file)
    {
        case 'showthread.php':
            ForumCommand('openThread', link);
            break;
        case 'member.php':
            ForumCommand('profile', $.url('?userid', link));
            break;
        case 'forumdisplay.php':
            ForumCommand('openForum', link);
            break;
        case 'search.php':
            if ($.url('?action', link) === 'do_search_posthistory') {
                ForumCommand('post_history', $.url('?userid', link));
            }
            break;
        case 'banlist.php':
            ForumCommand('rap_sheet', $.url('?userid', link));
    }
    return false;
};

var ResizeWebviewFont = function (value) {

    if (value >= 16) {
        $('.av').each(function () {
            $(this).css('width', 92);
            $(this).css('height', 92);
        });
    }
    else if (value <= 15 && value >= 10) {
        $('.av').each(function () {
            $(this).css('width', 64);
            $(this).css('height', 64);
        });
    }
    else if (value < 10) {
        $('.av').each(function () {
            $(this).css('width', 32);
            $(this).css('height', 32);
        });
    }

    $('body').css('font-size', value + 'px');
    $('h4').css('font-size', value + 'px');
    $('input').css('font-size', value + 'px');
    $('a').css('font-size', value + 'px');
    $('div').css('font-size', value + 'px');
    $('tr').css('font-size', value + 'px');
    $('td').css('font-size', value + 'px');
    $('dl').css('font-size', value + 'px');
    $('dt').css('font-size', value + 'px');
};

var RemoveCustomStyle = function() {
    $('body').removeAttr('style');
    $('h4').removeAttr('style');
    $('input').removeAttr('style');
    $('a').removeAttr('style');
    $('div').removeAttr('style');
    $('tr').removeAttr('style');
    $('td').removeAttr('style');
    $('dl').removeAttr('style');
    $('dt').removeAttr('style');
};

window.SA || (SA = {});

$(document).ready(function () {
    // MP4 + WEBM Thanks again to the SALR team! :)
    // Should attempt MP4 stream first, then WEBM fallback
    // Only MP4. Edge does not support WebM.

    if ($("body").attr("data-show-embedded-tweets").toLowerCase() === "true") {
        // TWITTER TEST - Thank you to SALR Chrome :)
        var tweets = $('.postbody a[href*="twitter.com"]');
        tweets = tweets.not(".postbody:has(img[title=':nws:']) a").not(".postbody:has(img[title=':nms:']) a");
        tweets = tweets.not('.bbc-spoiler a');
        tweets.each(function () {
            var match = $(this).attr('href').match(/(https|http):\/\/twitter.com\/[0-9a-zA-Z_]+\/(status|statuses)\/([0-9]+)/);
            if (match == null) {
                return;
            }
            var tweetId = match[3];
            var link = this;
            $.ajax({
                url: "https://api.twitter.com/1/statuses/oembed.json?id=" + tweetId,
                dataType: 'jsonp',
                success: function (data) {
                    link = $(link).wrap("<div class='tweet'>").parent();
                    // WinRT Webviews hate calling out to external web javascript. So for now, we will load our own. :(
                    data.html = data.html.replace('<script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>', '<script async src="ms-appx-web:///Assets/widgets.js" charset="utf-8"></script>');
                    $(link).html(data.html);
                }
            });
        });
    }

    if ($("body").attr("data-show-embedded-gifv").toLowerCase() === "true") {
        //IMGUR GIFV
        var gifvs = $('a[href$="gifv"]');
        gifvs = gifvs.not(".postbody:has(img[title=':nws:']) a").not(".postbody:has(img[title=':nms:']) a");
        gifvs = gifvs.not('.bbc-spoiler a');
        gifvs.each(function () {
            // ($(this).attr('href').substr($(this).attr('href').length-4).indexOf('mp4') != -1)
            $(this).html('<video preload="auto" autoplay="true" loop max muted="true"> <source src="' + $(this).attr('href').replace(/\.gifv$/i, '.mp4') + '" type="video/mp4"> </video>');
        });
    }

    if ($("body").attr("data-show-embedded-video").toLowerCase() === "true") {
        var mp4 = $('.postbody a[href$="mp4"]');
        mp4 = mp4.not(".postbody:has(img[title=':nws:']) a").not(".postbody:has(img[title=':nms:']) a");
        mp4 = mp4.not('.bbc-spoiler a');
        mp4.each(function () {
            $(this).html('<video preload="auto" autoplay="false" controls loop max muted="true"><source src="' + $(this).attr('href') + '" type="video/mp4"> </video>');
        });
    }

    $(".bbc-spoiler").bind("touchstart", function(e) {
        e.target === this && ($(this).toggleClass("stay"),
            e.stopPropagation(),
            e.preventDefault());
    });

    $(".bbc-spoiler").click(function(e) {
        e.target === this && $(this).toggleClass("stay");
    });

});

var timg = new function (l, j, b) {
    var h = this
      , d = function (p, n) {
          var a = $(this).siblings("img"), k, o;
          if (a.attr("t_width")) {
              $(this).removeClass("expanded"),
              a.attr({
                  width: a.attr("t_width"),
                  height: a.attr("t_height")
              }),
              a.removeAttr("t_width"),
              a.removeAttr("t_height")
          } else {
              $(this).addClass("expanded");
              a.attr({
                  t_width: a.attr("width"),
                  t_height: a.attr("height")
              });
              var m = a.parents("blockquote");
              m.length || (m = a.parents(".postbody"));
              k = parseInt(a.attr("o_width"), 10);
              o = parseInt(a.attr("o_height"), 10);
              m = Math.min(900, m.width());
              if (n && k > m) {
                  var e = a.position()
                    , m = (m - 3 * e.left) / k;
                  a.attr("width", k * m);
                  a.attr("height", o * m)
              } else {
                  a.removeAttr("width"),
                  a.removeAttr("height")
              }
              m = "body";
              o = $(m).scrollTop();
              k = a.offset().top;
              a = k + a.height();
              a - o > $(l).height() && (o = a - $(l).height());
              k < o && (o = k);
              o != $(m).scrollTop()
          }
          return !1
      }
      , c = function () {
          var n = $(this);
          if (n.hasClass("loading")) {
              n.removeClass("loading");
              var k = n[0].naturalWidth || n.width()
                , a = n[0].naturalHeight || n.height();
              if (200 > a && 500 >= k || 170 > k) {
                  n.removeClass("timg")
              } else {
                  n.addClass("complete");
                  n.attr({
                      o_width: k,
                      o_height: a
                  });
                  var k = k + "x" + a
                    , a = 1
                    , g = n[0].naturalWidth || n.width()
                    , m = n[0].naturalHeight || n.height();
                  170 < g && (a = 170 / g);
                  200 < m * a && (a = 200 / m);
                  n.attr({
                      width: g * a,
                      height: m * a
                  });
                  var a = $('<span class="timg_container"></span>')
                    , i = $('<div class="note"></div>');
                  i.text(k);
                  i.attr("title", "Click to toggle");
                  a.append(i);
                  n.before(a);
                  a.prepend(n);
                  i.click(d);
                  a.click(function (e) {
                      if (1 === e.which || b.browser.msie && 9 > parseInt(b.browser.version, 10) && 0 === e.which) {
                          return d.call(i, e, !0),
                          !1
                      }
                  })
              }
              n.trigger("timg.loaded")
          }
      }
    ;
    h.scan = function (a) {
        $(a).find("img.timg").each(function (f, e) {
            e = $(e);
            e.hasClass("complete") || (e.addClass("loading"),
            e[0].complete || null !== e[0].naturalWidth && 0 < e[0].naturalWidth ? c.call(e) : e.load(c))
        })
    }
    ;
    $(j).ready(function () {
        h.scan("body")
    });
    $(l).load(function () {
        var a = $("img.timg.loading");
        a.length && a.each(function (e, f) {
            c.call(f)
        })
    })
}