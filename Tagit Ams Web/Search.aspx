<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="Search.aspx.cs" Inherits="Search" %>

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
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
          $(function () {

           $("[id$=txtFrmTranDate]").attr("readonly", "true");  
            $("[id$=txtFrmTranDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
                dateFormat: 'dd-mm-yy',
            });
            $("[id$=txtToTranDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
                dateFormat: 'dd-mm-yy',

            });
           
        });
        
    </script>
    <script type="text/javascript">
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Master</a> </li>
                    <li><a href="#">Asset Master</a> </li>
                    <li><a href="#">Search </a></li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Search</h1>
            </div>
            <!-- /.page-header -->
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>
                    <script language="javascript" type="text/javascript">
                        function Validate() {
                            var txtFrmTranDate = '<%=txtFrmTranDate.ClientID %>';
                            var txtToTranDate = '<%=txtToTranDate.ClientID %>';

                            if (document.getElementById(txtFrmTranDate).value != '' && document.getElementById(txtToTranDate).value != '') {

                                return true;
                            }
                            else {
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


//                                if (strUser == '--Select Category--') {
//                                    alert("Please Select Category");

//                                    e.focus();

//                                    return false;

//                                }

//                                if (guser == '--Select Sub Category--') {
//                                    alert("Please Select Sub Category");

//                                    g.focus();

//                                    return false;

//                                }
//                                if (fuser == '--Select Location--') {
//                                    alert("Please Select Location");

//                                    f.focus();

//                                    return false;

//                                }
//                                if (huser == '--Select Building--') {
//                                    alert("Please Select Building");

//                                    h.focus();

//                                    return false;

//                                }
//                                if (Iuser == '--Select Floor--') {
//                                    alert("Please Select Floor");

//                                    I.focus();

//                                    return false;

//                                }


//                                if (Juser == '--Select Department--') {
//                                    alert("Please Select Department");

//                                    J.focus();

//                                    return false;

//                                }

                            return true;
//                            }

                        }
                                        
                    </script>
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
                            <label runat="server" id="Label2" class="col-sm-2 control-label no-padding-right"
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
                            <label runat="server" id="lblFromDate" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                From Transaction Date:
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtFrmTranDate" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <label runat="server" id="Label11" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1">
                                To Transaction Date:
                            </label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtToTranDate" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix form-actions">
                        <div class="col-md-offset-3 col-md-9">
                            <asp:Button Text="Search" runat="server" ID="btnsubmit" CssClass="btn" OnClick="btnsubmit_Click" />
                            <asp:Button ID="btnreset" CssClass="btn" runat="server" Text="Reset" OnClick="btnreset_Click" />
                            <asp:Button ID="BtnExport" CssClass="btn" runat="server" Text="Export" OnClick="btnExport_Click" />
                            <asp:HiddenField ID="hdncatidId" runat="server" />
                            <asp:HiddenField ID="hidcatcode" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.col -->
        </div>
    </div>
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
                <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                    HeaderText="DELIVERYDATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                    ReadOnly="true">
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
