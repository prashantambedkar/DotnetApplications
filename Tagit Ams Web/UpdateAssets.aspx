<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="UpdateAssets.aspx.cs" Inherits="UpdateAssets" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js">
    </script>
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.ui/1.8.6/jquery-ui.min.js">
    </script>
    <script type="text/javascript">
        $(function () {
            $(".date").attr("readonly", "true");
            $(".date").datepicker();
        });
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("Do you want to update these assets ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);


            //            var combo = $find("<%= cboField.ClientID %>");
            //            var vall = combo.get_selectedIndex();
            //            if (vall == null) {
            //                return;
            //            }
            //            if (combo.get_selectedIndex() >= 0) {
            //                if (confirm("Do you want to update selected asset details ?")) {
            //                    confirm_value.value = "Yes";
            //                } else {
            //                    confirm_value.value = "No";
            //                }
            //                document.forms[0].appendChild(confirm_value);
            //            }
        }
    </script>
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
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Master</a> </li>
                    <li><a href="#">Asset Master</a> </li>
                    <li><a href="#">Update Assets</a></li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Search Assets</h1>
            </div>
            <!-- /.page-header -->
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>
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
                            <label runat="server" id="Label4" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Asset Code :
                            </label>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtAssetCode" runat="server" Style="width: 240px"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <label runat="server" id="lblcattype" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                Category Type :
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
                                <%-- <asp:Label ID="Label5" Text="*" ForeColor="Red" runat="server" />--%>
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
                                <%-- <asp:Label ID="Label2" Text="*" ForeColor="Red" runat="server" />--%>
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
                                <%-- <asp:Label ID="Label7" Text="*" ForeColor="Red" runat="server" />--%>
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
                                <%--  <asp:Label ID="Label9" Text="*" ForeColor="Red" runat="server" />--%>
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
                                <%-- <asp:Label ID="Label10" Text="*" ForeColor="Red" runat="server" />--%>
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
                        </div>
                    </div>
                    <div class="clearfix form-actions">
                        <div class="col-md-offset-3 col-md-9">
                            <asp:Button Text="Search" runat="server" ID="btnsubmit" CssClass="btn" OnClick="btnsubmit_Click" />
                            <asp:Button ID="btnreset" CssClass="btn" runat="server" Text="Reset" OnClick="btnreset_Click" />
                            <asp:HiddenField ID="hdncatidId" runat="server" />
                            <asp:HiddenField ID="hidcatcode" runat="server" />
                        </div>
                    </div>
                    <div class="page-header">
                        <h1>
                            Update Assets</h1>
                    </div>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label runat="server" id="Label3" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1-1">
                                Choose Field
                                <%-- <asp:Label ID="Label5" Text="*" ForeColor="Red" runat="server" />--%>
                                :
                            </label>
                            <div class="col-sm-3">
                                <%--                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>--%>
                                <asp:DropDownList runat="server" ID="cboField" AutoPostBack="true" class="form-control"
                                    OnSelectedIndexChanged="cboField_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </div>
                            <%--                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cboField"
                                ValidationGroup="a" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            <label runat="server" id="Label2" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1-1">
                                Value
                                <%-- <asp:Label ID="Label5" Text="*" ForeColor="Red" runat="server" />--%>
                                :
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtValue" runat="server"></asp:TextBox>
                                <%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtValue"
                                    ValidationGroup="a" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-sm-3">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="cboValue" AutoPostBack="true" class="form-control"
                                            Visible="false">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="cboValue"
                                    ValidationGroup="a" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix form-actions">
                        <div class="col-md-offset-3 col-md-9">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" Font-Bold="True" CssClass="btn"
                                ValidationGroup="a" OnClick="btnUpdate_Click" OnClientClick="Confirm()" /><%--  OnClientClick="Confirm()" OnClick="btnPrint_Click"--%>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.col -->
        </div>
    </div>
    <br />
    <telerik:RadGrid ID="gvData" runat="server" Width="98%" CellSpacing="0" GridLines="None"
        CssClass="gvData" OnNeedDataSource="gvData_NeedDataSource">
        <%-- OnItemDataBound="gvData_ItemDataBound" --%>
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
                <%--                <telerik:GridTemplateColumn UniqueName="Select">
                    <HeaderTemplate>
                        <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cboxSelect" runat="server" onclick="unCheckHeader(this)" />
                        <asp:HiddenField ID="hdnAstID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AssetId") %>' />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <%--                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                    SortExpression="ID" UniqueName="ID" ReadOnly="true" AllowSorting="false" AllowFiltering="false">
                </telerik:GridBoundColumn>--%>
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
                <telerik:GridBoundColumn DataField="Custodian" FilterControlAltText="Filter CUSTODIAN column"
                    HeaderText="CUSTODIAN" SortExpression="CUSTODIAN" UniqueName="CUSTODIAN" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SupplierName" FilterControlAltText="Filter SUPPLIER column"
                    HeaderText="SUPPLIER" SortExpression="SUPPLIER" UniqueName="SUPPLIER" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                    HeaderText="DELIVERYDATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                    ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AssignDate" FilterControlAltText="Filter AssignDate column"
                    HeaderText="ASSIGNDATE" SortExpression="AssignDate" UniqueName="AssignDate" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="IsEncodedTHR" FilterControlAltText="Filter IsEncodedTHR column"
                    HeaderText="IsEncodedTHR" SortExpression="IsEncodedTHR" UniqueName="IsEncodedTHR"
                    ReadOnly="true" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="IsEncodedTHS" FilterControlAltText="Filter IsEncodedTHS column"
                    HeaderText="IsEncodedTHS" SortExpression="IsEncodedTHS" UniqueName="IsEncodedTHS"
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
</asp:Content>
