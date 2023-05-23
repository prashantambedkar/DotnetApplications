<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="REmptyLocation.aspx.cs" Inherits="REmptyLocation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
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
            $("[id$=txtFrmDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif',
                dateFormat: 'dd-mm-yy',
            });
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
            <%--            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Report</a> </li>
                    <li><a href="#">Encoded Labels </a></li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Encoded Labels</h1>
            </div>--%>
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
                                                        <asp:Label runat="server" ID="lblcattype"> <%#Location %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLocation"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label1">  <%#Building %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlbuild" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label2">  <%#Floor %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlfloor" AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label3">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="SEARCH" runat="server" class="btn btn-primary form-control"
                                                            OnClick="btnSearch_Click" />
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label4">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="CLEAR" runat="server" ID="btnReset" class="btn btn-danger form-control"
                                                            OnClick="btnRefresh_Click" />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="form-group">


                                            <div class="col-sm-3">
                                            </div>
                                            <div class="col-sm-4">
                                            </div>
                                        </div>
                                        <%--<div class="col-sm-3 pu">
                                              
                                            </div>--%>
                                        <%--OnClick="btnsubmit_Click"--%>
                                        <%-- OnClick="btnclear_Click"--%>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <!-- start top menu -->
                                    <div class="hidden">
                                        <uc1:topmenu runat="server" ID="topmenu1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="page-header">
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">Empty Locations</h1>
                    </div>
                    <div>
                        <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                            OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">
                             <GroupingSettings CaseSensitive="false"></GroupingSettings>
                            <%-- <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" GridLines="None" CssClass="gvData" OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">--%>
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
                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="SR.NO.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="60px" />
                                        <HeaderStyle Width="60px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="FloorId" FilterControlAltText="Filter ID column"
                                        HeaderText="ID" SortExpression="FloorId" UniqueName="FloorId" ReadOnly="true"
                                        AllowSorting="false" AllowFiltering="false" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Code" FilterControlAltText="Filter Code column"
                                        HeaderText="CODE" SortExpression="Code" UniqueName="Code" ReadOnly="true"
                                        AllowSorting="true" AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="FloorName" FilterControlAltText="Filter Floor column"
                                        HeaderText="FLOOR" SortExpression="FloorName" UniqueName="FloorName" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="BuildingName" FilterControlAltText="Filter Building column"
                                        HeaderText="BUILDING" SortExpression="BuildingName" UniqueName="BuildingName" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter Location column"
                                        HeaderText="LOCATION" SortExpression="LocationName" UniqueName="LocationName" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                        HeaderText="CREATED DATE" SortExpression="CreatedDate" UniqueName="CreatedDate" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter CreatedBy column"
                                        HeaderText="CREATED BY" SortExpression="CreatedBy" UniqueName="CreatedBy" ReadOnly="true">
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
                    <!-- /.page-header -->
                </div>
            </div>
        </div>
</asp:Content>


