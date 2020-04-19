//Sample
//function initMoveTable() {
//    if ($('.previous').length) {
//        var ele = $('.previous');
//        ele.on('click', function (event) {
//        });
//    }
//}
;
var firstGetResult = true;

$(document).ready(function () {
    $(':text').on('focus', function (event) {
        $(this).attr('placeholder_backup', $(this).attr('placeholder'));
        $(this).attr('placeholder', '');
    });
    $(':text').on('blur', function (event) {
        if ($(this).text() == '') {
            $(this).attr('placeholder', $(this).attr('placeholder_backup'));
        }
    });
    $('.HCVSCheckbox').bind("click", function (e) {
        $("#EHCVS").click();
    })
});
$(window).on('resize', function () {
    var scrollLeft = $('#divResult').scrollLeft();
    $('#divResult').scrollLeft(scrollLeft - 30);

    var width = $("#divResult").width();
    var left = $("#divResult").scrollLeft();
    var trs = $("#divResult>table>tbody>tr").not(".mobile-clinic");
    trs.each(function (i) {
        for (var i = 0; i < 2; i++) {
            $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
        }
    });

    if (hasVSS) {
        $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0, "width": width - 100 });
    } else {
        $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
    }
    
    hideResultLRButton();
});

function initClearButton() {
    if ($('#btnClearProfession').length) {
        var ele = $('#btnClearProfession');
        ele.on('click', function (event) {
            $('#Profession').find('.chk').prop('checked', false);
            $('#Profession .chk_container').css('color', '#585858');
            if ($('.chk_container').hasClass('disabled')) {
                $('.chk_container').removeClass('disabled');
            };
            $('#Scheme .chk').attr('disabled', false);
            $('#Scheme .chk').removeClass('disabled');
            //professionBadge
            $('.professionBadge').empty();
        });
    }
    if ($('#btnClearScheme').length) {
        var ele = $('#btnClearScheme');
        ele.on('click', function (event) {
            $('#Scheme').find('.chk').prop('checked', false);
            $('#Scheme .chk_container').css('color', '#585858');
            $('.schemeBadge').empty();
        });
    }

    if ($('#btnClearDistrict').length) {
        var ele = $('#btnClearDistrict');
        ele.on('click', function (event) {
            $('#District').find('.chk').prop('checked', false);
            $('#District .chk_container').css('color', '#585858');
            $('.districtBadge').empty();
        });
    }
}

function initMoveTable() {
    if ($('.previous').length) {
        var ele = $('.previous');
        ele.on('click', function (event) {
            var scrollLeft = $('#divResult').scrollLeft();
            $('#divResult').scrollLeft(scrollLeft - 30);
            
            var left = $("#divResult").scrollLeft();
            var trs = $("#divResult>table>tbody>tr").not(".mobile-clinic");
            trs.each(function (i) {
                for (var i = 0; i < 2; i++) {
                    $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
                }
            });
            $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
            hideResultLRButton();
        });
    }

    if ($('.next').length) {
        var ele = $('.next');
        ele.on('click', function (event) {
            var scrollLeft = $('#divResult').scrollLeft();
            $('#divResult').scrollLeft(scrollLeft + 30);

            var left = $("#divResult").scrollLeft();
            var trs = $("#divResult>table>tbody>tr").not(".mobile-clinic");
            trs.each(function (i) {
                for (var i = 0; i < 2; i++) {
                    $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
                }
            });
            $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
            hideResultLRButton();
        });
    }
    hideResultLRButton();

}

//hide/show arrow button
function hideResultLRButton() {
    var pre = $('.previous');
    var next = $('.next');
    var divInnerWidth = $('#divResult').innerWidth();
    var spTableWidth = $(".SPTable").width()

    if (divInnerWidth >= spTableWidth) {
        pre.hide();
        next.hide();
    } else {
        if ($('#divResult').scrollLeft() == 0)
            pre.hide();
        else
            pre.show();

        if (spTableWidth - divInnerWidth <= $('#divResult').scrollLeft())
            next.hide();
        else
            next.show();
    }
}

function setOffSortButton() {
 $("span[data-column='SPName'],span[data-column='PracticeName'],span[data-column='DistrictName'],span[data-column='Profession'],span[data-column='JoinedScheme']").each(function () {
        $(this).off('click');
        $(this).css('color', '#bab9b9');
        $(this).parent().off('click');
    });
}

function setOffAllSortButton() {
    if ($('.sort-up').length) {
        var ele = $('.sort-up');
        ele.off('click');
    }
    if ($('.sort-down').length) {
        var ele = $('.sort-down');
        ele.off('click');
    }
    $(".SPTable tr").eq(2).find("td").off('click');
}

function initSortButton() {
    if ($('.sort-up').length) {
        var ele = $('.sort-up');
        ele.on('click', function (event) {
            //#0171BA
            //#bab9b9
            event.stopPropagation();
            if ($(this).css('color') == 'rgb(1, 113, 186)') {
                //un-highlighted
                $(this).css('color', '#bab9b9');
                //set to default value
                sortField = '';
                sortColName = '';
                sortType = 'asc';
            }
            else {
                //highlighted
                $(this).css('color', '#0171BA');
                //set value
                sortField = $(this).attr('data-column');
                sortColName = $(this).attr('colname');
                sortType = 'asc';
            }
            requestType = 'sort';
            $('#requestType').val(requestType);
            $('#sortField').val(sortField);
            $('#sortColName').val(sortColName);
            $('#sortType').val(sortType);
            console.log('initSortButton UP:' + sortColName);
            GetResult(false);
        });
    }
    if ($('.sort-down').length) {
        var ele = $('.sort-down');
        ele.on('click', function (event) {
            //#0171BA
            //#bab9b9
            event.stopPropagation();
            if ($(this).css('color') == 'rgb(1, 113, 186)') {
                $(this).css('color', '#bab9b9');
                //set to default value
                sortField = '';
                sortColName = '';
                sortType = 'asc';
            }
            else {
                $(this).css('color', '#0171BA');
                sortField = $(this).attr('data-column');
                sortColName = $(this).attr('colname');
                sortType = 'desc';
            }
            requestType = 'sort';
            $('#requestType').val(requestType);
            $('#sortField').val(sortField);
            $('#sortColName').val(sortColName);
            $('#sortType').val(sortType);
            console.log('initSortButton Down:' + sortColName);
            GetResult(false);
        });
    }
}

function initTableColOrder() {
    $(".SPTable tr").eq(2).find("td").off('click').on('click', function (event) {
        firstGetResult = false;
        var upele = $(this).find('.sort-up');
        var downele = $(this).find('.sort-down');
        if (downele.css('color') == 'rgb(1, 113, 186)') {
            upele.click();
        }
        else {
            downele.click();
        }
    });
}

//function setSortButtonStyle() {
//    console.log('setSortButtonStyle:' + $("#sortColName").val());
//    console.log('setSortButtonStyle:' + $("#sortType").val());
//    if (sortType == 'asc') {
//        $(".sort-up").each(function (index, domEle) {
//            if (sortColName == $(this).attr('colname')) {
//                $(this).css('color', '#0171BA');
//            }           
//        });
//    } else {
//        $(".sort-down").each(function (index, domEle) {
//            if (sortColName == $(this).attr('colname')) {
//                $(this).css('color', '#0171BA');
//            }
//        });
//    }
//}

function setSortButtonStyle() {
    console.log('setSortButtonStyle:' + $("#sortColName").val());
    console.log('setSortButtonStyle:' + $("#sortType").val());
    if ($("#sortType").val() == 'asc') {
        $(".sort-up").each(function (index, domEle) {
            if ($("#sortColName").val() == $(this).attr('colname')) {
                $(this).css('color', '#0171BA');
            }
        });
    } else {
        $(".sort-down").each(function (index, domEle) {
            if ($("#sortColName").val() == $(this).attr('colname')) {
                $(this).css('color', '#0171BA');
            }
        });
    }
}

function initClinicClick() {
    if ($('.left-cell').length) {
        var ele = $('.left-cell');
        ele.on('click', function (event) {
            var tr = $(this).find('img').first().attr("id") + '_clinic';
            if ($('#' + tr).css('display') == 'none') {
                $('#' + tr).css('display', 'table-row');
                $('#' + tr).css('background-color', '#FFFFF0');
                $(this).css('background-color', '#FFFFF0');
            }
            else {
                $('#' + tr).css('display', 'none');
                $(this).css('background-color', '#ffffff');
            }
        });
    }
}

function ShowResultStyle() {
    $('#point_to_note').css('display', 'block');
    $('#desktopResultContainer').css('display', 'block');
}

function HideResultStyle() {
    $('#point_to_note').css('display', 'none');
    $('#desktopResultContainer').css('display', 'none');
    $('.scrollToResult_container').addClass('hidden');
    $("#mobileLegend").hide();
}

function ResetRecordTotal(RecordTotal) {
    $('#RecordTotal').text(RecordTotal);

}

function ResetPaginator(PageIndex, PageTotal) {
    //var 
    var numofbuttons = 10;
    if (PageTotal <= numofbuttons) {
        numofbuttons = PageTotal;
    }
    $('.pagination').empty();

    if (PageTotal > numofbuttons)
        $('.pagination').append('<li class="paginatorLeft"><a href="javascript:void(0);" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>');
    if (numofbuttons == PageTotal) {
        curMaxPage = 0;
        for (i = 1; i <= numofbuttons; i++) {
            if (PageIndex == i) {
                $('.pagination').append('<li class="active"><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
            }
            else {
                $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
            }
        }
    } else { //PageTotal > numofbuttons
        curMaxPage = numofbuttons;
        var startPage = Math.floor((PageIndex - 1) / numofbuttons) * numofbuttons
        for (i = 1; i <= curMaxPage; i++) {
            if ((startPage + i) <= PageTotal) {
                if (PageIndex == (i + startPage)) {
                    $('.pagination').append('<li class="active"><a href="javascript:void(0);" class="pageNum">' + (startPage + i) + '</a></li>');
                }
                else {
                    $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + (startPage + i) + '</a></li>');
                }
            }
        }
        if (PageTotal > (startPage + curMaxPage)) {
            $('.pagination').append('<li class="nextPart paginatorRight"><a href="javascript:void(0);" >' + '...' + '</a></li>');
            $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + PageTotal + '</a></li>');
        }
        curMaxPage = startPage + numofbuttons;
    }
    if (PageTotal > numofbuttons)
        $('.pagination').append('<li class="paginatorRight"><a href="javascript:void(0);" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>');
}

function nextPaginator(PageIndex) {
    var numofbuttons = 10;
    if (curMaxPage != 0 && curMaxPage < pageTotal) {
        $('.pagination').empty();
        $('.pagination').append('<li class="paginatorLeft"><a href="javascript:void(0);" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>');
        if (parseInt(curMaxPage) + parseInt(numofbuttons) < parseInt(pageTotal)) {
            for (i = curMaxPage + 1; i <= parseInt(curMaxPage) + parseInt(numofbuttons) ; i++) {
                if (PageIndex == i) {
                    $('.pagination').append('<li class="active"><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
                }
                else {
                    $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
                }
            }
            $('.pagination').append('<li class="nextPart paginatorRight"><a href="javascript:void(0);">' + '...' + '</a></li>');
            $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + pageTotal + '</a></li>');

        }
        else {
            for (i = curMaxPage + 1; i <= pageTotal; i++) {
                if (PageIndex == i) {
                    $('.pagination').append('<li class="active"><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
                }
                else {
                    $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
                }
            }
        }
        curMaxPage = parseInt(curMaxPage) + parseInt(numofbuttons);
        $('.pagination').append('<li class="paginatorRight"><a href="javascript:void(0);" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>');
        initPageNumberClick();
    }
}

function previousPaginator(PageIndex) {
    var numofbuttons = 10;
    var maxPageNew = 0;
    if (curMaxPage != 0 && curMaxPage >= parseInt(numofbuttons)) { //not first page
        maxPageNew = curMaxPage - parseInt(numofbuttons);
        if (maxPageNew < numofbuttons) {
            maxPageNew = numofbuttons;
        }
        $('.pagination').empty();
        $('.pagination').append('<li class="paginatorLeft"><a href="javascript:void(0);" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>');
        for (i = maxPageNew - numofbuttons + 1; i <= maxPageNew; i++) {
            if (PageIndex == i) {
                $('.pagination').append('<li class="active"><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
            }
            else {
                $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + i + '</a></li>');
            }
        }
        $('.pagination').append('<li class="nextPart paginatorRight"><a href="javascript:void(0);">' + '...' + '</a></li>');
        $('.pagination').append('<li><a href="javascript:void(0);" class="pageNum">' + pageTotal + '</a></li>');
        curMaxPage = maxPageNew;
        $('.pagination').append('<li class="paginatorRight"><a href="javascript:void(0);" aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>');
        initPageNumberClick();
    }
}


function initPageNumberClick() {
    if ($('.pagination>li>a.pageNum').length) {
        var ele = $('.pagination>li>a.pageNum');
        ele.off('click').on('click', function (event) {
            $(this).parent().addClass('active');
            pageIndex = parseInt($(this).text());
            requestType = 'pageIndex';
            $(".pagination>li").each(function (index, domEle) {
                // domEle == this 
                $(domEle).removeClass('active');
            });
            $(this).parent().addClass('active');
            $('#requestType').val(requestType);
            $('#pageIndex').val(pageIndex);
            $('#pageActualSize').val($('#pageSize').val());
            GetResult(false);
        });
    }
    if ($('.pagination>li.paginatorRight').length) {
        var ele = $('.pagination>li.paginatorRight');
        ele.off('click').on('click', function (event) {
            nextPaginator(pageIndex);
        });
    }
    if ($('.pagination>li.paginatorLeft').length) {
        var ele = $('.pagination>li.paginatorLeft');
        ele.off('click').on('click', function (event) {
            previousPaginator(pageIndex);
        });
    }
}

function copyToHidden(obj) {
    var id = $(obj).attr('id');
    $('#hidden' + id).val($(obj).val());
}

function freezeCol() {
    var box = $('#divResult');
    var startX = 0;
    box.scrollLeft(0);
    hideResultLRButton();
    $('#divResult').on('touchstart', function (e) {
        if (hasVSS) {
            var touch = e.originalEvent.targetTouches[0];
            startX = touch.pageX;
        }

    });
    var lasttop = $(window).scrollTop();
    var topdowncount = 0;
    $('#divResult').on('touchmove', function (e) {
        var touch = e.originalEvent.targetTouches[0];
        var n = $(window).scrollTop();
        if (n != lasttop) {
            lasttop = $(window).scrollTop();
            topdowncount = 0;
            return;
        }
        topdowncount++;
        if (topdowncount < 8) {
            startX = touch.pageX;
            hideResultLRButton();
            return;
        };

        var endX = touch.pageX - startX;
        var left = $("#divResult").scrollLeft();
        var scrollWidth = left + $("#divResult").width()
        if (hasVSS) {
            if (left >= 0) {
                var trs = $("#divResult>table>tbody>tr").not(".mobile-clinic");
                trs.each(function (i) {
                    for (var i = 0; i < 2; i++) {
                        if (endX < 0) {
                            // move left
                            if (scrollWidth > 1030) {
                                $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
                            } else {
                                if (left < Math.abs(endX)) {
                                    $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasVSS ? 0 - endX : 0 });
                                }
                                else {
                                    $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasVSS ? left - endX : 0 });
                                }
                            }
                        }

                        if (endX >= 0) { //move right.
                            if (left > endX) {
                                $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasVSS ? left - endX : 0 });
                            } else {
                                $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": 0 });
                            }
                        }
                    }
                });

                if (endX < 0) {
                    // move left
                    if (scrollWidth > 1030) {
                        $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? left : 0 });
                    } else {
                        if (left < Math.abs(endX)) {
                            $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? 0 - endX : 0 });
                        }
                        else {
                            $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? left - endX : 0 });
                        }
                    }
                }

                if (endX >= 0) { //move right.
                    if (left > endX) {
                        $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasVSS ? left - endX : 0 });
                    } else {
                        $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": 0 });
                    }

                }

            }
            currentScroll = box.scrollLeft();
            if (endX < 0) {
                box.scrollLeft(left + Math.abs(endX));
            }
            else {
                box.scrollLeft(left - Math.abs(endX));
            }

            startX = touch.pageX
            hideResultLRButton();
        }
    });
    InitFreezeDivWidth();
}

function initColScroll(hasvss) {
    var left = $("#divResult").scrollLeft();
    var trs = $("#divResult>table>tbody>tr").not(".mobile-clinic");
    trs.each(function (i) {
        for (var i = 0; i < 2; i++) {
            $(this).children().eq(i).css({ "position": "relative", "z-index": 10, "left": hasvss ? left : 0 });
        }
    });
    $(".freezeDiv").css({ "position": "relative", "z-index": 10, "left": hasvss ? left : 0 });
    hideResultLRButton();
}

function InitFreezeDivWidth() {
    var divInnerWidth = $('#divResult').innerWidth();

    if (hasVSS) {
        $(".freezeDiv").css({ "width": divInnerWidth - 100});
    }
    //$(".freezeDiv").css({ "width": divInnerWidth - 100 });
}