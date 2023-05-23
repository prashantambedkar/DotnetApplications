<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="TestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Src="ProductDetailsCS.ascx" TagName="ProductDetails" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns='http://www.w3.org/1999/xhtml'>
<head runat="server">
    <title>Telerik ASP.NET Example</title>
    <link rel="Stylesheet" href="styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
        <div class="demo-container size-medium">
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
            </telerik:RadAjaxLoadingPanel>
            <telerik:RadToolTipManager RenderMode="Lightweight" ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose"
                Width="350" Height="250" runat="server" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element"
                Position="MiddleRight">
            </telerik:RadToolTipManager>
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server"
                GridLines="None" OnItemDataBound="RadGrid1_ItemDataBound" AllowPaging="true"
                AllowSorting="true" PageSize="10" OnItemCommand="RadGrid1_ItemCommand"
                OnNeedDataSource="gvData_NeedDataSource">

                <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="None" CurrentResetPageIndexAction="SetPageIndexToFirst"
                    DataKeyNames="AssetId" Dir="LTR" Frame="Border"
                    TableLayout="Auto">
                    <Columns>
                        <telerik:GridBoundColumn CurrentFilterFunction="NoFilter" DataField="AssetId" Display="false"
                            DataType="System.Int32" FilterListOptions="VaryByDataType" ForceExtractValue="None"
                            HeaderText="ProductID" ReadOnly="True" SortExpression="AssetId" UniqueName="AssetId">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn AllowSorting="true" DataField="ImageName" HeaderText="ImageName"
                            SortExpression="ImageName" UniqueName="ImageName" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="ImageName" SortExpression="ImageName">
                            <ItemTemplate>
                                <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="#" Text='<%# Eval("ImageName") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn AllowSorting="true" DataField="Category" HeaderText="Supplier"
                            SortExpression="CompanyName" UniqueName="CompanyName">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <EditFormSettings>
                        <EditColumn CurrentFilterFunction="NoFilter" FilterListOptions="VaryByDataType">
                        </EditColumn>
                    </EditFormSettings>                    
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
