<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="TransferAssetsTSB.aspx.cs" Inherits="TransferAssetsTSB" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="css/Site.css" />
    <script type="text/javascript">

        function CheckAll(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    if (masterTable.get_dataItems()[i].findElement("cboxSelect").disabled != true)
                        masterTable.get_dataItems()[i].findElement("cboxSelect").checked = true; // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = false; // for unchecking the checkboxes
                }
            }
        }
        function unCheckHeader(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == false) {
                //var hidden = document.getElementById("HiddenField3");
                var checkBox = document.getElementById(document.getElementById("<%=HiddenField3.ClientID%>").value);
                checkBox.checked = false;
            }
        }
        <%--  function undoformat() {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
             var row = masterTable.get_dataItems();
             for (var i = 0; i < row.length; i++) {
                 if (masterTable.get_dataItems()[i].findElement("cboxSelect").disabled == true) {
                     masterTable.get_dataItems()[i].findElement("cboxSelect").disabled = false;
                     //masterTable.get_dataItems()[i].row.addClass("rgRowNoColor");
                 }
             }
            
         }--%>
    </script>

    <script type="text/javascript">
        var mouseOverActiveElement = false;

        $(document).ready(function () {


            $('#ContentPlaceHolder1_txtSearch').click(function () {


                if ($('#ContentPlaceHolder1_txtSearch').val().length > 1) {
                    $('#ContentPlaceHolder1_divSearch').hide(500);
                } else {
                    $('#ContentPlaceHolder1_divSearch').show(500);
                }
            })
            //            $('#ContentPlaceHolder1_btnRefresh').click(function () {

            //                $('#ContentPlaceHolder1_ddlproCategory').val(0);
            //                $('#ContentPlaceHolder1_txtAssetCode').val('');
            //                $('#ContentPlaceHolder1_ddlsubcat').val('0');

            //                $('#ContentPlaceHolder1_ddlloc').val('0');
            //                $('#ContentPlaceHolder1_ddlbuild').val('0');
            //                $('#ContentPlaceHolder1_ddlfloor').val('0');
            //                $('#ContentPlaceHolder1_ddldept').val('0');
            //                $('#ContentPlaceHolder1_ddlCustodian').val('0');
            //                
            //                $('#ContentPlaceHolder1_divSearch').show();

            //            });
            $('#ContentPlaceHolder1_GetGrid').click(function () {
                if ($('#ContentPlaceHolder1_txtSearch').val().length == 0) {
                    alert('Enter some text to search');
                    return false;
                }
            });
            $('#ContentPlaceHolder1_txtSearch').keydown(function (e) {

                if ($('#ContentPlaceHolder1_txtSearch').val().length > 0) {
                    $('#ContentPlaceHolder1_divSearch').hide(500);
                }
                else {
                    $('#ContentPlaceHolder1_divSearch').show(500);

                }


            })

            $('#divSearchClose').click(function () {
                $('#ContentPlaceHolder1_divSearch').hide(500);
            });


            $(':file').on('change', ':file', function () {
                var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });

            //            $('#txtSearch,#ContentPlaceHolder1_divSearch').on('mouseenter', function () {
            //                mouseOverActiveElement = true;
            //            }).on('mouseleave', function () {
            //                mouseOverActiveElement = false;
            //            });
            //            $("html").click(function () {
            //                if (!mouseOverActiveElement) {
            //                    $('#ContentPlaceHolder1_divSearch').hide(500);
            //                }
            //                else {

            //                }
            //            });
        })

    </script>
    <script type="text/javascript">

        var lat = document.getElementById("HdnLatValue");
        var logn = document.getElementById("HdnLogValue");
        var x = document.getElementById("<%=HdnError.ClientID%>");
        getLocation();

        function getLocation() {

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError, { maximumAge: 600000, timeout: 30000, enableHighAccuracy: false });
            }
            else { x.value = "Geolocation is not supported by this browser."; }
        }

        function showPosition(position) {
            var latlondata = position.coords.latitude + "," + position.coords.longitude;
            var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:=" + position.coords.longitude;

            document.getElementById("<%=HdnLatValue.ClientID%>").value = position.coords.latitude;
            document.getElementById("<%=HdnLogValue.ClientID%>").value = position.coords.longitude;
            //alert(latlon);
        }

        function showError(error) {
            if (error.code == 1) {
                x.value = "User denied the request for Geolocation."
            }
            else if (err.code == 2) {
                x.value = "Location information is unavailable."
            }
            else if (err.code == 3) {
                x.value = "The request to get user location timed out."
            }
            else {
                x.value = "An unknown error occurred."
            }
        }
    </script>
    <%--    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>--%>
    <style type="text/css">
        @media only screen and (min-width: 480px) and (max-width: 767px) {
            .additionalColumn {
                display: none !important;
            }
        }
    </style>
    <style type="text/css">
        .gvData {
            margin-left: auto !important;
            margin-right: auto !important;
        }
    </style>
    <script type="text/javascript">
        function GridCreated(sender, args) {
            var scrollArea = sender.GridDataDiv;
            var dataHeight = sender.get_masterTableView().get_element().clientHeight; if (dataHeight < 350) {
                scrollArea.style.height = dataHeight + 17 + "px";
            }
        }
    </script>
    <script type="text/javascript">
        function HideModalPopup() {
            $find("mpe").hide();
            txt2 = document.getElementById('<%= txtAreaScannedItems.ClientID%>');
            txt2.value = "";
            txt1 = document.getElementById('<%= hdnScanned.ClientID%>');
            txt1.value = "";
            StopScanning();
            return false;
        }
    </script>
    <script type="text/javascript">


        function ConfirmTo(ToValue) {
            var FromValue = document.getElementById("<%=cboFromCustodian.ClientID%>").value;

            var From = document.getElementById("<%=cboFromCustodian.ClientID%>");
            var To = document.getElementById("<%=cboToCustodian.ClientID%>");

            if (FromValue == ToValue) {
                To.selectedIndex = 0;
                document.getElementById("<%=HdnSelectedTo.ClientID%>").value = "0";
                document.getElementById("<%=HdnTo.ClientID%>").value = "";
                return false;
            }
            for (var i = 0; i < To.options.length; i++) {
                if (To.options[i].value == ToValue) {
                    To.selectedIndex = i;

                   <%-- document.getElementById("<%=cboToCustodian.ClientID%>").value =  To.options[i].text;--%>
                    document.getElementById("<%=HdnSelectedTo.ClientID%>").value = ToValue;
                    document.getElementById("<%=HdnTo.ClientID%>").value = To.options[i].text;;
                    break;
                }
                else {
                    document.getElementById("<%=HdnSelectedTo.ClientID%>").value = "0";
                    document.getElementById("<%=HdnTo.ClientID%>").value = "";
                    document.getElementById("<%=cboToCustodian.ClientID%>").value = "0";
                }
            }

            return false;
        }
        function ConfirmFrom(FromValue) {
            var ToValue = document.getElementById("<%=cboToCustodian.ClientID%>").value;
            var From = document.getElementById("<%=cboFromCustodian.ClientID%>");
            var To = document.getElementById("<%=cboToCustodian.ClientID%>");

            if (ToValue == FromValue) {
                From.selectedIndex = 0;
                document.getElementById("<%=HdnFrom.ClientID%>").value = "";
                document.getElementById("<%=HdnSelectedFrom.ClientID%>").value = "0";
                return false;
            }
            for (var i = 0; i < From.options.length; i++) {
                if (From.options[i].value == FromValue) {
                    From.selectedIndex = i;
                    document.getElementById("<%=HdnFrom.ClientID%>").value = From.options[i].text;
                  <%--  document.getElementById("<%=cboFromCustodian.ClientID%>").value =  From.options[i].text;--%>
                    document.getElementById("<%=HdnSelectedFrom.ClientID%>").value = FromValue;
                    break;
                } else {
                    document.getElementById("<%=HdnFrom.ClientID%>").value = "";
                    document.getElementById("<%=HdnSelectedFrom.ClientID%>").value = "0";
                    document.getElementById("<%=cboFromCustodian.ClientID%>").value = "0";
                }
            }
            //PageMethods.ToCustodian(document.getElementById("").value);

            return false;
        }
        function ConfirmCustLoca(CustValue) {

            var CustLoc = document.getElementById("<%=cboLocaCustodian.ClientID%>");

            for (var i = 0; i < CustLoc.options.length; i++) {
                if (CustLoc.options[i].value == CustValue) {
                    CustLoc.selectedIndex = i;
                    document.getElementById("<%=HdnCustLoc.ClientID%>").value = CustLoc.options[i].text;
                  <%--  document.getElementById("<%=cboFromCustodian.ClientID%>").value =  From.options[i].text;--%>
                    document.getElementById("<%=HdnSelectedCustLoc.ClientID%>").value = CustValue;
                    break;
                } else {
                    document.getElementById("<%=HdnCustLoc.ClientID%>").value = "";
                    document.getElementById("<%=HdnSelectedCustLoc.ClientID%>").value = "0";
                    document.getElementById("<%=cboLocaCustodian.ClientID%>").value = "0";
                }
            }
            //PageMethods.ToCustodian(document.getElementById("").value);

            return false;
        }


    </script>
    <style type="text/css">
        .rgRowNoColor {
            background-color: none;
        }

        .rgRowNoRedColor {
            background-color: red;
        }

        .drpwidth {
            width: 200px;
        }

        .btn-file {
            position: relative;
            overflow: hidden;
        }

            .btn-file input[type=file] {
                position: absolute;
                top: 0;
                right: 0;
                min-width: 100%;
                min-height: 50%;
                font-size: 100px;
                text-align: right;
                filter: alpha(opacity=0);
                opacity: 0;
                outline: none;
                background: white;
                cursor: inherit;
                display: block;
            }

        .input-file .input-group-addon {
            border: 0px;
            padding: 0px;
        }

            .input-file .input-group-addon .btn {
                border-radius: 0 4px 4px 0;
            }

            .input-file .input-group-addon input {
                cursor: pointer;
                position: absolute;
                width: 72px;
                z-index: 2;
                top: 0;
                right: 0;
                filter: alpha(opacity=0);
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                opacity: 0;
                background-color: transparent;
                color: transparent;
            }

        .focus {
            background-color: Red !important;
            color: White !important;
            text-decoration: blink;
        }

        .targetButton {
            border-color: Green !important;
            color: White !important;
            text-decoration: blink;
        }
    </style>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .modalBackgroundN {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        .modalPopupN {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
            height: 65%;
        }
    </style>
    <style type="text/css">
        .changePosition {
            margin-right: 10px;
            border: 0;
        }

        .bgimage {
            width: 21px;
            height: 21px;
            background: url("images/CloseGray.jpg");
            border: 0;
            display: inline-block;
            text-transform: uppercase;
            margin-right: 5px;
        }

        .modalBackground {
            background-color: gray;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 250px;
            border: 3px solid #98CODA;
        }

            .modalPopup .headerModal {
                background-color: #2FBDF1;
                height: 30px;
                color: White;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .body {
                min-height: 50px;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .footer {
                padding: 1px;
            }

            .modalPopup .yes, .modalPopup .no {
                height: 23px;
                color: White;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
            }

            .modalPopup .yes {
                background-color: #2FBDF1;
                border: 1px solid #0DA9D0;
            }
    </style>
    <script type="text/javascript">
        var mouseOverActiveElement = false;

        $(document).ready(function () {
            Oncall();
            function Oncall() {
                var tab = document.getElementById('<%= hidTAB.ClientID%>').value;
                if (tab == "divbyLocation") {
                    $('#ContentPlaceHolder1_divbyLocation').tab('show');
                    $('#ContentPlaceHolder1_divbyCustodian').hide();
                    //$('#BtnByLocation').addClass("targetButton");
                    //$('#BtnByCustodian').removeClass("targetButton");
                } else {
                    $('#ContentPlaceHolder1_divbyLocation').hide();
                    $('#ContentPlaceHolder1_divbyCustodian').tab('show');
                    //$('#BtnByCustodian').addClass(" targetButton");
                    //$('#BtnByLocation').removeClass("targetButton");
                }
                var FromID = document.getElementById('<%= HdnSelectedFrom.ClientID%>').value;
                var ToID = document.getElementById('<%= HdnSelectedTo.ClientID%>').value;
                var From = document.getElementById("<%=cboFromCustodian.ClientID%>");
                var To = document.getElementById("<%=cboToCustodian.ClientID%>");

                for (var i = 0; i < From.options.length; i++) {
                    if (From.options[i].value == FromID) {
                        From.selectedIndex = i;
                        break;
                    }
                }

                for (var i = 0; i < To.options.length; i++) {
                    if (To.options[i].value == ToID) {
                        To.selectedIndex = i;
                        break;
                    }
                }

                var CustLoc = document.getElementById("<%=cboLocaCustodian.ClientID%>");
                var CustLocID = document.getElementById('<%= HdnSelectedCustLoc.ClientID%>').value;

                for (var i = 0; i < CustLoc.options.length; i++) {
                    if (CustLoc.options[i].value == CustLocID) {
                        CustLoc.selectedIndex = i;
                        break;
                    }
                }
            }

            function CustodianReset() {
                Cust1 = document.getElementById('<%=cboFromCustodian.ClientID %>');
                Cust1.selectedIndex = 0;
                Cust2 = document.getElementById('<%=cboToCustodian.ClientID %>');
                Cust2.selectedIndex = 0;
                document.getElementById("<%=HdnSelectedTo.ClientID%>").value = "0";
                document.getElementById("<%=HdnTo.ClientID%>").value = "";
                document.getElementById("<%=HdnFrom.ClientID%>").value = "";
                document.getElementById("<%=HdnSelectedFrom.ClientID%>").value = "0";
                StopScanning();
            }

            function CustodianResetLoc() {
                Cust = document.getElementById('<%=cboLocaCustodian.ClientID %>');
                Cust.selectedIndex = 0;

                document.getElementById("<%=HdnSelectedCustLoc.ClientID%>").value = "0";
                document.getElementById("<%=HdnCustLoc.ClientID%>").value = "";
                StopScanning();
            }

            //$('#divCloseCustodian').click(function () {
            //    $('#ContentPlaceHolder1_divbyCustodian').hide(500);
            //});

            //$('#divCloseLocation').click(function () {
            //    $('#ContentPlaceHolder1_divbyLocation').hide(500);
            //});

          <%--  $('#BtnByLocation').click(function () {
                debugger;
                $('#ContentPlaceHolder1_divbyLocation').show(500);
                $('#ContentPlaceHolder1_divbyCustodian').hide(500);
                document.getElementById('<%=hidTAB.ClientID %>').value = "divbyLocation";
               
            });

            $('#BtnByCustodian').click(function () {
                $('#ContentPlaceHolder1_divbyCustodian').show(500);
                $('#ContentPlaceHolder1_divbyLocation').hide(500);
                document.getElementById('<%=hidTAB.ClientID %>').value = "divbyCustodian";
            });--%>

            $('#txtSearch').click(function () {
                $('#ContentPlaceHolder1_divSearch').show(500);
            });

            $('#divSearchClose').click(function () {
                $('#ContentPlaceHolder1_divSearch').hide(500);
            });
            $('#btnCustodianScan').click(function () {

            });

            $('#Btn_LocationScan').click(function () {

            });
            $('#btnCustodianReset').click(function () {
                CustodianReset();
            });
            $('#btnCustodianResetLoc').click(function () {
                CustodianResetLoc();
            });

            $('#lkCust').click(function () {
                Cust1 = document.getElementById('<%=cboFromCustodian.ClientID %>');
                Cust1.selectedIndex = 0;
                Cust2 = document.getElementById('<%=cboToCustodian.ClientID %>');
                Cust2.selectedIndex = 0;
                ScanningFor = "Custodian";
                ScanTag();

            });
            $('#lkLocCust').click(function () {
                Cust = document.getElementById('<%=cboLocaCustodian.ClientID %>');
                Cust.selectedIndex = 0;
                ScanningFor = "Custodian_Loc";
                ScanTag();

            });
            //$('#lkToCust').click(function () {
            //    ScanningFor = "ToCust";
            //    ScanTag();



        });


        $(':file').on('change', ':file', function () {
            var input = $(this),
    numFiles = input.get(0).files ? input.get(0).files.length : 1,
    label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileselect', [numFiles, label]);
        });

        //            $('#txtSearch,#ContentPlaceHolder1_divSearch').on('mouseenter', function () {
        //                mouseOverActiveElement = true;
        //            }).on('mouseleave', function () {
        //                mouseOverActiveElement = false;
        //            });
        //            $("html").click(function () {
        //                if (!mouseOverActiveElement) {
        //                    $('#ContentPlaceHolder1_divSearch').hide(500);
        //                }
        //                else {

        //                }
        //            });
        //})
        //$(document).ready(function () {

        //    $('#BtnByLocation').click(function () {
        //        $(this).addClass("targetButton");
        //        $('#BtnByCustodian').removeClass("targetButton");
        //    });
        //    $('#BtnByCustodian').click(function () {
        //        $(this).addClass("targetButton");
        //        $('#BtnByLocation').removeClass("targetButton");
        //    });

        //});

    </script>
    <object id="ctr1" classid="CLSID:4CF51716-6C2C-3F23-AA9C-FE69888AA646" codebase="http://tagitglobal.com/Tagit.cab"
        hidden="hidden">
        <param name="Interval" value="1000">
        <param name="Enabled" value="1">
    </object>
    <script type="text/javascript">
        var v = "";
        var stat = new Boolean();
        var Read1Status = new Boolean();
        Read1Status = false;
        ts = document.getElementById("ctr1");
        try {
            var tg = "1"
            function ConfirmBox() {
                if (tg == "1") {
                    try {
                        stat = ts.LoginToTSBV2("COM3", 10);
                        document.getElementById('<%=hfConfirmValue.ClientID %>').value = stat;
                    }
                    catch (err) {
                        document.getElementById('<%=hfConfirmValue.ClientID %>').value = "false";
                    }
                }
            }
        }
        catch (err) {
            document.getElementById('<%=hfConfirmValue.ClientID %>').value = "false";
        }
    </script>
    <script language="javascript" type="text/javascript">
        var v = "";
        var my = "";
        var x = "";
        var status = new Boolean();
        var EncodeStatus = new Boolean();
        EncodeStatus = false;
        var ScanningFor = "";
        ts = document.getElementById("ctr1");

        function EncodeTag() {
            var cell = "";
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (row.length > 0) {
                for (var i = 0; i < row.length; i++) {
                    if (masterTable.get_dataItems()[i].findElement("cboxSelect").checked == true) // for checking the checkboxes
                    {
                        cell = masterTable.get_dataItems()[i].findElement("hdbAstCode").value;
                        break;
                    }
                }
            } else {
                alert("No Records to Encode..!!");
                return false;
            }
            if (cell == "") {
                alert("Select any item..!!");
                return false;
            }
            ConfirmBox();
            if (document.getElementById('<%=hfConfirmValue.ClientID %>').value == "false") {
                alert("Connection to TSB failed..!!");
                return false;
            }
            var code = document.getElementById('<%=hdnClientCode.ClientID %>').value;
            cell = code + String(cell);
            status = ts.EncodeTheTagTSBV2(cell);
            if (status == false) {
                alert("Encode Failed..!!");
                ts.LogOutTSBV2();
                return false;
            }
            else {
                //alert("Encoded successfully..!!");
                ts.LogOutTSBV2();
                return true;
            }
        }
        function ScanTag() {
            my = 0;
            x = 1;
            ConfirmBox();
            if (document.getElementById('<%=hfConfirmValue.ClientID %>').value == "false") {
                alert("Connection to TSB failed..!!");
                return false;
            }
          <%--  txt2 = document.getElementById('<%=txtAreaScannedItems.ClientID %>');
            txt2.value = "";--%>

            var code = document.getElementById('<%=hdnClientCode.ClientID %>').value;
            ts.StartReadTSBV2(code);
            InitializeTimer();
            //$find("mpe").show();
        }
        function StopScanning() {

            <%--  txt2 = document.getElementById('<%=txtAreaScannedItems.ClientID %>');
            v = ts.TagData;
            txt2.value = v;--%>
            secs = 0;
            ts.StopReadTSBV2();
            ts.LogOutTSBV2();

        }
        function ScanTag1(y) {
            if (y == "Custodian") {
                var From = document.getElementById("<%=cboFromCustodian.ClientID%>");
                var To = document.getElementById("<%=cboToCustodian.ClientID%>");
                if (From.selectedIndex == 0 && To.selectedIndex == 0) {
                    return;
                }
            } else if (y == "Location") {
                var CustLoc = document.getElementById("<%=cboLocaCustodian.ClientID%>");
                    if (CustLoc.selectedIndex == 0) {
                        return;
                    }
                }
            ScanningFor = "Items";
            ScanTag();
            txt2 = document.getElementById('<%= txtAreaScannedItems.ClientID%>');
            txt2.value = "";
            txt1 = document.getElementById('<%= hdnScanned.ClientID%>');
            txt1.value = "";
            btn = document.getElementById('<%=btnConfirm.ClientID %>');
            btn.disabled = true;
            lbl = document.getElementById('<%=lblCount.ClientID %>');
            lbl.textContent = "0";
            $find("mpe").show(500);
            //$find("mpe").show();
        }
        function StopScanning1() {
            secs = 0;
            ts.StopReadTSBV2();
            ts.LogOutTSBV2();
            btn = document.getElementById('<%=btnConfirm.ClientID %>');
            btn.disabled = false;

        }
        function add() {
            if (ScanningFor == "Items") {
                txt2 = document.getElementById('<%=txtAreaScannedItems.ClientID %>');
                txt1 = document.getElementById('<%= hdnScanned.ClientID%>');
                v = ts.TagData;
                txt2.value = v;
                txt1.value = v;
                if (v != "") {
                    var res = v.split(",");
                    lbl = document.getElementById('<%=lblCount.ClientID %>');
                    lbl.textContent = res.length;
                }
            }
            else if (ScanningFor == "Custodian") {

                v = ts.TagData;
                if (v != "" && x == 1) {
                    ConfirmFrom(ts.TagData);
                    x = 2;
                    ts.TagData = "";
                }
                else if (v != "" && x == 2) {
                    ConfirmTo(ts.TagData);
                    x = "Done";
                }

                //}while(x=="Done" || my==500)

                if (x == "Done") {
                    StopScanning();
                }
                if (my == 2500 && x != "Done") {
                    StopScanning();
                    alert('Tag Not Found');
                }
                my++;
                // StopScanning();
                ts.TagData = "";
            }
            else if (ScanningFor == "Custodian_Loc") {

                v = ts.TagData;
                if (v != "" && x == 1) {
                    ConfirmCustLoca(ts.TagData);
                    x = "Done";
                }

                if (x == "Done") {
                    StopScanning();
                }
                if (my == 2500 && x != "Done") {
                    StopScanning();
                    alert('Tag Not Found');
                }
                my++;
                // StopScanning();
                ts.TagData = "";
            }
        }

        //------Timmer--------
        var secs
        var timerID = null
        var timerRunning = false
        var delay = 1


        function InitializeTimer() {
            secs = 1
            StopTheClock()
            StartTheTimer()
        }

        function StopTheClock() {
            if (timerRunning)
                clearTimeout(timerID)
            timerRunning = false
        }

        function StartTheTimer() {
            if (secs == 0) {
                StopTheClock()
            }
            else {
                add();
                timerRunning = true
                timerID = self.setTimeout("StartTheTimer()", delay)
            }
        }
        //--------------------------
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hidTAB" runat="server" Value="divbyLocation" />
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <asp:Button ID="btnErrorPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="btnErrorPopup"
        PopupControlID="pnlErrpopup" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlErrpopup" runat="server" CssClass="modalPopup" Height="140px" Width="400px"
        Style="display: none">
        <div class="headerModal">
            Confirmation
        </div>
        <div class="body">
            <asp:Label ID="Label16" runat="server" Text="You are not authorized to view this page." />
        </div>
        <div align="center">
            <asp:Button ID="btnYesErr" runat="server" Text="Ok" OnClick="btnYesErr_Click" CssClass="yes" />
        </div>
    </asp:Panel>
    <div class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <%-- <div class="breadcrumbs" id="breadcrumbs">
                <script type="text/javascript">
                    try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
                </script>

                <ul class="breadcrumb">
                   <li>
                        <i class="ace-icon fa fa-home home-icon"></i>
                        <a href="#">Asset</a>
                    </li>
                </ul>                
            </div>--%>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group input-group-unstyled">
                                <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="GetGrid" Style="height: 34px" runat="server" OnClick="GetGrid_Click"
                                        class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div id="divSearch" runat="server" style="width: 100%;" class="panel-group col-md-9 form_wrapper">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label>
                                                Search</label>
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divSearchClose" href="#"><span id="spanSerch" style="top: 0px;"
                                                class="badge">X</span></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="container" style="width: 100%">
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="lblcattype"> <%#Assets %> Code</asp:Label>
                                                        <asp:TextBox ID="txtAssetCode" runat="server" class="form-control"
                                                            placeholder="Enter the value" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label13"> <%#Category %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlproCategory" OnSelectedIndexChanged="OnSelectedIndexChangedCategory"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label15"> <%#Location %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLocation"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label17"> <%#Building %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlbuild" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label18"><%#Floor %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlfloor" AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-0">
                                                        <asp:Label runat="server" Visible="false" ID="Label19">Department</asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel6" Visible="false" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddldept" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label20">Custodian</asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlCustodian" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label21">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="SEARCH" runat="server"
                                                            ID="btnsearchsubmit" OnClick="btnsubmit_Click" class="btn btn-primary form-control" />
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label22">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="CLEAR" runat="server"
                                                            ID="btnRefresh" class="btn btn-danger form-control" OnClick="BtnClearLocation_Click" />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>

                                    </div>
                                </div>
                            </div>
                            <!-- /.page-header -->
                            <div class="row">
                                <div class="col-xs-12">
                                    <!-- start top menu -->
                                    <div class="hidden">
                                        <uc1:topmenu runat="server" ID="topmenu" />
                                    </div>
                                </div>
                            </div>
                            <!-- /.col -->
                        </div>
                    </div>
                    <div class="page-header">
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">
                            <%#Assets %> Transfer using TSB</h1>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Button runat="server" Text="TRANSFER BY LOCATION" ID="BtnByLocation"
                                class="btn btn-primary form-control" OnClick="BtnByLocation_Click" />
                            <%--OnClick="BtnByLocation_Click"--%>
                        </div>
                        <div class="col-sm-3">
                            <asp:Button runat="server" Text="TRANSFER BY CUSTODIAN" ID="BtnByCustodian"
                                class="btn btn-primary form-control" OnClick="BtnByCustodian_Click" />
                            <%--OnClick="BtnByCustodian_Click"--%>
                        </div>
                        <div class="col-sm-8">
                        </div>
                    </div>
                    <br />
                    <div id="divbyCustodian" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="form-inline">
                                        <div class="col-sm-2" style="padding-left: 10px" class="input-group input-group-unstyled">
                                            Transfer By Custodian
                                        </div>
                                        <%-- <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divCloseCustodian" href="#"><span id="span1" style="top: 0px;"
                                                class="badge"><b>Close</b></span></a>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="container" style="width: 100%">
                                            <div class="row" style="width: 80%">
                                                <div class="col-sm-3">
                                                    <br />
                                                    <asp:Label runat="server" ID="Label23">Scan Sender and Reciever ID </asp:Label>
                                                    <img alt="" src="images/custordian.png" id="lkCust" style="cursor: pointer; font-size: 10px; border-radius: 5px" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <br />
                                                    <input type="button" style="width: 100%; height: 32px; top: -2px" value="RESET" id="btnCustodianReset" class="btn btn-primary form-control" />

                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label10"> Sender Name :</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="cboFromCustodian" Enabled="false" Width="100%" AutoPostBack="true" class="form-control"
                                                                OnSelectedIndexChanged="cboFromCustodian_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cboFromCustodian" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label1"> Reciever Name :</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="cboToCustodian" Enabled="false" Width="100%" AutoPostBack="true" class="form-control">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label5"> Reason :</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtReasonCusChange" MaxLength="100" class="form-control" runat="server"
                                                                TextMode="MultiLine" placeholder="Enter the value" AutoPostBack="false"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                            </div>
                                            <div class="row" style="width: 100%">
                                                <div class="col-sm-4"></div>
                                                <div class="col-sm-1">
                                                    <asp:Label runat="server" ID="Label25">&nbsp;</asp:Label><br />
                                                    <input type="button" value="SCAN" id="btnCustodianScan"
                                                        class="btn btn-primary form-control" onclick="ScanTag1('Custodian')" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Label runat="server" ID="Label26">&nbsp;</asp:Label><br />
                                                    <asp:Button Text="TRANSFER" runat="server" ID="btnCustodianTransfer"
                                                        CssClass="btn btn-success form-control" OnClick="btnCustodianTransfer_Click" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Label runat="server" ID="Label27">&nbsp;</asp:Label><br />
                                                    <asp:Button Text="CLEAR" runat="server" ID="btnClearCustodianTransfer"
                                                        class="btn btn-danger form-control" OnClick="btnClearCustodianTransfer_Click" />
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="divbyLocation" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="form-inline">
                                        <div class="col-sm-2" style="padding-left: 10px" class="input-group input-group-unstyled">
                                            Transfer By Location
                                        </div>
                                        <%--<div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divCloseLocation" href="#"><span id="span2" style="top: 0px;"
                                                class="badge"><b>Close</b></span></a>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="container" style="width: 100%">
                                            <div class="row" style="width: 100%">
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="Label7">Custodian</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="cboLocaCustodian" Enabled="false" AutoPostBack="true" class="form-control">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>

                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="Label28">&nbsp;</asp:Label><br />
                                                    <img alt="" src="images/custordian.png" id="lkLocCust" style="cursor: pointer; font-size: 12px; border-radius: 5px" />
                                                    <input type="button" style="width: 50%;" value="RESET" id="btnCustodianResetLoc"
                                                        class="btn btn-primary form-control" />
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label9"> To <%#Location %> </asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="cboLoc" AutoPostBack="true" class="form-control"
                                                                OnSelectedIndexChanged="OnSelectedIndexChangedToLocation">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label3"> To <%#Building %> :</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="cboBuild" AutoPostBack="true" class="form-control"
                                                                OnSelectedIndexChanged="OnSelectedIndexChangedToBuilding">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label12">  To <%#Floor %> :</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="cboFloor" AutoPostBack="true" class="form-control">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                            </div>
                                            <div class="row" style="width: 100%">
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label2">   Reason :</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtReasonLocChange" MaxLength="100" class="form-control" runat="server"
                                                                TextMode="MultiLine" placeholder="Enter the value" AutoPostBack="false"></asp:TextBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-1">
                                                    <asp:Label runat="server" ID="Label4">&nbsp;</asp:Label><br />
                                                    <input type="button" value="SCAN" id="Btn_LocationScan" onclick="ScanTag1('Location')"
                                                        class="btn btn-primary form-control" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Label runat="server" ID="Label6">&nbsp;</asp:Label><br />
                                                    <asp:Button Text="TRANSFER" runat="server" ID="Button1" OnClick="Button1_Click"
                                                        CssClass="btn btn-success form-control" />
                                                </div>
                                                <div class="col-sm-1">
                                                    <asp:Label runat="server" ID="Label24">&nbsp;</asp:Label><br />
                                                    <asp:Button Text="CLEAR" runat="server" ID="BtnClearLocation"
                                                        class="btn btn-danger form-control" OnClick="BtnClearLocation_Click" />

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                    OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                    OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">
                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <%--   <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" GridLines="None" CssClass="gvData" OnDataBinding="gvData_DataBinding" 
                                    OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">--%>
                                    <%----%>
                                    <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                    <ClientSettings EnablePostBackOnRowClick="false">
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                        <ClientEvents OnGridCreated="GridCreated" />
                                    </ClientSettings>
                                    <SortingSettings EnableSkinSortStyles="false" />
                                    <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true">
                                        <PagerStyle AlwaysVisible="true" Position="Top" />
                                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Select">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cboxSelect" runat="server" onclick="unCheckHeader(this)" />
                                                    <asp:HiddenField ID="hdnAstID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AssetId") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="AssetId" FilterControlAltText="Filter AssetId column"
                                                HeaderText="AssetId" SortExpression="AssetId" UniqueName="AssetId" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter ID column"
                                                HeaderText="ASSETCODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                                                AllowSorting="true" AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                                                HeaderText="CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubCategory" FilterControlAltText="Filter SubCategory column"
                                                HeaderText="SUBCATEGORY" Visible="false" SortExpression="SubCategory" UniqueName="SubCategory"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                                                HeaderText="LOCATION" SortExpression="Location" UniqueName="Location" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Building" FilterControlAltText="Filter Building column"
                                                HeaderText="BUILDING" SortExpression="Building" UniqueName="Building" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Floor" FilterControlAltText="Filter Floor column"
                                                HeaderText="FLOOR" SortExpression="Floor" UniqueName="Floor" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CustodianCode" FilterControlAltText="Filter CustodianCode column"
                                                HeaderText="CustodianCode" SortExpression="CustodianCode" UniqueName="CustodianCode" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Custodian" FilterControlAltText="Filter Custodian column"
                                                HeaderText="CUSTODIAN" SortExpression="Custodian" UniqueName="Custodian" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                                                HeaderText="DEPARTMENT" SortExpression="Department" Visible="false" UniqueName="Department" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                                                HeaderText="DELIVERY DATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                                                HeaderText="SERIAL NO" SortExpression="SerialNo" Visible="false" UniqueName="SerialNo" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                                HeaderText="DESCRIPTION" Visible="false" SortExpression="Description" UniqueName="Description"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                                                HeaderText="PRICE" Visible="false" SortExpression="Price" UniqueName="Price" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                                HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column1" FilterControlAltText="Filter Column1 column"
                                                HeaderText="FC Number" SortExpression="Column1" UniqueName="Column1"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column2" FilterControlAltText="Filter Column2 column"
                                                HeaderText="Case Assignee Name" SortExpression="Column2" UniqueName="Column2"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column3" FilterControlAltText="Filter Column3 column"
                                                HeaderText="Client Name" SortExpression="Column3" UniqueName="Column3"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column4" FilterControlAltText="Filter Column4 column"
                                                HeaderText="Document Controller Name" SortExpression="Column4" UniqueName="Column4"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column5" FilterControlAltText="Filter Column5 column"
                                                HeaderText="Case Manager Full Name" SortExpression="Column5" UniqueName="Column5"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column6" FilterControlAltText="Filter Column6 column"
                                                HeaderText="Case Manager Email" SortExpression="Column6" UniqueName="Column6"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column7" FilterControlAltText="Filter Column7 column"
                                                HeaderText="Case Worker 1 Name" SortExpression="Column7" UniqueName="Column7"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="Column8" FilterControlAltText="Filter Column8 column"
                                                HeaderText="Case Worker 1 Email" SortExpression="Column8" UniqueName="Column8"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column9" FilterControlAltText="Filter Column9 column"
                                                HeaderText="Case Status" SortExpression="Column9" UniqueName="Column9"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column10" FilterControlAltText="Filter Column10 column"
                                                HeaderText="Case Person Association" SortExpression="Column10" UniqueName="Column10"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column11" FilterControlAltText="Filter Column11 column"
                                                HeaderText="Column11" SortExpression="Column11" UniqueName="Column11"
                                                ReadOnly="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column12" FilterControlAltText="Filter Column12 column"
                                                HeaderText="Column12" SortExpression="Column12" UniqueName="Column12"
                                                ReadOnly="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column13" FilterControlAltText="Filter Column13 column"
                                                HeaderText="Column13" SortExpression="Column13" UniqueName="Column13"
                                                ReadOnly="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column14" FilterControlAltText="Filter Column14 column"
                                                HeaderText="Column14" SortExpression="Column14" UniqueName="Column14"
                                                ReadOnly="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column15" FilterControlAltText="Filter Column15 column"
                                                HeaderText="Column15" SortExpression="Column15" UniqueName="Column15"
                                                ReadOnly="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                            </EditColumn>
                                        </EditFormSettings>
                                    </MasterTableView>
                                    <FilterMenu EnableImageSprites="False">
                                    </FilterMenu>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="page-header">
            </div>
        </div>
    </div>
    <!-- /.page-content -->
    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel22"
        TargetControlID="btnShow" CancelControlID="btnClose">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel22" runat="server" align="center" Style="display: none" CssClass="modalPopup"
        Height="132px" Width="250px">
        <%--CssClass="modalPopup" border: 1px solid Gray; --%>
        <table style="width: 100%">
            <tr style="height: 25px;" id="trheader" runat="server">
                <td colspan="1">
                    <label id="Label8" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label></td>
                <td align="right" style="margin-right: 10px;">
                    <asp:Button ID="btnClose" runat="server" CssClass="bgimage" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" style="border: none; width: 20%;">
                    <asp:Image ID="imgpopup" runat="server" Width="50px" Height="50px" />
                    <%--ImageUrl="~/images/Success.png"--%>
                </td>
                <td align="Left" style="border: none; width: 80%;">
                    <asp:Label ID="lblpopupmsg" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" style="margin-right: 10px; border: 0px;">
                    <asp:Button ID="btnCloseinner" runat="server" Text="OK" Width="50px" BackColor="152, 192, 218"
                        CssClass="changePosition" />
                </td>
            </tr>
            <tr style="height: 0px;" id="trfooter" runat="server">
                <td colspan="2" align="right" style="margin-right: 10px;"></td>
            </tr>
        </table>
    </asp:Panel>
    <ajax:ModalPopupExtender ID="GriddetailsPopup" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Height="400px" Width="50%"
        Style="display: none">
        <%--        <script type="text/javascript">
            function GridCreated_Pop(sender, args) {
                var scrollArea = sender.GridDataDiv;
                var dataHeight = sender.get_masterTableView().get_element().clientHeight; if (dataHeight < 500) {
                    scrollArea.style.height = dataHeight + 5 + "px";
                }
            }
        </script>--%>
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-sm-1">
                        <label runat="server">
                            <b>SCAN&nbsp;ITEMS&nbsp;</b>&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </div>

                    <br />
                    <br />
                    <br />
                    <div class="col-sm-8">
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="Label11" Text="Count :" runat="server"></asp:Label>
                        <asp:Label ID="lblCount" Text="0" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lbl1" Text="Scanned Items :" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtAreaScannedItems" TextMode="MultiLine" Enabled="false" Height="200px" Width="400px" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-offset-4">
                <input type="button" id="btnStopScanning" class="btn btn-primary" onclick="StopScanning1()" runat="server" value="Stop Scaning"
                    style="font-size: 12px; border-radius: 5px" />
                <asp:Button Text=" Close" runat="server" ID="Button2" OnClientClick="javascript:return HideModalPopup();"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
                <asp:Button Text=" Confirm" runat="server" Enabled="false" ID="btnConfirm" OnClick="btnConfirm_Click"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
            </div>
        </div>
    </asp:Panel>
    <%--<ajax:ModalPopupExtender ID="GriddetailsPopup2" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup2" BackgroundCssClass="modalBackgroundN" CancelControlID="btnClose2" BehaviorID="mpe2">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup2" runat="server" CssClass="modalPopupN" Height="170px" Width="50%"
        Style="display: none">
     
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-sm-1">
                        <label runat="server">
                            <b>SCAN&nbsp;CUSTORDIAN&nbsp;</b>&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </div>

                    <br />
                    <br />
                    <br />
                    <div class="col-sm-3">
                        <asp:Label ID="lbl2" Text="From Custordian :" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtFromCustordian" Enabled="false" Height="30px" Width="300px" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-offset-4">
                <asp:Button Text=" Close" runat="server" ID="btnClose2"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
                <asp:Button Text=" Confirm" runat="server" ID="btnfromCustordian" OnClientClick="javascript:return ConfirmFrom();"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
            </div>
        </div>
    </asp:Panel>
    <ajax:ModalPopupExtender ID="GriddetailsPopup3" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup3" BackgroundCssClass="modalBackgroundN" CancelControlID="btnCancel3" BehaviorID="mpe3">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup3" runat="server" CssClass="modalPopupN" Height="170px" Width="50%"
        Style="display: none">
     
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-sm-1">
                        <label runat="server">
                            <b>SCAN&nbsp;CUSTORDIAN&nbsp;</b>&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </div>

                    <br />
                    <br />
                    <br />
                    <div class="col-sm-3">
                        <asp:Label ID="Label9" Text="TO Custordian :" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtToCustordian" Height="30px" Enabled="false" Width="300px" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-offset-4">
                <asp:Button Text=" Close" runat="server" ID="btnCancel3"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
                <asp:Button Text=" Confirm" runat="server" ID="btnConfirmTo" OnClientClick="javascript:return ConfirmTo();"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
            </div>
        </div>
    </asp:Panel>--%>
    <asp:HiddenField runat="server" ID="hfConfirmValue" />
    <asp:HiddenField runat="server" ID="hdnClientCode" />
    <asp:HiddenField runat="server" ID="HiddenField3" />
    <asp:HiddenField runat="server" ID="HdnSelectedFrom" Value="0" />
    <asp:HiddenField runat="server" ID="HdnSelectedTo" Value="0" />
    <asp:HiddenField runat="server" ID="HdnFrom" />
    <asp:HiddenField runat="server" ID="HdnTo" />
    <asp:HiddenField runat="server" ID="HdnCustLoc" />
    <asp:HiddenField runat="server" ID="HdnSelectedCustLoc" Value="0" />
    <asp:HiddenField runat="server" ID="hdnScanned" />
    <asp:HiddenField runat="server" ID="HdnLatValue" />
    <asp:HiddenField runat="server" ID="HdnLogValue" />
    <asp:HiddenField runat="server" ID="HdnLocation" />
    <asp:HiddenField runat="server" ID="HdnLastLocation" />

    <asp:HiddenField runat="server" ID="HdnError" />

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">TSB Transfer</h5>

                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <h5>
                                    <label id="lbllocname"></label>
                                </h5>
                                <%--<asp:TextBox ID="txtid" Visible="false" Width="100%" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtCategoryName" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>--%>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <div class="container-fluid">
                        <div class="row">

                            <div class="col-md-12">
                                <button type="button" class="btn btn-warning" data-dismiss="modal">OK</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function openModal() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal').modal('show');
        }
        function setvalueforlocation(lbllocname) {
            document.getElementById('lbllocname').innerHTML = lbllocname;
            //document.getElementById('lbllocname').innerHTML = lbllocname;
        }
    </script>
</asp:Content>

