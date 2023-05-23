<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="ViewDocumentRequest.aspx.cs" Inherits="_Default" %>

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
    <div id="div1" class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-6">
                                <span style="font-family: 'Calibri'; font-size: x-large">Document Tagging Approve Request -Case ID:
                                    <asp:Label ID="lblCaseId" runat="server" Text=""></asp:Label></span>&nbsp;
                                <%--<a style="float: inline-end; font-size: 24px" onclick="return (event.keyCode!=13);" runat="server" id="txtSearch"><i class="fa fa-filter"></i></a>--%>
                            </div>

                            <div class="col-sm-6">

                                <i style="float: inline-end; font-size: 24px" class="fa"></i>
                                &nbsp;&nbsp;&nbsp;&nbsp;<img src="" alt="" style="float: right;" runat="server" id="CompanyImg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                    <h6><a href="ViewDocumentRequestt.aspx" class="pull-left"><b><< Go To Previous Page</b></a>
                    </h6>
                    <br />
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

                    <div class="container" id="container2" runat="server">
                        <div class="container">
                            <div class="row">
                                <div class="col-md-3">
                                    <h7>Custodian Name:</h7>
                                    <asp:TextBox ID="txtCustodianName" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-md-3">
                                    <h7>Organisation Name:</h7>
                                    <asp:TextBox ID="txtOrganisationName" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <h7>Status:</h7>
                                    <asp:TextBox ID="txtStatus" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <h7>Tag Type:</h7>
                                    <asp:DropDownList Width="100%" ID="ddlTagType" runat="server">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <h7>Major Location:</h7>
                                    <asp:DropDownList Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlMajorLocation_SelectedIndexChanged" ID="ddlMajorLocation" runat="server">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <h7>Minor Location:</h7>
                                    <asp:DropDownList Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlMinorLocation_SelectedIndexChanged" ID="ddlMinorLocation" runat="server">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <h7>Minor Sub Location:</h7>
                                    <asp:DropDownList Width="100%" ID="ddlMinorSubLocation" runat="server">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <h7>&nbsp;</h7><br />
                                    <asp:Button Text="UPDATE" runat="server" ID="btnupdate" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 50%;"
                                        class="btn btn-primary" OnClick="btnupdate_Click" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-3"></div>
                                <div class="col-md-3"></div>

                            </div>
                        </div>
                    </div>
                    <div class="container" id="Div2" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData2" runat="server" OnItemDataBound="gvData2_ItemDataBound" OnNeedDataSource="gvData2_NeedDataSource"
                                    CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data2_ItemCommand">
                                     <GroupingSettings CaseSensitive="false"></GroupingSettings>
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
                                            <telerik:GridBoundColumn DataField="excelid" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter excelid column"
                                                HeaderText="excelid" SortExpression="excelid" UniqueName="excelid" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="id" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter id column"
                                                HeaderText="id" SortExpression="id" UniqueName="id" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ReqHdrId" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter ReqHdrId column"
                                                HeaderText="ReqHdrId" SortExpression="CaseID" UniqueName="ReqHdrId" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CategoryId" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CategoryId column"
                                                HeaderText="CategoryId" SortExpression="CategoryId" UniqueName="CategoryId" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CategoryName" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter CategoryName column"
                                                HeaderText="Document Category" SortExpression="CategoryName" UniqueName="CategoryName" ReadOnly="true"
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
                                            <telerik:GridBoundColumn DataField="Name" HeaderStyle-Width="100%" ItemStyle-Width="100%" FilterControlAltText="Filter Name column"
                                                HeaderText="Name" SortExpression="Name" UniqueName="Name" ReadOnly="true"
                                                AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn HeaderStyle-Width="100%" ItemStyle-Width="100%" CommandName="dit" HeaderText="EDIT" ButtonType="ImageButton" UniqueName="Edit"
                                                ImageUrl="~/images/pencil.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn HeaderStyle-Width="100%" ItemStyle-Width="100%" CommandName="del" HeaderText="DELETE" ButtonType="ImageButton" UniqueName="Delete"
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
                </div>
            </div>

        </div>

        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">Document Request Details</h5>

                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Category Name:</h5>
                                    <asp:TextBox ID="txtid" Visible="false" Width="100%" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="txtCategoryName" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <h5>No. of Document:</h5>
                                    <%--<asp:TextBox ID="txtqty" Width="100%" runat="server"></asp:TextBox>--%>
                                    <asp:TextBox runat="server" Width="100%" ID="txtqty" type="number" />
                                    <asp:RangeValidator runat="server" ControlToValidate="txtqty" ErrorMessage="Invalid No."
                                        Type="Integer" MinimumValue="1" MaximumValue="1000" ForeColor="Red"></asp:RangeValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Remark:</h5>
                                    <asp:TextBox ID="txtRemark" Width="100%" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <h5>Name:</h5>
                                    <asp:TextBox ID="txtName" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Button Text="UPDATE" runat="server" ID="btnupdates" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                        class="btn btn-info" OnClick="btnupdates_Click" />
                                </div>
                                <div class="col-md-6">
                                    <asp:Button Text="CLOSE" runat="server" ID="btnclose" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                        class="btn btn-warning" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="myModal1" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">View Document Request</h5>

                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Are you sure you want to delete the data??
                                    <asp:Label ID="lbllocname" runat="server" Text=""></asp:Label>??</h5>
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
                                    <asp:Button Text="Yes" runat="server" ID="Button2" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                        class="btn btn-info" OnClick="Button2_Click" />
                                </div>
                                <div class="col-md-6">
                                    <asp:Button Text="No" runat="server" ID="Button3" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                        class="btn btn-warning" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.page-header -->
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
    <script type="text/javascript">
        function openModal() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal').modal('show');
        }
        function setvalueforCategory(CategoryName) {
            document.getElementById('<%= txtCategoryName.ClientID %>').value = CategoryName;
        }
        function setvalueforid(idd) {
            document.getElementById('<%= txtid.ClientID %>').value = id;
        }
        function setvalueforqty(Qty) {
            document.getElementById('<%= txtqty.ClientID %>').value = Qty;
        }
        function setvalueforRemark(Remark) {
            document.getElementById('<%= txtRemark.ClientID %>').value = Remark;
        }
        function setvalueforName(Name) {
            document.getElementById('<%= txtName.ClientID %>').value = Name;
        }
    </script>
    <script type="text/javascript">
        function openModal1() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal1').modal('show');
        }
    </script>
    <!-- /.page-content -->
      <div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Document Request Details</h5>

                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <h5>Updated!!</h5>
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
        function openModal2() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal2').modal('show');
        }

    </script>
</asp:Content>

