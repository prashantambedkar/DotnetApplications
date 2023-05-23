<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="SuplierMaster.aspx.cs" Inherits="SuplierMaster" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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


            //            $('#txtserchbox').click(function () {
            //               // $('#ContentPlaceHolder1_divSearch').show(500);
            //            })

            //            $('#divSearchClose').click(function () {
            //                $('#ContentPlaceHolder1_divSearch').hide(500);
            //            });
            //            $('#ContentPlaceHolder1_chkActive').prop("checked", true);
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
            var txtcat = '<%=txtFullName.ClientID %>';
            var txtcnname = '<%=txtcnname.ClientID %>';

            var txtphone = '<%=txtphn.ClientID %>'
            var txtemail = '<%=txtemail.ClientID %>'
            var txtadd = '<%=txtadd.ClientID %>'
            var txtrmk = '<%=txtrmk.ClientID %>'


            if (document.getElementById(txtcat).value == '') {
                //alert("Please Enter The supplier");

                document.getElementById(txtcat).focus();

                return false;

            }
            else {
                var isValid = false;
                var regex = /^[a-zA-Z0-9 ]*$/;
                isValid = regex.test(document.getElementById(txtcat).value);
                if (isValid == false) {
                    alert('Special character not allowed');
                    document.getElementById(txtcat).focus();
                    return false;
                }
            }
            if (document.getElementById(txtcnname).value == '') {
                //alert("Please Enter The Contact Person Name");

                document.getElementById(txtcnname).focus();

                return false;

            }
            else {
                var isValid = false;
                var regex = /^[a-zA-Z0-9 ]*$/;
                isValid = regex.test(document.getElementById(txtcnname).value);
                if (isValid == false) {
                    alert('Special character not allowed');
                    document.getElementById(txtcnname).focus();
                    return false;
                }
            }
            if (document.getElementById(txtemail).value == '') {
                //alert("Please Enter The Email");

                document.getElementById(txtemail).focus();

                return false;

            }

            if (document.getElementById(txtphone).value == '') {
                //alert("Please Enter The Phone");

                document.getElementById(txtphone).focus();

                return false;

            }

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
    <div class="main-content-inner" style="font-family: Calibri;font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
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
                        <div id="divSearch" runat="server" class="panel-group col-md-7 form_wrapper">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label>
                                                Search</label>
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divSearchClose" href="#"><span id="spanSerch" style="top: 0px;"
                                                class="badge"><b>Close</b></span></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                Supplier Name:</label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtserchbox" CssClass="text-control" runat="server" onkeydown="return (event.keyCode!=13);" />
                                            </div>
                                            <div class="col-sm-5 pull-right">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-primary" Text="search" runat="server"
                                                    OnClick="btnSearch_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="page-header">
                        <h1>Supplier Master</h1>
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
                                <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                    ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblImgUp" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label>
                                <asp:Label runat="server" ID="showerror" Style="color: Red"></asp:Label>
                                <label runat="server" id="lblcattype" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Supplier Name <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtFullName" MaxLength="50" class="form-control" runat="server"
                                        placeholder="Enter the value" AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label2" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Contact Person <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcnname" MaxLength="50" class="form-control" runat="server" placeholder="Enter the value"
                                        AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label5" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Email Id <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtemail" class="form-control" runat="server" onchange="javascript:return ValidateEmail();"
                                        placeholder="Enter the value" AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label4" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Phone Number <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtphn" MaxLength="20" class="form-control" runat="server" placeholder="Enter the value"
                                        onkeypress="javascript:return isNumber(event)" onchange="javascript:return ValidateNo();"
                                        AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label1" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Address :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtadd" MaxLength="100" class="form-control" runat="server" TextMode="MultiLine"
                                        placeholder="Enter the value" AutoPostBack="false"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label6" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Remarks :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtrmk" MaxLength="100" class="form-control" runat="server" TextMode="MultiLine"
                                        placeholder="Enter the value" AutoPostBack="false"></asp:TextBox>
                                </div>
                                <label style="padding-top: 7px; text-align: right" class="col-sm-1" for="name">
                                    Active :</label>
                                <div class="col-sm-1" style="padding-top: 7px;">
                                    <asp:CheckBox ID="chkActive" runat="server" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button Text="Submit" runat="server" ID="btnsubmit" OnClick="btnsubmit_Click"
                                        Style="height: 35px; border-radius: 5px; padding: 0" OnClientClick="javascript:return Validate();"
                                        class="btn btn-primary" />
                                    <asp:HiddenField ID="hdncatidId" runat="server" />
                                    <asp:HiddenField ID="hidcatcode" runat="server" />
                                    <asp:Button ID="btnreset" class="btn btn-primary" runat="server" Text="Reset" OnClick="btnreset_Click"
                                        Style="height: 35px; border-radius: 5px; padding: 0" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--end main page--%>
                    <span style="font-weight: bold;">Total Records.</span>
                    <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>
                    <!-- /.col -->
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"
                                Width="100%"></asp:Label>
                        </div>
                        <!-- /.span -->
                    </div>
                    <div>
                        <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                            OnItemCommand="GvData_ItemCommand">

                            <SortingSettings EnableSkinSortStyles="false" />
                            <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                            <AlternatingItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                            <MasterTableView AllowPaging="True" AutoGenerateColumns="false" DataKeyNames="SupplierId"
                                AllowSorting="true" CellSpacing="2">
                                <PagerStyle AlwaysVisible="true" Position="Top" />
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <Columns>
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
                                    <telerik:GridBoundColumn DataField="SupplierCode" FilterControlAltText="Filter SupplierCode column"
                                        HeaderText="SUPPLIER ID" SortExpression="SupplierCode" UniqueName="SupplierCode"
                                        ReadOnly="true" AllowSorting="true" AllowFiltering="false">
                                        <ItemStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SupplierName" FilterControlAltText="Filter SupplierName column"
                                        HeaderText="SUPPLIER NAME" SortExpression="SupplierName" UniqueName="SupplierName"
                                        ReadOnly="true">
                                        <ItemStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ContactPersonName" FilterControlAltText="Filter ContactPersonName column"
                                        HeaderText="CONTACT PERSON" SortExpression="ContactPersonName" UniqueName="ContactPersonName"
                                        ReadOnly="true">
                                        <ItemStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="EmailId" FilterControlAltText="Filter EmailId column"
                                        HeaderText="EMAIL ID" SortExpression="EmailId" UniqueName="EmailId" ReadOnly="true">
                                        <ItemStyle Width="120px" />
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="PhoneNo" FilterControlAltText="Filter PhoneNo column"
                                        HeaderText="PHONE NO" SortExpression="PhoneNo" UniqueName="PhoneNo" ReadOnly="true">
                                        <ItemStyle Width="120px" />
                                        <HeaderStyle Width="120px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Address" FilterControlAltText="Filter Address column"
                                        HeaderText="ADDRESS" SortExpression="Address" UniqueName="Address" ReadOnly="true">
                                        <ItemStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Remarks" FilterControlAltText="Filter Remarks column"
                                        HeaderText="REMARKS" SortExpression="Remarks" UniqueName="Remarks" ReadOnly="true">
                                        <ItemStyle Width="130px" />
                                        <HeaderStyle Width="130px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                        HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true">
                                        <ItemStyle Width="90px" />
                                        <HeaderStyle Width="90px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SupplierCode" FilterControlAltText="Filter SupplierCode column"
                                        HeaderText="STATUS" SortExpression="SupplierCode" UniqueName="SupplierCode" ReadOnly="true"
                                        Visible="false">
                                        <ItemStyle Width="90px" />
                                        <HeaderStyle Width="90px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SupplierId" FilterControlAltText="Filter SupplierId column"
                                        HeaderText="SupplierId" SortExpression="SupplierId" UniqueName="SupplierId" ReadOnly="true"
                                        Visible="false">
                                        <ItemStyle Width="90px" />
                                        <HeaderStyle Width="90px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter Status column"
                                        HeaderText="CreatedBy" SortExpression="CreatedBy" UniqueName="CreatedBy" ReadOnly="true">
                                        <ItemStyle Width="90px" />
                                        <HeaderStyle Width="90px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                        HeaderText="Created Date" SortExpression="CreatedDate" UniqueName="CreatedDate"
                                        ReadOnly="true">
                                        <ItemStyle Width="150px" />
                                        <HeaderStyle Width="150px" />
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
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:TextBox ID="txtPageCount" runat="server" Visible="False"></asp:TextBox>
    <asp:Label ID="lblSort" runat="server" Visible="False"></asp:Label>
    <asp:HiddenField ID="hdnOldSuppName" runat="server" />
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
</asp:Content>
