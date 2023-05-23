

var theform;
var isIE;
var isNS;
function detectBrowser() {
    if (window.navigator.appName.toLowerCase().indexOf("netscape") > -1) {
        theform = document.forms["Form1"];
    }
    else {
        theform = document.Form1;
    }
    //browser detection
    var strUserAgent = navigator.userAgent.toLowerCase();
    isIE = strUserAgent.indexOf("msie") > -1;
    isNS = strUserAgent.indexOf("netscape") > -1;
}
//===============================================================
//Select Values From Dropdown Validation For Dropdown onChange Event [onkeypress] 
//===============================================================


function Validation(dropdownname) {
    if (document.getelementbyid(dropdownname).value == 0) {
        alert('Plese select value for dropdown')
        return false;
    }
    return true;
}

function ValidateForm(dp1, dp2, dp3, dp4, dp5) {
    if (document.getelementbyid(dp1).value == 0) {
        alert('Plese select Industry Type.')
        return false;
    }
    if (document.getelementbyid(dp2).value == 0) {
        alert('Plese select Category.')
        return false;
    }
    if (document.getelementbyid(dp3).value == 0) {
        alert('Plese select Country.')
        return false;
    }
    if (document.getelementbyid(dp4).value == 0) {
        alert('Plese select State.')
        return false;
    }
    if (document.getelementbyid(dp5).value == 0) {
        alert('Plese select Location.')
        return false;
    }
    return true;
}

//===============================================================
//Script For Selecting All The Check Boxes
//===============================================================

//===============================================================
//Mony Control Validation For TextBox onChange Event [onkeypress] 
//===============================================================

function Round_Number(ths, dec) {

    if (ths.value == "") {
        result = 0
        ths.value = result
    }
    var result = Math.round(ths.value * Math.pow(10, dec)) / Math.pow(10, dec);
    var result = parseFloat(result)
    ths.value = result.toFixed(dec);
}

function isDecimalKey(evt, ths, declngth) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    var ths_val = ths.value
    var splited_val = ths_val.split(".")
    if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105)) {
        if (charCode != 46 && charCode != 39 && charCode != 37 && charCode != 9) {
            if (charCode == 110 || charCode == 190) {
                if (splited_val.length > 1) {
                    return false;
                }
            }
            else {
                return false;
            }
        }
    }
    if (splited_val.length >= 2) {
        if (splited_val[1].length >= declngth) {
            if (charCode == 46 || charCode == 8 || charCode == 9 || charCode == 37 || charCode == 39) {
                return true
            }
            return false
        }
    }
    return true;
}






function FormatAmtControl(ctl) {
    var vMask;
    var vDecimalAfterPeriod;
    var ctlVal;
    var iPeriodPos;
    var sTemp;
    var iMaxLen
    var ctlVal;
    var tempVal;
    ctlVal = ctl.value;
    vDecimalAfterPeriod = 2
    iMaxLen = ctl.maxLength;

    if (isNaN(ctlVal)) {

        ctl.value = ""
    }
    else {
        ctlVal = ctl.value;
        iPeriodPos = ctlVal.indexOf(".");

        if (iPeriodPos < 0) {
            if (ctl.value.length > (iMaxLen - 3)) {
                sTemp = ctl.value
                tempVal = sTemp.substr(0, (iMaxLen - 3)) + ".00";
            }
            else
                tempVal = ctlVal + ".00"
        }
        else {
            if ((ctlVal.length - iPeriodPos - 1) == 1)
                tempVal = ctlVal + "0"
            if ((ctlVal.length - iPeriodPos - 1) == 0)
                tempVal = ctlVal + "00"
            if ((ctlVal.length - iPeriodPos - 1) == 2)
                tempVal = ctlVal;
            if ((ctlVal.length - iPeriodPos - 1) > 2) {
                tempVal = ctlVal.substring(0, iPeriodPos + 3);
            }
        }
        ctl.value = tempVal;
    }
}

//===================================================================================
//Mony Control Validation For Mumber And [.] Only TextBox Key Press Event [onkeypress] 
//====================================================================================
function HandleAmountFiltering(ctl) {
    var iKeyCode, objInput;
    var iMaxLen
    var reValidChars = /[0-9.]/;
    var strKey;
    var sValue;
    var event = window.event || arguments.callee.caller.arguments[0];
    iMaxLen = ctl.maxLength;
    sValue = ctl.value;
    detectBrowser();
    if (isIE) {
        iKeyCode = event.keyCode;
        objInput = event.srcElement;
    }
    else {
        iKeyCode = event.which;
        objInput = event.target;
    }
    strKey = String.fromCharCode(iKeyCode);

    if (reValidChars.test(strKey)) {
        if (iKeyCode == 46) {
            if (objInput.value.indexOf('.') != -1)
                if (isIE)
                event.keyCode = 0;
            else {
                if (event.which != 0 && event.which != 8)
                    return false;
            }
        }
        else {
            if (objInput.value.indexOf('.') == -1) {
                if (objInput.value.length >= (iMaxLen - 3)) {
                    if (isIE)
                        event.keyCode = 0;
                    else {
                        if (event.which != 0 && event.which != 8)
                            return false;
                    }
                }
            }
            if ((objInput.value.length == (iMaxLen - 3)) && (objInput.value.indexOf('.') == -1)) {
                objInput.value = objInput.value + '.';
            }
        }
    }
    else {
        if (isIE)
            event.keyCode = 0;
        else {
            if (event.which != 0 && event.which != 8)
                return false;
        }
    }

}
//========================================================================================
//End Mony Control Validation For Mumber And [.] Only TextBox Key Press Event [onkeypress] 
//========================================================================================
function FillListByJavaScript(frm, val, objs) {
    openModalWindow();
    ClearListBoxGen(objs);
    document.getElementById(frm).src = val;
    CloseModalWindow();
    return false;
}
function ClearListBoxGen(objs) {
    for (var i = document.getElementById(objs).options.length - 1; i >= 0; i--) {
        document.getElementById(objs).options[i] = null;
    }
    document.getElementById(objs).selectedIndex = -1;
    return false;
}

function openModalWindow() {
    winW = document.body.offsetWidth;
    winH = document.body.offsetHeight;
    document.getElementById('modalBackgroundDiv').style.display = '';
    document.getElementById('modalBackgroundDiv').style.height = winH;
    document.getElementById('modalBackgroundDiv').style.width = winW;
    document.getElementById('modalBackgroundDiv1').style.display = '';
    document.getElementById('modalBackgroundDiv1').style.top = winH / 2;
    document.getElementById('modalBackgroundDiv1').style.left = winW / 2;
    document.getElementById('imgWait').focus();
}

function CloseModalWindow() {

    document.getElementById('modalBackgroundDiv').style.display = 'none';
    document.getElementById('modalBackgroundDiv1').style.display = 'none';
}

//=================================================================
//Function For Only Char using TextBox Key Press Event [onkeypress] 
//=================================================================
function AllowOnlyChar() {
    if ((event.keyCode < 97 || event.keyCode > 122) && (event.keyCode < 65 || event.keyCode > 90))
    { event.keyCode = 0; }
}
//=======================================================================
//Function For Only Number using onkeypress="return AllowOnlyNum(event)"
//=======================================================================
function AllowOnlyNum(evt) {
    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        status = "This field accepts numbers only."
        return false
    }
    status = ""
    return true
}
//=============================================================================
//Function For Only Number using onkeypress="return AllowOnlyNum(this, event);"
//=============================================================================
function AllowOnlyNumWr(field, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    var keychar = String.fromCharCode(charCode);

    if (charCode > 31 && (charCode < 48 || charCode > 57) && keychar != "." && keychar != "-") {
        return false;
    }

    if (keychar == "." && field.value.indexOf(".") != -1) {
        return false;
    }

    if (keychar == "-") {
        if (field.value.indexOf("-") != -1 /* || field.value[0] == "-" */) {
            return false;
        }
        else {
            //save caret position
            var caretPos = getCaretPosition(field);
            if (caretPos != 0) {
                return false;
            }
        }
    }
    return true;
}

//=================================================================
//Function For Only First UpperCase onkeypress="return initialCap(this);"
//=================================================================
function initialCap(field) {
    field.value = field.value.substr(0, 1).toUpperCase() + field.value.substr(1);
}

//=================================================================
//Function For Only Decimal onblur="ValidateAndFormatNumber(this)" 
//=================================================================
function ValidateAndFormatNumber(NumberTextBox) {
    if (NumberTextBox.value == "") return;
    UnFormatNumber(NumberTextBox);
    var IsFound = /^-?\d+\.{0,1}\d*$/.test(NumberTextBox.value);
    if (!IsFound) {
        alert("Not a number");
        NumberTextBox.focus();
        NumberTextBox.select();
        return;
    }
    if (isNaN(parseFloat(NumberTextBox.value))) {
        alert("Number exceeding float range");
        NumberTextBox.focus();
        NumberTextBox.select();
    }
    NumberTextBox.value = FormatNumber(NumberTextBox.value);
}
function ValidateNumberKeyUp(field) {
    if (document.selection.type == "Text") {
        return;
    }
    //save caret position
    var caretPos = getCaretPosition(field);
    var fdlen = field.value.length;
    UnFormatNumber(field);
    var IsFound = /^-?\d+\.{0,1}\d*$/.test(field.value);
    if (!IsFound) {
        setSelectionRange(field, caretPos, caretPos);
        return false;
    }
    field.value = FormatNumber(field.value);
    fdlen = field.value.length - fdlen;
    setSelectionRange(field, caretPos + fdlen, caretPos + fdlen);
}
function FormatNumber(fnum) {
    var orgfnum = fnum;
    var flagneg = false;
    if (fnum.charAt(0) == "-") {
        flagneg = true;
        fnum = fnum.substr(1, fnum.length - 1);
    }
    psplit = fnum.split(".");
    var cnum = psplit[0],
	            parr = [],
	            j = cnum.length,
	            m = Math.floor(j / 3),
	            n = cnum.length % 3 || 3;
    // break the number into chunks of 3 digits; first chunk may be less than 3
    for (var i = 0; i < j; i += n) {
        if (i != 0) { n = 3; }
        parr[parr.length] = cnum.substr(i, n);
        m -= 1;
    }
    // put chunks back together, separated by comma
    fnum = parr.join(",");
    // add the precision back in
    //if (psplit[1]) {fnum += "." + psplit[1];}
    if (orgfnum.indexOf(".") != -1) {
        fnum += "." + psplit[1];
    }
    if (flagneg == true) {
        fnum = "-" + fnum;
    }
    return fnum;
}
function UnFormatNumber(obj) {
    if (obj.value == "") return;
    obj.value = obj.value.replace(/,/gi, "");
}
function getCaretPosition(objTextBox) {
    var objTextBox = window.event.srcElement;
    var i = objTextBox.value.length;
    if (objTextBox.createTextRange) {
        objCaret = document.selection.createRange().duplicate();
        while (objCaret.parentElement() == objTextBox &&
                objCaret.move("character", 1) == 1) --i;
    }
    return i;
}
function setSelectionRange(input, selectionStart, selectionEnd) {
    if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }
    else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}

//==========================================================================
//Function For Only Number & Char using TextBox Key Press Event [onkeypress] 
//==========================================================================
function AllowOnlyNumChar() {
    if ((event.keyCode <= 47 || event.keyCode >= 123) || (event.keyCode >= 58 && event.keyCode < 65) || (event.keyCode >= 91 && event.keyCode <= 96)) {
        if (event.keyCode == 13) {

        }
        else {
            event.keyCode = 0;
        }
    }
}

//======================================================================================================
//Function For  removeSpaces Using TextBox onKeyPress[onKeyPress="this.value=removeSpaces(this.value);"]
//======================================================================================================

function removeSpaces(string) {
    var tstring = "";
    string = '' + string;
    splitstring = string.split(" ");
    for (i = 0; i < splitstring.length; i++)
        tstring += splitstring[i];
    return tstring;
}
//========================================================================
//Function For Email Validation Using TextBox Focus Leave Event [onChange]
//========================================================================
function ValidateEmail() {
    if (window.event.srcElement.value.length > 0 && (window.event.srcElement.value.indexOf("@", 0) == -1 || window.event.srcElement.value.indexOf(".", 0) == -1)) {
        alert('Please Enter a Valid Mail-Id');
        window.event.srcElement.focus();
        return (false);
    }
}

//=============================================================
//Function For Color Change At TextBox Focus Event [onKeyPress]
//=============================================================
function SetFColor() {
    try {
        window.event.srcElement.style.backgroundColor = "#e8e8e8"
        tdStatus.innerText = window.event.srcElement.title
    }
    catch (e)
     { }
}

//===========================================================
//Function For Color Change At TextBox Focus Leave [onChange]
//===========================================================
function SetLColor() {
    try {
        window.event.srcElement.style.backgroundColor = "#ffffff"
        tdStatus.innerText = ""
    }
    catch (e)
    { }
}

//===========================
//Varchar Validation Function
//===========================

function ValidateVarchar(field) {
    var valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789&./,- "
    var ok = "yes";
    var temp;
    var flag;
    flag = 0;

    if (field) {

        for (var i = 0; i < field.length; i++) {
            temp = "" + field.substring(i, i + 1);
            if (valid.indexOf(temp) == "-1") ok = "no";

            if (temp != " ") {
                flag = 1;
            }

        }
    }
    if ((ok == "no") || (flag == 0)) {
        return false;
    }
    else {
        return true;
    }
}
//===========================
//Numeric Validation Function
//===========================
function ValidateNumeric(field) {
    var valid = "0123456789."
    var ok = "yes";
    var temp;
    var flag;
    flag = 0;

    if (field) {
        for (var i = 0; i < field.length; i++) {
            temp = "" + field.substring(i, i + 1);
            if (temp != " ") {
                flag = 1
            }
            if (valid.indexOf(temp) == "-1") {
                ok = "no";
            }
        }
    }

    if ((ok == "no") || (flag == 0)) {
        return false;
    }
    else {
        return true;
    }
}

//===========================
//Integer Validation Function
//===========================

function ValidateInteger(field) {
    var valid = "0123456789"
    var ok = "yes";
    var temp;
    var flag;
    flag = 0;

    if (field) {
        for (var i = 0; i < field.length; i++) {
            temp = "" + field.substring(i, i + 1);
            if (temp != " ") {
                flag = 1
            }
            if (valid.indexOf(temp) == "-1") {
                ok = "no";
            }
        }
    }
    if ((ok == "no") || (flag == 0)) {
        return false;
    }
    else {
        return true;
    }
}
//=============================
//Firstchar Validation Function
//=============================
function ValidateFirstchar(field) {
    var valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789&,./- "
    var ok = "yes";
    var temp;
    var flag;
    flag = 0;

    if (field) {

        for (var i = 0; i < field.length; i++) {
            temp = "" + field.substring(i, i + 1);
            if (temp != " ") {
                flag = 1
            }
            if (valid.indexOf(temp) == "-1") {
                ok = "no";
            }
        }

        if ((ok == "no") || (flag == 0) || (isNaN(field.substring(0, 1)) == false)) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }
}
//============================
//Decimal Validation Function
//============================
function ValidateDecimal(before_dec, field, after_dec) {
    if (field && before_dec && after_dec) {

        var beftext;
        if (isNaN(field) || field == "" || field < 0) {
            return false;
        }

        if (field.indexOf('.') == -1)
            field += ".";
        dectext = field.substring(field.indexOf('.') + 1, field.length);
        if (dectext.length > after_dec) {
            return false;
        }
        beftext = field.substring(1, field.indexOf('.') + 1)
        if (beftext.length > before_dec) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }
}
function y2k(number) {
    return (number < 1000) ? number + 1900 : number;
}
//================================
//Date Function For Day Month Year 
//================================
function systemDate(obj) {
    var currentTime = new Date();
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var Date_Today = day + "/" + month + "/" + year;
    obj.value = Date_Today;
}

function isDate(day, month, year) {
    var today = new Date();
    year = ((!year) ? y2k(today.getYear()) : year);
    month = ((!month) ? today.getMonth() : month - 1);
    if (!day) return false
    var test = new Date(year, month, day);
    if ((y2k(test.getYear()) == year) && (month == test.getMonth()) && (day == test.getDate()))
        return true;
    else
        return false
}
//=====================================================
//Calender [Day Month Year] Using onkeypress Event [F2]
//=====================================================
function callCalender() {
    try {
        if (event.keyCode == 113) {
            ReturnEl = window.event.srcElement
            window.open("CalenderControl/calendar.htm", null, "height=170,width=270,resizable=no,status=no,toolbar=no,menubar=no,left=360,top=200");
        }
    }
    catch (e) {
        alert('GetDt : ' + e.description)
    }
}
//=======================================
//Date Validation [Day Month Year] Format
//=======================================
function validatedate(ddmmyyyy) {
    if (ddmmyyyy.substring(2, 3) != "/" || ddmmyyyy.substring(5, 6) != "/") {
        return false;
    }

    if (ddmmyyyy.length != 10) {
        return false;
    }
}

//===========================================
//Trimming Function is For Trim, Ltrim, Rtrim
//===========================================
function trim(str, chars) {
    return ltrim(rtrim(str, chars), chars);
}
function ltrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}
function rtrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}

String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/g, '');
}
//===========================================================
//Trimming Function for stripping leading and trailing spaces
//===========================================================
function trim(str) {
    if (str != null) {
        var i;
        for (i = 0; i < str.length; i++) {
            if (str.charAt(i) != " ") {
                str = str.substring(i, str.length);
                break;
            }
        }

        for (i = str.length - 1; i >= 0; i--) {
            if (str.charAt(i) != " ") {
                str = str.substring(0, i + 1);
                break;
            }
        }

        if (str.charAt(0) == " ") {
            return "";
        }
        else {
            return str;
        }
    }
}

function launch() {
    window.open("http://www.yahoo.com/", "", "toolbar=no, menubar=no, location=no, scrollbars=yes, directories=no, width=400, height=300, left=100, top=75");
}
function high(which2) {
    theobject = which2
    highlighting = setInterval("highlightit(theobject)", 70)
}
function low(which2) {
    clearInterval(highlighting)
    which2.filters.alpha.opacity = 80
}
function highlightit(cur2) {
    if (cur2.filters.alpha.opacity < 200)
        cur2.filters.alpha.opacity += 20
    else if (window.highlighting)
        clearInterval(highlighting)
}
 