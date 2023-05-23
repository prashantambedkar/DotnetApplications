<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="RAssetMovement.aspx.cs" Inherits="RAssetMovement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
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
    <asp:HiddenField ID="hdnCustodianGrid" runat="server" />
    <ajax:ModalPopupExtender ID="GriddetailsPopup" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Width="80%"
        Style="display: none; height: auto;">
        <div class="body">
            <div class="modal-body modal-lg  modal-dialog-scrollable overflow-scroll" style="width: 100%; height: 80vh; overflow-y: auto; display: block !important;">
                <div class="row" style="width: 100%; overflow-y: initial !important;">
                    <div class="col-md-6">
                        <span style="font-weight: bold;">Total Records.</span>
                        <asp:Label ID="LblDetails" runat="server" Style="font-weight: bold;"></asp:Label>
                    </div>
                    <div class="col-md-6">
                    </div>
                    <div class="col-md-12">
                        <div id="divGrid" style="overflow: auto; overflow: scroll;">
                            <asp:DataGrid ID="gridDetails" runat="server" Width="100%" Height="100%" CssClass="table table-striped table-bordered table-hover"
                                BorderStyle="None" OnPreRender="gridDetails_PreRender" AutoGenerateColumns="false">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="SR. NO.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblserial" Width="100%" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="ASSET CODE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAstCode" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssetCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="false" HeaderText="SERIALNO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerialNo" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="false" HeaderText="PRICE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrice" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Price") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="DOCUMENT CATEGORY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="false" HeaderText="SUB CATEGORY">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCategory" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SubCatName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="FROM CUSTODIAN">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustName" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CustodianName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="OLD LOCATION">
                                        <ItemTemplate>
                                            <asp:Label ID="lbloldLocation" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExistingLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="NEW LOCATION">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewLocation" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NewLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Column1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCol1" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FCNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Column2">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCol2" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CaseAssigneeName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Column3">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCol3" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClientName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </div>
                    <div class="col-md-12 right">
                        <span style="font-weight: bold; float: right;">Customer Signature  :</span><br />
                        <asp:Image ID="ImgSignAsset" ImageAlign="Right" runat="server" Visible="true" Height="150px" Width="150px" />
                        <asp:Image ID="Image1" ImageAlign="Right" runat="server" Visible="false" Height="150px" Width="150px" />
                    </div>

                </div>
                <div class="col-md-2"></div>
                <div class="col-md-1"></div>
                <div class="col-md-2">
                    <input type="button" id="btnLocationClose" value="CLOSE" class="btn btn-primary pull-right" style="width: 100%" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnExport" OnClick="btnExport_Click" CssClass="btn btn-success pull-right" Width="100%" runat="server" Text="EXPORT EXCEL" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnpdfDownload" OnClick="btnpdfDownload_Click" CssClass="btn btn-danger pull-right" Width="100%" ForeColor="Black" runat="server" Text="EXPORT PDF" />
                </div>
                <div class="col-md-2"></div>
                <div class="col-md-2"></div>
            </div>
        </div>
    </asp:Panel>
    <asp:Button ID="btnShowPopupCustodian" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="GriddetailsPopupCustodian" runat="server" TargetControlID="btnShowPopupCustodian"
        PopupControlID="pnlpopupCustodian" BackgroundCssClass="modalBackgroundN" BehaviorID="mpes">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopupCustodian" runat="server" CssClass="modalPopupN"
        Width="800px" Style="display: none">
        <div class="body">
            <div class="modal-body">
                <div class="row">

                    <div class="col-xs-12">
                        <span style="font-weight: bold;">Total Records.</span>
                        <asp:Label ID="LblDetailsCustodian" runat="server" Style="font-weight: bold;"></asp:Label>

                    </div>
                    <div class="col-xs-12">
                        <div id="divGrid_Custodian" style="overflow: auto; height: 250px">
                            <asp:DataGrid ID="gridDetailsCustodian" runat="server" CssClass="table table-striped table-bordered table-hover"
                                BorderStyle="None" AutoGenerateColumns="false" OnPreRender="gridDetailsCustodian_PreRender">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="SR. NO.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="ASSET CODE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAstCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssetCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="FC NUMBER">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCol1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FCNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CASE ASSIGNEE">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCol2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CaseAssigneeName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="CLIENT NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCol3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ClientName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-xs-4 pull-right">
                        <span style="font-weight: bold;">Customer Signature  :</span>
                    </div>
                    <div class="col-xs-4 pullright">
                        <asp:Image ID="ImgSignCust" runat="server" Visible="true" Height="150px" Width="150px" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-1"></div>
                    <div class="col-md-2"></div>
                     <div class="col-md-2">
                         <input type="button" id="IdCustodianClose" value="Close" class="btn btn-primary" style="font-size: 12px; border-radius: 5px;" />
                     </div>
                    <div class="col-sm-3 pull-right">
                        <asp:Button Text=" Export to Excel" runat="server" ID="BtnExportExcelCustodian" CssClass="btn"
                            Visible="false" />
                    </div>
                   


                    <%--  <asp:Button Text=" Close" runat="server" ID="btnCloseCustodian"  CssClass="btn" />--%>
                </div>

                <%--OnClientClick="javascript:return HideModalPopupHideModalPopupCustodian();--%>
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
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group input-group-unstyled">
                                <input type="text" id="txtSearch" class="form-control" placeholder="Search" id="inputGroup" />
                                <span class="input-group-addon"><i class="fa fa-search"></i></span>
                            </div>
                            <div class="btn-group pull-right">
                                <asp:Button ID="BtnExportExcel" class="btn btn-primary" Style="font-size: 12px" runat="server"
                                    Text="EXPORT" OnClick="BtnExportExcel_Click" />
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
                                            <div class="row" style="width: 100%">
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="lblcattype">From Date:</asp:Label>
                                                    <asp:TextBox ID="txtFrmDate" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label1">To Date:</asp:Label>
                                                    <asp:TextBox ID="txtToDate" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Label runat="server" ID="Label2">&nbsp;</asp:Label><br />
                                                    <asp:Button Text="SEARCH" runat="server" ID="btnsubmit" class="btn btn-primary form-control"
                                                        OnClick="btnSearch_Click" />
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button Text="Clear" runat="server" Style="height: 35px" ID="btnReset" class="btn btn-primary"
                                                        Visible="false" />
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
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">
                            <%#Assets %> Transfer Report</h1>
                    </div>
                    <div class="row">
                        <div class="col-sm-2">
                            <%--<input type="button" value="By Location" id="BtnByLocation" height="37px" class="btn btn-primary"
                                style="font-size: 12px" />--%>
                            <asp:Button runat="server" ID="BtnByLocation" class="btn btn-primary form-control"
                                Text="BY LOCATION" OnClick="BtnByLocation_Click" />
                            <%--OnClick="BtnByLocation_Click"--%>
                        </div>
                        <div class="col-sm-2">
                            <%-- <input type="button" value="By Custodian" id="BtnByCustodian" height="37px" class="btn btn-primary"
                                style="font-size: 12px" />--%>
                            <asp:Button runat="server" ID="BtnByCustodian" class="btn btn-primary form-control"
                                Text="BY CUSTODIAN" OnClick="BtnByCustodian_Click" />
                        </div>
                        <%-- <div class="col-sm-7">
                        </div>
                        <div class="col-sm-1">
                            <asp:Button Text=" Export to Excel" runat="server" ID="BtnExportExcel" class="btn btn-primary" Style="font-size: 12px; border-radius: 5px"
                                OnClick="BtnExportExcel_Click" Visible="true" />
                        </div>--%>
                    </div>
                    <div id="divbyLocation" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="form-inline">
                                        <div class="col-sm-2" style="padding-left: 10px" class="input-group input-group-unstyled">
                                            Location Report
                                        </div>
                                        <%--                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="A1" href="#"><span id="span2" style="top: 0px;" class="badge"><b>
                                                Close</b></span></a>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                            CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                            OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                            OnItemDataBound="gvData_ItemDataBound" OnItemCommand="gv_data_ItemCommand">
                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                            <%--<telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                                            CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data_ItemCommand"
                                            OnItemDataBound="gvData_ItemDataBound">--%>
                                            <%----%>
                                            <ItemStyle HorizontalAlign="Center" Wrap="true"></ItemStyle>
                                            <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                            <ClientSettings EnablePostBackOnRowClick="false">
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                                <ClientEvents OnGridCreated="GridCreated" />
                                            </ClientSettings>
                                            <SortingSettings EnableSkinSortStyles="false" />
                                            <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                                DataKeyNames="TransferId">
                                                <PagerStyle AlwaysVisible="true" Position="Top" />
                                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="TransferId" FilterControlAltText="Filter TransferId column"
                                                        HeaderText="TRANSFER ID" SortExpression="TransferId" UniqueName="TransferId"
                                                        ReadOnly="true" AllowSorting="true" Visible="false" AllowFiltering="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridButtonColumn CommandName="TransferCode" ButtonType="LinkButton" UniqueName="TransferCode"
                                                        DataTextField="TransferCode" HeaderText="TRANSFER CODE" ButtonCssClass="MyButton">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridBoundColumn DataField="tsc" FilterControlAltText="Filter tsc column"
                                                        HeaderText="tsc" SortExpression="tsc" Visible="false" UniqueName="tsc" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ToLocation" FilterControlAltText="Filter ToLocation column"
                                                        HeaderText="TO LOCATION" SortExpression="ToLocation" UniqueName="ToLocation" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Trans_Type" FilterControlAltText="Filter Transfer Type column"
                                                        HeaderText="TRANSFER TYPE" SortExpression="Trans_Type" UniqueName="Trans_Type" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GPS_Location" FilterControlAltText="Filter GPS Location column"
                                                        HeaderText="GPS LOCATION" SortExpression="GPS_Location" UniqueName="GPS_Location" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="TransferredBy" FilterControlAltText="Filter TransferredBy column"
                                                        HeaderText="LOGIN BY" SortExpression="TransferredBy" UniqueName="TransferredBy"
                                                        ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="CreateDate" FilterControlAltText="Filter CreatedDate column"
                                                        HeaderText="CREATED DATE" SortExpression="CreateDate" UniqueName="CreateDate"
                                                        ReadOnly="true">
                                                        <ItemStyle Width="150px" />
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="REASON" FilterControlAltText="Filter Reason column"
                                                        HeaderText="REASON" SortExpression="Reason" UniqueName="Reason" ReadOnly="true">
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
                    <div id="divbyCustodian" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="form-inline">
                                        <div class="col-sm-2" style="padding-left: 10px" class="input-group input-group-unstyled">
                                            Custodian Report
                                        </div>
                                        <%--                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divCloseCustodian" href="#"><span id="span1" style="top: 0px;"
                                                class="badge"><b>Close</b></span></a>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <telerik:RadGrid ID="gvData_Custodian" runat="server" Width="98%" OnNeedDataSource="gvData_Custodian_NeedDataSource"
                                            CellSpacing="0" GridLines="None" CssClass="gvDataCustodian" OnItemCommand="gvData_Custodian_ItemCommand"
                                            OnItemDataBound="gvData_Custodian_ItemDataBound">
                                            <%----%>
                                            <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                            <ItemStyle HorizontalAlign="Center" Wrap="true"></ItemStyle>
                                            <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                            <ClientSettings EnablePostBackOnRowClick="false">
                                                <%--<Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                                 <ClientEvents OnGridCreated="GridCreatedCustodian" />--%>
                                            </ClientSettings>
                                            <SortingSettings EnableSkinSortStyles="false" />
                                            <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                                DataKeyNames="TransferId">
                                                <PagerStyle AlwaysVisible="true" Position="Top" />
                                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="TransferId" FilterControlAltText="Filter TransferId column"
                                                        HeaderText="TRANSFER ID" SortExpression="TransferId" UniqueName="TransferId"
                                                        ReadOnly="true" AllowSorting="true" AllowFiltering="false" Visible="false">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridButtonColumn CommandName="TransferCode" ButtonType="LinkButton" UniqueName="TransferCode"
                                                        DataTextField="TransferCode" HeaderText="TRANSFER CODE" ButtonCssClass="MyButton">
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridBoundColumn DataField="FromCustodian" FilterControlAltText="Filter FromCustodian column"
                                                        HeaderText="FROM CUSTODIAN" SortExpression="FromCustodian" UniqueName="FromCustodian"
                                                        ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ToCustodian" FilterControlAltText="Filter ToCustodian column"
                                                        HeaderText="TO CUSTODIAN" SortExpression="ToCustodian" UniqueName="ToCustodian"
                                                        ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Trans_Type" FilterControlAltText="Filter Transfer Type column"
                                                        HeaderText="TRANSFER TYPE" SortExpression="Trans_Type" UniqueName="Trans_Type" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="GPS_Location" FilterControlAltText="Filter GPS Location column"
                                                        HeaderText="GPS LOCATION" SortExpression="GPS_Location" UniqueName="GPS_Location" ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="TransferredBy" FilterControlAltText="Filter TransferredBy column"
                                                        HeaderText="LOGIN BY" SortExpression="TransferredBy" UniqueName="TransferredBy"
                                                        ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="CreateDate" FilterControlAltText="Filter CreatedDate column"
                                                        HeaderText="CREATED DATE" SortExpression="CreateDate" UniqueName="CreateDate"
                                                        ReadOnly="true">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Reason" FilterControlAltText="Filter Reason column"
                                                        HeaderText="REASON" SortExpression="Reason" UniqueName="Reason" ReadOnly="true">
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
    </div>
</asp:Content>
