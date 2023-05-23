<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="Sendasset.aspx.cs" Inherits="Sendasset" %>

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
            height: 65%;
        }
    </style>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }
        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function SearchValidate() {


            var values = "";
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

            return true;
        }
                     
    </script>
    <%--    <script type="text/javascript">
        function checkAll(gvExample, colIndex) {
            var GridView = gvExample.parentNode.parentNode.parentNode;
            for (var i = 1; i < GridView.rows.length; i++) {
                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                chb.checked = gvExample.checked;
            }
        }

        function checkItem_All(objRef, colIndex) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var selectAll = GridView.rows[0].cells[colIndex].getElementsByTagName("input")[0];
            if (!objRef.checked) {
                selectAll.checked = false;
            }
            else {
                var checked = true;
                for (var i = 1; i < GridView.rows.length; i++) {
                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                    if (!chb.checked) {
                        checked = false;
                        break;
                    }
                }
                selectAll.checked = checked;
            }
        }
        function validateCheckBoxes() {
            var isValid = false;
            var gridView = document.getElementById('<%= gridlist.ClientID %>');
            for (var i = 1; i < gridView.rows.length - 1; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            isValid = true;
                            return true;
                        }
                    }
                }
            }
            alert("Please select atleast one checkbox");
            return false;
        }
 
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content-inner">
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
        <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="showmodal"
            PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Style="display: none;
            width: 90%; position: relative;">
            <div>
                <div class="main-content-inner">
                    <span style="font-size: large; font-family: cambriya; font-weight: bold; padding-left: 42%;
                        color: Blue;">SEARCH ASSET</span>
                    <asp:ImageButton ID="btnHide" runat="server" ImageUrl="~/images/Close.gif" Style="border: 0px"
                        align="right" OnClientClick="return HideModalPopup()" />
                    <hr />
                    <label runat="server" id="lblcattype" class="col-sm-2 control-label no-padding-right"
                        for="form-field-1">
                        Category
                        <%--<asp:Label ID="Label4" Text="*" ForeColor="Red" runat="server" />--%>
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
                        <%--<asp:Label ID="Label5" Text="*" ForeColor="Red" runat="server" />--%>
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
                    <label runat="server" id="Label6" class="col-sm-2 control-label no-padding-right"
                        for="form-field-1">
                         Department :
                    </label>
                    <div class="col-sm-3">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddldept" class="form-control">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <label runat="server" id="Label1" class="col-sm-2 control-label no-padding-right"
                        for="form-field-1">
                         Location
                        <%--<asp:Label ID="Label2" Text="*" ForeColor="Red" runat="server" />--%>
                        :
                    </label>
                    <div class="col-sm-3">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLcocation"
                                    AutoPostBack="true" class="form-control">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <label runat="server" id="lblcomposition" class="col-sm-2 control-label no-padding-right"
                        for="form-field-1">
                         Building
                        <%--<asp:Label ID="Label7" Text="*" ForeColor="Red" runat="server" />--%>
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
                    <label runat="server" id="Label8" class="col-sm-2 control-label no-padding-right"
                        for="form-field-1">
                         Floor
                        <%--<asp:Label ID="Label9" Text="*" ForeColor="Red" runat="server" />--%>
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
                    <%-- <label runat="server" id="Label3" class="col-sm-2 control-label no-padding-right"
                        for="form-field-1">
                        Asset Id
                    </label>
                    <div class="col-sm-3">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <asp:ListBox runat="server" ID="ddlAsstID" AutoPostBack="true" Style="height: 10%">
                                </asp:ListBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>--%>
                    <%--<div class="clearfix form-actions">--%>
                    <div class="col-md-offset-3 col-md-9">
                        <asp:Button Text=" Submit" runat="server" ID="btnsearchsubmit" OnClick="btnsearchsubmit_Click"
                            CssClass="btn" />
                        <asp:HiddenField ID="hdncatidId" runat="server" />
                        <asp:HiddenField ID="hidcatcode" runat="server" />
                        &nbsp; &nbsp; &nbsp;
                        <%--</div>--%>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Operation</a> </li>
                    <li><a href="#">Asset Verification - Send Asset</a> </li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Asset Verification - Send Asset</h1>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>
                    <%--start main page--%>
                    <div class="form-horizontal">
                        <div class="clearfix form-actions">
                            <div class="col-md-offset-3 col-md-9">
                                <asp:Button runat="server" ID="showmodal" CssClass="btn" Text="AdvanceSearch" 
                                    OnClientClick="return ShowModalPopup()" onclick="showmodal_Click" />
                                <asp:Button ID="btnRefresh" CssClass="btn" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                                <asp:Button ID="BtnSendTHR" CssClass="btn" runat="server" Text="Send To THR" OnClick="BtnSendTHR_Click" />
                                <%-- OnClientClick="javascript:return validateCheckBoxes()"--%>
                                <asp:Button ID="BtnSendTHS" CssClass="btn" runat="server" Text="Send To THS" OnClick="BtnSendTHS_Click" />
                                <%-- OnClientClick="javascript:return validateCheckBoxes()"--%>
                                <asp:Button ID="BtnMasterDownload" CssClass="btn" runat="server" Text="DownloadMaster"
                                    OnClick="BtnMasterDownload_Click" Visible="false" />
                            </div>
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12">
                    <span style="font-weight: bold;">Total Records.</span>
                    <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>
                </div>
                <div class="col-xs-12">
                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"
                        Width="100%"></asp:Label>
                    <div id="divGrid" style="overflow: auto; height: 0px">
                    </div>
                </div>
                <%--end main page--%>
            </div>
        </div>
    </div>
    <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
        CellSpacing="0" GridLines="None" CssClass="gvData">
        <%----%>
        <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
        <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
        <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px">
        </HeaderStyle>
        <ClientSettings EnablePostBackOnRowClick="false">
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
            <ClientEvents OnGridCreated="GridCreated" />
        </ClientSettings>
        <SortingSettings EnableSkinSortStyles="false" />
        <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" DataKeyNames="AssetId">
            <PagerStyle AlwaysVisible="true" Position="Top" />
            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                    SortExpression="ID" UniqueName="ID" ReadOnly="true" AllowSorting="true" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AssetId" FilterControlAltText="Filter AssetId column"
                    HeaderText="AssetId" SortExpression="AssetId" UniqueName="AssetId" ReadOnly="true"
                    AllowSorting="true" AllowFiltering="false" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter ID column"
                    HeaderText="ASSETCODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                    AllowSorting="true" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                    HeaderText="SERIALNO" SortExpression="SerialNo" UniqueName="SerialNo" ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                    HeaderText="DESCRIPTION" SortExpression="Description" UniqueName="Description" AllowSorting="true"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                    HeaderText="PRICE" SortExpression="Price" UniqueName="Price" ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                    HeaderText="CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SubCategory" FilterControlAltText="Filter SubCategory column"
                    HeaderText="SUBCATEGORY" SortExpression="SubCategory" UniqueName="SubCategory"
                    ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                    HeaderText="LOCATION" SortExpression="Location" UniqueName="SubCategory" ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Building" FilterControlAltText="Filter Building column"
                    HeaderText="BUILDING" SortExpression="Building" UniqueName="Building" ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Floor" FilterControlAltText="Filter Floor column"
                    HeaderText="FLOOR" SortExpression="Floor" UniqueName="Floor" ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                    HeaderText="DEPARTMENT" SortExpression="Department" UniqueName="Department" ReadOnly="true" AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                    HeaderText="DELIVERYDATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                    ReadOnly="true" AllowSorting="true">
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
