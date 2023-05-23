<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="UserMaster.aspx.cs" Inherits="UserMaster" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
    <script type="text/javascript">
        try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
    </script>
    <style type="text/css">
        .mycheckbox input[type="checkbox"]
        {
            margin-right: 5px;
            font-family: Cambria;
            font-size: large;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function Validate() {


            var chck = '<%=chkstatus.ClientID %>';
            var pass = '<%=txtboxpas.ClientID %>';

            var txtcat = '<%=txtboxname.ClientID %>';

            var e = document.getElementById('<%=ddtype.ClientID %>');
            var strUser = e.options[e.selectedIndex].text;

            if (document.getElementById(txtcat).value == '') {
                alert("Please Enter The User Name");

                document.getElementById(txtcat).focus();

                return false;

            }

            if (document.getElementById(pass).value == '') {
                alert("Please Enter Password");

                document.getElementById(pass).focus();

                return false;

            }

            if (strUser == '--Select Type--') {
                alert("Please Select Type");

                e.focus();

                return false;

            }

            if (document.getElementById('<%=btnsubmit.ClientID %>').value == 'Update') {
                if (document.getElementById(chck).checked == false) {
                    return confirm('Do you really want to deactivate this user?');

                }
            }


            return true;
        }
                     
    </script>
    <div class="main-content-inner">
        <div class="page-content">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <label style="font-size: x-large; color: #2679b5">
                        User Management</label>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <!-- start top menu -->
                            <div class="hidden">
                                <uc1:topmenu runat="server" ID="topmenu" />
                            </div>
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
                                        User Name <span style="color: Red">*</span> :
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtboxname" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <label runat="server" id="Label1" class="col-sm-2 control-label no-padding-right"
                                        for="form-field-1">
                                        Password <span style="color: Red">*</span> :
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtboxpas" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <label runat="server" id="lblsubcattype" class="col-sm-2 control-label no-padding-right"
                                        for="form-field-1-1">
                                        Type <span style="color: Red">*</span> :
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList runat="server" ID="ddtype" AutoPostBack="true" class="form-control"
                                                    OnSelectedIndexChanged="ddtype_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <label runat="server" id="Label2" class="col-sm-2 control-label no-padding-right"
                                        for="form-field-1-1">
                                        Status <span style="color: Red">*</span> :
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:CheckBox ID="chkstatus" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <label runat="server" id="Label3" class="col-sm-2 control-label no-padding-right"
                                        for="form-field-1-1">
                                        Permission :
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:ListBox ID="lstPermission" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-sm-3">
                                        <%--                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>--%>
                                        <asp:Button Text="Submit" runat="server" ID="btnsubmit" OnClick="btnsubmit_Click"
                                            OnClientClick="javascript:return Validate();" CssClass="btn btn-primary" />
                                        <%--                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                    <asp:HiddenField ID="hiduserid" runat="server" />
                                    <div class="col-xs-12">
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"
                                            Width="100%"></asp:Label>
                                        <span style="font-weight: bold;"></span>
                                        <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtPageCount" runat="server" Visible="False"></asp:TextBox>
                                        <asp:Label ID="lblSort" runat="server" Visible="False"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6 col-sm-3">
                        </div>
                        <div class="col-xs-6 col-sm-3">
                            .col-xs-6 .col-sm-3</div>
                        <!-- Add the extra clearfix for only the required viewport -->
                        <div class="clearfix visible-xs-block">
                        </div>
                        <div class="col-xs-6 col-sm-3">
                            .col-xs-6 .col-sm-3</div>
                        <div class="col-xs-6 col-sm-3">
                            .col-xs-6 .col-sm-3</div>
                    </div>
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data_ItemCommand">
                                    <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px">
                                    </HeaderStyle>
                                    <ClientSettings EnablePostBackOnRowClick="false">
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                        <ClientEvents OnGridCreated="GridCreated" />
                                    </ClientSettings>
                                    <SortingSettings EnableSkinSortStyles="false" />
                                    <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                        DataKeyNames="UserId">
                                        <PagerStyle AlwaysVisible="true" Position="Top" />
                                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridButtonColumn CommandName="dit" ButtonType="ImageButton" UniqueName="Edit"
                                                ImageUrl="~/images/pencil.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridTemplateColumn HeaderText="SR NO">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="UserId" FilterControlAltText="Filter UserId column"
                                                HeaderText="UserId" SortExpression="UserId" UniqueName="UserId" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column"
                                                HeaderText="Type" SortExpression="Type" UniqueName="Type" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column"
                                                HeaderText="USER NAME" SortExpression="UserName" UniqueName="UserName" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="PASSWORD" FilterControlAltText="Filter PASSWORD column"
                                                HeaderText="PASSWORD" SortExpression="PASSWORD" UniqueName="PASSWORD" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter CreatedBy column"
                                                HeaderText="CREATED BY" SortExpression="CreatedBy" UniqueName="CreatedBy" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                                HeaderText="CREATED DATE" SortExpression="CreatedDate" UniqueName="CreatedDate"
                                                ReadOnly="true" AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                                HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true"
                                                AllowFiltering="false">
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
