var GifWrap = function () {
    $('.imgurGif').load(function (event) {
        $(event.target).parent().addClass('overlay');
    }).each(function () {
        if (this.complete) {
            $(this).load();
        }
    });

    // Toggles between first frame of Imgur gif and full-on animated gif.
    $('body').unbind('click tap').on('click tap', '.gifWrap', function (event) {
        // .closest() because sometimes the img is the target for some reason.
        var $gifwrap = $(event.target).closest('.gifWrap');
        toggleAnimation($gifwrap);
    });
}

function toggleAnimation($gifwrap) {
    var $img = $gifwrap.find('img');
    if (!$gifwrap.hasClass('playing')) {
        $gifwrap.addClass('loading');

        var link = $img.attr('src');
        var newLink = $img.data('originalurl');

        var image = new Image();
        image.onload = function () {
            $img.attr("src", newLink);
            $gifwrap.addClass('playing');
            $gifwrap.removeClass('loading');
        };
        image.src = newLink;
    } else {
        var link = $img.attr('src');
        var newLink = $img.data('posterurl');
        $gifwrap.removeClass('playing');
        $img.attr('src', newLink);
    }
}

var ForumTemplate = function () {
    var tweets = $('.post-body a[href*="twitter.com"]');
    tweets = tweets.not(".post-body:has(img[title=':nws:']) a").not(".post-body:has(img[title=':nms:']) a");
    tweets = tweets.not('.bbc-spoiler a');
    tweets.each(function () {
       var match = $(this).attr('href').match(/(https|http):\/\/twitter.com\/[0-9a-zA-Z_]+\/(status|statuses)\/([0-9]+)/);
       if (match == null) {
           return;
       }
       var tweetId = match[3];
       var link = this;
       $.ajax({
           url: "https://publish.twitter.com/oembed?omit_script=true&url=" + $(this).attr('href'),
           dataType: 'jsonp',
           success: function (data) {
               link = $(link).wrap("<div class='tweet'>").parent();
               data.html = data.html.replace('<script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>', '<script async src="ms-appx-web:///Assets/Forums/widgets.js" charset="utf-8"></script>');
               $(link).html(data.html);
               try {
                   window.twttr.widgets.load(link);
               } catch (e) {
                   console.log(e);
               }
           }
       });
    });

    var gifvs = $('a[href$="gifv"]');
    gifvs = gifvs.not(".post-body:has(img[title=':nws:']) a").not(".post-body:has(img[title=':nms:']) a");
    gifvs = gifvs.not('.bbc-spoiler a');
    gifvs.each(function () {
        // ($(this).attr('href').substr($(this).attr('href').length-4).indexOf('mp4') != -1)
        $(this).html('<video preload="auto" autoplay="true" loop max muted="true"> <source src="' + $(this).attr('href').replace(/\.gifv$/i, '.mp4') + '" type="video/mp4"> </video>');
    });

    var mp4 = $('.post-body a[href$="mp4"]');
    mp4 = mp4.not(".post-body:has(img[title=':nws:']) a").not(".post-body:has(img[title=':nms:']) a");
    mp4 = mp4.not('.bbc-spoiler a');
    mp4.each(function () {
        $(this).html('<video preload="auto" autoplay="false" controls loop max muted="true"><source src="' + $(this).attr('href') + '" type="video/mp4"> </video>');
    });

    $(".bbc-spoiler").bind("touchstart", function (e) {
        e.target === this && ($(this).toggleClass("stay"),
            e.stopPropagation(),
            e.preventDefault());
    });

    $(".bbc-spoiler").click(function (e) {
        e.target === this && $(this).toggleClass("stay");
    });

    $('.imgurGif').load(function (event) {
        $(event.target).parent().addClass('overlay');
    }).each(function () {
        if (this.complete) {
            $(this).load();
        }
    });

    // Toggles between first frame of Imgur gif and full-on animated gif.
    $('body').unbind('click tap').on('click tap', '.gifWrap', function (event) {
        // .closest() because sometimes the img is the target for some reason.
        var $gifwrap = $(event.target).closest('.gifWrap');
        toggleAnimation($gifwrap);
    });
}

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
                m.length || (m = a.parents(".post-body"));
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