<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="CustodianMaster.aspx.cs" Inherits="CustodianMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
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
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var mouseOverActiveElement = false;

        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtSearch').click(function () {
                if ($('#ContentPlaceHolder1_txtSearch').val().length > 1) {
                    $('#ContentPlaceHolder1_divSearch').hide(500);
                } else {
                    $('#ContentPlaceHolder1_divSearch').show(500);
                    $('#divUpdateWaranty').hide(500);

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
                $('#divUpdateWaranty').show(500);
            });

            $('#ContentPlaceHolder1_btnSearchInfo').click(function () {
                if ($('#ContentPlaceHolder1_txtSearch').val().length == 0) {
                    alert('Enter some text to search');
                    return false;
                }
            });

            //$('#txtserchbox').click(function () {
            //   $('#ContentPlaceHolder1_divSearch').show(500);
            //})

            //$('#divSearchClose').click(function () {
            //    $('#ContentPlaceHolder1_divSearch').hide(500);
            //});

            $(':file').on('change', ':file', function () {
                var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });
        })
    </script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
    </script>
    <script language="javascript" type="text/javascript">
        function Validate() {


            var e = document.getElementById('<%=ddldepartment.ClientID %>');
            var strUser = e.options[e.selectedIndex].text;
            var txtcat = '<%=txtFullName.ClientID %>';
            var txtdes = '<%=txtdesg.ClientID %>';

            var txtphone = '<%=txtphn.ClientID %>'
            var txtemail = '<%=txtemail.ClientID %>'

            if (strUser == '--Select--') {
                //alert("Please Select Department");

                //e.focus();

                //return false;

            }

            if (document.getElementById(txtcat).value == '') {
                //alert("Please Enter The Custodian");

                document.getElementById(txtcat).focus();

                return false;

            }
            //                        else {
            //                            var isValid = false;
            //                           var regex = /^[a-zA-Z0-9 ]*$/; 
            //                            isValid = regex.test(document.getElementById(txtcat).value);
            //                            if (isValid == false) {
            //                                alert('Special character not allowed');
            //                                document.getElementById(txtcat).focus();
            //                                return false;
            //                            }
            //                        }
            //                        if (document.getElementById(txtdes).value == '') {
            //                            alert("Please Enter The Designation");

            //                            document.getElementById(txtdes).focus();

            //                            return false;

            //                        }
            //                        else {
            //                            var isValid = false;
            //                           var regex = /^[a-zA-Z0-9 ]*$/;
            //                            isValid = regex.test(document.getElementById(txtdes).value);
            //                            if (isValid == false) {
            //                                alert('Special character not allowed');
            //                                document.getElementById(txtdes).focus();
            //                                return false;
            //                            }
            //                        }

            //                        if (document.getElementById(txtphone).value == '') {
            //                            alert("Please Enter The Phone");

            //                            document.getElementById(txtphone).focus();

            //                            return false;

            //                        }
            //                        if (document.getElementById(txtemail).value == '') {
            //                            alert("Please Enter The Email");

            //                            document.getElementById(txtemail).focus();

            //                            return false;

            //                        }
            return true;
        }

    </script>
    <script type="text/javascript" language="javascript">
        var txtemail = '<%=txtemail.ClientID %>'
        function ValidateEmail() {
            var x = document.getElementById(txtemail).value;
            var atpos = x.indexOf("@");
            var dotpos = x.lastIndexOf(".");
            if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
                alert("Not a valid e-mail address");
                document.getElementById(txtemail).focus();
                return false;
            }
        }

    </script>
    <script type="text/javascript" language="javascript">
        var txtphone = '<%=txtphn.ClientID %>'
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert("Please enter only Numbers.");
                document.getElementById('<%=txtphn.ClientID %>').focus();
                return false;
            }

            return true;
        }


    </script>
    <script type="text/javascript" language="javascript">
        function ValidateNo() {
            var txtphone = '<%=txtphn.ClientID %>'
            var phoneNo = document.getElementById(txtphone);


            if (phoneNo.value.length < 10 || phoneNo.value.length > 21) {
                alert("Mobile No. is not valid, Please Enter 10 Digit Mobile No.");
                document.getElementById('<%=txtphn.ClientID %>').focus();
                return false;
            }


            return true;
        }
    </script>
    <script type="text/javascript">

        function CheckBoxCheck(rb) {


            var gv = document.getElementById("<%=gvData.ClientID%>");

            var chk = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;

            for (var i = 0; i < chk.length; i++) {

                if (chk[i].type == "checkbox") {

                    if (chk[i].checked && chk[i] != rb) {

                        chk[i].checked = false;

                        break;
                    }

                }

            }
        }
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
        var status = new Boolean();
        var EncodeStatus = new Boolean();
        EncodeStatus = false;
        ts = document.getElementById("ctr1");

        function EncodeTag() {
            var cell = "";
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (row.length > 0) {
                for (var i = 0; i < row.length; i++) {
                    if (masterTable.get_dataItems()[i].findElement("ChkEncode").checked == true) // for checking the checkboxes
                    {
                        cell = masterTable.get_dataItems()[i].findElement("hdbCustCode").value;
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
            status = ts.EncodeTheTagTSBV2(code + cell);
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
    </script>

    <asp:Button ID="btnErrorPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnErrorPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackground" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopup" Height="140px" Width="400px"
        Style="display: none">
        <div class="headerModal">
            Confirmation
        </div>
        <div class="body">
            <asp:Label ID="lblmodmsg" runat="server" Text="You are not authorized to view this page." />
        </div>
        <div align="center">
            <asp:Button ID="btnYes" runat="server" Text="Ok" OnClick="btnYes_Click" CssClass="yes" />
        </div>
    </asp:Panel>
    <div class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content" style="width: 100%;">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group">
                                <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnSearchInfo" Style="height: 34px" runat="server" OnClick="btnSearchInfo_Click"
                                        class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton>
                                </span>
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
                                                        <h7><asp:Label runat="server" ID="Label8"> Custodian Code</asp:Label></h7>
                                                        <%--<span style="color: Red">*</span> :--%>
                                                        <asp:TextBox ID="txtCustCode" MaxLength="100" class="form-control" runat="server"
                                                            ToolTip="Enter the value" placeholder="Enter the value" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7><asp:Label runat="server" ID="Label9">Custodian Name</asp:Label></h7>
                                                        <asp:TextBox ID="txtCustName" MaxLength="100" class="form-control" runat="server" placeholder="Enter the value"
                                                            ToolTip="Enter the value" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-0">
                                                        <h7><asp:Label Visible="false" runat="server" ID="Label10">Department</asp:Label></h7>
                                                        <asp:DropDownList Visible="false" runat="server" ID="ddlCustDepartment" class="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7><asp:Label runat="server" ID="Label11">Designation</asp:Label></h7>
                                                        <asp:TextBox runat="server" ID="txtDesignation" MaxLength="100" class="form-control" placeholder="Enter the value"
                                                            onkeydown="return (event.keyCode!=13);">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7><asp:Label runat="server" ID="Label12">Phone Number</asp:Label></h7>
                                                        <asp:TextBox runat="server" ID="txtphoneno" class="form-control" placeholder="Enter the value"
                                                            onkeydown="return (event.keyCode!=13);">
                                                        </asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7> <asp:Label runat="server" ID="Label13">Email ID</asp:Label></h7>
                                                        <asp:TextBox runat="server" ID="txtEmailId" class="form-control" placeholder="Enter the value"
                                                            onkeydown="return (event.keyCode!=13);">
                                                        </asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3"></div>
                                                    <div class="col-sm-1">
                                                        <h7> Encoded :</h7>
                                                        <asp:CheckBox CssClass="form-control" ID="chkEncoded" runat="server" />
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label14">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="SEARCH" runat="server" ID="BtnSearch" OnClick="BtnSearch_Click" class="btn btn-primary form-control" />
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label15">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="CLEAR" runat="server" ID="BtnClear" OnClick="BtnClear_Click" class="btn btn-danger form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>




                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="page-header">
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">Custodian Master</h1>
                    </div>
                    <!-- /.page-header -->
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
                                    Font-Size="Medium"></asp:Label>
                            </div>
                            <div class="form-group">
                                <div class="container" style="width: 100%">
                                    <div class="row" style="width: 100%">
                                        <div class="col-sm-6">
                                            <h7><asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                                ForeColor="Red"></asp:Label></h7>
                                            <asp:Label ID="lblImgUp" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            <asp:Label runat="server" ID="showerror" Style="color: Red"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="width: 100%">
                                        <div class="col-sm-0">
                                            <h7><asp:Label runat="server" Visible="false" ID="Label1">Department</asp:Label></h7>
                                            <%--<span style="color: Red">*</span> :--%>
                                            <asp:UpdatePanel Visible="false" ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" class="form-control" ID="ddldepartment" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-sm-3">
                                            <h7><asp:Label runat="server" ID="lblcattype">Custodian Name</asp:Label></h7>
                                            <%-- <span style="color: Red">*</span> :--%>
                                            <asp:TextBox ID="txtFullName" MaxLength="100" class="form-control" runat="server"
                                                placeholder="Enter the value" AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <h7><asp:Label runat="server" ID="Label2"> Designation Name</asp:Label></h7>
                                            <%--<span style="color: Red">*</span> :--%>
                                            <asp:TextBox ID="txtdesg" class="form-control" runat="server" placeholder="Enter the value"
                                                AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <h7><asp:Label runat="server" ID="Label4">Phone Number</asp:Label></h7>
                                            <%-- <span style="color: Red"></span>:--%>
                                            <asp:TextBox ID="txtphn" class="form-control" runat="server" placeholder="Enter the value"
                                                onkeypress="javascript:return isNumber(event)" onchange="javascript:return ValidateNo();"
                                                AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <h7><asp:Label runat="server" ID="Label5">Email Id</asp:Label></h7>
                                            <%--  <span style="color: Red">*</span> :--%>
                                            <asp:TextBox ID="txtemail" class="form-control" runat="server" onchange="javascript:return ValidateEmail();"
                                                placeholder="Enter the value" AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1">
                                            Active :
                                                <asp:CheckBox CssClass="form-control" ID="chkActive" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row" style="width: 100%">
                                        <div class="col-sm-4"></div>

                                        <div class="col-sm-1">
                                            <asp:Label runat="server" ID="lblsbmt">&nbsp;</asp:Label>
                                            <asp:Button Text="SUBMIT" runat="server" ID="btnsubmit"
                                                OnClick="btnsubmit_Click" OnClientClick="javascript:return Validate();"
                                                class="btn btn-primary form-control" />
                                            <asp:HiddenField ID="hdncatidId" runat="server" />
                                            <asp:HiddenField ID="hidcatcode" runat="server" />
                                        </div>
                                        <div class="col-sm-1">
                                            <asp:Label runat="server" ID="Label6">&nbsp;</asp:Label>
                                            <asp:Button ID="btnreset" class="btn btn-danger form-control"
                                                runat="server" Text="RESET" OnClick="btnreset_Click" />
                                        </div>
                                        <div class="col-sm-1">
                                            <asp:Label runat="server" ID="Label7">&nbsp;</asp:Label>
                                            <asp:Button runat="server" ID="btnEncode" class="btn btn-success form-control" Text="ENCODE"
                                                OnClientClick="javascript:return EncodeTag()" OnClick="btnEncode_Click" />
                                        </div>
                                    </div>
                                </div>




                            </div>
                            <%--<div class="col-sm-3"  >--%>
                        </div>
                    </div>

                    <%--end main page--%>
                    <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                    <!-- /.col -->
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"
                                Width="100%"></asp:Label>
                            <asp:TextBox ID="txtPageCount" runat="server" Visible="False"></asp:TextBox>
                            <asp:Label ID="lblSort" runat="server" Visible="False"></asp:Label>
                            <asp:HiddenField ID="hdnOldCust" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                            <telerik:RadGrid ID="gvData" ClientSettings-Scrolling-AllowScroll="true" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik"
                                GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                OnItemCommand="GvData_ItemCommand">
                                <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <SortingSettings EnableSkinSortStyles="false" />
                                <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                                <AlternatingItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                                <MasterTableView AllowPaging="True" AutoGenerateColumns="false" DataKeyNames="CustodianId"
                                    CellSpacing="2">
                                    <PagerStyle AlwaysVisible="true" Position="Top" />
                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                    <RowIndicatorColumn Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridTemplateColumn AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkEncode" runat="server" onclick="CheckBoxCheck(this);"></asp:CheckBox>
                                                <asp:HiddenField ID="hdbCustId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CustodianId") %>' />
                                                <asp:HiddenField ID="hdbCustCode" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CustodianCode") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" />
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn CommandName="dit" ButtonType="ImageButton" UniqueName="column1"
                                            ImageUrl="~/images/pencil.png" HeaderStyle-Width="40px" HeaderText="EDIT">
                                            <ItemStyle Width="60px" />
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridButtonColumn>

                                        <telerik:GridTemplateColumn HeaderText="SR.NO." AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" />
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="CustodianCode" FilterControlAltText="Filter CustodianCode column"
                                            HeaderText="CUSTODIAN CODE" SortExpression="CustodianCode" UniqueName="CustodianCode"
                                            ReadOnly="true" AllowSorting="true" AllowFiltering="false">
                                            <ItemStyle Width="100px" />
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CustodianName" FilterControlAltText="Filter CustodianName column"
                                            HeaderText="CUSTODIAN NAME" SortExpression="CustodianName" UniqueName="CustodianName"
                                            ReadOnly="true">
                                            <ItemStyle Width="100px" />
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DepartmentName" FilterControlAltText="Filter DepartmentName column"
                                            HeaderText="DEPARTMENT NAME" SortExpression="DepartmentName" UniqueName="DepartmentName"
                                            ReadOnly="true" Visible="false">
                                            <ItemStyle Width="100px" />
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CustodianId" FilterControlAltText="Filter CustodianId column"
                                            HeaderText="CUSTODIANID" SortExpression="CustodianId" UniqueName="CustodianId"
                                            ReadOnly="true" Visible="false">
                                            <ItemStyle Width="120px" />
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DepartmentId" FilterControlAltText="Filter DepartmentId column"
                                            HeaderText="DEPARTMENTID" SortExpression="DepartmentId" UniqueName="DepartmentId"
                                            ReadOnly="true" Visible="false">
                                            <ItemStyle Width="120px" />
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="custodiancode" FilterControlAltText="Filter custodiancode column"
                                            HeaderText="CUSTODIANCODE" SortExpression="custodiancode" UniqueName="custodiancode"
                                            ReadOnly="true" Visible="false">
                                            <ItemStyle Width="120px" />
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Designation" FilterControlAltText="Filter Designation column"
                                            HeaderText="DESIGNATION" SortExpression="Designation" UniqueName="Designation"
                                            ReadOnly="true">
                                            <ItemStyle Width="120px" />
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Phone" FilterControlAltText="Filter Phone column"
                                            HeaderText="PHONE" SortExpression="Phone" UniqueName="Phone" ReadOnly="true">
                                            <ItemStyle Width="120px" />
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="EmailId" FilterControlAltText="Filter EmailId column"
                                            HeaderText="EMAILID" SortExpression="EmailId" UniqueName="EmailId" ReadOnly="true">
                                            <ItemStyle Width="120px" />
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                            HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true">
                                            <ItemStyle Width="90px" />
                                            <HeaderStyle Width="90px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="IsEncoded" FilterControlAltText="Filter IsEncoded column"
                                            HeaderText="ENCODED" SortExpression="IsEncoded" UniqueName="IsEncoded" ReadOnly="true">
                                            <ItemStyle Width="90px" />
                                            <HeaderStyle Width="90px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter Status column"
                                            HeaderText="CREATED BY" SortExpression="CreatedBy" UniqueName="CreatedBy" ReadOnly="true">
                                            <ItemStyle Width="90px" />
                                            <HeaderStyle Width="90px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                            HeaderText="CREATED DATE" SortExpression="CreatedDate" UniqueName="CreatedDate"
                                            ReadOnly="true">
                                            <ItemStyle Width="130px" />
                                            <HeaderStyle Width="130px" />
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
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <!-- /.span -->
        </div>
    </div>
    <!-- /.row -->
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
                    <label id="Label3" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label>
                </td>
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
    <asp:HiddenField runat="server" ID="hfConfirmValue" />
    <asp:HiddenField runat="server" ID="hdnClientCode" />
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Custodian Master</h5>

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
