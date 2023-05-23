<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="Approve.aspx.cs" Inherits="Approve" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <style type="text/css">
        .imageButtonClass {
            height: 20px;
        }
    </style>
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
    <script type="text/javascript">
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <asp:Button ID="btnErrorPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnErrorPopup"
        PopupControlID="pnlpopup1" BackgroundCssClass="modalBackground" BehaviorID="mpe1">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup1" runat="server" CssClass="modalPopup" Height="140px" Width="400px"
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
    <ajax:ModalPopupExtender ID="GriddetailsPopup" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Height="450px" Width="90%"
        Style="display: none">
        <%--        <script type="text/javascript">
            function GridCreated_Pop(sender, args) {
                var scrollArea = sender.GridDataDiv;
                var dataHeight = sender.get_masterTableView().get_element().clientHeight; if (dataHeight < 500) {
                    scrollArea.style.height = dataHeight + 5 + "px";
                }
            }
        </script>--%>
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-sm-1">
                        <label runat="server">
                            <b><%#Assets.ToUpper() %>&nbsp;DETAILS&nbsp;</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </div>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <div class="col-sm-2">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-8 pull-right">
                        <%--<div class="col-sm-2">--%>
                        <asp:Label ID="LblDoneBy" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--</div>--%>
                        <%-- <div class="col-sm-2">--%>
                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                        <%-- </div>--%>
                    </div>
                </div>
            </div>
            <div>
                <telerik:RadGrid ID="gv_Popup" runat="server" CellSpacing="0" GridLines="None" CssClass="gvData"
                    OnNeedDataSource="gv_Popup_NeedDataSource" OnItemDataBound="gv_Popup_ItemDataBound"
                    OnItemCreated="gv_Popup_ItemCreated">
                    <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                    <ClientSettings>
                        <%--EnablePostBackOnRowClick="false"--%>
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="280px" />
                        <%-- <ClientEvents OnGridCreated="GridCreated_Pop" />--%>
                    </ClientSettings>
                    <SortingSettings EnableSkinSortStyles="false" />
                    <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="false">
                        <PagerStyle AlwaysVisible="true" Position="Top" />
                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter AssetCode column"
                                HeaderText="ASSET CODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                                AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                                HeaderText="SERIALNO" SortExpression="SerialNo" UniqueName="SerialNo" ReadOnly="true"
                                AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                                HeaderText="CATEGORY" SortExpression="CategoryName" UniqueName="CategoryName"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="SubCatName" FilterControlAltText="Filter SubCatName column"
                                HeaderText="SUB CATEGORY" SortExpression="SubCatName" UniqueName="SubCatName"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="From_Location" FilterControlAltText="Filter From_Location column"
                                HeaderText="FROM LOCATION" SortExpression="From_Location" UniqueName="From_Location"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="To_Location" FilterControlAltText="Filter To_Location column"
                                HeaderText="TO LOCATION" SortExpression="To_Location" UniqueName="To_Location"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FROM_CUSTODOAN" FilterControlAltText="Filter FROM_CUSTODOAN column"
                                HeaderText="FROM_CUSTODOAN" SortExpression="FROM_CUSTODOAN" UniqueName="FROM_CUSTODOAN"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TO_CUSTODOAN" FilterControlAltText="Filter TO_CUSTODOAN column"
                                HeaderText="TO_CUSTODOAN" SortExpression="TO_CUSTODOAN" UniqueName="TO_CUSTODOAN"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <%--                            <telerik:GridBoundColumn DataField="CustodianName" FilterControlAltText="Filter CustodianName column"
                                HeaderText="CustodianName" SortExpression="CustodianName" UniqueName="CustodianName"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true" Visible="true">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>--%>
                            <%--                            <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column"
                                HeaderText="Location" SortExpression="LocationName" UniqueName="LocationName"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true" Visible="false">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Buildingname" FilterControlAltText="Filter Buildingname column"
                                HeaderText="Building" SortExpression="Buildingname" UniqueName="Buildingname"
                                ReadOnly="true" AllowSorting="true" AllowFiltering="true" Visible="false">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="FloorName" FilterControlAltText="Filter FloorName column"
                                HeaderText="Floor" SortExpression="FloorName" UniqueName="FloorName" ReadOnly="true"
                                AllowSorting="true" AllowFiltering="true" Visible="false">
                                <ItemStyle Width="130px" />
                                <HeaderStyle Width="130px" />
                            </telerik:GridBoundColumn>--%>
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
            <div class="col-sm-offset-4">
                <asp:Button Text=" Close" runat="server" ID="btnClose" OnClientClick="javascript:return HideModalPopup();"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
            </div>
        </div>
    </asp:Panel>
    <div class="hidden">
        <uc1:topmenu runat="server" ID="topmenu" />
    </div>
    <div class="main-content-inner">
        <div class="page-content">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group">
                                <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                    onkeydown="return (event.keyCode!=13);" ReadOnly="true"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnSearchInfo" Style="height: 34px" runat="server" class="btn btn-default"
                                        type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="page-header">
                        <h1>Approval Module</h1>
                    </div>
                    <div>
                        <telerik:RadWindowManager ID="radwinman1" runat="server"></telerik:RadWindowManager>
                        <telerik:RadWindow ID="radwin1" runat="server"></telerik:RadWindow>
                        <%-- <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" GridLines="None" AllowPaging="True" PageSize="10" AllowFilteringByColumn="false"
                            OnItemCommand="gv_data_ItemCommand" OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated"
                            OnInit="gvData_Init">--%>
                        <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                            OnItemDataBound="gvData_ItemDataBound" OnItemCommand="gv_data_ItemCommand" OnItemCreated="gvData_ItemCreated">

                            <SortingSettings EnableSkinSortStyles="false" />
                            <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                            <ClientSettings EnablePostBackOnRowClick="false">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                <ClientEvents OnGridCreated="GridCreated" />
                            </ClientSettings>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                            <AlternatingItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                            <MasterTableView AllowPaging="True" AutoGenerateColumns="false" EditMode="InPlace"
                                DataKeyNames="REQUEST ID" AllowSorting="true">
                                <PagerStyle AlwaysVisible="true" Position="Top" Mode="Advanced" />
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridButtonColumn HeaderText="DOWNLOAD" CommandName="Download" ButtonType="LinkButton"
                                        UniqueName="Download1" Text="Download">
                                        <ItemStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                    </telerik:GridButtonColumn>
                                    <telerik:GridButtonColumn HeaderText="VIEW" CommandName="VIEW" ButtonType="LinkButton"
                                        UniqueName="VIEW" Text="VIEW">
                                        <ItemStyle Width="100px" />
                                        <HeaderStyle Width="100px" />
                                    </telerik:GridButtonColumn>
                                    <telerik:GridBoundColumn DataField="TYPE" FilterControlAltText="Filter TYPE column"
                                        HeaderText="TYPE" SortExpression="TYPE" UniqueName="TYPE" ReadOnly="true">
                                        <ItemStyle Width="150px" />
                                        <HeaderStyle Width="150px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn Visible="false" DataField="REQUEST ID" FilterControlAltText="Filter REQUEST ID column"
                                        HeaderText="REQUEST ID" SortExpression="REQUEST ID" UniqueName="REQUESTID" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="REQUEST BY" FilterControlAltText="Filter REQUEST BY column"
                                        HeaderText="REQUEST BY" SortExpression="REQUEST BY" UniqueName="REQUESTBY" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="REQUEST DATE" FilterControlAltText="Filter REQUEST DATE column"
                                        HeaderText="REQUEST DATE" SortExpression="REQUEST DATE" UniqueName="REQUESTDATE"
                                        ReadOnly="true">
                                        <ItemStyle Width="170px" />
                                        <HeaderStyle Width="170px" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="STATUS" FilterControlAltText="Filter STATUS column"
                                        HeaderText="STATUS" SortExpression="STATUS" UniqueName="STATUS" ReadOnly="true">
                                        <ItemStyle Width="200px" />
                                        <HeaderStyle Width="200px" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="TRANSFER_TYPE" FilterControlAltText="Filter TRANSFER_TYPE column"
                                        HeaderText="TRANSFER TYPE" SortExpression="TRANSFER_TYPE" UniqueName="TRANSFER_TYPE" ReadOnly="true">
                                        <ItemStyle Width="200px" />
                                        <HeaderStyle Width="200px" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="REMARKS" FilterControlAltText="Filter REMARKS column"
                                        HeaderText="REMARKS" SortExpression="REMARKS" UniqueName="REMARKS" ReadOnly="true">
                                        <ItemStyle Width="200px" />
                                        <HeaderStyle Width="200px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn HeaderText="APPROVE" CommandName="APPROVE" ButtonType="ImageButton"
                                        UniqueName="APPROVE" Text="APPROVE" ImageUrl="~/images/Approve.png" ButtonCssClass="imageButtonClass"
                                        ConfirmText="Are you sure you want to approve this request?" ConfirmDialogType="RadWindow" ConfirmTitle="Confirm" ConfirmDialogHeight="100px">
                                        <%--ConfirmText="Are you sure you want to approve this request? "--%>
                                        <ItemStyle Width="90px" />
                                        <HeaderStyle Width="90px" />
                                    </telerik:GridButtonColumn>
                                    <telerik:GridButtonColumn HeaderText="REJECT" CommandName="REJECT" ButtonType="ImageButton"
                                        UniqueName="REJECT" Text="REJECT" ImageUrl="~/images/Reject.png" ButtonCssClass="imageButtonClass"
                                        ConfirmText="Are you sure you want to reject this request?" ConfirmDialogType="RadWindow" ConfirmTitle="Confirm" ConfirmDialogHeight="100px">
                                        <ItemStyle Width="90px" />
                                        <HeaderStyle Width="90px" />
                                    </telerik:GridButtonColumn>
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
                    <div>
                        <telerik:RadWindow RenderMode="Lightweight" runat="server" ID="RadWindow1" NavigateUrl="~/RequestDetails.aspx" Width="1028px" Height="590px">
                        </telerik:RadWindow>
                    </div>
                </div>
            </div>
        </div>
    </div>
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
                    <asp:Label ID="Label1" Style="font-size: large" Text="&nbsp;Tagit&nbsp;Ams" runat="server"></asp:Label>
                </td>
                <td align="right" style="margin-right: 10px;">
                    <asp:Button ID="Button1" runat="server" CssClass="bgimage" />
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
