////////////////////////////////////////////// from layout

$("document").ready(
    function() {

        if (window.location != $rootUrl) {
            CreateNewLikeButton(window.location);
        }

    }
);

function CreateNewLikeButton(url) {


    var elem = $(document.createElement("fb:like"));
    elem.attr("href", url);
    elem.attr("send", "false");
    elem.attr("width", "200");
    elem.attr("colorscheme", "dark");
    elem.attr("layout", "button_count");


    //$("div#fb-root").empty().append(elem);

    if (jQuery.isReady) {
        if (typeof(FB) != 'undefined' && FB != undefined && FB != null) {
            // FB.XFBML.parse($("div#fb-root").get(0));
        }
    }

}


////////////////////////////////////////////// fade in/ out


/* plugin */
jQuery.fn.dwFadingLinks = function(settings) {
    settings = jQuery.extend({
        color: '#7e7e7e',
        duration: 500
    }, settings);
    return this.each(function() {
        var original = $(this).css('color');
        $(this).mouseover(function() { $(this).animate({ color: settings.color }, settings.duration); });
        $(this).mouseout(function() { $(this).animate({ color: original }, settings.duration); });
    });
};

/* usage */
$(document).ready(function() {
    $('.fade').dwFadingLinks({
        // color: '#FFFFFF',
        color: '#7e7e7e',
        duration: 500
    });
});


////////////////////////////////////////////// video image fade rollover

/**
    * jquery.froll-0.1.js - Fancy Image Roll
    * ==========================================================
    * (C) 2011 José Ramón Díaz - jrdiazweb@gmail.com
    *
    * http://3nibbles.blogspot.com/2011/07/plugin-jquery-sliding-buttons.html
    * http://plugins.jquery.com/project/froll
    *
    * FRoll is a jQuery plugin to simplify the task of providing a simple and
    * efficient way to expand the image information of a picture using a sequence
    * of images.
    *
    * The direct use of this plugin is to provide a preview of a youtube video
    * using the Google API. In that case it presents a sucession of three
    * different moments of the video in a smooth sucession when mouse enters into
    * the image.
    *
    * Everything is self-contained. No need of extra CSS or complex controls,
    * just call the method over the image and you are ready.
    *
    * INSTANTIATION
    * Just call the ".froll()" method over the images selector.
    *
    *     $('.videoCaptionImg').froll( [ options_object ] );
    *
    * OPTIONS
    *     - transform  $.froll.youtube  Array that defines [0] as the regex to apply
    *                                   to src and [1] as the resulting string with
    *                                   {number} placeholder for the animation images.
    *     - frames     [1, 2, 3],       Array with the {number} of each frame of the animation
    *     - width      null,            Width of the animation frames.  Null = automatic
    *     - height     null,            Height of the animation frames. Null = automatic
    *     - speed      750,             Fade animation speed
    *     - time       1500,            Time between frames
    *     - click      function(taget)  Click callback function. Defaults to trigger click
    *                                   event over the container <a> of the img.
    *
    * PUBLIC API
    *     -  $.froll.stop()    Stops current animation and hides the preview
    *
    * HELPER CONSTANTS
    *     $.froll.youtube       Transform array that converts origin src stored in youtube
    *                           (http://img.youtube.com/vi/<video_ID>/0.jpg) into youtube previews.
    *     $.froll.youtubeLocal  Transform array that converts local stored captions with name the
    *                           id of the video, into youtube previews.
    *     $.froll.local         Transform array that converts local stored captions into local
    *                           stored previews with the same name but ended with "_<number>"
    *                           in the same directory than caption.
    *
    * CSS CLASSES
    *     - Container: #froll-overlay
    *     - Frames:    .froll-frame
    *
    * Legal stuff
    *     You are free to use this code, but you must give credit and/or keep header intact.
    *     Please, tell me if you find it useful. An email will be enough.
    *     If you enhance this code or correct a bug, please, tell me.
    */
(function($) {
    var imgcurrently;
    ///////////////////////////////////////////////////////////////////////////////
    // Private members
    ///////////////////////////////////////////////////////////////////////////////

    var busy = false,
        overlay = null,
        frames = [],
        frame = -1,
        lframe = 0,
        options = {},
        target = null,
        src = "",
        tickTimer = null,
        imgPreloader = new Image(),
        //isIE6 = $.browser.msie && $.browser.version < 7 && !window.XMLHttpRequest,

        // ========================================================================
        // Starts the animation
        _start = function() {

            _stop(); // Hides current animation (if any)

            if (!target.attr('src')) return; // No image src

            // Gets the options and src transform
            options = target.data('froll');
            src = target.attr('src').replace(options.transform[0], options.transform[1]);

            // Moves the overlay to the target position
            var pos = _getPos(target);
            overlay.css({
                'position': 'absolute',
                'left': pos.left,
                'top': pos.top,
                'width': pos.width,
                'height': pos.height,
                'zIndex': 99,
                'background': 'transparent'
                //,'border': '1px solid red'
            }).show();

            // ========================================================================
            // Starts the frames preload chain loading first frame
            lframe = 0; // Frame being loaded
            if (!imgPreloader) imgPreloader = new Image();
            imgPreloader.onerror = function() { _error(); };
            imgPreloader.onload = _preloadCompleted;

            imgPreloader.src = src.replace(/\{number\}/, "" + options.frames[lframe]);
            if (imgPreloader.complete) _preloadCompleted(); // Cached images don't fire onload events
        },
        // ========================================================================
        // Gracefully stops the animation
        _stop = function() {

            clearInterval(tickTimer); // Disables the timer
            imgPreloader.onerror = imgPreloader.onload = null;
            if (target && overlay.is(':visible'))
                overlay.hide().empty(); // Hides the overlay and deletes the frames

            frames = [];
            frame = -1;
            //target = options = null;
            busy = false;
        },
        // ========================================================================
        // Frame image load error
        _error = function() {
            alert("Error loading image at " + src);
        },
        // ========================================================================
        // Function called on animation click
        _click = function(e) {
            if (typeof options.click !== 'undefined')
                options.click(target);
        },
        // ========================================================================
        // Image preload complete event
        _preloadCompleted = function() {
            // Gets default image dimensions
            if (!options.width) options.width = overlay.width(); //imgPreloader.width;
            if (!options.height) options.height = overlay.height(); //imgPreloader.height;
             

            // Creates frame ima
            $('<img />').attr({
                //   'id': 'froll-frame-' + lframe,
                //   'class': 'froll-frame',
                'src': imgPreloader.src
            }).css({
                'cursor': 'pointer',
                'position': 'absolute',
                'display': 'block',
                'left': '0px',
                'top': '0px',
                'width': options.width + 'px',
                'height': options.height + 'px',
                'zIndex': lframe + 1,
                'opacity': 0
                //,'visibility': 'hidden'
            }).appendTo(overlay);

            // Shows first frame
            if (lframe == 0) {
                _tick();
                tickTimer = setInterval(_tick, options.time);
            }

            // Preloads next frame
            frames[lframe++] = 1; // Marks frame as done
            if (lframe < options.frames.length) {
                // Intermediate frame
                imgPreloader.src = src.replace(/\{number\}/i, options.frames[lframe]);
                if (imgPreloader.complete) _preloadCompleted(); // Cached images don't fire onload events
            } else
                imgPreloader.onerror = imgPreloader.onload = null; // Last frame
        },
        // ========================================================================
        // Shows next frame
        _tick = function() {

            var l = options.frames.length - 1;
            var children = overlay.children();

            // Animates next frame
            if (frame == -1) {
                // First run
                children.eq(0).css('opacity', 0).animate({ 'opacity': 1 }, options.speed);
                frame = 0;
            } else if (frame == 0) {
                // First frame (after a full run)
                for (var i = 1; i < l; i++) children.eq(i).css('opacity', 0); // Hides all but first and last frames
                children.eq(0).css('opacity', 1).show(); // Shows first frame (bellow last frame)
                children.eq(l).animate({ 'opacity': 0 }, options.speed);
            } else if (frame <= l) {
                // Intermediate frame
                var next = children.eq(frame);
                if (next)
                    next.css('opacity', 0).animate({ 'opacity': 1 }, options.speed);
            } else {
                // The last frame. Resets animation to show first frame and hide the last one
                children.eq(0).css('opacity', 1);
                children.eq(l).animate({ 'opacity': 0 }, options.speed);
            }
            frame = (frame + 1) % (l + 1);
        },
        // ========================================================================
        // Helper function to get the exact obj position in the page
        _getPos = function(obj) {

            var pos = obj.offset();

            pos.top += parseInt(obj.css('paddingTop'), 10) || 0;
            pos.left += parseInt(obj.css('paddingLeft'), 10) || 0;

            pos.top += parseInt(obj.css('border-top-width'), 10) || 0;
            pos.left += parseInt(obj.css('border-left-width'), 10) || 0;

            pos.width = obj.width();
            pos.height = obj.height();

            return pos;
        };


    ///////////////////////////////////////////////////////////////////////////////
    // Public members
    ///////////////////////////////////////////////////////////////////////////////

    // ========================================================================
    // Instantiation. Called on every object of the supplied selector
    $.fn.froll = function(obj) {
        if (!$(this).length) {
            return this;
        }

        if ($(this).data('froll')) {
            // Object already initialized. Starts the animation over it simulating a click

            if (loadFromHash) // for some reasons this is autoclicking
            {
                if ($(this).click) $(this).click();
            }
        } else {
            // Object not initialized. Sets data and binds events
            $(this)
                .data('froll', $.extend($.fn.froll.defaults, obj))
                .unbind('mouseenter')
                .bind('mouseenter', function(e) {

                    var self = $(this);
                    imgcurrently = $(this);
                    e.preventDefault();

                    showTooltip(e, self.attr('longdesc'));

                    if (busy && self !== target) _stop(); // Stops current animation
                    busy = true;
                    target = self;

                    _start(); // Starts the animation over target element
                    return;
                });
        }

        return this;
    };


    // ========================================================================
    // Container class for the public interface
    $.froll = function(obj) {
    };

    // ========================================================================
    // Inits the components needed for the animation overlay
    $.froll.init = function() {

        if ($('#froll-overlay').length) {
            return;
        }

        // Components
        $('body').append(
            overlay = $('<div id="froll-overlay"></div>')
        );

        // Animation controls events
        overlay.mouseleave(_stop);
        overlay.mouseleave(hideTooltip);
        overlay.click(_click);

        return this;
    };

    // ========================================================================
    // Stops current animation and hides overlay
    $.froll.stop = function() { _stop(); };

    // ========================================================================
    // Sample transformation arrays
    $.froll.youtube = [/.*\/(.*)\/2\.(jpg|gif|png|bmp|jpeg)(.*)?/i, 'http://img.youtube.com/vi/$1/{number}.$2']; // Caption image is located in youtube (http://img.youtube.com/vi/<video_ID>/0.jpg)
    $.froll.youtubeLocal = [/.*\/(.*)\.(jpg|gif|png|bmp|jpeg)(.*)?/i, 'http://img.youtube.com/vi/$1/{number}.$2']; // Caption image is located elsewhere but caption image name is the youtube video_ID
    $.froll.local = [/(.*)\/(.*)\.(jpg|gif|png|bmp|jpeg)(.*)?/i, '$1/$2_{number}.$3$4']; // Caption image is located elsewhere and frames are in format "originalImage_<frame>.jpg" in the same directory

    // ========================================================================
    // Froll options defaults
    $.fn.froll.defaults = {
        transform: $.froll.youtube, // Array that defines [0] as the regex to apply to src and [1] as the resulting string with {number} placeholder for the animation images
        frames: [1, 2, 3],       // Array with the {number} of each frame of the animation
        width: null,            // Width of the animation frames.  Null = automatic frame image width
        height: null,            // Height of the animation frames. Null = automatic frame image height
        speed: 750,             // Fade animation speed
        time: 1500,            // Time between frames

        click: function(taget) { window.location = target.attr('alt'); /* $(target).closest('a').click();*/ } // Click callback function
    };

    // ========================================================================
    // Inits the animation overlay on DOM ready
    $(document).ready(function() {
        $.froll.init();
    });

})(jQuery); 

  
////////////////////////////////////////////// tool tip


/************************************************************************************************************
    (C) www.dhtmlgoodies.com, October 2005
	
    This is a script from www.dhtmlgoodies.com. You will find this and a lot of other scripts at our website.	
	
    Terms of use:
    You are free to use this script as long as the copyright message is kept intact. However, you may not
    redistribute, sell or repost it without our permission.
	
    Thank you!
	
    Updated:	April, 6th 2006, Using iframe in IE in order to make the tooltip cover select boxes.
	
    www.dhtmlgoodies.com
    Alf Magne Kalleland
	
    ************************************************************************************************************/
var dhtmlgoodies_tooltip = false;
var dhtmlgoodies_tooltipShadow = false;
var dhtmlgoodies_shadowSize = 4;
var dhtmlgoodies_tooltipMaxWidth = 200;
var dhtmlgoodies_tooltipMinWidth = 100;
var dhtmlgoodies_iframe = false;
var tooltip_is_msie = (navigator.userAgent.indexOf('MSIE') >= 0 &&
    navigator.userAgent.indexOf('opera') == -1 && document.all) ? true : false;

function showTooltip(e, tooltipTxt) {

    var bodyWidth = Math.max(document.body.clientWidth, document.documentElement.clientWidth) - 20;

    if (!dhtmlgoodies_tooltip) {
        dhtmlgoodies_tooltip = document.createElement('DIV');
        dhtmlgoodies_tooltip.id = 'dhtmlgoodies_tooltip';
        dhtmlgoodies_tooltipShadow = document.createElement('DIV');
        dhtmlgoodies_tooltipShadow.id = 'dhtmlgoodies_tooltipShadow';

        document.body.appendChild(dhtmlgoodies_tooltip);
        document.body.appendChild(dhtmlgoodies_tooltipShadow);

        if (tooltip_is_msie) {
            dhtmlgoodies_iframe = document.createElement('IFRAME');
            dhtmlgoodies_iframe.frameborder = '5';
            dhtmlgoodies_iframe.style.backgroundColor = '#FFFFFF';
            dhtmlgoodies_iframe.src = '#';
            dhtmlgoodies_iframe.style.zIndex = 100;
            dhtmlgoodies_iframe.style.position = 'absolute';
            document.body.appendChild(dhtmlgoodies_iframe);
        }

    }

    dhtmlgoodies_tooltip.style.display = 'block';
    dhtmlgoodies_tooltipShadow.style.display = 'block';
    if (tooltip_is_msie) dhtmlgoodies_iframe.style.display = 'block';

    var st = Math.max(document.body.scrollTop, document.documentElement.scrollTop);
    if (navigator.userAgent.toLowerCase().indexOf('safari') >= 0) st = 0;
    var leftPos = e.clientX + 10;

    dhtmlgoodies_tooltip.style.width = null; // Reset style width if it's set 
    dhtmlgoodies_tooltip.innerHTML = tooltipTxt;
    dhtmlgoodies_tooltip.style.left = leftPos + 'px';
    dhtmlgoodies_tooltip.style.top = e.clientY + 10 + st + 'px';

    dhtmlgoodies_tooltipShadow.style.left = leftPos + dhtmlgoodies_shadowSize + 'px';
    dhtmlgoodies_tooltipShadow.style.top = e.clientY + 10 + st + dhtmlgoodies_shadowSize + 'px';

    if (dhtmlgoodies_tooltip.offsetWidth > dhtmlgoodies_tooltipMaxWidth) { /* Exceeding max width of tooltip ? */
        dhtmlgoodies_tooltip.style.width = dhtmlgoodies_tooltipMaxWidth + 'px';
    }

    var tooltipWidth = dhtmlgoodies_tooltip.offsetWidth;
    if (tooltipWidth < dhtmlgoodies_tooltipMinWidth) tooltipWidth = dhtmlgoodies_tooltipMinWidth;


    dhtmlgoodies_tooltip.style.width = tooltipWidth + 'px';
    dhtmlgoodies_tooltipShadow.style.width = dhtmlgoodies_tooltip.offsetWidth + 'px';
    dhtmlgoodies_tooltipShadow.style.height = dhtmlgoodies_tooltip.offsetJHeight + 'px';

    if ((leftPos + tooltipWidth) > bodyWidth) {
        dhtmlgoodies_tooltip.style.left = (dhtmlgoodies_tooltipShadow.style.left.replace('px', '') - ((leftPos + tooltipWidth) - bodyWidth)) + 'px';
        dhtmlgoodies_tooltipShadow.style.left = (dhtmlgoodies_tooltipShadow.style.left.replace('px', '') - ((leftPos + tooltipWidth) - bodyWidth) + dhtmlgoodies_shadowSize) + 'px';
    }

    if (tooltip_is_msie) {
        dhtmlgoodies_iframe.style.left = dhtmlgoodies_tooltip.style.left;
        dhtmlgoodies_iframe.style.top = dhtmlgoodies_tooltip.style.top;
        dhtmlgoodies_iframe.style.width = dhtmlgoodies_tooltip.offsetWidth + 'px';
        dhtmlgoodies_iframe.style.height = dhtmlgoodies_tooltip.offsetHeight + 'px';

    }

}

function hideTooltip() {
    dhtmlgoodies_tooltip.style.display = 'none';
    dhtmlgoodies_tooltipShadow.style.display = 'none';
    if (tooltip_is_msie) dhtmlgoodies_iframe.style.display = 'none';
}   


////////////////////////////////////////////// video image roll preview

$(window).ready(function() {
    $('img.preview_thmb').froll();
});


////////////////////////////////////////////// clock

// Anytime Anywhere Web Page Clock Generator
// Clock Script Generated at
// http://www.rainbow.arch.scriptmania.com/tools/clock

function tS() {
    x = new Date(tN().getUTCFullYear(), tN().getUTCMonth(), tN().getUTCDate(), tN().getUTCHours(), tN().getUTCMinutes(), tN().getUTCSeconds());
    x.setTime(x.getTime());
    return x;
}

function tN() { return new Date(); }

function lZ(x) { return (x > 9) ? x : '0' + x; }

function dT() {

    if (document.getElementById('current_running_time') != null) {
        if (fr == 0) {
            fr = 1;
            //document.write('<font size=2 face=Arial><b><span id="tP">' + eval(oT) + '</span></b></font>');
            //$("#current_running_time").html(eval(oT));
            document.getElementById('current_running_time').innerHTML = '<span id="tP">' + eval(oT) + '</span>';
        }
        document.getElementById('current_running_time').innerHTML = eval(oT);

        setTimeout('dT()', 1000);

    }

}

function y4(x) { return (x < 500) ? x + 1900 : x; }

var fr = 0, oT = "y4(tS().getYear())+'-'+lZ((tS().getMonth()+1))+'-'+lZ(tS().getDate())+' '+lZ(tS().getHours())+':'+lZ(tS().getMinutes())+':'+lZ(tS().getSeconds())+' '+'U'+'T'+'C'";


dT();
// begin the clock


////////////////////////////////////////////// history


/*
* jQuery history plugin
* 
* The MIT License
* 
* Copyright (c) 2006-2009 Taku Sano (Mikage Sawatari)
* Copyright (c) 2010 Takayuki Miwa
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

(function($) {
    var locationWrapper = {
        put: function(hash, win) {
            (win || window).location.hash = this.encoder(hash);
        },
        get: function(win) {
            var hash = ((win || window).location.hash).replace(/^#/, '');
            try {
                return $.browser.mozilla ? hash : decodeURIComponent(hash);
            } catch(error) {
                return hash;
            }
        },
        encoder: encodeURIComponent
    };

    var iframeWrapper = {
        id: "__jQuery_history",
        init: function() {
            var html = '<iframe id="' + this.id + '" style="display:none" src="javascript:false;" />';
            $("body").prepend(html);
            return this;
        },
        _document: function() {
            return $("#" + this.id)[0].contentWindow.document;
        },
        put: function(hash) {
            var doc = this._document();
            doc.open();
            doc.close();
            locationWrapper.put(hash, doc);
        },
        get: function() {
            return locationWrapper.get(this._document());
        }
    };

    function initObjects(options) {
        options = $.extend({
            unescape: false
        }, options || {});

        locationWrapper.encoder = encoder(options.unescape);

        function encoder(unescape_) {
            if (unescape_ === true) {
                return function(hash) { return hash; };
            }
            if (typeof unescape_ == "string" &&
                (unescape_ = partialDecoder(unescape_.split("")))
                || typeof unescape_ == "function") {
                return function(hash) { return unescape_(encodeURIComponent(hash)); };
            }
            return encodeURIComponent;
        }

        function partialDecoder(chars) {
            var re = new RegExp($.map(chars, encodeURIComponent).join("|"), "ig");
            return function(enc) { return enc.replace(re, decodeURIComponent); };
        }
    }

    var implementations = {};

    implementations.base = {
        callback: undefined,
        type: undefined,

        check: function() {
        },
        load: function(hash) {
        },
        init: function(callback, options) {
            initObjects(options);
            self.callback = callback;
            self._options = options;
            self._init();
        },

        _init: function() {
        },
        _options: {}
    };

    implementations.timer = {
        _appState: undefined,
        _init: function() {
            var current_hash = locationWrapper.get();
            self._appState = current_hash;
            self.callback(current_hash);
            setInterval(self.check, 100);
        },
        check: function() {
            var current_hash = locationWrapper.get();
            if (current_hash != self._appState) {
                self._appState = current_hash;
                self.callback(current_hash);
            }
        },
        load: function(hash) {
            if (hash != self._appState) {
                locationWrapper.put(hash);
                self._appState = hash;
                self.callback(hash);
            }
        }
    };

    implementations.iframeTimer = {
        _appState: undefined,
        _init: function() {
            var current_hash = locationWrapper.get();
            self._appState = current_hash;
            iframeWrapper.init().put(current_hash);
            self.callback(current_hash);
            setInterval(self.check, 100);
        },
        check: function() {
            var iframe_hash = iframeWrapper.get(),
                location_hash = locationWrapper.get();

            if (location_hash != iframe_hash) {
                if (location_hash == self._appState) { // user used Back or Forward button
                    self._appState = iframe_hash;
                    locationWrapper.put(iframe_hash);
                    self.callback(iframe_hash);
                } else { // user loaded new bookmark
                    self._appState = location_hash;
                    iframeWrapper.put(location_hash);
                    self.callback(location_hash);
                }
            }
        },
        load: function(hash) {
            if (hash != self._appState) {
                locationWrapper.put(hash);
                iframeWrapper.put(hash);
                self._appState = hash;
                self.callback(hash);
            }
        }
    };

    implementations.hashchangeEvent = {
        _init: function() {
            self.callback(locationWrapper.get());
            $(window).on('hashchange', self.check);
        },
        check: function() {
            self.callback(locationWrapper.get());
        },
        load: function(hash) {
            locationWrapper.put(hash);
        }
    };

    var self = $.extend({}, implementations.base);

    if ($.browser.msie && ($.browser.version < 8 || document.documentMode < 8)) {
        self.type = 'iframeTimer';
    } else if ("onhashchange" in window) {
        self.type = 'hashchangeEvent';
    } else {
        self.type = 'timer';
    }

    $.extend(self, implementations[self.type]);
    $.history = self;
})(jQuery);

  
////////////////////////////////////////////// html helper

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}


function htmlDecode(value) {
    return $('<div/>').html(value).text();
}


////////////////////////////////////////////// youtube player


var playlistVideos;
var eventData;
var playlistID = 1;
var vidDefault;
var done = false;
var player;
var hasStarted = false;
var optionTexts = [];
var hasPlaylistChanged = false;
var firstLoaded = true;
var videoArray;
var currentVideoID;
var endTime = 0;
var isFireFox = false;
var level = 50;
var youtubeVideoWidth = 300;
var youtubeVideoHeight = 300;
var loadFromHash = true;
var autoPlay = 1; 


// This function is called when an error is thrown by the player

function onPlayerError(errorCode) {
    alert("An error occured of type:" + errorCode);
}

//


/////// ////// video

var isPlayerLoaded = false;

function loadJSONVideo(jVid) {


    if (jVid == undefined || jVid == null) return;

    currentKey = jVid.ProviderKey;

    endTime = jVid.EndTime;

    if (!isPlayerLoaded) {
        //if(player != undefined) {
        player = new YT.Player('player',
            {
                height: youtubeVideoHeight,
                width: youtubeVideoWidth,
                videoId: jVid.ProviderKey,
                events: { 'onReady': onPlayerReady, 'onStateChange': onPlayerStateChange },
                playerVars: {
                    'autoplay': autoPlay,
                    'autohide': 1,
                    'showinfo': 0,
                    'rel': 0,
                    'start': jVid.StartTime
                }
            }
        );
        isPlayerLoaded = true;
    } else {

        if (typeof player.loadVideoById == 'function') {

            player.loadVideoById(jVid.ProviderKey, jVid.StartTime, 'default');
        }
    }        

        
    vidDefault = jVid;
    setInterval(updateytplayerInfo, 1000);


    done = false;

    if (loadFromHash) {
        window.location.hash = '!' + jVid.ProviderKey;
    }

    endTime = jVid.EndTime;
    vidDefault = jVid;

    $("#current_song").html(htmlDecode(jVid.SongDisplay));

    $("#user_account").html(
        '<a href="' + $rootUrl + jVid.UserAccount + '">' +
            jVid.UserAccount
            + '</a>'
    );

    //$("#make_favorite_video").attr("href", $rootUrl + 'addvideo.aspx?vtype=F&vidid=YT-' + jVid.ProviderKey);

    $("#add_to_playlist").attr("href", $rootUrl + 'addvideo.aspx?vtype=P&vidid=YT-' + jVid.ProviderKey);


    $("#claim_video").attr("href", $rootUrl + 'addvideo.aspx?vtype=U&vidid=YT-' + jVid.ProviderKey);

//        $("#claim_video").html(
//       '<a   href="/addvideo.aspx?vtype=U&vidid=YT-' + jVid.ProviderKey + 
//       '">Claim You Uploaded This Video</a>'
//        );

//        $("#buy_link").html(
//            htmlDecode(jVid.ITunesLink)
//        );

    $("#related_videos").html(
        htmlDecode(jVid.RelatedVids)
    );


    if (jVid.RelatedVids != '') {


        $(window).ready(function() {
            $('img.preview_thmb').froll();
        });

    }

    if (loadFromHash) {
        CreateNewLikeButton(window.location);
    }
}


function getYouTubeInfo(vidID) {
    //http://stackoverflow.com/questions/5194619/getting-youtube-video-information-using-javascript-jquery

    $.ajax({
        url: "http://gdata.youtube.com/feeds/api/videos/" + vidID + "?v=2&alt=json",
        dataType: "jsonp",
        success: function(data) { parseresults(data); }
    });
}

function parseresults(data) {
    var title = data.entry.title.$t;
    var description = data.entry.media$group.media$description.$t;
    var viewcount = data.entry.yt$statistics.viewCount;
    var author = data.entry.author[0].name.$t;
    $('#title').html(title);
    //  $('#description').html('<b>Description</b>: ' + description);
//        $('#extrainfo').html('<b>Author</b>: ' + author + '<br/><b>Views</b>: ' + viewcount);

    getComments(data.entry.gd$comments.gd$feedLink.href + '&max-results=50&alt=json', 1);

}

function getComments(commentsURL, startIndex) {

    $.ajax({
        url: commentsURL + '&start-index=' + startIndex,
        dataType: "jsonp",
        success: function(data) {
            $.each(data.feed.entry, function(key, val) {
                //  $('#comments').append('<li>Author: ' + val.author[0].name.$t + ', Comment: ' + val.content.$t + '</li>');
            });
            if ($(data.feed.entry).size() == 50) {
                getComments(commentsURL, startIndex + 50);
            }
        }
    });
}
     

function beginTime(videoID) {

    if (jSONgs == undefined) return;

    var startTime = 0;

    for (var i = 0; i < jSONgs.ArrayOfSongRecord.SongRecord.length; i++) {

        if (jSONgs.ArrayOfSongRecord.SongRecord[i].ProviderKey == currentVideoID)
            startTime = jSONgs.ArrayOfSongRecord.SongRecord[i].StartTime;
    }

    return startTime;
}

     
function updateytplayerInfo() {

    if (typeof player.getCurrentTime != 'function') {
        return;
    }

    if (player != undefined && endTime != 0 && endTime <= player.getCurrentTime() && !done) {

        goForwardNewVideo();


    }
}


function onPlayerPlaybackQualityChange() {

}
 

function onPlayerReady(evt) {
    if (autoPlay == 1) evt.target.playVideo();
}


(function($) {


    var origContent = "";

    function loadContent(hash) {


        hash = hash.replace('!', '');

        if (hash != "") {
            if (origContent == "") {
                playNewVideo(hash);
            }

        } else if (origContent != "") {
            playNewVideo(hash);
        }
    }

    $(document).ready(function() {
        if ($.history != "undefined")
            $.history.init(loadContent);
    });


})(jQuery);
     

function goForwardNewVideo() {
            

    done = true;

    var urlToFetch;

    if (loadFromHash) {

        urlToFetch = $rootUrl + "io/operation.ashx?param_type=random&currentvidid=" + vidDefault.ProviderKey;
    } else {
        if (vidDefault == null) return;

        urlToFetch = $rootUrl + "io/operation.ashx?param_type=video_playlist&playlist=" + playlistID + "&currentvidid=" + vidDefault.ProviderKey;
    }

    //var jsonVidata;

    $.getJSON(urlToFetch, function(data) {
        //jsonVidata = data;
        //loadJSONVideo(jsonVidata);   
        loadJSONVideo(data);
    });
}


function totalSeconds(videoID) {

    var startTime = 0;

    for (var i = 0; i < jSONgs.ArrayOfSongRecord.SongRecord.length; i++) {

        if (jSONgs.ArrayOfSongRecord.SongRecord[i].ProviderKey == currentVideoID)
            startTime = jSONgs.ArrayOfSongRecord.SongRecord[i].EndTime;
    }

    return startTime;
}


function onPlayerStateChange(evt) {

    if (evt.data == YT.PlayerState.PLAYING && !done) {

        if (vidDefault != null) {
            level = vidDefault.VolumeLevel * 10;
        }

        if ($("#speaker_sound").attr("alt") == "off") {

            evt.target.setVolume(0);
        } else {


            evt.target.setVolume(level);
        }
    } else if (evt.data == YT.PlayerState.ENDED) {

        goForwardNewVideo();

//         var urlToFetch;

//         if ( loadFromHash)
//         {
//            urlToFetch = "/io/operation.ashx?random=1";
//         }
//         else 
//         {
//            urlToFetch = "/io/operation.ashx?playlist=1&currentvidid=" + vidDefault.ProviderKey;
//         }

        // $.getJSON("/io/operation.ashx?playlist=1&currentvidid=" + vidDefault.ProviderKey, function (data) {
        // $.getJSON(urlToFetch, function (data) {

        //loadJSONVideo(data);
//                loadJSONVideo(vidDefault);

        //  vidDefault = data;
        //});
    } else {
    }
}


function stopVideo() {
    player.stopVideo();
}


$("#playNextVideo").click(function() {

    $.getJSON($rootUrl + "io/operation.ashx?param_type=random", function(data) {

        //   loadJSONVideo(data);
    });
});


var fetchVid = true;
var currentKey = '';

function getJSONVideo(key) {

//     if ( fetchVid && key != currentKey )
//        {
//            currentKey = key;
//            fetchVid = false;

//            $.getJSON("/io/operation.ashx?param_type=video&vid=" + key, function (data) {

//                fetchVid = true;

//                return data;
//            });
//        }

    //getJSONVideo(key);
}


function playNewVideo(key) {

    if (fetchVid && key != currentKey) {
        currentKey = key;
        fetchVid = false;

        $.getJSON($rootUrl + "io/operation.ashx?param_type=video&vid=" + key, function(data) {

            loadJSONVideo(data);
            fetchVid = true;
        });

    }

 
//    loadJSONVideo(getJSONVideo(key));

//     if ( key != currentKey )
//     {
//        currentKey = key;

////        if ( !loadFromHash) return;

////            $.getJSON("/io/operation.ashx?param_type=video&vid=" + key, function (data) {
////                loadJSONVideo(data);
////            });
//        }
//        else 
//        {
//           
//        }
}


function getYouTubeInfo(vidID) {

    if (vidID != undefined) {
        $.ajax({
            url: "http://gdata.youtube.com/feeds/api/videos/" + vidID + "?v=2&alt=json",
            dataType: "jsonp",
            success: function(data) { parseresults(data); }
        });
    }
}

function parseresults(data) {
    var title = data.entry.title.$t;
    var description = data.entry.media$group.media$description.$t;
    var viewcount = data.entry.yt$statistics.viewCount;
    var author = data.entry.author[0].name.$t;
    $('#title').html(title);
    $('#description').html('<b>Description</b>: ' + description);
    $('#extrainfo').html('<b>Author</b>: ' + author + '<br/><b>Views</b>: ' + viewcount);

    getComments(data.entry.gd$comments.gd$feedLink.href + '&max-results=50&alt=json', 1);

}

function getComments(commentsURL, startIndex) {

    $.ajax({
        url: commentsURL + '&start-index=' + startIndex,
        dataType: "jsonp",
        success: function(data) {
            $.each(data.feed.entry, function(key, val) {
                $('#comments').append('<li>Author: ' + val.author[0].name.$t + ', Comment: ' + val.content.$t + '</li>');
            });
            if ($(data.feed.entry).size() == 50) {
                getComments(commentsURL, startIndex + 50);
            }

        }
    });

}

$(document).ready(function() {
    getYouTubeInfo();
});


var hashkey;


function onYouTubePlayerAPIReady() {

//        
//                hashkey = window.location.hash;

//                hashkey = hashkey.replace('#!', '');

//                var urlToFetch;

//                if ( loadFromHash )
//                {
//                    urlToFetch = "/io/operation.ashx?vid=" + hashkey;
//                }
//                else 
//                {
//                    urlToFetch = "/io/operation.ashx?playlist=1";
//                }

//                //alert(hashkey);
//                $.getJSON(urlToFetch, function (data) {

//                    vidDefault = data;

//                    loadJSONVideo(vidDefault);
//                });
}

     
////////////////////////////////////////////// mute button  


//            function toggleSound() {
//                if (player != null) {
//                // BUG: YOUTUBE API SAYS THESE AREN'T FUNCTIONS
//                    if (player.getVolume() > 0) {
//                        player.setVolume(0);
//                        $("#speaker_sound").attr("src", $rootUrl + "content/images/speaker_off.png");
//                        $("#speaker_sound").attr("alt", "off");
//                        $("#speaker_sound").attr("title", "Sound Off");
//                        $("#speaker_words").text("Un-Mute");
//                    }
//                    else {
//                        player.setVolume(level);
//                        $("#speaker_sound").attr("src", $rootUrl + "content/images/icons/speaker_on.png");
//                        $("#speaker_sound").attr("alt", "on");
//                        $("#speaker_sound").attr("title", "Sound On");
//                        $("#speaker_words").text("Mute");
//                    }

//                }
//            }


////////////////////////////////////////////// tool tip


var overTip =
{
    getMousePosition: function(e) {
        var posX = 0;
        var posY = 0;
        if (!e) var e = window.event;
        if (e.pageX || e.pageY) {
            posX = e.pageX;
            posY = e.pageY;
        } else if (e.clientX || e.clientY) {
            posX = e.clientX + document.body.scrollLeft
                + document.documentElement.scrollLeft;
            posY = e.clientY + document.body.scrollTop
                + document.documentElement.scrollTop;
        }
        return { x: posX, y: posY };
    },
    exist: function(id) {
        return ($("#" + id).length > 0);
    }
//	,
//	showBegin : function(e,id)
//	{
//		overTip.hideAll();
//		
//		var position = overTip.getMousePosition(e);
//	
//		if (!overTip.exist(id))
//		{
//			$(document.body).append("<div id=\"" + id + "\" class=\"overTip\" style=\"display:none;\"><img src=\"/images/loadingMini.gif\" class=\"loading\" /></div>");
//		}
//		
//		$("#" + id).css("left", position.x + "px").css("top", position.y + "px").fadeIn("fast");
//		
//		if (e.stopPropagation)
//		{
//			e.stopPropagation();
//		}
//		else
//		{
//			e.cancelBubble = true
//		}
//	}
//	,
//	showEnd : function(id, content)
//	{
//		$("#" + id).empty().append($("div.value", content));
//	}
    ,
    show: function(e, id, content, width, persist) {
        var position = overTip.getMousePosition(e);

        if (!overTip.exist(id)) {
            $(document.body).append("<div id=\"" + id + "\" class=\"overTip\" style=\"display:none;\"></div>");
            $("#" + id).html(content);
        } else {
            if (!persist) {
                $("#" + id).html(content);
            }
        }
        var tip = $("#" + id);
        tip.css("left", position.x + "px").css("top", (position.y + 20) + "px");

        if (width) {
            tip.width(width);
        }

        if (tip.is(":hidden")) {
            overTip.hideAll();
            tip.fadeIn("fast");
        }

        if (e.stopPropagation) {
            e.stopPropagation();
        } else {
            e.cancelBubble = true;
        }
    },
    hideAll: function() {
        $("div.overTip").hide();
    }
};


////////////////////////////////////////// text hint

jQuery.fn.hint = function(blurClass) {
    if (!blurClass) {
        blurClass = 'blur';
    }

    return this.each(function() {
        // get jQuery version of 'this'
        var $input = jQuery(this),
            // capture the rest of the variable to allow for reuse
            title = $input.attr('title'),
            $form = jQuery(this.form),
            $win = jQuery(window);

        function remove() {
            if ($input.val() === title && $input.hasClass(blurClass)) {
                $input.val('').removeClass(blurClass);
            }
        }

        // only apply logic if the element has the attribute
        if (title) {
            // on blur, set value to title attr if text is blank
            $input.blur(function() {
                if (this.value === '') {
                    $input.val(title).addClass(blurClass);
                }
            }).focus(remove).blur(); // now change all inputs to title

            // clear the pre-defined text when form is submitted
            $form.submit(remove);
            $win.unload(remove); // handles Firefox's autocomplete
        }
    });
};


$(function() {
    // find all the input elements with title attributes
    $('input[title!=""]').hint();
});


////////////////////////////////////////////// specific pages
$('#red_submit').click(function() {
    $('#video_form').submit();
});
                     

$("#Password").keyup(function(event) {
    if (event.keyCode == 13) {
        //document.login_form.submit();
    }
});

    
//Button disable when Click
$(".disable_when_clicked").click(function() {
    $(this).attr("disabled", true);
    $(this).val("Processing...");
});


/*
 * jQuery EasIng v1.1.2 - http://gsgd.co.uk/sandbox/jquery.easIng.php
 *
 * Uses the built In easIng capabilities added In jQuery 1.1
 * to offer multiple easIng options
 *
 * Copyright (c) 2007 George Smith
 * Licensed under the MIT License:
 *   http://www.opensource.org/licenses/mit-license.php
 */

// t: current time, b: begInnIng value, c: change In value, d: duration

jQuery.extend(jQuery.easing,
    {
        easeInQuad: function(x, t, b, c, d) {
            return c * (t /= d) * t + b;
        },
        easeOutQuad: function(x, t, b, c, d) {
            return -c * (t /= d) * (t - 2) + b;
        },
        easeInOutQuad: function(x, t, b, c, d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t + b;
            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        },
        easeInCubic: function(x, t, b, c, d) {
            return c * (t /= d) * t * t + b;
        },
        easeOutCubic: function(x, t, b, c, d) {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        },
        easeInOutCubic: function(x, t, b, c, d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t + 2) + b;
        },
        easeInQuart: function(x, t, b, c, d) {
            return c * (t /= d) * t * t * t + b;
        },
        easeOutQuart: function(x, t, b, c, d) {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        },
        easeInOutQuart: function(x, t, b, c, d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        },
        easeInQuint: function(x, t, b, c, d) {
            return c * (t /= d) * t * t * t * t + b;
        },
        easeOutQuint: function(x, t, b, c, d) {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        },
        easeInOutQuint: function(x, t, b, c, d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        },
        easeInSine: function(x, t, b, c, d) {
            return -c * Math.cos(t / d * (Math.PI / 2)) + c + b;
        },
        easeOutSine: function(x, t, b, c, d) {
            return c * Math.sin(t / d * (Math.PI / 2)) + b;
        },
        easeInOutSine: function(x, t, b, c, d) {
            return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b;
        },
        easeInExpo: function(x, t, b, c, d) {
            return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b;
        },
        easeOutExpo: function(x, t, b, c, d) {
            return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b;
        },
        easeInOutExpo: function(x, t, b, c, d) {
            if (t == 0) return b;
            if (t == d) return b + c;
            if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
            return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
        },
        easeInCirc: function(x, t, b, c, d) {
            return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b;
        },
        easeOutCirc: function(x, t, b, c, d) {
            return c * Math.sqrt(1 - (t = t / d - 1) * t) + b;
        },
        easeInOutCirc: function(x, t, b, c, d) {
            if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b;
            return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b;
        },
        easeInElastic: function(x, t, b, c, d) {
            var s = 1.70158;
            var p = 0;
            var a = c;
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            if (!p) p = d * .3;
            if (a < Math.abs(c)) {
                a = c;
                var s = p / 4;
            } else var s = p / (2 * Math.PI) * Math.asin(c / a);
            return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
        },
        easeOutElastic: function(x, t, b, c, d) {
            var s = 1.70158;
            var p = 0;
            var a = c;
            if (t == 0) return b;
            if ((t /= d) == 1) return b + c;
            if (!p) p = d * .3;
            if (a < Math.abs(c)) {
                a = c;
                var s = p / 4;
            } else var s = p / (2 * Math.PI) * Math.asin(c / a);
            return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b;
        },
        easeInOutElastic: function(x, t, b, c, d) {
            var s = 1.70158;
            var p = 0;
            var a = c;
            if (t == 0) return b;
            if ((t /= d / 2) == 2) return b + c;
            if (!p) p = d * (.3 * 1.5);
            if (a < Math.abs(c)) {
                a = c;
                var s = p / 4;
            } else var s = p / (2 * Math.PI) * Math.asin(c / a);
            if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
            return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
        },
        easeInBack: function(x, t, b, c, d, s) {
            if (s == undefined) s = 1.70158;
            return c * (t /= d) * t * ((s + 1) * t - s) + b;
        },
        easeOutBack: function(x, t, b, c, d, s) {
            if (s == undefined) s = 1.70158;
            return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
        },
        easeInOutBack: function(x, t, b, c, d, s) {
            if (s == undefined) s = 1.70158;
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
        },
        easeInBounce: function(x, t, b, c, d) {
            return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b;
        },
        easeOutBounce: function(x, t, b, c, d) {
            if ((t /= d) < (1 / 2.75)) {
                return c * (7.5625 * t * t) + b;
            } else if (t < (2 / 2.75)) {
                return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
            } else if (t < (2.5 / 2.75)) {
                return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
            } else {
                return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
            }
        },
        easeInOutBounce: function(x, t, b, c, d) {
            if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b;
            return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b;
        }
    });


/*
|--------------------------------------------------------------------------
| UItoTop jQuery Plugin 1.1
| http://www.mattvarone.com/web-design/uitotop-jquery-plugin/
|--------------------------------------------------------------------------
*/

(function($) {
    $.fn.UItoTop = function(options) {

        var defaults = {
            text: 'To Top',
            min: 200,
            inDelay: 600,
            outDelay: 400,
            containerID: 'toTop',
            containerHoverID: 'toTopHover',
            scrollSpeed: 1200,
            easingType: 'linear'
        };

        var settings = $.extend(defaults, options);
        var containerIDhash = '#' + settings.containerID;
        var containerHoverIDHash = '#' + settings.containerHoverID;

        $('body').append('<a href="#" id="' + settings.containerID + '">' + settings.text + '</a>');
        $(containerIDhash).hide().click(function() {
            $('html, body').animate({ scrollTop: 0 }, settings.scrollSpeed, settings.easingType);
            $('#' + settings.containerHoverID, this).stop().animate({ 'opacity': 0 }, settings.inDelay, settings.easingType);
            return false;
        })
            .prepend('<span id="' + settings.containerHoverID + '"></span>')
            .hover(function() {
                $(containerHoverIDHash, this).stop().animate({
                    'opacity': 1
                }, 600, 'linear');
            }, function() {
                $(containerHoverIDHash, this).stop().animate({
                    'opacity': 0
                }, 700, 'linear');
            });

        $(window).scroll(function() {
            var sd = $(window).scrollTop();
            if (typeof document.body.style.maxHeight === "undefined") {
                $(containerIDhash).css({
                    'position': 'absolute',
                    'top': $(window).scrollTop() + $(window).height() - 50
                });
            }
            if (sd > settings.min)
                $(containerIDhash).fadeIn(settings.inDelay);
            else
                $(containerIDhash).fadeOut(settings.Outdelay);
        });

    };
})(jQuery);


//http://www.mediacollege.com/internet/javascript/form/limit-characters.html

function limitText(limitField, limitCount, limitNum) {
    if (limitField.value.length > limitNum) {
        limitField.value = limitField.value.substring(0, limitNum);
    } else {
        limitCount.value = limitNum - limitField.value.length;
    }
}


function initMenu() {
    $('#market_menu ul').hide();
    $('#market_menu ul.current_dept').show(); // shows last sub list

//  $('#market_menu li a').click(
//    function() {
//      var checkElement = $(this).next();
//      if((checkElement.is('ul')) && (checkElement.is(':visible'))) {
//        return false;
//        }
//      if((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
//        $('#market_menu ul:visible').slideUp('normal');
//        checkElement.slideDown('normal');
//        return false;
//        }
//      }
//    );

}

$(document).ready(function() { initMenu(); });


function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}