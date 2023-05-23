<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="topClients.aspx.cs" Inherits="_Default" %>


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
        var mouseOverActiveElement = false;

        $(document).ready(function () {
            //             $("#ContentPlaceHolder1_divbyCustodian").hide();
            var hdnValue = $('#ContentPlaceHolder1_hdnCustodianGrid').val();
            if (hdnValue == "show") {
                $("#ContentPlaceHolder1_divbyCustodian").show();
            }
            else {
                $("#ContentPlaceHolder1_divbyCustodian").hide();
            }
            $('#ContentPlaceHolder1_BtnByCustodian').click(function () {

                $("#ContentPlaceHolder1_divbyCustodian").show();
            });
            //ContentPlaceHolder1_BtnByCustodian
            $('#btnLocationClose').click(function () {
                $find("mpe").hide();

                $('#divbyLocation').show();
                $('#divbyCustodian').hide();
            });

            $('#IdCustodianClose').click(function () {
                $find("mpes").hide();

                $('#divbyLocation').hide();
                $('#divbyCustodian').show();
            });

            //             $('#divCloseCustodian').click(function () {
            //                 $('#ContentPlaceHolder1_divbyCustodian').hide(500);
            //             });

            //             $('#divCloseLocation').click(function () {
            //                 $('#ContentPlaceHolder1_divbyLocation').hide(500);
            //             });

            $('#BtnByLocation').click(function () {
                $('#ContentPlaceHolder1_divbyLocation').show(500);
                $('#ContentPlaceHolder1_divbyCustodian').hide(500);
            });

            $('#BtnByCustodian').click(function () {
                $('#ContentPlaceHolder1_divbyCustodian').show(500);
                $('#ContentPlaceHolder1_divbyLocation').hide(500);
            });

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
    <style type="text/css">
        .MyButton {
            color: Blue;
        }
    </style>
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
    <script type="text/javascript">
        $(document).ready(function () {
            function HideModalPopup() {
                $find("mpe").hide();
                return false;
            }
        });

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
            <asp:Button ID="btnYesErr" runat="server" Text="Ok" CssClass="yes" />
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
                                <%-- <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                                <%-- <span class="input-group-btn">
                                    <asp:LinkButton ID="GetGrid" Style="height: 34px" runat="server"
                                        class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton></span>--%>
                            </div>
                            <div class="btn-group pull-right">
                                <asp:Button ID="Button3" class="btn btn-primary" Style="font-size: 12px" runat="server"
                                    Text="EXPORT" OnClick="Button3_Click" /><%--OnClick="btnExportExcel_Click"--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div id="divSearch" runat="server" class="panel-group col-md-9 form_wrapper">
                            <%--<div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label>
                                                Search</label>
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divSearchClose" href="#"><span id="spanSerch" style="top: 0px;"
                                                class="badge"><b>Close</b></span></a>
                                            
                                        </div>
                                    </div>
                                </div>
                                
                            </div>--%>
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
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">Top Clients Data</h1>
                    </div>
                    <div>
                        <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                            OnItemCommand="gv_data_ItemCommand" OnItemDataBound="gvData_ItemDataBound">
                            <%-- <telerik:RadGrid ID="gvData" runat="server" Width="100%"
                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik"
                                 GridLines="None" AllowFilteringByColumn="false" CssClass="gvData"
                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init">--%>
                            <%----%>
                            <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                            <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                            <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                            <ClientSettings EnablePostBackOnRowClick="false">
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                <ClientEvents OnGridCreated="GridCreated" />
                            </ClientSettings>
                            <SortingSettings EnableSkinSortStyles="false" />
                            <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                DataKeyNames="countClientData">
                                <PagerStyle AlwaysVisible="true" Position="Top" />
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <%--<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>--%>
                                <Columns>
                                    <%--<telerik:GridButtonColumn CommandName="clientName" ButtonType="LinkButton" UniqueName="clientName"
                                        DataTextField="clientName" HeaderText="CLIENT NAME" ButtonCssClass="MyButton">
                                    </telerik:GridButtonColumn>--%>
                                     <telerik:GridBoundColumn DataField="clientName" FilterControlAltText="Filter clientName column"
                                        HeaderText="CLIENT NAME" SortExpression="clientName" UniqueName="clientName" ReadOnly="true" AllowSorting="false"
                                        AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="countClientData" FilterControlAltText="Filter countClientData column"
                                        HeaderText="CLIENT DATA COUNT" SortExpression="countClientData" UniqueName="countClientData" ReadOnly="true" AllowSorting="false"
                                        AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn CommandName="dit" HeaderText="View" ButtonType="ImageButton" UniqueName="Edit"
                                        ImageUrl="~/images/view.png">
                                    </telerik:GridButtonColumn>
                                    <%--<telerik:GridBoundColumn DataField="USER_NAME" FilterControlAltText="Filter USER_NAME column"
                                        HeaderText="USER NAME" SortExpression="USER_NAME" UniqueName="USER_NAME" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                        HeaderText="CREATED DATE" SortExpression="CreatedDate" UniqueName="CreatedDate"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="variance" FilterControlAltText="Filter variance column"
                                        HeaderText="% VARIANCE" SortExpression="variance" UniqueName="variance" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                                        HeaderText="CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Mismatch" FilterControlAltText="Filter Mismatch column"
                                        HeaderText="% MISMATCH" SortExpression="Mismatch" UniqueName="Mismatch" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CurrentLocation" FilterControlAltText="Filter CurrentLocation column"
                                        HeaderText="CURRENT LOCATION" SortExpression="CurrentLocation" UniqueName="CurrentLocation"
                                        ReadOnly="true">
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
                    <!-- /.page-header -->
                </div>
            </div>
        </div>
</asp:Content>

