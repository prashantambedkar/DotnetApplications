<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="AddAsset.aspx.cs" Inherits="AddAsset" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
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
            // debugger;
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
    <script src="css/MapCSS/Scripts/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
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
        Style="display: none">
        <div class="headerN">
            Confirmation
            <asp:ImageButton ID="btnHide" runat="server" ImageUrl="~/images/Close.gif" Style="border: 0px"
                align="right" OnClientClick="return HideModalPopup()" />
        </div>
        <div class="body">
            <asp:Label ID="lblmodmsg" runat="server" />
        </div>
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
                                <div class="col-sm-12">
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
    <div>
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

                            if (guser == '--Select Sub Category--') {
                                alert("Please Select Sub Category");

                                g.focus();

                                return false;

                            }
                            if (fuser == '--Select Location--') {
                                alert("Please Select Location");

                                f.focus();

                                return false;

                            }
                            if (huser == '--Select Building--') {
                                alert("Please Select Building");

                                h.focus();

                                return false;

                            }
                            if (Iuser == '--Select Floor--') {
                                alert("Please Select Floor");

                                I.focus();

                                return false;

                            }


                            if (Juser == '--Select Department--') {
                                alert("Please Select Department");

                                J.focus();

                                return false;

                            }
                            if (kuser == '--Select Custodian--') {
                                alert("Please Select Custodian");

                                k.focus();

                                return false;

                            }

                            if (luser == '--Select Supplier--') {
                                alert("Please Select Supplier");

                                l.focus();

                                return false;

                            }


                            //                        if (document.getElementById(txtdes).value == '') {
                            //                            alert("Please Enter The Sr.No.");

                            //                            document.getElementById(txtdes).focus();

                            //                            return false;

                            //                        }
                            if (document.getElementById(txtquant).value == '') {
                                alert("Please Enter The Quanitity");

                                document.getElementById(txtquant).focus();

                                return false;

                            }
                            if (document.getElementById(txtprice).value == '') {
                                alert("Please Enter The Price");

                                document.getElementById(txtprice).focus();

                                return false;

                            }
                            if (document.getElementById(txtcat).value == '') {
                                alert("Please Enter The Asset Description");

                                document.getElementById(txtcat).focus();

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
                                    Sub Category
                                    <asp:Label ID="Label5" Text="*" ForeColor="Red" runat="server" />
                                    :
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
                                    Select Building
                                    <asp:Label ID="Label7" Text="*" ForeColor="Red" runat="server" />
                                    :
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
                                    Select Floor
                                    <asp:Label ID="Label9" Text="*" ForeColor="Red" runat="server" />
                                    :
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
                                    Select Department
                                    <asp:Label ID="Label10" Text="*" ForeColor="Red" runat="server" />
                                    :
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
                                    Select Custodian
                                    <asp:Label ID="Label12" Text="*" ForeColor="Red" runat="server" />
                                    :
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
                                    Select Supplier
                                    <asp:Label ID="Label13" Text="*" ForeColor="Red" runat="server" />
                                    :
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
                                    <asp:HiddenField ID="hdnSerialNo" runat="server" />
                                </div>
                                <label runat="server" id="lblqty" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Quantity <span style="color: Red">*</span> :
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
                                    Price <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprice" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                </div>
                                <label runat="server" id="lbldesc" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Asset Description <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtdesc" class="form-control" MaxLength="100" TextMode="MultiLine"
                                        runat="server" placeholder="Description!" AutoPostBack="false"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label15" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1-1">
                                    Active <span style="color: Red">*</span> :
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
                                    <br />
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
                                    &nbsp; &nbsp; &nbsp;&nbsp;
                                    <asp:Button Text="Export" runat="server" ID="exptxl" OnClick="exptxl_Click" CssClass="btn" />
                                </div>
                                <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                    ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <%--end main page--%>
                    <span style="font-weight: bold;">Total Records:</span>
                    <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>
                </div>
                <!-- /.col -->
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"
                        Width="100%"></asp:Label>
                    <asp:DataGrid ID="gridlist" runat="server" AllowPaging="True" AllowSorting="True"
                        CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                        BorderStyle="None" OnEditCommand="EditDataGrid" OnSortCommand="gridlist_SortCommand"
                        OnPageIndexChanged="myDataGrid_PageChanger" PageSize="10" OnItemDataBound="gridlist_ItemDataBound">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Sr. No.">
                                <ItemTemplate>
                                    <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Category" SortExpression="Category">
                                <ItemTemplate>
                                    <asp:Label ID="CategoryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Category") %>'></asp:Label>
                                    <asp:HiddenField ID="hidcatid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CategoryId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="AssetId" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssetId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Sub Category" SortExpression="SubCategory">
                                <ItemTemplate>
                                    <asp:Label ID="subCategoryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SubCategory") %>'></asp:Label>
                                    <asp:HiddenField ID="SubCatId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "SubCatId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Location" SortExpression="Location">
                                <ItemTemplate>
                                    <asp:Label ID="Location" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location") %>'></asp:Label>
                                    <asp:HiddenField ID="hidlocid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "LocationId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Building" SortExpression="Building">
                                <ItemTemplate>
                                    <asp:Label ID="Building" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Building") %>'></asp:Label>
                                    <asp:HiddenField ID="hidBldid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "BuildingId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Floor" SortExpression="Floor">
                                <ItemTemplate>
                                    <asp:Label ID="Floor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Floor") %>'></asp:Label>
                                    <asp:HiddenField ID="hidflrid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "FloorId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Department" SortExpression="Department">
                                <ItemTemplate>
                                    <asp:Label ID="Department" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Department") %>'></asp:Label>
                                    <asp:HiddenField ID="hiddptid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "DepartmentId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Custodian" SortExpression="Custodian">
                                <ItemTemplate>
                                    <asp:Label ID="Custodian" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Custodian") %>'></asp:Label>
                                    <asp:HiddenField ID="hidcstid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CustodianId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Supplier" SortExpression="Supplier">
                                <ItemTemplate>
                                    <asp:Label ID="Supplier" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Supplier") %>'></asp:Label>
                                    <asp:HiddenField ID="hidsplrid" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "SupplierId") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Asset Id" SortExpression="AssetCode">
                                <ItemTemplate>
                                    <asp:Label ID="AssetCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssetCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Serial No" SortExpression="SerialNo">
                                <ItemTemplate>
                                    <asp:Label ID="SerialNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Item Description" SortExpression="Description">
                                <ItemTemplate>
                                    <asp:Label ID="Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Quantity" SortExpression="Quantity">
                                <ItemTemplate>
                                    <asp:Label ID="Quantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Price" SortExpression="Price">
                                <ItemTemplate>
                                    <asp:Label ID="Price" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Price") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Delivery Date" SortExpression="DeliveryDate">
                                <ItemTemplate>
                                    <asp:Label ID="DeliveryDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DeliveryDate","{0:MM-dd-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Assign Date" SortExpression="AssignDate">
                                <ItemTemplate>
                                    <asp:Label ID="AssignDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssignDate","{0:MM-dd-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="Active" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Type">
                                <ItemTemplate>
                                    <asp:Label ID="Type" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="EDIT">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" Text="View" CssClass="btn btn-xs btn-info"
                                        CommandName="Edit">
                                        <i class="ace-icon fa fa-pencil bigger-120"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="left" CssClass="GridPager1" Mode="NumericPages" />
                        <PagerStyle BackColor="#F9F9F9" ForeColor="#393939" HorizontalAlign="Center" Mode="NumericPages"
                            Font-Bold="True" />
                        <HeaderStyle BackColor="#F9F9F9" Font-Bold="True" ForeColor="#393939" Height="25px" />
                    </asp:DataGrid>
                    <asp:TextBox ID="txtPageCount" runat="server" Visible="False"></asp:TextBox>
                    <asp:Label ID="lblSort" runat="server" Visible="False"></asp:Label>
                </div>
                <%--Confirm Box--%>
                <!-- /.span -->
            </div>
            <!-- /.row -->
        </div>
        <!-- /.page-content -->
    </div>
    <div>
    </div>
</asp:Content>
