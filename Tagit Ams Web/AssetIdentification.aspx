<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="AssetIdentification.aspx.cs" Inherits="AssetIdentification" %>


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
                        <h1 id="PgHeader" runat="server"><%#Assets %> Identification Module</h1>
                    </div>
                    <div>
                        <div class="row">
                            <telerik:RadWindowManager ID="radwinman1" runat="server"></telerik:RadWindowManager>
                            <telerik:RadWindow ID="radwin1" runat="server"></telerik:RadWindow>
                            <%--                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                <ContentTemplate>--%>
                            <%-- <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource" OnItemCommand="gv_data_ItemCommand"
                                CellSpacing="0" GridLines="None" CssClass="gvData" OnPageIndexChanged="gvData_PageIndexChanged" OnItemDataBound="gvData_ItemDataBound">--%>
                            <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                OnItemDataBound="gvData_ItemDataBound">

                                <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                                <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
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
                                        <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column"
                                            HeaderText="ID" SortExpression="ID" UniqueName="ID" ReadOnly="true"
                                            AllowFiltering="false" Visible="false" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Trans_Id" FilterControlAltText="Filter Trans_Id column"
                                            HeaderText="IDENTIFICATION ID" SortExpression="Trans_Id" UniqueName="Trans_Id" ReadOnly="true"
                                            AllowFiltering="false" Visible="true" ItemStyle-Width="180px" HeaderStyle-Width="180px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Created_Date" FilterControlAltText="Filter Created_Date column"
                                            HeaderText="DATE" SortExpression="Created_Date" UniqueName="Created_Date" ReadOnly="true"
                                            ItemStyle-Width="180px" HeaderStyle-Width="180px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Created_By" FilterControlAltText="Filter Created_By column"
                                            HeaderText="IDENTIFIED BY" SortExpression="Created_By" UniqueName="Created_By"
                                            ReadOnly="true" ItemStyle-Width="180px" HeaderStyle-Width="180px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                            HeaderText="STATUS" SortExpression="Status" UniqueName="Status"
                                            ReadOnly="true" ItemStyle-Width="180px" HeaderStyle-Width="180px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn HeaderText="APPROVE" CommandName="APPROVE" ButtonType="ImageButton"
                                            UniqueName="APPROVE" Text="APPROVE" ImageUrl="~/images/Approve.png" ButtonCssClass="imageButtonClass"
                                            ConfirmText="Are you sure you want to approve this request?" ConfirmDialogType="Classic" ConfirmTitle="Confirm" ConfirmDialogHeight="100px">
                                            <%--ConfirmText="Are you sure you want to approve this request? "--%>
                                            <ItemStyle Width="90px" />
                                            <HeaderStyle Width="90px" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn HeaderText="REJECT" CommandName="REJECT" ButtonType="ImageButton"
                                            UniqueName="REJECT" Text="REJECT" ImageUrl="~/images/Reject.png" ButtonCssClass="imageButtonClass"
                                            ConfirmText="Are you sure you want to reject this request?" ConfirmDialogType="Classic" ConfirmTitle="Confirm" ConfirmDialogHeight="100px">
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
                            <%--                                </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </div>
                    <div>
                        <telerik:RadWindow RenderMode="Lightweight" runat="server" ID="RadWindow1" NavigateUrl="~/RequestDetails.aspx" Width="1028px" Height="590px">
                        </telerik:RadWindow>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel22"
        TargetControlID="btnShow" CancelControlID="btnClose">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel22" runat="server" align="center" Style="display: none" CssClass="modalPopup"
        Height="132px" Width="250px">
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
    </asp:Panel>--%>

    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel22"
        TargetControlID="btnShow" CancelControlID="btnClose">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel22" runat="server" align="center" Style="display: none" CssClass="modalPopup"
        Height="132px" Width="250px">
        <table style="width: 100%">
            <tr style="height: 25px;" id="trheader" runat="server">
                <td colspan="1">
                    <label id="Label2" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label>
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

