<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AMCWarrentyReport.aspx.cs"
    Inherits="AMCWarrentyReport" MasterPageFile="~/adminMasterPage.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = true; // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = false; // for unchecking the checkboxes
                }
            }
        }
        function unCheckHeader(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == false) {
                var hidden = document.getElementById("HiddenField3");
                var checkBox = document.getElementById(hidden.value);
                checkBox.checked = false;
            }
        }
    </script>
    <%--    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>--%>
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
        .drpwidth {
            width: 200px;
        }

        .btn-file {
            position: relative;
            overflow: hidden;
        }

            .btn-file input[type=file] {
                position: absolute;
                top: 0;
                right: 0;
                min-width: 100%;
                min-height: 50%;
                font-size: 100px;
                text-align: right;
                filter: alpha(opacity=0);
                opacity: 0;
                outline: none;
                background: white;
                cursor: inherit;
                display: block;
            }

        .input-file .input-group-addon {
            border: 0px;
            padding: 0px;
        }

            .input-file .input-group-addon .btn {
                border-radius: 0 4px 4px 0;
            }

            .input-file .input-group-addon input {
                cursor: pointer;
                position: absolute;
                width: 72px;
                z-index: 2;
                top: 0;
                right: 0;
                filter: alpha(opacity=0);
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                opacity: 0;
                background-color: transparent;
                color: transparent;
            }
    </style>
    <script type="text/javascript">
        var mouseOverActiveElement = false;

        $(document).ready(function () {


            $('#txtSearch').click(function () {
                $('#ContentPlaceHolder1_divSearch').show(500);
            })

            $('#divSearchClose').click(function () {
                $('#ContentPlaceHolder1_divSearch').hide(500);
            });

            $(':file').on('change', ':file', function () {
                var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });

            //            $('#txtSearch,#ContentPlaceHolder1_divSearch').on('mouseenter', function () {
            //                mouseOverActiveElement = true;
            //            }).on('mouseleave', function () {
            //                mouseOverActiveElement = false;
            //            });
            //            $("html").click(function () {
            //                if (!mouseOverActiveElement) {
            //                    $('#ContentPlaceHolder1_divSearch').hide(500);
            //                }
            //                else {

            //                }
            //            });
        })

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Button ID="btnErrorPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="btnErrorPopup"
        PopupControlID="pnlErrpopup" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlErrpopup" runat="server" CssClass="modalPopup" Height="140px" Width="400px"
        Style="display: none">
        <div class="headerModal">
            Confirmation
        </div>
        <div class="body">
            <asp:Label ID="Label16" runat="server" Text="You are not authorized to view this page." />
        </div>
        <div align="center">
            <asp:Button ID="btnYesErr" runat="server" Text="Ok" OnClick="btnYesErr_Click" CssClass="yes" />
        </div>
    </asp:Panel>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("[id$=txtFrmDate]").attr("readonly", "true");
            $("[id$=txtFrmDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
                dateFormat: 'dd-mm-yy',
            });
            $("[id$=txtToDate]").attr("readonly", "true");
            $("[id$=txtToDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
                dateFormat: 'dd-mm-yy',

            });
        });

    </script>
    <div class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group input-group-unstyled">
                                <input type="text" id="txtSearch" class="form-control" placeholder="Search" id="inputGroup"
                                    onkeydown="return (event.keyCode!=13);" />
                                <span class="input-group-addon"><i class="fa fa-search"></i></span>
                            </div>
                            <div class="btn-group pull-right">
                                <asp:Button ID="Button3" class="btn btn-primary" Style="font-size: 12px" runat="server"
                                    Text="EXPORT" OnClick="btnExportExcel_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div id="divSearch" runat="server" style="width: 100%;" class="panel-group col-md-12 form_wrapper">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label>
                                                Search</label>
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divSearchClose" href="#"><span id="spanSerch" style="top: 0px;"
                                                class="badge"><b>X</b></span></a>
                                            <%--class="badge"--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="container" style="width: 100%">
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="lblcattype"> <%#Category %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlproCategory" AutoPostBack="true" class="form-control"
                                                                    OnSelectedIndexChanged="OnSelectedIndexChangedCategory">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label1">  <%#SubCategory %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlsubcat" AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label2">Expiring within (Days)</asp:Label>
                                                        <asp:TextBox ID="txtExpiry" class="form-control" runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtExpiry" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="Label3">&nbsp;</asp:Label>
                                                        <asp:Button Text="SEARCH" runat="server" ID="btnsubmit" class="btn btn-primary form-control"
                                                            OnClick="btnSearch_Click" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="Label4">&nbsp;</asp:Label>
                                                        <asp:Button Text="CLEAR" runat="server" ID="btnReset" class="btn btn-danger form-control"
                                                            OnClick="btnRefresh_Click" />
                                                    </div>
                                                </div>
                                            </div>



                                        </div>

                                        <div class="form-group">
                                            <div class="col-sm-2">
                                            </div>
                                            <div class="col-sm-3">
                                            </div>
                                        </div>
                                        <div class="form-group" style="display: none">
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                From Date:</label>
                                            <div class="col-sm-3">
                                                <%-- <input type="text" style="width: 200px" class="form-control" id="Text5" placeholder="Category" />--%>
                                                <asp:TextBox ID="txtFrmDate" class="form-control" runat="server" Style="width: 200px"></asp:TextBox>
                                            </div>
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                To Date:</label>
                                            <div class="col-sm-4">
                                                <%--<input type="text" style="width: 200px" class="form-control" id="name" placeholder="Category" />--%>
                                                <asp:TextBox ID="txtToDate" class="form-control" runat="server" Style="width: 200px"></asp:TextBox>
                                            </div>
                                            <%--OnSelectedIndexChanged="OnSelectedIndexChangedCategory--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <!-- start top menu -->
                                    <div class="hidden">
                                        <uc1:topmenu runat="server" ID="topmenu" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="page-header">
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">Warranty / AMC Reports</h1>
                    </div>
                    <div>
                        <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" GridLines="None" CssClass="gvData">
                            <ItemStyle HorizontalAlign="Left" Wrap="false"></ItemStyle>
                            <AlternatingItemStyle HorizontalAlign="Left"></AlternatingItemStyle>
                            <HeaderStyle HorizontalAlign="Left" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                            <ClientSettings EnablePostBackOnRowClick="false">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                <ClientEvents OnGridCreated="GridCreated" />
                            </ClientSettings>
                            <SortingSettings EnableSkinSortStyles="false" />
                            <MasterTableView AllowPaging="True" AllowSorting="true" PageSize="250" AutoGenerateColumns="false">
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
                                        AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter Category column"
                                        HeaderText="CATEGORY" SortExpression="CategoryName" UniqueName="CategoryName"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SubCatName" FilterControlAltText="Filter SubCategory column"
                                        HeaderText="SUBCATEGORY" SortExpression="SubCatName" UniqueName="SubCatName"
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
                                    <telerik:GridBoundColumn DataField="SupplierName" FilterControlAltText="Filter SupplierName column"
                                        HeaderText="SUPPLIER" SortExpression="SupplierName" UniqueName="SupplierName"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column"
                                        HeaderText="AMC TYPE" SortExpression="Type" UniqueName="Type" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="StartDate" FilterControlAltText="Filter StartDate column"
                                        HeaderText="STARTDATE" SortExpression="StartDate" UniqueName="StartDate" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="EndDate" FilterControlAltText="Filter EndDate column"
                                        HeaderText="ENDDATE" SortExpression="EndDate" UniqueName="EndDate" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Remarks" FilterControlAltText="Filter Remarks column"
                                        HeaderText="REMARKS" SortExpression="Remarks" UniqueName="Remarks" ReadOnly="true">
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
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
