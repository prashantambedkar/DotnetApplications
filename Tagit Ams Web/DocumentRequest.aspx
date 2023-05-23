<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="DocumentRequest.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ScrollToTop() {
            window.scrollTo(500, 0);
        }
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    ScrollToTop();
                }
            });
        };
    </script>
    <script type="text/javascript">
        function OnClientItemsRequestedHandler(sender, eventArgs) {
            //set the max allowed height of the combo  
            var MAX_ALLOWED_HEIGHT = 220;
            //this is the single item's height  
            var SINGLE_ITEM_HEIGHT = 22;

            var calculatedHeight = sender.get_items().get_count() * SINGLE_ITEM_HEIGHT;

            var dropDownDiv = sender.get_dropDownElement();

            if (calculatedHeight > MAX_ALLOWED_HEIGHT) {
                setTimeout(
                    function () {
                        dropDownDiv.firstChild.style.height = MAX_ALLOWED_HEIGHT + "px";
                    }, 20
                );
            }
            else {
                setTimeout(
                    function () {
                        dropDownDiv.firstChild.style.height = calculatedHeight + "px";
                    }, 20
                );
            }
        }
    </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }

        .RadListBox1 {
            left: 37px;
            top: 84px;
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

        $(document).ready(function () {
            $('#ContentPlaceHolder1_chkstatus').prop("checked", true);
            //            $('#ContentPlaceHolder1_chkstatus').click(function () {

            //                
            //            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField runat="server" ID="HdnLocation" />
    <div id="div1" class="main-content-inner" style="font-family: Calibri;font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-6">
                                <span style="font-family: 'Calibri'; font-size: x-large">Create Document Request</span>&nbsp;
                                <%--<a style="float: inline-end; font-size: 24px" onclick="return (event.keyCode!=13);" runat="server" id="txtSearch"><i class="fa fa-filter"></i></a>--%>
                            </div>

                            <div class="col-sm-6">

                                <i style="float: inline-end; font-size: 24px" class="fa">

                                    <%-- <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                                </i>
                                &nbsp;&nbsp;&nbsp;&nbsp;<img src="" alt="" style="float: right;" runat="server" id="CompanyImg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                    <%-- collapse demo--%>

                    <%--  collapse start--%>
                    <div class="container">
                        <div id="demo" class="collapse">
                            <br />
                            <div class="container">
                                <div class="row">
                                    <div class="col-md-12 text-center">
                                        <asp:Button ID="Button1" class="btn btn-info" runat="server" Text="Apply" />
                                        &nbsp;&nbsp;
                                        <input type="button" id="btnClosePopup" value="Close" class="btn btn-danger" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <%--collapse end--%>
                <div class="panel-body">
                    <div class="container" id="container1" runat="server">
                        <div class="row">

                            <div class="col-md-3">
                                <h5>Select Case ID:</h5>
                                <asp:DropDownList Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlCaseId_SelectedIndexChanged" ID="ddlCaseId" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <h5>Select Assignee Name:</h5>
                                <asp:DropDownList Width="100%" ID="ddlAssigneeName" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <h5>Select Organisation Name:</h5>
                                <asp:DropDownList Width="100%" ID="ddlOrganisationName" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <h5>&nbsp;</h5>
                                <asp:Button Text="Search" runat="server" ID="btnsubmit" Style="height: 100%; border-radius: 5px; padding: 0; width: 80%; margin-left: 10%;"
                                    class="btn btn-primary" OnClick="btnsubmit_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="container" id="container2" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data_ItemCommand">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ClientSettings EnablePostBackOnRowClick="false">
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                        <ClientEvents OnGridCreated="GridCreated" />
                                    </ClientSettings>
                                    <SortingSettings EnableSkinSortStyles="false" />
                                    <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                        DataKeyNames="id">
                                        <PagerStyle AlwaysVisible="true" Position="Top" />
                                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="SR NO" HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="id" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter id column"
                                                HeaderText="id" SortExpression="id" UniqueName="id" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CaseID" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CaseID column"
                                                HeaderText="CaseID" SortExpression="CaseID" UniqueName="CaseID" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CaseAssigneeFullName" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CaseAssigneeFullName column"
                                                HeaderText="Assignee Name" SortExpression="CaseAssigneeFullName" UniqueName="CaseAssigneeFullName" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ApplicantNames" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter ApplicantNames column"
                                                HeaderText="Applicant Name" SortExpression="ApplicantNames" UniqueName="ApplicantNames" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ParentOrganizationName" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter ParentOrganizationName column"
                                                HeaderText="Parent Organisation Name" SortExpression="ParentOrganizationName" UniqueName="ParentOrganizationName" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CaseManagerFullName" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CaseManagerFullName column"
                                                HeaderText="Manager Name" SortExpression="CaseManagerFullName" UniqueName="CaseManagerFullName" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CaseWorker1Name" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CaseWorker1Name column"
                                                HeaderText="Worker Name" SortExpression="CaseWorker1Name" UniqueName="CaseWorker1Name" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CaseStatus" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CaseStatus column"
                                                HeaderText="Status" SortExpression="CaseStatus" UniqueName="CaseStatus" ReadOnly="true"
                                                AllowFiltering="false" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn HeaderStyle-Width="100%" ItemStyle-Width="100%" CommandName="dit" HeaderText="Action" ButtonType="ImageButton" UniqueName="Edit"
                                                ImageUrl="~/images/pencil.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn HeaderStyle-Width="100%" ItemStyle-Width="100%" CommandName="del" HeaderText="Action" ButtonType="ImageButton" UniqueName="Delete"
                                                ImageUrl="~/images/Delete.png">
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="container" id="container3" runat="server">
                        <div class="row">
                            <div class="col-md-4">
                                <h5>Select Custodian:</h5>
                                <asp:DropDownList Width="100%" ID="ddlCustodian" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <h5>Select Location:</h5>
                                <asp:DropDownList Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" ID="ddlLocation" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <h5>Assignee Name:</h5>
                                <asp:TextBox ID="txtAssigneeName" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <h5>Case ID:</h5>
                                <asp:TextBox ID="txtid" Width="100%" ReadOnly="true" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtCaseID" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <h5>Organisation Name:</h5>
                                <asp:TextBox ID="txtOrganisationName" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <h5>Select Document Controller:</h5>
                                <asp:DropDownList Width="100%" ID="ddlDocumentControllerUSER_ID" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <h5>Select Minor Location:</h5>
                                <asp:DropDownList Width="100%" ID="ddlBuildingId" AutoPostBack="true" OnSelectedIndexChanged="ddlBuildingId_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <h5>Select Sub-Minor Location:</h5>
                                <asp:DropDownList Width="100%" ID="ddlFloorId" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <h5>Select Tag Type:</h5>
                                <asp:DropDownList Width="100%" ID="ddlTagtypeId" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <h5>Status:</h5>
                                <asp:TextBox ID="txtStatus" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <h5>&nbsp;</h5>
                                <asp:Button Text="SUBMIT" runat="server" ID="Button2" Style="height: 45px; font-size: 20px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-success text-dark" OnClick="Button2_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

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
            </div>
            <%--end main page--%>
        </div>
        <!-- /.col -->
    </div>
    <!-- /.page-content -->
</asp:Content>

