<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="Rprintlabels.aspx.cs" Inherits="Rprintlabels" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("[id$=txtFrmDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
            });
            $("[id$=txtToDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
            });

        });
        
    </script>
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Report</a> </li>
                    <li><a href="#">Printed Labels</a> </li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Printed Labels</h1>
            </div>
            <!-- /.page-header -->
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>
                    <div class="form-horizontal">
                        <%-- form-group--%>
                        <%--<div class="clearfix form-actions">--%>
                        <div class="form-group">
                            <div class="col-xs-12">
                                <label runat="server" id="lblFromDate" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    From Date:
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtFrmDate" class="form-control" runat="server"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label11" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    To Date:
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtToDate" class="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <label runat="server" id="lblcattype" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    Category
                                </label>
                                <div class="col-sm-3">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlproCategory" AutoPostBack="true" class="form-control"
                                                OnSelectedIndexChanged="OnSelectedIndexChangedCategory">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <label runat="server" id="lblsubcattype" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1-1">
                                    Sub Category
                                </label>
                                <div class="col-sm-3">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlsubcat" class="form-control">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-xs-12">
                                <label runat="server" id="Label1" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    Location</label>
                                <div class="col-sm-3">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlloc" AutoPostBack="true" class="form-control"
                                                OnSelectedIndexChanged="OnSelectedIndexChangedLocation">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <label runat="server" id="lblcomposition" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    Building
                                </label>
                                <div class="col-sm-3">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlbuild" AutoPostBack="true" class="form-control"
                                                OnSelectedIndexChanged="OnSelectedIndexChangedBuilding">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <label runat="server" id="Label8" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    Floor
                                </label>
                                <div class="col-sm-3">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlfloor" class="form-control">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-offset-3 col-md-9">
                            </div>
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="clearfix form-actions">
                            <div class="col-md-offset-3 col-md-9">
                                <asp:Button runat="server" ID="btnSearch" CssClass="btn" Text="Search" OnClick="btnSearch_Click" />
                                <asp:Button ID="btnRefresh" CssClass="btn" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                                <asp:Button ID="btnExportExcel" CssClass="btn" runat="server" Text="Export"
                                    OnClick="btnExportExcel_Click" />
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <%--<asp:Label ID="lblTotHeader" runat="server" Text="Total Records." Style="font-weight: bold;" Visible="false"></asp:Label>--%>
                            <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div id="divGrid" style="overflow: auto; height: 0px">
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--end main page--%>
                </div>
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
        <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true">
            <PagerStyle AlwaysVisible="true" Position="Top" />
            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn DataField="AssetId" FilterControlAltText="Filter AssetId column"
                    HeaderText="AssetId" SortExpression="AssetId" UniqueName="AssetId" ReadOnly="true"
                    AllowSorting="false" AllowFiltering="false" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter ID column"
                    HeaderText="ASSETCODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                    AllowSorting="true" AllowFiltering="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                    HeaderText="SERIALNO" SortExpression="SerialNo" UniqueName="SerialNo" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                    HeaderText="DESCRIPTION" SortExpression="Description" UniqueName="Description"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                    HeaderText="PRICE" SortExpression="Price" UniqueName="Price" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter Category column"
                    HeaderText="CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SubCatName" FilterControlAltText="Filter SubCategory column"
                    HeaderText="SUBCATEGORY" SortExpression="SubCategory" UniqueName="SubCategory"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter Location column"
                    HeaderText="LOCATION" SortExpression="Location" UniqueName="SubCategory" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Buildingname" FilterControlAltText="Filter Building column"
                    HeaderText="BUILDING" SortExpression="Building" UniqueName="Building" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="FloorName" FilterControlAltText="Filter Floor column"
                    HeaderText="FLOOR" SortExpression="Floor" UniqueName="Floor" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DepartmentName" FilterControlAltText="Filter Department column"
                    HeaderText="DEPARTMENT" SortExpression="Department" UniqueName="Department" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                    HeaderText="DATE" SortExpression="CreatedDate" UniqueName="CreatedDate" ReadOnly="true">
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
