/* JS Document */

/******************************

[Table of Contents]



******************************/
var helpBarActive = false;
var errMsg = "";
var mode = 1;
var onFocusField = "";
var errMsgList = [];

function findMsg(lang, code) {
    // iterate over each element in the array
    for (var i = 0; i < errMsgList.length; i++) {
        // look for the entry with a matching `code` value
        if (errMsgList[i].code == code && errMsgList[i].lang == lang) {
            // we found it
            // obj[i].name is the matched result
            return errMsgList[i].desc;
        }
    }
}

function findMsg(code) {
    if (!errMsgList || errMsgList.length == 0) {
        errMsgList = JSON.parse(sessionStorage.getItem("errMsg"))
    }
    // iterate over each element in the array
    for (var i = 0; i < errMsgList.length; i++) {
        // look for the entry with a matching `code` value
        if (errMsgList[i].code == code) {
            // we found it
            // obj[i].name is the matched result
            return errMsgList[i].desc;
        }
    }
    return "Undefined"
}

$(document).ready(function () {
    /* 

	1. Vars and Inits

	*/
    init();


    $('#btnTypCE').on('click', function (event) {
        event.stopPropagation();
        $('#Dob_label').css('display', 'none');
        $('#Yob_label').css('display', 'block');
        $('#Dob_input').css('display', 'none');
        $('#Yob_input').css('display', 'block');
        $('#dob-title').css('display', 'none');
        $('#yob-title').css('display', 'block');
        mode = 2;
        $('#inputType').val('CE');
        $('.help-img').css('display', 'none');
        $('.help-text-tips').css('display', 'block');
        if ($('.rightSection').css('display') == 'none') {

            $('.help-text-tips-yob').css('display', 'block');
            if ($('#selType').val() == 'DOB') {
                $('.help-text-dob-xs').css('display', 'block');
                $('.help-text-yob-xs').css('display', 'none');
            }
            else {
                $('.help-text-dob-xs').css('display', 'none');
                $('.help-text-yob-xs').css('display', 'block');
            }
        }
    });

    $('#btnTypID').on('click', function (event) {
        event.stopPropagation();
        $('#Dob_label').css('display', 'block');
        $('#Yob_label').css('display', 'none');
        $('#Dob_input').css('display', 'block');
        $('#Yob_input').css('display', 'none');
        $('#dob-title').css('display', 'block');
        $('#yob-title').css('display', 'none');
        $('#inputType').val('IC');
        $('.help-img').css('display', 'block');
        $('.help-text-dob-xs').css('display', 'block');
        $('.help-text-yob-xs').css('display', 'none');
        mode = 1;
        
        $('#headlineForIC').css('display', 'block');
        $('#headlineForCE').css('display', 'none');
    });

    $('#btnSubmit').on('click', function (event) {
        //Input checking
        if (validate()) {
            var str = $('#txtHKIC').val();
            str = str.toUpperCase();
            $('#txtHKIC').val(str);
            return true;
        }
        else {
            $('#txtErrMsg').html(errMsg);
            $('.alert').css('display', 'block');
            scrollToElem($('.alert'));
            return false;
        }
    });



    function test() {
        var s = 's12345';
        alert(s.charCodeAt(1));
    }
    function refreshCaptcha() {
        $.ajax({
            async: false,
            type: 'GET',
            url: applicationPath + '/en/Error/CheckTimeout',
            complete: function (XMLHttpRequest, textStatus) {
                var errorUrl = XMLHttpRequest.getResponseHeader("ErrorUrl");
                if (errorUrl) {
                    var u = rootPath + rootLang + errorUrl;
                    location.href = u;
                } else {
                    $('.valiCodeImg').attr('src', applicationPath + '/en/VBE/GetValidateCode?query=' + Math.random());
                }
            }
        });

    }

    function verifyCaptcha() {
        var rtnValue;
        var date = new Date();
        $.ajax({
            async: false,
            type: 'POST',
            url: applicationPath + "/en/VBE/VerifyCaptcha",
            dataType: 'json',
            data: JSON.stringify({
                code: $('#txtCaptcha').val().toUpperCase(),
                submitTime: date
            }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.Rtn == 0) {
                    rtnValue = data.Rtn;
                }
                else {
                    rtnValue = data.Rtn;
                    refreshCaptcha();
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
                var errorUrl = XMLHttpRequest.getResponseHeader("ErrorUrl");
                if (errorUrl) {
                    var u = rootPath + rootLang + errorUrl;
                    location.href = u;
                }
            }
        });
        return rtnValue;
    }

    function validate() {
        errMsg = "";
        var rtn = true;
        initAlertSign();
        if (!valHKID()) {
            rtn = false;
        }
        if (mode == 1) {
            if (!valDateofBirth($('#txtDob').val())) {
                rtn = false;
            }
        }
        if (mode == 2) {

            if ($('#selType').val() == 'DOB') {
                if (!valDateofBirth($('#txtDob_CE').val())) {
                    rtn = false;
                }
            }
            if ($('#selType').val() == 'YOB') {
                if (!valAge(true)) {
                    rtn = false;
                }
                if (!valDateofReg()) {
                    rtn = false;
                }
            }

        }
        if (!valCaptcha()) {
            rtn = false;
            $('#txtCaptcha').val('');
        }
        if (!rtn) {
            refreshCaptcha();
        }
        return rtn;
    }

    function isValidDate(date) {
        return date instanceof Date && !isNaN(date.getTime())
    }

    function valDateofReg() {
        var msg;
        var useLang = $('#useLang').attr('data-uselang');
        var errCode;

        var strYear = $('#txtYear').val();
        var strMonth = $('#selMonth').val();
        var strDay = $('#txtDay').val();
        var rtn = true;
        var currentDate = new Date();
        var currentYear = currentDate.getFullYear();

        if (strYear.length == 0 || strDay.length == 0) {
            errCode = '990000-E-00072';
            //msg = findMsg(useLang, errCode);
            msg = findMsg(errCode)
            errMsg += msg + '[' + errCode + ']<br>';
            SetAlertSign('990000-E-00072');
            return false;
        }
        else {
            //Check for Year
            if (!isNaN(parseInt(strYear))) {
                //Year must larger than 1753
                var intYear = parseInt(strYear);
                if (intYear < 1753 || intYear > currentYear) {
                    rtn = false;
                }
            }
            //Check for Day
            if (!isNaN(parseInt(strDay))) {
                var intDay = parseInt(strDay);
                if (intDay < 1 || intDay > 31) {
                    rtn = false;
                }
            }

            var myDate = new Date()
            var currentDate = new Date()
            if (!isdate(intYear, parseInt(strMonth), intDay)) {
                rtn = false;
            }
            if (myDate > currentDate) {
                //bigger than current date
                rtn = false;
            }

            if (!rtn) {
                errCode = '990000-E-00073';
                //msg = findMsg(useLang, errCode);
                msg = findMsg(errCode)
                errMsg += msg + '[' + errCode + ']<br>';
                SetAlertSign('990000-E-00073');
                return rtn;
            }
        }
        return rtn;
    }

    function valAge(isShowMsg) {
        var msg;
        var useLang = $('#useLang').attr('data-uselang');
        var errCode;
        var str = $('#txtAge').val();
        if (str.length == 0) {
            errCode = '990000-E-00074';
            if (isShowMsg) {
                //msg = findMsg(useLang, errCode);
                msg = findMsg(errCode)
                errMsg += msg + '[' + errCode + ']<br>';
            }
            SetAlertSign(errCode);
            return false;
        }
        if (str.length > 3) {
            errCode = '990000-E-00074';
            if (isShowMsg) {
                //msg = findMsg(useLang, errCode);
                msg = findMsg(errCode)
                errMsg += msg + '[' + errCode + ']<br>';
            }
            SetAlertSign(errCode);
            return false;
        }
        return true;
    }

    function valCaptcha() {
        var msg;
        var useLang = $('#useLang').attr('data-uselang');
        var errCode;
        var str = $('#txtCaptcha').val();
        if (str.length == 0) {
            errCode = '990000-E-00027';
            //msg = findMsg(useLang, errCode);
            msg = findMsg(errCode)
            errMsg += msg + '[' + errCode + ']<br>';
            SetAlertSign('990000-E-00027');
            return false;
        }
        else {
            //Call captcha checking api
            var rtn = verifyCaptcha()

            if (rtn == 1) {
                //Input is not correct, return value = 1
                errCode = '990000-E-00033';
                //msg = findMsg(useLang, errCode);
                msg = findMsg(errCode)
                errMsg += msg + '[' + errCode + ']<br>';
                SetAlertSign('990000-E-00033');
            }
            if (rtn == 2) { //The code is expired, return value = 2
                errCode = '990000-E-00031';
                //msg = findMsg(useLang, errCode);
                msg = findMsg(errCode)
                errMsg += msg + '[' + errCode + ']<br>';
                SetAlertSign('990000-E-00031');
            }

            if (rtn == 3) { //The code is expired, return value = 2
                errCode = '990000-E-00032';
                //msg = findMsg(useLang, errCode);
                msg = findMsg(errCode)
                errMsg += msg + '[' + errCode + ']<br>';
                SetAlertSign('990000-E-00032');
            }

            if (rtn != 0) {
                return false;
            }
        }
        return true;
    }

    function matchMMYYYY(strdate) {
        reg = /^(\d{2})(-|\/)(\d{4})$/;
        r = strdate.match(reg);
        if (r == null) {
            reg = /^(\d{2})(\d{4})$/;
            r = strdate.match(reg);
            if (r == null) {
                return false;
            }
        }
        return true;
    }

    //This function is to match format DD-MM-YYYY and MM-YYYY
    function matchDDMMYYYY(strdate) {
        var reg = /^(\d{2})(-|\/)(\d{2})(-|\/)(\d{4})$/;
        var r = strdate.match(reg);
        if (r == null) {
            reg = /^(\d{2})(\d{2})(\d{4})$/;
            r = strdate.match(reg);
            if (r == null) {
                return false;
            }
        }
        return true;
    }

    function valDateofBirth(str) {
        var msg;
        var useLang = $('#useLang').attr('data-uselang');
        var errCode;
        var strYear;
        var intMonth;
        var intDay;
        var matchResult;
        var currentDate = new Date();
        var currentYear = currentDate.getFullYear();
        if (str.length != 0) {
            matchResult = matchDDMMYYYY(str);
            if (!matchResult) {
                //Check Format MM-YYYY
                matchResult = matchMMYYYY(str);
                if (!matchResult) {
                    reg = /^(\d{4})$/;
                    r = str.match(reg);
                    if (r == null) {
                        errCode = '990000-E-00004';
                        msg = findMsg(errCode)
                        errMsg += msg + '[' + errCode + ']<br>';
                        SetAlertSign('990000-E-00004');
                        return false;
                    }
                    else {
                        strYear = parseInt(str);
                        if (strYear < 1753 || strYear > currentYear) {
                            errCode = '990000-E-00004';
                            msg = findMsg(errCode)
                            errMsg += msg + '[' + errCode + ']<br>';
                            SetAlertSign('990000-E-00004');
                            return false;
                        }
                    }
                }
                else {
                    //The format is by MM-YYYY or MMYYYY
                    if (str.length == 7) {
                        strYear = parseInt(str.substr(3, 4));
                        intMonth = parseInt(str.substr(0, 2));
                    } else {
                        if (str.length == 6) {
                            strYear = parseInt(str.substr(2, 4));
                            intMonth = parseInt(str.substr(0, 2));
                        } else {
                            return false;
                        }
                    }
                    if (strYear < 1753) {
                        errCode = '990000-E-00004';
                        msg = findMsg(errCode)
                        errMsg += msg + '[' + errCode + ']<br>';
                        SetAlertSign('990000-E-00004');
                        return false;
                    }
                    if (intMonth > 12 || intMonth < 1) {
                        errCode = '990000-E-00004';
                        
                        msg = findMsg(errCode)
                        errMsg += msg + '[' + errCode + ']<br>';
                        SetAlertSign('990000-E-00004');
                        return false;
                    }
                }

            }
            else {
                if (str.length == 10) {
                    strYear = parseInt(str.substr(6, 4));
                    intMonth = parseInt(str.substr(3, 2));
                    intDay = parseInt(str.substr(0, 2));
                } else {
                    if (str.length == 8) {
                        strYear = parseInt(str.substr(4, 4));
                        intMonth = parseInt(str.substr(2, 2));
                        intDay = parseInt(str.substr(0, 2));
                    } else {
                        return false;
                    }
                }

                if (strYear < 1753 || strYear > currentYear) {
                    errCode = '990000-E-00004';
                    msg = findMsg(errCode)
                    errMsg += msg + '[' + errCode + ']<br>';
                    SetAlertSign('990000-E-00004');
                    return false;
                }
                if (!isdate(strYear, intMonth, intDay)) {
                    errCode = '990000-E-00004';
                    msg = findMsg(errCode)
                    errMsg += msg + '[' + errCode + ']<br>';
                    SetAlertSign('990000-E-00004');
                    return false;
                }

            }
        }
        else {
            errCode = '990000-E-00003';
            msg = findMsg(errCode)
            errMsg += msg + '[' + errCode + ']<br>';
            SetAlertSign('990000-E-00003');
            return false;
        }
        return true;
    }
    
    function formatHKID() {
        var str = $.trim($('#txtHKIC').val());
        str = str.replace(/\(+/g, '');
        str = str.replace(/\)+/g, '');
        str = str.replace(/\s+/g, '');
        str = str.toUpperCase();
        if (str.length > 0) {
            var lastChar = str.substr(str.length - 1, 1);
            $('#txtHKIC').val(str.substr(0,str.length-1) + '(' +  lastChar + ')');
        }
    }

    function formatDateOfBirth(dateofbirth) {
        var tmpdateofbirth = dateofbirth;
        tmpdateofbirth = tmpdateofbirth.replace(/\-+/g, '');
        if (tmpdateofbirth.length <= 6 && tmpdateofbirth.length > 4) {
            dateofbirth = tmpdateofbirth.substr(0, 2) + '-' + tmpdateofbirth.substr(2, tmpdateofbirth.length - 2)
        }
        if (tmpdateofbirth.length >6) {
            dateofbirth = tmpdateofbirth.substr(0, 2) + '-' + tmpdateofbirth.substr(2, 2) + '-' + tmpdateofbirth.substr(4, tmpdateofbirth.length - 4)
        }
        return dateofbirth;
    }

    function valHKID() {
        var str = $.trim($('#txtHKIC').val());
        str = str.toUpperCase();
        var msg;
        var useLang = $('#useLang').attr('data-uselang');
        var errCode;
        if (str.length == 0) {
            //990000-E-00001
            errCode = '990000-E-00001';
            msg = findMsg(errCode);
            errMsg += msg + '[' + errCode + ']<br>';
            SetAlertSign('990000-E-00001')
            return false;
        }
        else {
            var reg = /^([a-zA-Z]{1,2})(\d{6})(\d{1}|A|a)$/;
            var r = str.match(reg);
            if (r == null) {
                reg = /^([a-zA-Z]{1,2})(\d{6})\((\d{1}|A|a)\)$/;
                r = str.match(reg);
                if (r == null) {
                    errCode = '990000-E-00002';
                    msg = findMsg(errCode);
                    errMsg += msg + '[' + errCode + ']<br>';
                    SetAlertSign('990000-E-00002')
                    return false;
                }
            }
            else {
                //format with bracket
                var lastChar = str.substr(str.length-1,1);
                $('#txtHKIC').val(str.substr(0, str.length - 1) + '(' + lastChar  +')' );
            }
            //Validate check bit
            if (!verifyHKIDCheckBit(str)) {
                errCode = '990000-E-00002';
                msg = findMsg(errCode);
                errMsg += msg + '[' + errCode + ']<br>';
                SetAlertSign('990000-E-00002')
                return false;
            }
        }
        return true;
    }

    function verifyHKIDCheckBit(str) {
        var ids = new Array(8);
        var checkbit;
        var c;
        //1. replace ()
        var str1 = str.replace(/\(/g, "");
        str1 = str1.replace(/\)/g, "");
        var isA = false;

        checkbit = str1.charCodeAt(str1.length - 1);
        //if the last char is 0~9
        if (checkbit >= 48 && checkbit <= 57) {
            checkbit = checkbit - 48;
        }
        else if (checkbit == 97 || checkbit == 65) {
            //if the last char is a or A
            isA = true;
        }
        else {
            // the last character is not number and 'a'
            return false;
        }

        //2. change to array
        if (str1.length == 8) {
            ids[0] = 36;
            for (i = 0; i < 7; i++) {
                c = str1.charCodeAt(i);
                if (c >= 48 && c <= 57) {
                    c = c - 48;
                    ids[i + 1] = c;
                }

                if (c >= 65 && c <= 90) {
                    c = c - 55;
                    ids[i + 1] = c;
                }
            }
        }

        if (str1.length == 9) {
            //ids[0] = 36;
            for (i = 0; i < 8; i++) {
                c = str1.charCodeAt(i);
                if (c >= 48 && c <= 57) {
                    c = c - 48;
                    ids[i] = c;
                }

                if (c >= 65 && c <= 90) {
                    c = c - 55;
                    ids[i] = c;
                }
            }
        }

        //3. compute the check bit
        var total = 0;
        var factor = 9;
        for (i = 0; i < 8; i++) {
            total += ids[i] * (factor - i);
        }
        //4. return 
        var remain = total % 11;
        var checkbit2 = 11 - remain;
        if (remain == 0) {
            if (checkbit == 0)
                return true;
        } else if (remain == 1) {
            if (isA)
                return true;
        }
        else if (remain >= 0 && remain <= 10) {
            if (checkbit == checkbit2) {
                return true;
            }
        }

        return false;
    }

    function openHelpBar(obj) {
        var fs = $(obj);
        fs.addClass('active');
        $(document.body).css('overflow-y', 'hidden');
    }

    function init() {
        if ($('.helpBar-Open').length) {
            var helpBarOpen = $('.helpBar-Open');
            helpBarOpen.on('click', function (event) {
                event.stopPropagation();
                if (onFocusField == "txtHKIC_blur" || onFocusField == "txtDob_blur") {

                }
                else {
                    $('.help-mobile-img').attr('src', applicationPath + '/Image/vbe/web/help-1-' + currentLanguage + '.png');
                }
                if (!helpBarActive) {
                    $('.helpBar').addClass('active');
                    helpBarActive = true;
                }
                else {
                    $('.helpBar').removeClass('active');
                    helpBarActive = false;
                }

            });
        }

        if ($('.helpBar-Close').length) {
            var helpBarClose = $('.helpBar-Close');
            helpBarClose.on('click', function (event) {
                event.stopPropagation();

                if (helpBarActive) {
                    $('.helpBar').removeClass('active');
                    helpBarActive = false;
                }

            });
        }

        if ($('#txtHKIC').length) {
            var obj = $('#txtHKIC');
            obj.on('focus', function (event) {
                //event.stopPropagation();
                if ($(".rightSection").css('display') == 'none') {
                    //Mobile
                    onFocusField = "txtHKIC";
                }
                else {
                    $('.help-img').attr('src', applicationPath + '/Image/vbe/web/help-2-' + currentLanguage + '.png');
                }

            });
            obj.on('blur', function (event) {
                //event.stopPropagation();
                if ($(".rightSection").css('display') == 'none') {
                    //Mobile
                    onFocusField = "txtHKIC_blur";
                }
                else {
                    $('.help-img').attr('src', applicationPath + '/Image/vbe/web/help-1-' + currentLanguage + '.png');
                }
				formatHKID();
            });
        }

        if ($('#txtDob').length) {
            var obj = $('#txtDob');

            obj.on('focus', function (event) {
                //event.stopPropagation();
                if ($(".rightSection").css('display') == 'none') {
                    //Mobile
                    onFocusField = "txtDob";
                }
                else {
                    $('.help-img').attr('src', applicationPath + '/Image/vbe/web/help-3-' + currentLanguage + '.png');
                }

            });

            obj.on('blur', function (event) {
                //event.stopPropagation();
                if ($(".rightSection").css('display') == 'none') {
                    //Mobile
                    onFocusField = "txtDob_blur";
                }
                else {
                    $('.help-img').attr('src', applicationPath + '/Image/vbe/web/help-1-' + currentLanguage + '.png');
                }
                //var rtn = '11-2222';
                var rtn = formatDateOfBirth($('#txtDob').val());
                $('#txtDob').val(rtn);
                //$('#txtDob').text(rtn);
                //$('#txtDob').val('1111111');
            });
        }

        if ($('#txtDob_CE').length) {
            var obj = $('#txtDob_CE');
            obj.on('focus', function (event) {
                //event.stopPropagation();
                if ($(".rightSection").css('display') == 'none') {
                    //Mobile
                    onFocusField = "txtDob_CE";
                }
                else {
                    $('.help-img').attr('src', applicationPath + '/Image/vbe/web/help-3-' + currentLanguage + '.png');
                }
            });

            obj.on('blur', function (event) {
                //event.stopPropagation();
                if ($(".rightSection").css('display') == 'none') {
                    //Mobile
                    onFocusField = "txtDob_CE_blur";
                }
                else {
                    $('.help-img').attr('src', applicationPath + '/Image/vbe/web/help-1-' + currentLanguage + '.png');
                }
                $('#txtDob_CE').val(formatDateOfBirth($('#txtDob_CE').val()));
            });
        }


        //closeAlert
        if ($('#closeAlert').length) {
            $('#closeAlert').on('click', function (event) {
                $('.alert').css('display', 'none');
            });
        }

        //closeAlert
        if ($('#selType').length) {
            $('#selType').on('change', function (event) {
                if ($('#selType').val() == 'DOB') {
                    $('#contDob').css('display', 'block');
                    $('#contYob').css('display', 'none');
                    $('.help-text-dob-xs').css('cssText', 'display:block!important; top:0px;');
                    $('.help-text-yob-xs').css('cssText', 'display:none!important; top:0px;');

                    $('.help-text-dob').css('display', 'block');
                    $('.help-text-yob').css('display', 'none');

                }
                else {
                    $('#contDob').css('display', 'none');
                    $('#contYob').css('display', 'block');
                    $('.help-text-dob-xs').css('cssText', 'display:none!important; top:0px;');
                    $('.help-text-yob-xs').css('cssText', 'display:block!important; top:0px;');

                    $('.help-text-dob').css('display', 'none');
                    $('.help-text-yob').css('display', 'block');

                }
            });
        }

        //valiCodeImg
        $('.valiCodeImg').on('click', function (event) {
            refreshCaptcha();
        });

        //refreshCaptcha

        $('#refreshCaptcha').on('click', function (event) {
            refreshCaptcha();
        });

        initAlertSign();

        $(':text').on('focus', function (event) {
            $(this).attr('placeholder_backup', $(this).attr('placeholder'));
            $(this).attr('placeholder', '');
        });
        $(':text').on('blur', function (event) {
            if ($(this).text() == '') {
                $(this).attr('placeholder', $(this).attr('placeholder_backup'));
            }
        });

        if ($('#inputType').val() == '') {
            $('#inputType').val('IC');
        }

        getErrorMsg();
        //if (currentLanguage == 'zh-HK') {
        if (currentLanguage.toLowerCase() == cultureLanguageTradChinese) {
            $('#inActiveCEButton').addClass('inActiveButton-ce-ts');
            //$('.help-text').addClass('help-text-ts');
            $('.help-text-dob').addClass('help-text-ts');
            $('.help-text-yob').addClass('help-text-ts');
        }
        //Handle style for inactive button for CE

    }
    
    function getErrorMsg() {
        if (sessionStorage.getItem("errMsg")) {
            errMsgList = JSON.parse(sessionStorage.getItem("errMsg"))
        }
        else {
            if (!sessionStorage.getItem("lang"))
            {
                sessionStorage.setItem("lang","en")
            }
            $.ajax({
                async: false,
                url: applicationPath+"/"+sessionStorage.getItem("lang") + "/value/systemmsg",
                dataType: "json",
                success: function (data) {
                    if (data) {
                        errMsgList = data;
                        sessionStorage.setItem("errMsg", JSON.stringify(data))
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    var errorUrl = XMLHttpRequest.getResponseHeader("ErrorUrl");
                    if (errorUrl) {
                        var u = rootPath + rootLang + errorUrl;
                        location.href = u;
                    }
                }
            });
        }
    }

    function initAlertSign() {
        $('#alertCaptCha').css('display', 'none');
        $('#alertHKIC').css('display', 'none');
        $('#alertDob').css('display', 'none');
        $('#alertYob').css('display', 'none');
        $('#alertCaptChaMobile').css('cssText', 'display:none!important');
        $('#alertHKICMobile').css('cssText', 'display:none!important');
        $('#alertDobMobile').css('cssText', 'display:none!important');
        $('#alertYobMobile').css('cssText', 'display:none!important');
    }

    function SetAlertSign(errorCode) {
        switch (errorCode) {
            case "990000-E-00002":
                $('#alertHKIC').css('display', 'block');
                $('#alertHKICMobile').css('display', 'block');
                break;
            case "990000-E-00001":
                $('#alertHKIC').css('display', 'block');
                $('#alertHKICMobile').css('display', 'block');
                break;
            case "990000-E-00003":
                //Dob_input
                if ($('#Dob_input').css('display') == 'block') {
                    $('#alertDob').css('display', 'block');
                    $('#alertDobMobile').css('display', 'block');
                }
                else {
                    $('#alertYob').css('display', 'block');
                    $('#alertYobMobile').css('display', 'block');
                }
                break;
            case "990000-E-00004":
                if ($('#Dob_input').css('display') == 'block') {
                    $('#alertDob').css('display', 'block');
                    $('#alertDobMobile').css('display', 'block');
                }
                else {
                    $('#alertYob').css('display', 'block');
                    $('#alertYobMobile').css('display', 'block');
                }
                break;
            case "990000-E-00027":
                $('#alertCaptCha').css('display', 'block');
                $('#alertCaptChaMobile').css('display', 'block');
                break;
            case "990000-E-00033":
                $('#alertCaptCha').css('display', 'block');
                $('#alertCaptChaMobile').css('display', 'block');
                break;
            case "990000-E-00031":
                $('#alertCaptCha').css('display', 'block');
                $('#alertCaptChaMobile').css('display', 'block');
                break;
            case "990000-E-00032":
                $('#alertCaptCha').css('display', 'block');
                $('#alertCaptChaMobile').css('display', 'block');
                break;
            case "990000-E-00074":
                $('#alertYob').css('display', 'block');
                $('#alertYobMobile').css('display', 'block');
                break;
            case "990000-E-00072":
                $('#alertYob').css('display', 'block');
                $('#alertYobMobile').css('display', 'block');
                break;
            case "990000-E-00073":
                $('#alertYob').css('display', 'block');
                $('#alertYobMobile').css('display', 'block');
                break;
        }
    }

    function isdate(intYear, intMonth, intDay) {

        if (isNaN(intYear) || isNaN(intMonth) || isNaN(intDay)) return false;

        if (intMonth > 12 || intMonth < 1) return false;

        if (intDay < 1 || intDay > 31) return false;

        if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intDay > 30)) return false;

        if (intMonth == 2) {

            if (intDay > 29) return false;

            if ((((intYear % 100 == 0) && (intYear % 400 != 0)) || (intYear % 4 != 0)) && (intDay > 28)) return false;

        }

        return true;

    }



});

function callJs(obj) {
    //1.Write the request data
    if (obj != null) {
        $('#txtHKIC').val(obj.VBERequestData.HKICNo);

        $('#txtDob').val(obj.VBERequestData.DateOfBirth_IC);

        $('#selType').val(obj.VBERequestData.DateType);
        if ($('#selType').val() == 'DOB') {
            $('#contDob').css('display', 'block');
            $('#contYob').css('display', 'none');
        }
        else {
            $('#contDob').css('display', 'none');
            $('#contYob').css('display', 'block');
        }

        $('#txtDob_CE').val(obj.VBERequestData.DateOfBirth_CE);
        
        if (obj.VBERequestData.Age != 0) {
            $('#txtAge').val(obj.VBERequestData.Age);
        } else {
            $('#txtAge').val('');
        }
        if (obj.VBERequestData.Day > 0) {
            $('#txtDay').val(obj.VBERequestData.Day);
        }
        if (obj.VBERequestData.Month > 0) {
            $('#selMonth').val(obj.VBERequestData.Month); 
        }
        if (obj.VBERequestData.Year) {
            $('#txtYear').val(obj.VBERequestData.Year);
        }

        if (obj.VBERequestData.InputType == 'IC') {
            $('#Dob_label').css('display', 'block');
            $('#Yob_label').css('display', 'none');
            $('#Dob_input').css('display', 'block');
            $('#Yob_input').css('display', 'none');
            $('#dob-title').css('display', 'block');
            $('#yob-title').css('display', 'none');
            $('#inputType').val('IC');
            $('.help-img').css('display', 'block');
            $('.help-text-dob-xs').css('display', 'block');
            $('.help-text-yob-xs').css('display', 'none');
            mode = 1;
        }
        else {
            $('#Dob_label').css('display', 'none');
            $('#Yob_label').css('display', 'block');
            $('#Dob_input').css('display', 'none');
            $('#Yob_input').css('display', 'block');
            $('#dob-title').css('display', 'none');
            $('#yob-title').css('display', 'block');
            mode = 2;
            $('#inputType').val('CE');
            $('.help-img').css('display', 'none');
            $('.help-text-tips').css('display', 'block');

            if ($('#selType').val() == 'DOB') {
                $('.help-text-dob').css('display', 'block');
                $('.help-text-yob').css('display', 'none');
            } else {
                $('.help-text-dob').css('display', 'none');
                $('.help-text-yob').css('display', 'block');
            }
            if ($('.rightSection').css('display') == 'none') {

                $('.help-text-tips-yob').css('display', 'block');
                if ($('#selType').val() == 'DOB') {
                    $('.help-text-dob-xs').css('display', 'block');
                    $('.help-text-yob-xs').css('display', 'none');
                }
                else {
                    $('.help-text-dob-xs').css('display', 'none');
                    $('.help-text-yob-xs').css('display', 'block');
                }
            }
        }

        if (obj.lstErrCodes != null) {
            var useLang = obj.VBERequestData.lang;
            for (var err in obj.lstErrCodes) {
                //var msg = findMsg(useLang, obj.lstErrCodes[err]);
                var msg = findMsg(obj.lstErrCodes[err])
                errMsg += msg + '[' + obj.lstErrCodes[err] + ']<br>';
            }
            $('#txtErrMsg').html(errMsg);
            $('.alert').css('display', 'block');
        }
    }
}

function scrollToElem(elem) {
    var height = $('.header_content').css('height');
    if (height == '90px') {
        height = 90;
    }
    else {
        height = 130;
    }
    $('html, body').animate({
        'scrollTop': elem.offset().top - height
    }, 1000);
}