/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init LangMenu
5. Init Close
6. Set Section Height
7. Init Section Shortcut
8. Init Skip to content
9. Init Introduction Bottom
10. Init Back To Top


******************************/

$(document).ready(function () {
    "use strict";

    /* 

	1. Vars and Inits

	*/

    var header = $('.header');
    var menuActive = false;
    var langMenuActive = false;
    var startWindowWidth = $(window).width();

    $(window).on('resize', function () {
        removeLangMenu();

        var windowWidth = $(window).width();
        if (windowWidth < 768) {


        }
        initSPSearchBox();
    });

    $(document).on('scroll', function () {
        var scrollToTop = $(document).scrollTop();
        if (scrollToTop > 50) {
            $('.backtotop_container').css('display', 'block');
        } else {
            $('.backtotop_container').css('display', 'none');
        }
        var mobileList = $('.result_tab').children('.row');
        if ($('#scrollToResult').length > 0 && mobileList.length > 0) {
            if ($('.mobile_result_container').offset().top - scrollToTop - $(window).height() <= -250) {
                $('.scrollToResult_container').addClass('hidden');
            } else {
                $('.scrollToResult_container').removeClass('hidden');
            }
        }
    });

    initMenu();
    initLangMenu();
    initClose();
    initSkipToContent();
    initBackToTop();
    initNavPills();
    initCheckBox();
    initInput();
    initTab();
    initRadioBtn();
    SetMenuStyle();
    initMLLink();
    initShortCut();
    initSPSearchBox();
    //initSortTab();

    function initMLLink() {

        $('.lnkML').on('click', function () {
            var lang_c = $(this).attr('data-langC');
            //Change Language
            var lang_store = sessionStorage.getItem("lang")
            if (lang_store != lang_c) {
                sessionStorage.setItem("lang", lang_c)
                sessionStorage.removeItem("errMsg")
            }
            //Check if post
            if ($('#method_flag').length) {
                if ($('#method_flag').val() == 'post') {
                    var path = window.location.pathname;
                    if (lang_c == 'tc') {
                        path = path.replace('/en/', '/' + lang_c + '/');
                    } else {
                        path = path.replace('/tc/', '/' + lang_c + '/');
                    }
                    var isVBEResult = false;
                    var isVBESearch = false;
                    var isSPS = false;
                    var reg = /\/VBE\/Result/;
                    isVBEResult = path.match(reg);
                    //If it is VBE Result Page
                    if (isVBEResult) {
                        window.document.forms['hiddenForm'].action = path;
                        window.document.forms['hiddenForm'].submit();
                    }
                    var reg = /\/VBE\/Search/;
                    isVBESearch = path.match(reg);
                    //If it is VBE Search Page
                    if (isVBESearch) {
                        window.document.forms[0].action = path;//POST TO WHERE
                        window.document.forms[0].submit();
                    }
                    var reg = /\/SPS\//;
                    isSPS = path.match(reg);
                    if (isSPS) {
                        window.document.forms[0].action = path;
                        window.document.forms[0].submit();
                    }
                }
            }
            else {
                var retUrl = window.location.href;// window.location.pathname;
                if (lang_c == 'tc') {
                    retUrl = retUrl.replace('/en', '/' + lang_c);
                } else {
                    retUrl = retUrl.replace('/tc', '/' + lang_c);
                }
                window.location.href = retUrl;
            }
        })
    }

    function urlencode(str) {
        str = (str + '').toString();

        return encodeURIComponent(str).replace(/!/g, '%21').replace(/'/g, '%27').replace(/\(/g, '%28').
            replace(/\)/g, '%29').replace(/\*/g, '%2A').replace(/%20/g, '+');
    }

    initScrollToResult()

    function initScrollToResult() {
        $('#scrollToResult').click(function (e) {
            scrollToElemTop($('.mobile_result_container'));
        })
    }

    function initShortCut() {
        var $secShortcut = $('.sec_shortcut');
        if ($secShortcut.length < 1) {
            return false;
        }
        var $secShortcutLinks = $secShortcut.find('.sec_shortcut_link');
        $secShortcutLinks.click(function (e) {
            onClickShortCut(this);
        });
        $('.btnPrev').click(function (e) {
            e.preventDefault();
            if ($(this).hasClass('disabled')) {
                return false;
            } else {
                $('.btnNext').removeClass('disabled');
                var $activeTab = $secShortcut.find('.sec_shortcut_item > .is-active');
                if ($activeTab.length == 1) {
                    var prevTab = $($activeTab).parent('.sec_shortcut_item').first().prev().find('a');
                    onClickShortCut(prevTab);
                }
            }
        })
        $('.btnNext').click(function (e) {
            e.preventDefault();
            if ($(this).hasClass('disabled')) {
                return false;
            } else {
                var $activeTab = $secShortcut.find('.sec_shortcut_item > .is-active');
                if ($activeTab.length == 1) {
                    var nextTab = $($activeTab).parent('.sec_shortcut_item').first().next().find('a');
                    onClickShortCut(nextTab);
                }
            }
        })
    }

    function onClickShortCut(onClickLink) {
        $('.sec_shortcut_link').removeClass('is-active');
        $('.scrollContent > div').removeClass('is-active');
        var thisTarId = $(onClickLink).attr('tarid');
        $(onClickLink).addClass('is-active');
        $('#' + thisTarId).addClass('is-active');
        $('.btnPrev').removeClass('disabled');
        $('.btnNext').removeClass('disabled');
        if ($(onClickLink).parent('li').prevAll().length == 0) {
            $('.btnPrev').addClass('disabled');
        } else if ($(onClickLink).parent('li').nextAll().length == 0) {
            $('.btnNext').addClass('disabled');
        }
    }


    function SetMenuStyle() {
        var str = window.location.href;
        var reg1 = /\/VBE\//;
        var reg2 = /\/SPS\//;
        var reg3 = /\/CU\//;
        var reg4 = /\/Static\/TextSize/;
        var reg5 = /\/Error\//;

        var r = str.match(reg1);
        if (r != null) {
            $('.en_menu .menu_2').css('font-weight', '600');
            $('.en_menu .menu_2 >a').css('color', '#0171BA');
            $('.ch_menu .menu_2').css('font-weight', '600');
            $('.ch_menu .menu_2 >a').css('color', '#0171BA');
            return;
        }
        var r = str.match(reg2);
        if (r != null) {
            $('.en_menu .menu_3').css('font-weight', '600');
            $('.en_menu .menu_3 >a').css('color', '#0171BA');
            $('.ch_menu .menu_3').css('font-weight', '600');
            $('.ch_menu .menu_3 >a').css('color', '#0171BA');
            return;
        }
        var r = str.match(reg3);
        if (r != null) {
            $('.en_menu .menu_5').css('font-weight', '600');
            $('.en_menu .menu_5 >a').css('color', '#0171BA');
            $('.ch_menu .menu_5').css('font-weight', '600');
            $('.ch_menu .menu_5 >a').css('color', '#0171BA');
            return;
        }
        var r = str.match(reg4);
        if (r != null) {
            $('#textSizeLink').css('color', '#0171BA');
        }

        var r5 = str.match(reg5);
        if (r != null || r5 != null) return;

        $('.en_menu .menu_1').css('font-weight', '600');
        $('.en_menu .menu_1 >a').css('color', '#0171BA');
        $('.ch_menu .menu_1').css('font-weight', '600');
        $('.ch_menu .menu_1 >a').css('color', '#0171BA');
    }

    function initRadioBtn() {
        $('#Profession .radioLabel').on('click', function (e) {
            $('#Profession .radioLabel').css('color', '#585858');
            $(this).css('color', '#0171BA');
            //HK Version commentted, but GZ version is not commentted.
            $(this).find('.chk').prop('checked', true);

            if ($(this).find('.chk').attr('eligible-scheme') != 'ALL') {
                $('.VSS_container').find('.chk').prop('checked', false);
                $('.VSS_container').find('.chk_container').css('color', '#585858');
                $('.VSS_container').find('.chk_container').addClass('disabled');
                $('.VSS_container').find('.chk').addClass('disabled');
                $('.VSS_container').find('.chk').attr('disabled', true);
                var countChecked = 0;
                $('.schemeBadge').empty();
                $('#Scheme').find('.chk').each(function () {
                    if ($(this).prop('checked')) {
                        countChecked++;
                    }
                });
                if (countChecked > 0) {
                    $('.schemeBadge').append(countChecked);
                } else {
                    $('.schemeBadge').empty();
                }
            } else {
                $('.VSS_container').find('.chk_container').removeClass('disabled');
                $('.VSS_container').find('.chk').removeClass('disabled');

                $('.VSS_container').find('.chk').attr('disabled', false);
            }
        })
    }

    function initTab() {
        $('.nav-pills > li > a').on('click', function () {
            if ($(this).parents('li').first().find('i').hasClass('active')) {
                $(this).find('i').removeClass('active');
                $(this).find('.badge').removeClass('active');
                $(this).find('.downArrow').addClass('none');
                $(this).find('.upArrow').removeClass('none');

            } else {
                $(this).find('i').addClass('active');
                $(this).find('.badge').addClass('active');
                $(this).find('.downArrow').removeClass('none');
                $(this).find('.upArrow').addClass('none');
                $(this).parents('li').siblings('li').find('i').removeClass('active');
                $(this).parents('li').siblings('li').find('.badge').removeClass('active');
                $(this).parents('li').siblings('li').find('.downArrow').addClass('none');
                $(this).parents('li').siblings('li').find('.upArrow').removeClass('none');
            }

            if ($(this).parent().attr('id') == 'professionTab') {
                if ($(this).find('i').hasClass('active')) {
                    //selectedTab
                    $('#selectedTab').val('1');
                }
            }
            if ($(this).parent().attr('id') == 'schemeTab') {
                if ($(this).find('i').hasClass('active')) {
                    //selectedTab
                    $('#selectedTab').val('2');
                }
            }
            if ($(this).parent().attr('id') == 'districtTab') {
                if ($(this).find('i').hasClass('active')) {
                    //selectedTab
                    $('#selectedTab').val('3');
                }
            }
            //console.log($('#selectedTab').val());
        });
    }

    function initInput() {
        $('input').focus(function () {
            $(this).prev('.SPSearch_icon').addClass('active')
        })
        $('input').blur(function () {
            $(this).prev('.SPSearch_icon').removeClass('active')
        })
    }

    function initCheckBox() {

        $(".chk").change(function (e) {
            var isChecked = $(this).prop('checked');
            if (isChecked) {
                if ($(this).parent('div').first().hasClass('radioLabel')) {
                    $('.radioLabel').css('color', '#585858');
                    //if ($(this).attr('id') != 'MP') {
                    if ($(this).attr('eligible-scheme') != 'ALL') {
                        $('.VSS_container').find('.chk').prop('checked', false);
                        $('.VSS_container').find('.chk_container').css('color', '#585858');
                        $('.VSS_container').find('.chk_container').addClass('disabled');
                        $('.VSS_container').find('.chk').addClass('disabled');
                        var countChecked = 0;
                        $('.schemeBadge').empty();
                        $('#Scheme').find('.chk').each(function () {
                            if ($(this).prop('checked')) {
                                countChecked++;
                            }
                        });
                        if (countChecked > 0) {
                            $('.schemeBadge').append(countChecked);
                        } else {
                            $('.schemeBadge').empty();
                        }
                    } else {
                        $('.VSS_container').find('.chk_container').removeClass('disabled');
                        $('.VSS_container').find('.chk').removeClass('disabled');
                    }
                }
                $(this).parent('.chk_container').css('color', ' #0171BA');
                $(this).parent('.chk_container').find('.chk').prop('checked', true);
            } else {
                $(this).parent('.chk_container').css('color', '#585858');
                $(this).parent('.chk_container').find('.chk').prop('checked', false);
            }
        })

        $('.chk').on('click', function (e) {
            e.stopPropagation();
            var isChecked = $(this).prop('checked');
            if ($(this).parents('.area').first().length > 0) {
                var areaCode = $(this).attr("id")
                if (isChecked) {
                    $('[area-code="' + areaCode + '"]').parents('ul').find('.chk_container').css('color', '#0171BA')
                    $('[area-code="' + areaCode + '"]').prop('checked', true);
                } else {
                    $('[area-code="' + areaCode + '"]').parents('ul').find('.chk_container').css('color', '#585858')
                    $('[area-code="' + areaCode + '"]').prop('checked', false);
                }
            } else if ($(this).parents('.district').first().length > 0) {
                var isNTArea = $(this).parents('ul').first().hasClass('NTArea');
                var areaCode = $(this).attr("area-code")
                if (isChecked) {
                    var countChecked = 0;
                    var chkAmount = $('[area-code="' + areaCode + '"]').length;
                    $('[area-code="' + areaCode + '"]').each(function (e) {
                        if ($(this).prop('checked')) {
                            countChecked++;
                        }
                    });
                    if (chkAmount === countChecked) {
                        $('[area-code="' + areaCode + '"]').parents('ul').find('.chk_container').css('color', '#0171BA')
                        $('[area-code="' + areaCode + '"]').parents('ul').find('.area').find('.chk').prop('checked', true);
                    }
                } else {
                    $('[area-code="' + areaCode + '"]').parents('ul').find('.chk_container').css('color', '#585858')
                    $('[area-code="' + areaCode + '"]').parents('ul').find('.area').find('.chk').prop('checked', false);
                }
            }
            if ($(this).parents('#Profession').first().length > 0) {
                var countChecked = 0;
                $('.professionBadge').empty();
                $('.professionBadge').append('1');
            } else if ($(this).parents('#Scheme').first().length > 0) {
                var countChecked = 0;
                $('.schemeBadge').empty();
                $(this).parents('#Scheme').first().find('.chk').each(function () {
                    if ($(this).prop('checked')) {
                        countChecked++;
                    }
                });
                if (countChecked > 0) {
                    $('.schemeBadge').append(countChecked);
                } else {
                    $('.schemeBadge').empty();
                }
            } else if ($(this).parents('#District').first().length > 0) {
                var countChecked = 0;
                $('.districtBadge').empty();
                $(this).parents('#District').first().find('.district').find('.chk').each(function () {
                    if ($(this).prop('checked')) {
                        countChecked++;
                    }
                });
                if (countChecked > 0) {
                    $('.districtBadge').append(countChecked);
                } else {
                    $('.districtBadge').empty();
                }
            }

            var strSelectedItem = '';
            if ($(this).parents('#Profession').length > 0 || $(this).parents('#Scheme').length > 0) {
                $('#Scheme').find('.chk').each(function () {
                    if ($(this).prop('checked')) {
                        strSelectedItem += $(this).attr('name') + ',';
                    }
                });
                if (strSelectedItem.length > 0) {
                    strSelectedItem = strSelectedItem.substring(0, strSelectedItem.length - 1);
                }
            } else if ($(this).parents('#District').length > 0) {
                $('#District .district').find('.chk').each(function () {
                    if ($(this).prop('checked')) {
                        strSelectedItem += $(this).attr('name') + ',';
                    }
                });
                if (strSelectedItem.length > 0) {
                    strSelectedItem = strSelectedItem.substring(0, strSelectedItem.length - 1);
                }
            }
        })

        $('.chk_container').on('click', function (e) {
            if ($(this).hasClass('disabled')) {
                return false;
            }
            $(this).find('.chk').first().click();
        });
    }

    function initNavPills() {
        $('#optSelect > li > a').on('click', function (e) {
            var curId = $(this).attr('href').substring(1, $(this).attr('href').length);
            var isActive = $(this).parent().attr('class') == 'active';
            if (isActive) {
                e.stopPropagation();
                $(this).parent().removeClass('active');
                $(this).attr('aria-expanded', 'false')
                $('#' + curId).removeClass('active');
                $('#' + curId).removeClass('in');
            }
        });
    }

    function removeLangMenu() {
        var showMainNav = $('.main_nav').css('display');
        if (showMainNav != 'none') {
            $('.menu').removeClass('active');
            menuActive = false;
            $('.langmenu').removeClass('active');
            langMenuActive = false;
            $('.btnclose').css('display', 'none');
            $('.hamburger').removeClass('unactive');
            $('.global').removeClass('unactive');
            $(document.body).css('overflow-y', 'scroll');
        }
    }

    function platform() {
        var ua = navigator.userAgent.toLocaleLowerCase();
        var pf = navigator.platform.toLocaleLowerCase();
        var isAndroid = (/android/i).test(ua) || ((/iPhone|iPod|iPad/i).test(ua) && (/linux/i).test(pf))
            || (/ucweb.*linux/i.test(ua));
        var isIOS = (/iPhone|iPod|iPad/i).test(ua) && !isAndroid;
        var isWinPhone = (/Windows Phone|ZuneWP7/i).test(ua);

        var mobileType = {
            pc: !isAndroid && !isIOS && !isWinPhone,
            ios: isIOS,
            android: isAndroid,
            winPhone: isWinPhone
        };
        return mobileType;
    }

    function scrollToElemTop(elem) {
        var height = $('.header_content').css('height');
        if (height == '90px') {
            height = 90;
        }
        else {
            height = 130;
        }
        $('html, body').animate({
            'scrollTop': elem.offset().top - height
        }, 500);
    }

    function openMenu(obj) {
        var fs = $(obj);
        fs.addClass('active');
        $('.btnclose').css('display', 'block');
        $('.hamburger').addClass('unactive');
        $('.global').addClass('unactive');
        $(document.body).css('overflow-y', 'hidden');
        menuActive = true;
    }
    /* 

	2. Set Header

	*/



    /* 

	3. Init Menu

	*/

    function initMenu() {
        if ($('.hamburger').length) {
            var hamb = $('.hamburger');
            hamb.on('click', function (event) {
                event.stopPropagation();

                if (langMenuActive) {
                    $('.langmenu').removeClass('active');
                    langMenuActive = false;
                }

                if (!menuActive) {
                    openMenu('.menu');
                }
                else {
                    $('.menu').removeClass('active');
                    menuActive = false;
                }

            });
        }
    }


    /*
    
    4. Init LangMenu
    
    */
    function initLangMenu() {
        if ($('.global').length) {
            var global = $('.global');

            global.on('click', function (event) {
                event.stopPropagation();

                if (menuActive) {
                    $('.menu').removeClass('active');
                    menuActive = false;
                }

                if (!langMenuActive) {
                    openMenu('.langmenu');
                }
                else {
                    $('.langmenu').removeClass('active');
                    langMenuActive = false;
                }
            });
        }
    }

    /*

    5. Init Close

    */
    function initClose() {
        if ($('.btnclose').length) {
            var btnclose = $('.btnclose');
            btnclose.on('click', function (event) {
                event.stopPropagation();
                if (menuActive == true || langMenuActive == true) {
                    $('.menu').removeClass('active');
                    $('.hamburger').removeClass('unactive');
                    menuActive = false;
                    $('.langmenu').removeClass('active');
                    $('.global').removeClass('unactive');
                    langMenuActive = false;
                    btnclose.css('display', 'none');
                    $(document.body).css('overflow-y', 'scroll');
                }
            });
        };
    }


    /*
    
    6. Set Section Height
    
    */

    function setSectionHeight() {
        var mobileType = platform();
        if (mobileType.pc == true) {

        }
        resetSectionHeight();
    }


    /*
   
   8. Init Skip to content
   
   */
    function initSkipToContent() {
        var $skipLink = $('.skip_link');
        $skipLink.click(function (e) {
            e.preventDefault();
            var thisTarId = $(this).attr('href');
            var thisTarget = $(thisTarId);
            scrollToElemTop(thisTarget);
            $('#skiptarget').attr('tabindex', '0');
            $('#skiptarget').focus();
        });
    }

    /*

    10. Init Back To Top

    */
    function initBackToTop() {
        var $backToTop = $('.backtotop');
        if ($backToTop.length < 1) {
            return false;
        }
        $backToTop.click(function (e) {
            e.preventDefault();
            $('#logoID').focus();
            $('html, body').animate({
                'scrollTop': 0
            }, 1000, function () {
            });
        });
    }
});

function initSortTab() {
    $('#ulSort > .nav-pills > li > a').off('click').on('click', function (e) {
        if ($(this).parents('li').first().hasClass('active')) {
            $(this).find('.downArrow').addClass('none');
            $(this).find('.upArrow').removeClass('none');
        }
        else {
            $(this).find('.downArrow').removeClass('none');
            $(this).find('.upArrow').addClass('none');
            $(this).parents('li').siblings('li').find('.downArrow').addClass('none');
            $(this).parents('li').siblings('li').find('.upArrow').removeClass('none');
        }

        var curId = $(this).attr('href').substring(1, $(this).attr('href').length);
        var isActive = $(this).parents('li').first().hasClass('active');
        if (isActive) {
            e.stopPropagation();
            $(this).parent().removeClass('active');
            $(this).attr('aria-expanded', 'false')
            $('#' + curId).removeClass('active');
            $('#' + curId).removeClass('in');
        }
        $('#scrollToResult').click();
    });
    $('#ulSort .radioLabel').off('click').on('click', function (e) {
        var orderRadio = $(this).find('.chk');
        var name = orderRadio.attr("name");
        $("input[name='" + name + "']").each(function () {
            $(this).parent().css("color", "#585858");
        })
        orderRadio.parent().css('color', '#0171BA');
        orderRadio.prop('checked', true);
    })
}

function OnClickFAQ() {
    if (currentLanguage == cultureLanguageEnglish) {
        document.getElementById('enFAQ').click();
    } else {
        document.getElementById('zhFAQ').click();
    }
}

function OnClickGuide() {
    if (currentLanguage == cultureLanguageEnglish) {
        document.getElementById('enGuide').click();
    } else {
        document.getElementById('zhGuide').click();
    }
}

function OnClickBack() {
    if (currentLanguage == cultureLanguageEnglish) {
        document.getElementById('enBack').click();
    } else {
        document.getElementById('zhBack').click();
    }
}

/*11.Init box height of Search page*/
function initSPSearchBox() {
    var startWindowWidth = $(window).width();
    var isMobile = startWindowWidth < 752;
    if (!isMobile) {
        $("#liIconButton").css("position", "absolute");
        $(".scr_search_lbox").css("height", "auto");
        $(".scr_search_rbox").css("height", "auto");
        var rslboxH = $(".scr_search_lbox").height();
        var rsrboxH = $(".scr_search_rbox").height();

        if (rslboxH > rsrboxH) {
            $(".scr_search_rbox").css("height", rslboxH + 20);
        }
        if (rsrboxH > rslboxH) {
            $(".scr_search_lbox").css("height", rsrboxH + 20);
        }
    }
    else {
        $("#liIconButton").css("position", "relative");
        $(".scr_search_lbox").css("height", "auto");
        $(".scr_search_rbox").css("height", "auto");
    }
}
