<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="Assethistory.aspx.cs" Inherits="Assethistory" %>

<%@ Register Src="ProductDetailsCS.ascx" TagName="ProductDetails" TagPrefix="uc1" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        .MyButton {
            color: Blue;
        }
    </style>
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
    <script type="text/javascript">
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
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
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .modalBackgroundN {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        .modalPopupN {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
        }

        .modalPopup .headerN {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }

        .modalPopup .bodyN {
            min-height: 70px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
            z-index: 9998 !important;
        }

        .modalPopup .footerN {
            padding: 3px;
        }

        .modalPopup .yesN, .modalPopup .noN {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
        }

        .modalPopup .yesN {
            background-color: #2FBDF1;
            border: 1px solid #0DA9D0;
        }

        .modalPopup .noN {
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }
    </style>
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="GriddetailsPopup" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Height="600px" Width="90%"
        Style="display: none;">
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
                        <label style="font-family: 'Calibri'; font-size: x-large; color: black;">
                            <b><%#Assets.ToUpper() %>&nbsp;VERIFICATION&nbsp;REPORT</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-8 pull-right">
                        <%--<div class="col-sm-2">--%>
                        <asp:Label ID="LblDoneBy" runat="server"></asp:Label>
                        <%--</div>--%>
                        <%-- <div class="col-sm-2">--%>
                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                        <%-- </div>--%>
                        <asp:Label ID="lblLocation" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="body">
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-1">
                        </div>
                        <div class="col-sm-1" style="font-weight: bold; border: thin;">
                            Summary:
                        </div>
                        <div style="width: 70px" class="col-sm-1">
                            Missing:
                        </div>
                        <div class="col-sm-1">
                            <span class="badge badge-success">
                                <asp:Label ID="lblmissing" runat="server" Style="font-weight: bold;"></asp:Label></span>
                        </div>
                        <div style="width: 70px" class="col-sm-1">
                            Found:
                        </div>
                        <div class="col-sm-1">
                            <span class="badge badge-success">
                                <asp:Label ID="lblFound" runat="server" Style="font-weight: bold;"></asp:Label></span>
                        </div>
                        <div style="width: 70px" class="col-sm-1">
                            Extra:
                        </div>
                        <div class="col-sm-1">
                            <span class="badge badge-success">
                                <asp:Label ID="lblExtra" runat="server" Style="font-weight: bold;"></asp:Label></span>
                        </div>
                        <div style="width: 70px" class="col-sm-1">
                            Mismatch:
                        </div>
                        <div class="col-sm-1">
                            <span class="badge badge-success">
                                <asp:Label ID="lblMismatch" runat="server" Style="font-weight: bold;"></asp:Label></span>
                        </div>
                    </div>
                    <hr style="margin-top: 2px; margin-bottom: 4px;" />
                    <div class="row">
                        <div class="form-horizantal">
                            <div class="form-group">
                                <label style="padding-top: 7px;" class="col-sm-1" for="name">
                                    Status:</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList runat="server" ID="ddlStatus" class="form-control" OnSelectedIndexChanged="BindGridOnStatusChange">
                                    </asp:DropDownList>
                                </div>
                                <label id="Label5" runat="server" style="padding-top: 7px;" class="col-sm-1" for="form-field-1-1">
                                    Custodian:
                                </label>
                                <div class="col-sm-3">
                                    <asp:DropDownList runat="server" ID="ddlCustodian" class="form-control">
                                    </asp:DropDownList>
                                </div>
                                <label id="Label1" runat="server" style="padding-top: 7px;" class="col-sm-1" for="form-field-1-1">
                                    Category:
                                </label>
                                <div class="col-sm-3">
                                    <asp:DropDownList runat="server" ID="ddlCategorys" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-xs-12">
                        <div id="divGrid" style="overflow: auto; height: 0px">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <%--class="col-md-offset-3 col-md-6"--%>
                    <%----%>
                    <div class="col-md-4">
                    </div>
                    <div class="col-md-1">
                        <asp:Button Text="Search" runat="server" ID="btnSearchPop" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                            class="btn btn-primary" OnClick="btnSearchPop_Click" />
                    </div>
                    <div class="col-md-1">
                          <asp:Button Text="Export" runat="server" ID="BtnExportExcel" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                            class="btn btn-success" OnClick="BtnExportExcel_Click" />
                    </div>
                    <div class="col-md-1">
                        <asp:Button Text="Close" runat="server" ID="btnClose" OnClientClick="javascript:return HideModalPopup();" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                            class="btn btn-danger" />
                    </div>
                    <%-- <div class="col-sm-offset-4">
                        <asp:Button Text=" Search" runat="server" ID="btnSearchPop" OnClick="btnSearchPop_Click"
                            CssClass="btn btn-primary" Style="height: 35px; border-radius: 5px; padding: 0" />
                        <asp:Button Text=" Export" runat="server" ID="BtnExportExcel" OnClick="BtnExportExcel_Click"
                            CssClass="btn btn-primary" Style="height: 35px; border-radius: 5px; padding: 0" />
                        <asp:Button Text=" Close" runat="server" ID="btnClose" OnClientClick="javascript:return HideModalPopup();"
                            CssClass="btn btn-primary" Style="height: 35px; border-radius: 5px; padding: 0" />
                    </div>--%>
                </div>
            </div>
            <br />
            <div>
                <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" Visible="false" />
                <div class="demo-container size-medium">
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>

                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <telerik:RadToolTipManager RenderMode="Lightweight" ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose"
                        Width="350" Height="250" runat="server" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element" Style="z-index: 100000"
                        Position="MiddleRight">
                    </telerik:RadToolTipManager>

                    <telerik:RadGrid ID="gv_Popup" runat="server" CellSpacing="0" Width="98%" GridLines="None" CssClass="gvData" OnDataBinding="gv_Popup_DataBinding"
                        OnItemCommand="grid_view_ItemCommand" OnItemDataBound="grid_view_ItemDataBound" OnItemCreated="grid_view_ItemCreated">
                         <GroupingSettings CaseSensitive="false"></GroupingSettings>
                        <%----%>
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
                                <telerik:GridTemplateColumn HeaderText="SR NO" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter AssetCode column"
                                    HeaderText="ASSET CODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                                    AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                                    HeaderText="CATEGORY" SortExpression="CategoryName" UniqueName="CategoryName" Visible="false"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn Visible="false" DataField="FromLocation" FilterControlAltText="Filter FromLocation column"
                                    HeaderText="PREVIOUS LOCATION" SortExpression="FromLocation" UniqueName="FromLocation"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn Visible="false" DataField="ToLocation" FilterControlAltText="Filter ToLocation column"
                                    HeaderText="NEW LOCATION" SortExpression="ToLocation" UniqueName="ToLocation"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column"
                                    HeaderText="LOCATION" SortExpression="LocationName" UniqueName="LocationName"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Buildingname" FilterControlAltText="Filter Buildingname column"
                                    HeaderText="BUILDING" SortExpression="Buildingname" UniqueName="Buildingname"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FloorName" FilterControlAltText="Filter FloorName column"
                                    HeaderText="FLOOR" SortExpression="FloorName" UniqueName="FloorName" ReadOnly="true"
                                    AllowSorting="true" AllowFiltering="true" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                    HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true"
                                    AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CustodianName" FilterControlAltText="Filter CustodianName column"
                                    HeaderText="CUSTODIAN NAME" SortExpression="CustodianName" UniqueName="CustodianName"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true" Visible="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Reason" FilterControlAltText="Filter Reason column"
                                    HeaderText="REASON NAME" SortExpression="Reason" UniqueName="Reason"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true" Visible="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Column1" FilterControlAltText="Filter Column1 column"
                                    HeaderText="COLUMN1" SortExpression="Column1" UniqueName="Column1"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Column2" FilterControlAltText="Filter Column2 column"
                                    HeaderText="COLUMN2" SortExpression="Column2" UniqueName="Column2"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Column3" FilterControlAltText="Filter Column3 column"
                                    HeaderText="COLUMN3" SortExpression="Column3" UniqueName="Column3"
                                    ReadOnly="true" AllowSorting="true" AllowFiltering="true">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn AllowSorting="true" DataField="ImageName" HeaderText="IMAGE NAME"
                                    SortExpression="ImageName" UniqueName="ImageName" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="IMAGE" SortExpression="ImageName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="#" Text='<%# Eval("AssetCode") %>' Style="color: darkcyan"></asp:HyperLink>
                                    </ItemTemplate>
                                    <%----%>
                                </telerik:GridTemplateColumn>


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
            <%--            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Report</a> </li>
                    <li><a href="#">Asset History</a> </li>
                </ul>
            </div>--%>
            <%--            <div class="page-header">
                <h1>
                    Asset Verification Report</h1>
            </div>--%>
            <!-- /.page-header -->
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group input-group-unstyled">
                                <input type="text" id="txtSearch" class="form-control" placeholder="Search" id="inputGroup"
                                    onkeydown="return (event.keyCode!=13);" />
                                <span class="input-group-addon"><i class="fa fa-search"></i></span>
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
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                From Date:
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtFrmDate" class="form-control" runat="server" Style="width: 200px"></asp:TextBox>
                                            </div>
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                To Date:
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtToDate" class="form-control" runat="server" Style="width: 200px"></asp:TextBox>
                                            </div>
                                            <%--OnSelectedIndexChanged="OnSelectedIndexChangedCategory--%>
                                        </div>
                                        <div class="form-group">
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                Type:
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:DropDownList runat="server" ID="DdlType" AppendDataBoundItems="true" class="form-control drpwidth">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-3">
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Button Text="Search" runat="server" ID="btnsubmit" class="btn btn-primary form-control"
                                                    OnClick="btnSearch_Click" />
                                                <asp:Button Text="Clear" runat="server" Style="height: 35px" ID="btnReset" class="'btn btn-primary"
                                                    OnClick="btnRefresh_Click" Visible="false" />
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
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;"><%#Assets %> Verification Report</h1>
                    </div>
                    <div>
                        <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None"
                            AllowFilteringByColumn="true" CssClass="gvData"
                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                            OnItemDataBound="gvData_ItemDataBound" OnItemCommand="gv_data_ItemCommand">
                             <GroupingSettings CaseSensitive="false"></GroupingSettings>

                            <%--   <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data_ItemCommand"
                            OnItemDataBound="gvData_ItemDataBound">--%>
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
                                DataKeyNames="VID">
                                <PagerStyle AlwaysVisible="true" Position="Top" />
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="SR NO" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="VID" FilterControlAltText="Filter VID column"
                                        HeaderText="VID" SortExpression="VID" UniqueName="VID" ReadOnly="true" AllowSorting="true"
                                        AllowFiltering="false" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn CommandName="VerificationID" ButtonType="LinkButton" UniqueName="VerificationID"
                                        DataTextField="VerificationID" HeaderText="VERIFICATION ID" ButtonCssClass="MyButton">
                                    </telerik:GridButtonColumn>
                                    <telerik:GridBoundColumn DataField="USER_NAME" FilterControlAltText="Filter USER_NAME column"
                                        HeaderText="USER NAME" SortExpression="USER_NAME" UniqueName="USER_NAME" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                        HeaderText="CREATED DATE" SortExpression="CreatedDate" UniqueName="CreatedDate"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="variance" FilterControlAltText="Filter variance column"
                                        HeaderText="% VARIANCE" SortExpression="variance" UniqueName="variance" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                 <%--   <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                                        HeaderText="CATEGORY" SortExpression="Category" Visible="false" UniqueName="Category" ReadOnly="true">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridBoundColumn DataField="Mismatch" FilterControlAltText="Filter Mismatch column"
                                        HeaderText="% MISMATCH" SortExpression="Mismatch" UniqueName="Mismatch" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="CurrentLocation" FilterControlAltText="Filter CurrentLocation column"
                                        HeaderText="CURRENT LOCATION" SortExpression="CurrentLocation" UniqueName="CurrentLocation"
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
                    <div class="row">
                        <div class="col-xs-12">
                            <!-- start top menu -->
                            <div class="col-xs-12">
                                <asp:Label ID="lblTotHeader" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                                <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                            </div>
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
                        <label id="Label2" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label>
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
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">Report Status</h5>

                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>
                                        <label id="lbllocname"></label>
                                    </h5>
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
                                    <button type="button" class="btn btn-warning" data-dismiss="modal">OK</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function openModal() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
                $('#myModal').modal('show');
            }
            function setvalueforlocation(lbllocname) {
                document.getElementById('lbllocname').innerHTML = lbllocname;
                //document.getElementById('lbllocname').innerHTML = lbllocname;
            }
        </script>
</asp:Content>
