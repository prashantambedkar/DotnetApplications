<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewAsset.aspx.cs" MasterPageFile="~/adminMasterPage.master"
    EnableEventValidation="false" Inherits="AddNewAsset" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>
    <style type="text/css">
        @media only screen and (min-width: 480px) and (max-width: 767px)
        {
            .additionalColumn
            {
                display: none !important;
            }
        }
    </style>
    <style type="text/css">
        .gvData
        {
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
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modalBackgroundN
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        .modalPopupN
        {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
        }
        .modalPopup .headerN
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .bodyN
        {
            min-height: 70px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .footerN
        {
            padding: 3px;
        }
        .modalPopup .yesN, .modalPopup .noN
        {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
        }
        .modalPopup .yesN
        {
            background-color: #2FBDF1;
            border: 1px solid #0DA9D0;
        }
        .modalPopup .noN
        {
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }
    </style>
    <script src="css/MapCSS/Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        var StartDate = new Date();
        $(function () {
            $("[id$=txtdeldate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
                setDate: StartDate
            });
            $("[id$=txtass]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
            });

            //            var startdate = new Date();
            //            $("[id$=txtdeldate]").datepicker("setDate", startdate);
            //            $("[id$=txtass]").datepicker("setDate", startdate);
        });
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }
        function HideMapModalPopup() {
            $find("mpeNew").hide();
            return false;
        }
    
    </script>
    <script type="text/javascript">

        function validatefile() {

            var array = ['xls', 'xlsx'];

            var xyz = document.getElementById('<%=productimguploder.ClientID %>');

            var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();

            if (array.indexOf(Extension) <= -1) {

                alert("Please Upload only .xls or .xlsx extension file");
                document.getElementById('<%=productimguploder.ClientID %>').focus();
                return false;

            }
        }         

    </script>
    <script type="text/javascript">

        function myFunction() {
            var e = document.getElementById('<%=ddlproCategory.ClientID %>');
            var strUser = e.options[e.selectedIndex].text;
            debugger;
            if (strUser != '--Select Category--') {
                document.getElementById('<%=btnImport.ClientID %>').disabled = true;
            }
            else {
                document.getElementById('<%=btnImport.ClientID %>').disabled = false;
            }

        }

        function MappingValidate() {
            var Category = document.getElementById('<%=ddl1.ClientID %>');
            var strCategory = Category.options[Category.selectedIndex].text;
            var Location = document.getElementById('<%=ddl3.ClientID %>');
            var strLocation = Location.options[Location.selectedIndex].text;
            if (strCategory == '-Select-') {
                alert("Please Select Category");
                Category.focus();
                return false;
            }

            if (strLocation == '-Select-') {
                alert("Please Select Location");
                Location.focus();
                return false;
            }

            return true;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Height="181px" Width="400px"
        Style="display: none;text-align: justify;">
        <div class="headerN" style="text-align: center">
            Confirmation
            <asp:ImageButton ID="btnHide" Visible="false" runat="server" ImageUrl="~/images/Close.gif"
                Style="border: 0px" align="right" OnClientClick="return HideModalPopup()" />
        </div>
        <br />
        <div class="body">
            <asp:Label ID="lblmodmsg" runat="server" />
        </div>
        <br />
        <div align="center">
            <asp:Button ID="btnYes" runat="server" Text="Yes" Visible="false" OnClick="btnYes_Click"
                CssClass="yesN" />
            <asp:Button ID="btnNo" runat="server" Text="Close" OnClick="btnNo_Click" CssClass="noN" />
        </div>
    </asp:Panel>
    <asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowPopup1"
        PopupControlID="pnlpopupNew" BackgroundCssClass="modalBackground" BehaviorID="mpeNew">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopupNew" runat="server" CssClass="modalPopup" Width="850px" Height="600px"
        ScrollBars="Vertical" Style="display: none">
        <div class="modal-dialog" style="width: 850px;">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="h1" runat="server" class="modal-title">
                        Map xls Columns with DataBase Columns
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlCatName" Text="Category Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl1" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlSubCatName" Text="Sub Category Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl7" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlBuildingName" Text="Building Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl10" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlFloorName" Text="Floor Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl5" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlLocationName" Text="Location Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl3" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="LtlDeptName" Text="Department Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl12" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlCustodianName" Text="Custodian Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl14" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlSupplierName" Text="Supplier Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl16" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlSerialNo" Text="Serial No"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl18" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlDecription" Text="Decription"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl19" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlQuantity" Text="Quantity"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl20" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlPrice" Text="Price"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl21" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlDeliveryDate" Text="Delivery Date"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl22" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlAssignDate" Text="AssignDate"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl23" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="llActive" Text="Active"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl30" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                &nbsp;
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn1" Text="Column1"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl24" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn2" Text="Column2"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl4" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn3" Text="Column3"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl11" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn4" Text="Column4"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl17" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn5" Text="Column5"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl6" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn6" Text="Column6"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl2" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn7" Text="Column7"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl8" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn8" Text="Column8"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl9" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn9" Text="Column9"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl13" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn10" Text="Column10"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl15" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn11" Text="Column11"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl25" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn12" Text="Column12"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl26" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn13" Text="Column13"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl27" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn14" Text="Column14"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl28" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn15" Text="Column15"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl29" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                &nbsp;
                            </div>
                            <div class="form-group">
                                <div class="col-sm-6">
                                    <div>
                                        <asp:Button Text="Submit" runat="server" ID="btnMapping" OnClientClick="javascript:return MappingValidate();"
                                            CssClass="btn" OnClick="btnMapping_Click" />
                                        <asp:Button Text="Close" runat="server" ID="BtnClose" OnClientClick="return HideMapModalPopup()"
                                            CssClass="btn" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </asp:Panel>
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <script type="text/javascript">
                    try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
                </script>
                <script language="javascript" type="text/javascript">
                    function Validate() {



                        var e = document.getElementById('<%=ddlproCategory.ClientID %>');
                        var strUser = e.options[e.selectedIndex].text;
                        var g = document.getElementById('<%=ddlsubcat.ClientID %>');
                        var guser = g.options[g.selectedIndex].text;


                        var f = document.getElementById('<%=ddlloc.ClientID %>');
                        var fuser = f.options[f.selectedIndex].text;

                        var h = document.getElementById('<%=ddlbuild.ClientID %>');
                        var huser = h.options[h.selectedIndex].text;
                        var I = document.getElementById('<%=ddlfloor.ClientID %>');
                        var Iuser = I.options[I.selectedIndex].text;
                        var J = document.getElementById('<%=ddldept.ClientID %>');
                        var Juser = J.options[J.selectedIndex].text;
                        var k = document.getElementById('<%=ddlcust.ClientID %>');

                        var kuser = k.options[k.selectedIndex].text;

                        var l = document.getElementById('<%=DropDownList1.ClientID %>');
                        var luser = l.options[l.selectedIndex].text;
                        var txtcat = '<%=txtdesc.ClientID %>';
                        var txtdes = '<%=txtserail.ClientID %>';
                        var txtprice = '<%=txtprice.ClientID %>';
                        var txtquant = '<%=txtquant.ClientID %>';






                        if (strUser == '--Select Category--') {
                            alert("Please Select Category");

                            e.focus();

                            return false;

                        }


                        if (fuser == '--Select Location--') {
                            alert("Please Select Location");

                            f.focus();

                            return false;

                        }
                        return true;
                    }
                     
                </script>
                <script type="text/javascript" language="javascript">
                    //                        var txtphone = '<%=txtquant.ClientID %>'
                    function isNumber(evt) {
                        evt = (evt) ? evt : window.event;
                        var charCode = (evt.which) ? evt.which : evt.keyCode;
                        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                            alert("Please enter only Numbers.");
                            document.getElementById('<%=txtquant.ClientID %>').focus();
                            return false;
                        }

                        return true;
                    }

                      
                </script>
                <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                <script type="text/javascript">
                    $(document).ready(function () {
                        $("#btnImport").click(function () {
                            alert(1);
                            var allowedFiles = [".xls", ".xlsx"];
                            var fileUpload = $("#productimguploder");
                            //        var lblError = $("#lblError");
                            var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
                            if (!regex.test(fileUpload.val().toLowerCase())) {
                                alert("Please upload files having extensions: <b>" + allowedFiles.join(', ') + "</b> only.");
                                document.getElementById('<%=productimguploder.ClientID %>').focus();
                                return false;
                            }

                            return true;
                        });
                    });
                </script>
                <ul class="breadcrumb">
                    <li><a href="#">Master</a> </li>
                    <li><a href="#">Asset Master</a> </li>
                    <li><a href="#">Add Asset</a> </li>
                </ul>
                <!-- /.breadcrumb -->
                <!-- /.nav-search -->
            </div>
            <div class="page-header">
                <h1>
                    Asset Master</h1>
            </div>
            <!-- /.page-header -->
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>
                    <%--end top menu--%>
                    <%--start main page--%>
                    <div class="form-horizontal">
                        <div class="col-sm-12" style="text-align: center">
                            <asp:Label runat="server" ID="lblSuccessMessage" Style="color: green" Font-Bold="true"
                                Font-Size="Medium" Visible="false"></asp:Label>
                        </div>
                        <div class="form-group" id="SearchDiv" runat="server">
                            <asp:Label ID="lblImgUp" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label>
                            <asp:Label runat="server" ID="showerror" Style="color: Red"></asp:Label>
                            <label runat="server" id="lblcattype" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Category Type
                                <asp:Label ID="Label4" Text="*" ForeColor="Red" runat="server" />
                                :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlproCategory" OnSelectedIndexChanged="OnSelectedIndexChangedCategory"
                                            AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <label runat="server" id="lblsubcattype" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1-1">
                                Sub Category :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlsubcat" AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--<hr />--%>
                            <%--<div class="form-group">--%>
                            <label runat="server" id="Label1" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Select Location
                                <asp:Label ID="Label2" Text="*" ForeColor="Red" runat="server" />
                                :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLocation"
                                            AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <label runat="server" id="lblcomposition" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Select Building :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlbuild" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding"
                                            AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <br />
                            <label runat="server" id="Label8" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Select Floor :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlfloor" AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--</div>--%>
                            <%--<div class="space-4"></div>

                        <div class="space-4"></div>--%>
                            <%--<div class="form-group">--%>
                            <label runat="server" id="Label6" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Select Department :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" OnSelectedIndexChanged="OnSelectedIndexChangedDepartment"
                                            ID="ddldept" AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--</div>--%>
                            <%--<div class="space-4"></div>--%>
                            <%--<div class="form-group">--%>
                            <label runat="server" id="Label11" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Select Custodian :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlcust" AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <label runat="server" id="Label3" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Select Supplier :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="DropDownList1" AutoPostBack="true" class="form-control">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--</div>--%>
                            <%--<div class="space-4"></div>--%>
                            <%--<div class="form-group">--%>
                            <label runat="server" id="lblsr" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Serial No.:
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtserail" class="form-control" runat="server" MaxLength="20" placeholder="Sr. No."
                                    AutoPostBack="false"></asp:TextBox>
                                <%-- <asp:HiddenField ID="hdnSerialNo" runat="server" />--%>
                            </div>
                            <label runat="server" id="lblqty" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Quantity :
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtquant" onkeypress="javascript:return isNumber(event)" MaxLength="10000"
                                    class="form-control" runat="server" placeholder="Quantity!" AutoPostBack="false"></asp:TextBox>
                            </div>
                            <label runat="server" id="lbldeldate" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Delivery Date:
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtdeldate" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                            </div>
                            <%--</div>--%>
                            <%--<div class="form-group">--%>
                            <label runat="server" id="lblassdt" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Assign Date:
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtass" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                            </div>
                            <label runat="server" id="Label14" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Price :
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtprice" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                            </div>
                            <label runat="server" id="lbldesc" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Asset Description :
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtdesc" class="form-control" MaxLength="100" TextMode="MultiLine"
                                    runat="server" placeholder="Description!" AutoPostBack="false"></asp:TextBox>
                            </div>
                            <label runat="server" id="Label15" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1-1">
                                Active :
                            </label>
                            <div class="col-sm-3">
                                <div class="checkbox">
                                    <label>
                                        <asp:CheckBox ID="chkstatus" runat="server" Class="ace" />
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="divImport" runat="server">
                            <div class="col-xs-12">
                                <label runat="server" id="lblimgbrs" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Import File <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:FileUpload runat="server" ID="productimguploder" class="id-input-file-3" />
                                    <asp:Image ID="mimage" runat="server" Style="width: 200px; width: 150px;" Visible="false" />
                                </div>
                            </div>
                        </div>
                        <hr />
                        <div class="clearfix form-actions">
                            <div class="col-md-offset-3 col-md-9">
                                <asp:Button Text="Submit" runat="server" ID="btnsubmit" OnClick="btnsubmit_Click"
                                    OnClientClick="javascript:return Validate();" CssClass="btn" />
                                <asp:HiddenField ID="hdncatidId" runat="server" />
                                <asp:HiddenField ID="hidcatcode" runat="server" />
                                &nbsp; &nbsp; &nbsp;
                                <asp:Button ID="btnreset" CssClass="btn" runat="server" Text="Reset" OnClick="btnreset_Click" />
                                &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                                &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
                                <asp:Button Text="Import" runat="server" ID="btnImport" OnClick="btnImport_Click"
                                    OnClientClick="javascript:return validatefile();" CssClass="btn" />
                                <asp:Button Text="Export" runat="server" ID="exptxl" OnClick="exptxl_Click" CssClass="btn" />
                            </div>
                            <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                </div>
                <span style="font-weight: bold;">Total Records:</span>
                <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>
            </div>
        </div>
    </div>
    <br />
    <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
        CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data_ItemCommand" OnPageIndexChanged="gvData_PageIndexChanged">
        <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
        <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
        <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px">
        </HeaderStyle>
        <ClientSettings EnablePostBackOnRowClick="false">
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
            <ClientEvents OnGridCreated="GridCreated" />
        </ClientSettings>
        <SortingSettings EnableSkinSortStyles="false" />
        <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false"
        AllowSorting="true">
            <PagerStyle AlwaysVisible="true" Position="Top" />
            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <Columns>
                <%--                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                    SortExpression="ID" UniqueName="ID" ReadOnly="true" AllowSorting="false" AllowFiltering="false"
                    Visible="false">
                </telerik:GridBoundColumn>--%>
                <telerik:GridBoundColumn DataField="AssetId" FilterControlAltText="Filter AssetId column"
                    HeaderText="AssetId" SortExpression="AssetId" UniqueName="AssetId" ReadOnly="true"
                    AllowSorting="false" AllowFiltering="false" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridButtonColumn CommandName="dit" ButtonType="ImageButton" UniqueName="Edit"
                    ImageUrl="~/images/pencil.png">
                </telerik:GridButtonColumn>
                <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter ID column"
                    HeaderText="ASSETCODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                    AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                    HeaderText="CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SubCategory" FilterControlAltText="Filter SubCategory column"
                    HeaderText="SUBCATEGORY" SortExpression="SubCategory" UniqueName="SubCategory"
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
                <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                    HeaderText="DEPARTMENT" SortExpression="Department" UniqueName="Department" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Custodian" FilterControlAltText="Filter Custodian column"
                    HeaderText="CUSTODIAN" SortExpression="Custodian" UniqueName="Custodian" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Supplier" FilterControlAltText="Filter Supplier column"
                    HeaderText="SUPPLIER" SortExpression="Supplier" UniqueName="Supplier" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                    HeaderText="SERIALNO" SortExpression="SerialNo" UniqueName="SerialNo" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                    HeaderText="DESCRIPTION" SortExpression="Description" UniqueName="Description"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column"
                    HeaderText="QUANTITY" SortExpression="Quantity" UniqueName="Quantity" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                    HeaderText="PRICE" SortExpression="Price" UniqueName="Price" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                    HeaderText="DELIVERYDATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ASSIGNDATE" FilterControlAltText="Filter AssignDate column"
                    HeaderText="ASSIGNDATE" SortExpression="AssignDate" UniqueName="AssignDate" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                    HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true">
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
</asp:Content>
