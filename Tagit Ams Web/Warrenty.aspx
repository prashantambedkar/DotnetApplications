<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="Warrenty.aspx.cs" Inherits="Warrenty" %>

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
    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
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
                var hidden = document.getElementById("HiddenField3");
                var checkBox = document.getElementById(hidden.value);
                checkBox.checked = false;
            }
        }
    </script>
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
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
            height: 65%;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 70px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .footer
        {
            padding: 3px;
        }
        .modalPopup .yes, .modalPopup .no
        {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
        }
        .modalPopup .yes
        {
            background-color: #2FBDF1;
            border: 1px solid #0DA9D0;
        }
        .modalPopup .no
        {
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }
    </style>
    <script type="text/javascript">
        function HideModalPopup() {
            $find("mpe").hide();
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

            //            if (strUser == '--Select Category--') {
            //                alert("Please Select Category");

            //                e.focus();

            //                return false;

            //            }

            //            if (fuser == '--Select Location--') {
            //                alert("Please Select Location");

            //                f.focus();

            //                return false;

            //            }

            return true;
        }
                     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="showmodal"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackground" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopup" Style="display: none;
        width: 90%; position: relative;">
        <div>
            <div class="main-content-inner">
                <span style="font-size: large; font-family: cambriya; font-weight: bold; padding-left: 42%;
                    color: Blue;">FILTER ASSETS FOR WARRANTY/AMC INFO</span>
                <asp:ImageButton ID="btnHide" runat="server" ImageUrl="~/images/Close.gif" Style="border: 0px"
                    align="right" OnClientClick="return HideModalPopup()" />
                <hr />
                <label runat="server" id="Label3" class="col-sm-2 control-label no-padding-right"
                    for="form-field-1">
                    Asset Code :
                </label>
                <div class="col-sm-3">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtAssetCode" runat="server" Style="width: 215px"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <label runat="server" id="lblcattype" class="col-sm-2 control-label no-padding-right"
                    for="form-field-1">
                    Category Type
                    <asp:Label ID="Label4" Text="" ForeColor="Red" runat="server" />
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
                    <asp:Label ID="Label5" Text="" ForeColor="Red" runat="server" />
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
                    Location
                    <asp:Label ID="Label2" Text="" ForeColor="Red" runat="server" />
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
                    Building
                    <asp:Label ID="Label7" Text="" ForeColor="Red" runat="server" />
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
                    Floor
                    <asp:Label ID="Label9" Text="" ForeColor="Red" runat="server" />
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
                    Department
                    <asp:Label ID="Label10" Text="" ForeColor="Red" runat="server" />
                    :
                </label>
                <div class="col-sm-3">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddldept" class="form-control">
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
                                        <asp:ListBox runat="server" 
                                            ID="ddlAsstID" AutoPostBack="true" style="height:10%" >
                                        </asp:ListBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>--%>
                <%--<div class="clearfix form-actions">--%>
                <div class="col-md-offset-3 col-md-9">
                    <asp:Button Text=" Search Submit" runat="server" ID="btnsearchsubmit" OnClick="btnsearchsubmit_Click"
                        OnClientClick="javascript:return SearchValidate();" CssClass="btn" />
                    <asp:HiddenField ID="hdncatidId" runat="server" />
                    <asp:HiddenField ID="hidcatcode" runat="server" />
                    &nbsp; &nbsp; &nbsp;
                    <%--</div>--%>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div>
        <div class="main-content-inner">
            <div class="page-content">
                <div class="breadcrumbs" id="breadcrumbs">
                    <script type="text/javascript">
                        try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
                    </script>
                    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
                    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
                        type="text/javascript"></script>
                    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
                        rel="Stylesheet" type="text/css" />
                    <script type="text/javascript">
                        $(function () {
                            $("[id$=txtstartdate]").datepicker({
                                showOn: 'button',
                                buttonImageOnly: true,
                                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
                            });
                            $("[id$=txtenddate]").datepicker({
                                showOn: 'button',
                                buttonImageOnly: true,
                                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
                            });
                        });
                    </script>
                    <%--,--%>
                    <script type="text/javascript" language="javascript">
                        function Validate() {
                            var z = document.getElementById('<%=ddlsupplier.ClientID %>');
                            var zuser = z.options[z.selectedIndex].text;
                            var txtwarr = '<%=txtwarr.ClientID %>';

                            if (zuser == '--Select Supplier--') {
                                alert("Please Select Supplier");

                                z.focus();

                                return false;

                            }
                            if (document.getElementById(txtwarr).value == '') {
                                alert("Please Enter The AMC Type");

                                document.getElementById(txtwarr).focus();

                                return false;
                            }

                            return true;
                        }
               
                    </script>
                    <script type="text/javascript">
                        function checkAll(gvExample, colIndex) {
                            var GridView = gvExample.parentNode.parentNode.parentNode;
                            for (var i = 1; i < GridView.rows.length - 1; i++) {
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
                                for (var i = 1; i < GridView.rows.length - 1; i++) {
                                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                                    if (!chb.checked) {
                                        checked = false;
                                        break;
                                    }
                                }
                                selectAll.checked = checked;
                            }
                        }
 
                    </script>
                    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
                    <ul class="breadcrumb">
                        <li><a href="#">Master </a></li>
                        <li><a href="#">Asset Master </a></li>
                        <li><a href="#">Warranty/ AMC Info</a> </li>
                    </ul>
                </div>
                <div class="page-header">
                    <h1>
                        Warranty/ AMC Info</h1>
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
                            <div class="form-group">
                                <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                    ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblImgUp" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label>
                                <asp:Label runat="server" ID="showerror" Style="color: Red"></asp:Label>
                                <label runat="server" id="Label12" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Supplier Name
                                    <asp:Label ID="Label13" Text="*" ForeColor="Red" runat="server" />
                                    :
                                </label>
                                <div class="col-sm-3">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlsupplier" AutoPostBack="true" class="form-control">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <label runat="server" id="lblassdt" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Warranty/AMC Type <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtwarr" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                </div>
                                <label runat="server" id="lbldeldate" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    AMC Start Date <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtstartdate" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label14" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    AMC End Date <span style="color: Red">*</span> :
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtenddate" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label15" class="col-sm-2 control-label no-padding-right"
                                    for="form-field-1">
                                    Remark
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtrmk" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                </div>
                            </div>
                            <hr />
                            <div class="clearfix form-actions">
                                <div class="col-md-offset-3 col-md-9">
                                    <asp:Button runat="server" ID="showmodal" CssClass="btn" Text="Filter Asset" OnClientClick="javascript:$find('mpe').show();" />
                                    <asp:Button Text="Submit" runat="server" ID="btnsubmit" OnClick="btnsubmit_Click"
                                        OnClientClick="javascript:return Validate();" CssClass="btn" />
                                    <asp:Button ID="btnreset" CssClass="btn" runat="server" Text="Reset" OnClick="btnreset_Click" />
                                    <asp:Button Text="Export" runat="server" ID="exptxl" OnClick="exptxl_Click" CssClass="btn" />
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <asp:HiddenField ID="HiddenField2" runat="server" />

                                </div>
                            </div>
                        </div>
                    </div>
                    <%--end main page--%>
                    <span style="font-weight: bold;">Total Records.</span>
                    <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>
                </div>
                <!-- /.col -->
            </div>
        </div>
        <!-- /.page-content -->
    </div>
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <br />
    <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
        CellSpacing="0" GridLines="None" CssClass="gvData">
        <%----%>
        <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
        <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
        <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px">
        </HeaderStyle>
        <ClientSettings EnablePostBackOnRowClick="false">
            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
            <Selecting AllowRowSelect="true" />
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
                <%--                                <telerik:GridTemplateColumn HeaderText="Check Box">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this,0);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkitem" runat="server" onclick="checkItem_All(this,0)" />
                        <asp:HiddenField ID="hdnAstID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AssetId") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn UniqueName="Select">
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
                    HeaderText="SUBCATEGORY" SortExpression="SubCategory" UniqueName="SubCategory"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                    HeaderText="LOCATION" SortExpression="Location" UniqueName="SubCategory" ReadOnly="true">
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
                <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                    HeaderText="SERIALNO" SortExpression="SerialNo" UniqueName="SerialNo" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                    HeaderText="DESCRIPTION" SortExpression="Description" UniqueName="Description"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SupplierName" FilterControlAltText="Filter SupplierName column"
                    HeaderText="SUPPLIER" SortExpression="SupplierName" UniqueName="SupplierName"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column"
                    HeaderText="AMC TYPE" SortExpression="Type" UniqueName="Type" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="StartDate" FilterControlAltText="Filter StartDate column"
                    HeaderText="STARTDATE" SortExpression="StartDate" UniqueName="StartDate" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EndDate" FilterControlAltText="Filter EndDate column"
                    HeaderText="ENDDATE" SortExpression="EndDate" UniqueName="EndDate" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Remarks" FilterControlAltText="Filter Remarks column"
                    HeaderText="REMARKS" SortExpression="Remarks" UniqueName="Remarks" ReadOnly="true">
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
