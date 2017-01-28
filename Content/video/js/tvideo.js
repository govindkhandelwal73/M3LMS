var video;
var display;
var fullscreenOn = false;

window.onload = function() {
	video = document.getElementById("videoscreen");
};

function play() {
    
    video.play();
    createSeek();
    video.ontimeupdate = function(e) {
        progressUpdate();
    }
    $('.selected').addClass('gradient');
    $('.play').toggle();
    $('.pause').toggle();
}

function pause() {
    video.pause();
    createSeek();
    $('.selected').removeClass('gradient');
}

function stop() {
    video.pause();
    $('.selected').removeClass('gradient');
    video.currentTime = 0;
    createSeek();
    $('.pause').css({'display':'none'});
    $('.play').css({'display':'block'});
}

function backward() {
        $('.pause').css({'display':'none'});
        $('.play').css({'display':'block'});
        transition();
        
        var  parentIndex = $('#tvplaylist > ul> li.selected').index('li');
        
        if (parentIndex != 0){
            src = $('.selected').prev().find('a').attr('href').replace('mp4', format);
            poster = $('.selected').prev().find('img').attr('src');
            $('#tvplaylist > ul> li.selected').prev().addClass('selected gradient').next().removeClass('selected gradient');
        } else {
            src = $('#tvplaylist > ul> li:last-child').find('a').attr('href').replace('mp4', format);
            poster = $('#tvplaylist > ul> li:last-child').find('img').attr('src');
            $('#tvplaylist > ul> li:last-child').addClass('selected gradient');
            $('#tvplaylist > ul> li:first-child').removeClass('selected gradient')
        }
        
        setTimeout(function () {
            $('video').attr({
                "src" : src,
                "poster" : poster
            });
        }, 825); // время в мс
        
        setTimeout(function () {
            play();
        }, 900); // время в мс
}

function forward() {
$('.pause').css({'display':'none'});
        $('.play').css({'display':'block'});
        transition();

        
        var  parentIndex = $('#tvplaylist > ul> li.selected').index('li');
        
        if (parentIndex < count){
            src = $('.selected').next().find('a').attr('href').replace('mp4', format);
            poster = $('.selected').next().find('img').attr('src');
            $('#tvplaylist > ul> li.selected').next().addClass('selected gradient').prev().removeClass('selected gradient');
        } else {
            src = $('#tvplaylist > ul> li:first-child').find('a').attr('href').replace('mp4', format);
            poster = $('#tvplaylist > ul> li:first-child').find('img').attr('src');
            $('#tvplaylist > ul> li:first-child').addClass('selected gradient');
            $('#tvplaylist > ul> li:last-child').removeClass('selected gradient')
        }
        
        setTimeout(function () {
            $('video').attr({
                "src" : src,
                "poster" : poster
            });
        }, 825); // время в мс
        
        setTimeout(function () {
            play();
        }, 900); // время в мс
}

function createSeek(){
    var video_duration = video.duration;
    $('#durationBar').slider({
        value: 0,
        step: 0.01,
        orientation: "horizontal",
        range: "min",
        max: video_duration,
        animate: true,                    
        slide: function(){                            
            seeksliding = true;
        },
        stop:function(e,ui){
            seeksliding = false;    
            val = ui.value;
            video.currentTime = val;
        }
    });
};

function progressUpdate() {
    /*left = (video.currentTime / video.duration * 100)  + "%";*/
    $('#durationBar').slider({
        value : video.currentTime
    });
    if (video.duration == video.currentTime){
        if (count < 0){
            stop();
            $('.pause').css({'display':'none'});
            $('.play').css({'display':'block'});
        } else {
            forward();
        }
    }
};

function goFullscreen() {
    var elem = document.getElementById("videoscreen");
    if (elem.requestFullscreen) {
      elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen) {
      elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) {
      elem.webkitRequestFullscreen();
    }
    fullscreenOn=true;
    $("#tvbuttons").addClass('hover');
}

function outFullscreen() {
    var elem = document.getElementById("videoscreen");
    if(document.cancelFullScreen) {
        document.cancelFullScreen();
    } 
    else if(document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
    } 
    else if(document.webkitCancelFullScreen) {
        document.webkitCancelFullScreen();
    }
    fullscreenOn=false;
    $("#tvbuttons").removeClass('hover');
}

function transition(){
    var trs = ["center","side","top"];
    m = parseInt(0);
    n = parseInt(2);
    number = Math.floor( Math.random( ) * (n - m + 1) ) + m;
    $('#tvplayer> .shadow> #transition > div').addClass('line '+trs[number]);
    setTimeout(function () {
        $('#tvplayer> .shadow> #transition > div').removeClass('line '+trs[number]);
    }, 1500); // время в мс
    
}

$(document).ready(function(){
    
    if (Modernizr.video) {
	  if (Modernizr.video.h264) {
	    format = 'mp4';
	  } else if (Modernizr.video.webm) {
	    format = 'webm';
	  } else if (Modernizr.video.ogg){
	    format = 'ogg';
	  }
	  }
    
    
    count = $('#tvplaylist > ul > li').size()-1;
    off = 0;

    $('.play').click(function(){
        play();
    });
    
    $('.pause').click(function(){
        pause();
        $('.pause').toggle();
        $('.play').toggle();
    });
    
    $('.stop').click(function(){
        stop();
    });
    
    $('.backward').click(function(){
        backward();
    });
    
    $('.forward').click(function(){
        forward();
    });

    $('.icon').click(function(){
        if (video.muted) {
            video.muted = false;
            $('.icon').addClass('on');
            $('.icon').removeClass('off');
            $('#volume-slider').slider({
                value: 1,
                orientation: "horizontal",
                range: "min",
                max: 1,
                step: 0.05,
                animate: true,
                slide:function(e,ui){
                    video.volume = ui.value;
                }
            });
        } else {
            video.muted = true;
            $('#volume-slider').slider({
                value: 0,
                orientation: "horizontal",
                range: "min",
                max: 1,
                step: 0.05,
                animate: true,
                slide:function(e,ui){
                    video.volume = ui.value;
                }
            });
            $('.icon').addClass('off');
            $('.icon').removeClass('on');
        }
    });
    
    $('.fullscreen').click(function(){
        if (fullscreenOn){
            outFullscreen();
        } else {
            goFullscreen();
        }
    });
    
    $('#volume-slider').slider({
        value: 1,
        orientation: "horizontal",
        range: "min",
        max: 1,
        step: 0.05,
        animate: true,
        slide:function(e,ui){
            video.volume = ui.value;
            val = ui.value;
            if (val < 0.10){
                $('.icon').addClass('off');
                $('.icon').removeClass('on');
            } else {
                $('.icon').addClass('on');
                $('.icon').removeClass('off');
            }
        }
    });
   
    $('video').bind('webkitfullscreenchange mozfullscreenchange fullscreenchange', function(e) {
        var state = document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen;
        var event = state ? true : false ;
        var fullscreenOn = state ? true : false ;
        if(event){
            $("#tvbuttons").addClass('hover');
        } else {
            $("#tvbuttons").removeClass('hover');
        }
    });
    
    //Playlist//

    
    $('#tvplaylist > ul > li').click(function(){
        transition();
        $('.selected').removeClass('gradient');
        $('.pause').css({'display':'none'});
        $('.play').css({'display':'block'});
        
        $('#tvplaylist > ul > li').removeClass('selected');
        
        $(this).addClass('selected');
        
        var src = $(this).find('a').attr('href').replace('mp4', format),
            poster = $(this).find('img').attr('src');
        
        setTimeout(function () {
            $('video').attr({
                "src" : src,
                "poster" : poster
            });
        }, 825); // время в мс
        
        setTimeout(function () {
            play();
        }, 900); // время в мс
        
        return false;
    });
    
//    $('#tvplaylist > ul > li').append('<div class="play-icon"></div>');
    
    $('#tvplaylist > ul> li:first-child').addClass('selected');
    
    $('video').attr({
        "src" : $('#tvplaylist > ul > li').eq(0).find('a').attr('href').replace('mp4', format),
        "poster" : $('#tvplaylist > ul > li').eq(0).find('img').attr('src')
    });
    
        setTimeout(function () {
            var h = $('#tvplaylist > ul > li:nth-child(1)').height();
            $('#tvplaylist > ul > li ').css({height:h});
        }, 10); // время в мс
    
    
    /*social*/
    close = true;
    $('.btn').click(function(){
        if (close){
            $('.social-container').addClass('open');
            $('.social-container').removeClass('close');
            close = false;
            
        } else {
            $('.social-container').removeClass('open');
            $('.social-container').addClass('close');
            close = true;
        }
    });
    l = window.location.href;
    t = $("title").text();
    d = $('meta[name="description"]').attr('content');
    
    $('.fb').click(function(){
    myWin=open("http://www.facebook.com/sharer.php?u="+l+"&caption="+t+"&description="+d+"","displayWindow","width=520,height=300,left=350,top=170,status=no,toolbar=no,menubar=no");
    })
    
        
        
    $('.gplus').click(function(){
    myWin=open("https://plus.google.com/share?url="+l+"","displayWindow","width=520,height=300,left=350,top=170,status=no,toolbar=no,menubar=no");
    })
    
        
    $('.tumblr').click(function(){
    myWin=open("http://www.tumblr.com/share/link?url="+l+"&name="+t+"&description="+d+"","displayWindow","width=520,height=300,left=350,top=170,status=no,toolbar=no,menubar=no");
    })      
    
    $('.twitter').click(function(){
    myWin=open("https://twitter.com/intent/tweet?url="+l+"&text="+t+"-"+d+"&original_referer="+l+"","displayWindow","width=520,height=300,left=350,top=170,status=no,toolbar=no,menubar=no");
    })
        
    $('.vk').click(function(){
    myWin=open("http://vkontakte.ru/share.php?url="+l+"","displayWindow","width=520,height=300,left=350,top=170,status=no,toolbar=no,menubar=no");
    })

});
