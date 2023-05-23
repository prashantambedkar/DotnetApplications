<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="RTaggedAssets.aspx.cs" Inherits="RTaggedAssets" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
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
    <script type="text/javascript">
        var mouseOverActiveElement = false;

        $(document).ready(function () {


            $('#ContentPlaceHolder1_txtSearch').click(function () {


                if ($('#ContentPlaceHolder1_txtSearch').val().length > 1) {
                    $('#ContentPlaceHolder1_divSearch').hide(500);
                } else {
                    $('#ContentPlaceHolder1_divSearch').show(500);
                }
            })
            //            $('#ContentPlaceHolder1_btnRefresh').click(function () {

            //                $('#ContentPlaceHolder1_ddlproCategory').val(0);
            //                $('#ContentPlaceHolder1_txtAssetCode').val('');
            //                $('#ContentPlaceHolder1_ddlsubcat').val('0');

            //                $('#ContentPlaceHolder1_ddlloc').val('0');
            //                $('#ContentPlaceHolder1_ddlbuild').val('0');
            //                $('#ContentPlaceHolder1_ddlfloor').val('0');
            //                $('#ContentPlaceHolder1_ddldept').val('0');
            //                $('#ContentPlaceHolder1_ddlCustodian').val('0');
            //                
            //                $('#ContentPlaceHolder1_divSearch').show();

            //            });
            $('#ContentPlaceHolder1_GetGrid').click(function () {
                if ($('#ContentPlaceHolder1_txtSearch').val().length == 0) {
                    alert('Enter some text to search');
                    return false;
                }
            });
            $('#ContentPlaceHolder1_txtSearch').keydown(function (e) {

                if ($('#ContentPlaceHolder1_txtSearch').val().length > 0) {
                    $('#ContentPlaceHolder1_divSearch').hide(500);
                }
                else {
                    $('#ContentPlaceHolder1_divSearch').show(500);

                }


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
                                <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="GetGrid" Style="height: 34px" runat="server" OnClick="GetGrid_Click"
                                        class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton></span>
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
                        <div id="divSearch" runat="server" style="width: 100%;" class="panel-group col-md-9 form_wrapper">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label>
                                                Search</label>
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divSearchClose" href="#"><span id="spanSerch" style="top: 0px;"
                                                class="badge">X</span></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="container" style="width: 100%">
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="lblcattype"> <%#Assets %> Code</asp:Label>
                                                        <asp:TextBox ID="txtAssetCode" runat="server" class="form-control"
                                                            placeholder="Enter the value" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label4"> <%#Category %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlproCategory" OnSelectedIndexChanged="OnSelectedIndexChangedCategory"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label6"> <%#Location %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLocation"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label7"> <%#Building %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlbuild" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label8"><%#Floor %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlfloor" AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-0">
                                                        <asp:Label runat="server" Visible="false" ID="Label9">Department</asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel6" Visible="false" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddldept" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label10">Custodian</asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlCustodian" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label1">From Date:</asp:Label>
                                                        <asp:TextBox ID="txtFrmDate" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label2">To Date:</asp:Label>
                                                        <asp:TextBox ID="txtToDate" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                    </div>
                                                    <div class="col-sm-2">
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label11">&nbsp;</asp:Label>
                                                        <asp:Button Text="SEARCH" runat="server"
                                                            ID="btnsubmit" OnClick="btnSearch_Click" class="btn btn-primary form-control" />
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label12">&nbsp;</asp:Label>
                                                        <asp:Button Text="CLEAR" runat="server"
                                                            ID="btnRefresh" class="btn btn-danger form-control" OnClick="btnRefresh_Click" />
                                                    </div>
                                                </div>
                                            </div>


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
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">Tagged <%#Assets %></h1>
                    </div>
                    <div>
                        <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                            OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">
                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                            <%--   <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" GridLines="None" CssClass="gvData" OnDataBinding="gvData_DataBinding" OnItemDataBound="gvData_ItemDataBound"
                             OnItemCreated="gvData_ItemCreated">--%>
                            <%----%>
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
                                    <telerik:GridBoundColumn DataField="AssetId" FilterControlAltText="Filter AssetId column"
                                        HeaderText="AssetId" SortExpression="AssetId" UniqueName="AssetId" ReadOnly="true"
                                        AllowSorting="false" AllowFiltering="false" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter ID column"
                                        HeaderText="ASSETCODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                                        AllowSorting="true" AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                                        HeaderText="SERIAL NO" SortExpression="SerialNo" Visible="false" UniqueName="SerialNo" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                        HeaderText="DESCRIPTION" Visible="false" SortExpression="Description" UniqueName="Description"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                                        HeaderText="PRICE" Visible="false" SortExpression="Price" UniqueName="Price" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter Category column"
                                        HeaderText="CATEGORY" SortExpression="CategoryName" UniqueName="Category" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SubCatName" FilterControlAltText="Filter SubCatName column"
                                        HeaderText="SUBCATEGORY" Visible="false" SortExpression="SubCatName" UniqueName="SubCategory">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column"
                                        HeaderText="LOCATION" SortExpression="LocationName" UniqueName="Location"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Buildingname" FilterControlAltText="Filter Buildingname column"
                                        HeaderText="BUILDING" SortExpression="Buildingname" UniqueName="Building"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FloorName" FilterControlAltText="Filter FloorName column"
                                        HeaderText="FLOOR" SortExpression="FloorName" UniqueName="Floor" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DepartmentName" FilterControlAltText="Filter DepartmentName column"
                                        HeaderText="DEPARTMENT" Visible="false" SortExpression="DepartmentName" UniqueName="DepartmentName"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CustodianName" FilterControlAltText="Filter Custodian column"
                                        HeaderText="CUSTODIAN NAME" SortExpression="CustodianName" UniqueName="CustodianName" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column1" FilterControlAltText="Filter Column1 column"
                                        HeaderText="FC Number" SortExpression="Column1" UniqueName="Column1"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column2" FilterControlAltText="Filter Column2 column"
                                        HeaderText="Case Assignee Name" SortExpression="Column2" UniqueName="Column2"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column3" FilterControlAltText="Filter Column3 column"
                                        HeaderText="Client Name" SortExpression="Column3" UniqueName="Column3"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column4" FilterControlAltText="Filter Column4 column"
                                        HeaderText="Document Controller Name" SortExpression="Column4" UniqueName="Column4"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column5" FilterControlAltText="Filter Column5 column"
                                        HeaderText="Case Manager FullName" SortExpression="Column5" UniqueName="Column5"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column6" FilterControlAltText="Filter Column6 column"
                                        HeaderText="Case Manager Email" SortExpression="Column6" UniqueName="Column6"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column7" FilterControlAltText="Filter Column7 column"
                                        HeaderText="Case Worker1 Name" SortExpression="Column7" UniqueName="Column7"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="Column8" FilterControlAltText="Filter Column8 column"
                                        HeaderText="Case Worker1 Email" SortExpression="Column8" UniqueName="Column8"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column9" FilterControlAltText="Filter Column9 column"
                                        HeaderText="Case Status" SortExpression="Column9" UniqueName="Column9"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column10" FilterControlAltText="Filter Column10 column"
                                        HeaderText="Case Person Association" SortExpression="Column10" UniqueName="Column10"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column11" FilterControlAltText="Filter Column11 column"
                                        HeaderText="Column11" SortExpression="Column11" UniqueName="Column11" Visible="false"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column12" FilterControlAltText="Filter Column12 column"
                                        HeaderText="Column12" SortExpression="Column12" UniqueName="Column12" Visible="false"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column13" FilterControlAltText="Filter Column13 column"
                                        HeaderText="Column13" SortExpression="Column13" UniqueName="Column13" Visible="false"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column14" FilterControlAltText="Filter Column14 column"
                                        HeaderText="Column14" SortExpression="Column14" UniqueName="Column14" Visible="false"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Column15" FilterControlAltText="Filter Column15 column"
                                        HeaderText="Column15" SortExpression="Column15" UniqueName="Column15" Visible="false"
                                        ReadOnly="true">
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
    </div>
</asp:Content>
