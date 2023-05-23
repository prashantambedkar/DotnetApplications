<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="DocumentRequestt.aspx.cs" Inherits="_Default" %>


<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
    <div id="div1" class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-6">
                                <span style="font-family: 'Calibri'; font-size: x-large">Document Tagging Request</span>&nbsp;
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
                </div>
                <div class="panel-body">
                    <div class="container" id="container1" runat="server">
                        <div class="row">
                            <div class="col-md-3">
                                <h7>Select Custodian:</h7>
                                <br />
                                <asp:DropDownList Width="100%" ID="ddlCustodian" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <h7>Select Location:</h7>
                                <br />
                                <asp:DropDownList Width="100%" ID="ddlLocation" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <h7>Select Case ID:</h7>
                                <br />
                                <asp:DropDownList Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlCaseId_SelectedIndexChanged" ID="ddlCaseId" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <h7>Select Case Association:</h7>
                                <br />
                                <asp:DropDownList Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlCasePersonAssociation_SelectedIndexChanged" ID="ddlCasePersonAssociation" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">


                            <div class="col-md-3" runat="server" id="OrganisationNameDiv">
                                <h7>Organisation Name:</h7>
                                <br />
                                <asp:TextBox ID="txtOrganisationName" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-3" runat="server" id="AssigneeNameDiv">
                                <h7>Applicant Name:</h7>
                                <br />
                                <%-- <asp:TextBox ID="txtAssigneeName" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList Width="100%" ID="ddlAssigneeName" AutoPostBack="true" OnSelectedIndexChanged="ddlAssigneeName_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtexcelid" Width="100%" ReadOnly="true" Visible="false" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <h7>Select Document Category:</h7>
                                <br />
                                <asp:DropDownList Width="100%" ID="ddlCategory" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3" id="divRemark" runat="server">
                                <h7>Remark:</h7>
                                <br />
                                <asp:TextBox ID="txtRemark" Width="100%" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <h7>Enter No. of Document:</h7>
                                <br />
                                <asp:TextBox runat="server" Width="100%" ID="txtQty" type="number" />
                                <asp:RangeValidator runat="server" ControlToValidate="txtQty" ErrorMessage="Invalid No."
                                    Type="Integer" MinimumValue="1" MaximumValue="1000" ForeColor="Red"></asp:RangeValidator>
                            </div>

                            <div class="col-md-3" id="divName" visible="false" runat="server">
                                <h7>Name:</h7>
                                <br />
                                <asp:TextBox ID="txtName" Width="100%" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <h7>&nbsp;</h7>
                                <br />
                                <asp:Button Text="+ ADD" runat="server" ID="btnadd" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 60%;"
                                    class="btn btn-primary pull-right" OnClick="btnadd_Click" />
                            </div>
                            <div class="col-md-2">
                                <h7>&nbsp;</h7>
                                <br />
                                <asp:Button Text="SUBMIT" runat="server" ID="btnSubmit" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 60%;"
                                    class="btn btn-success pull-left" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3"></div>



                        </div>
                    </div>
                    <br />
                    <div class="container" id="container2" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server"
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
                                            <telerik:GridBoundColumn DataField="ReqHdrId" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter ReqHdrId column"
                                                HeaderText="ReqHdrId" SortExpression="ReqHdrId" UniqueName="ReqHdrId" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CategoryId" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CategoryId column"
                                                HeaderText="CategoryId" SortExpression="CategoryId" UniqueName="CategoryId" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CategoryName" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CategoryName column"
                                                HeaderText="Document Category" SortExpression="CategoryName" UniqueName="CategoryName" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Qty" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter Qty column"
                                                HeaderText="Qty" SortExpression="Qty" UniqueName="Qty" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Remark" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter Remark column"
                                                HeaderText="Remark" SortExpression="Remark" UniqueName="Remark" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CasePersonAssociation" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CasePersonAssociation column"
                                                HeaderText="Case Association" SortExpression="CasePersonAssociation" UniqueName="CasePersonAssociation" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="OrganisationName" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter OrganisationName column"
                                                HeaderText="Organisation Name" SortExpression="OrganisationName" UniqueName="OrganisationName" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <%--     <telerik:GridBoundColumn DataField="AssigneeName" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter AssigneeName column"
                                                HeaderText="Assignee Name" SortExpression="AssigneeName" UniqueName="AssigneeName" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>--%>
                                            <telerik:GridBoundColumn DataField="ApplicantNames" Visible="false" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter ApplicantNames column"
                                                HeaderText="Applicant Names" SortExpression="ApplicantNames" UniqueName="ApplicantNames" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Name" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter Name column"
                                                HeaderText="Name" SortExpression="Name" UniqueName="Name" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
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
                        <div class="col-md-12">
                            <h5>&nbsp;</h5>
                            <%--<asp:Button Text="SUBMIT" runat="server" ID="btnSubmit" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                class="btn btn-success" OnClick="btnSubmit_Click" />--%>
                        </div>
                    </div>
                </div>



            </div>
        </div>

        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">View Document Request</h5>

                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Are you sure you want to delete the data??</h5>
                                    <%--<asp:TextBox ID="txtid" Visible="false" Width="100%" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtCategoryName" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>--%>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Button Text="Yes" runat="server" ID="btnupdates" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                        class="btn btn-info" OnClick="btnupdates_Click" />
                                </div>
                                <div class="col-md-6">
                                    <asp:Button Text="No" runat="server" ID="btnclose" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                        class="btn btn-warning" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
          <div class="col-xs-12" style="background-color: white">
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
    <script type="text/javascript">
        function openModal() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal').modal('show');
        }
    </script>
    <div class="modal fade" id="myModal1" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Document Request Details</h5>

                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <h5>Request Added!!</h5>
                                <%--<asp:TextBox ID="txtid" Visible="false" Width="100%" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtCategoryName" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>--%>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <div class="container-fluid">
                        <div class="row">

                            <div class="col-md-12">
                                <button type="button" class="btn btn-warning" data-dismiss="modal">Ok</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function openModal1() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal1').modal('show');
        }

    </script>
</asp:Content>


