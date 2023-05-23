<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="Assettransfer.aspx.cs" Inherits="Assettransfer" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function validateTHSFile() {
            //debugger;
            var array = ['txt', 'TXT', 'Txt'];

            var xyz = document.getElementById('<%=productimguploder.ClientID %>');

            var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();

            if (array.indexOf(Extension) <= -1) {

                alert("Please Upload only .txt extension file");
                document.getElementById('<%=productimguploder.ClientID %>').focus();
                return false;

            }
        }

        function validateTHRFile() {
            //debugger;
            var array = ['xml', 'XML'];

            var xyz = document.getElementById('<%=productimguploder.ClientID %>');

            var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();

            if (array.indexOf(Extension) <= -1) {

                alert("Please Upload only .xml extension file");
                document.getElementById('<%=productimguploder.ClientID %>').focus();
                return false;

            }
        }
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modalBackgroundN
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        .modalPopupN
        {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
        }
        .modalPopup .headerN
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .bodyN
        {
            min-height: 70px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .footerN
        {
            padding: 3px;
        }
        .modalPopup .yesN, .modalPopup .noN
        {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
        }
        .modalPopup .yesN
        {
            background-color: #2FBDF1;
            border: 1px solid #0DA9D0;
        }
        .modalPopup .noN
        {
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }
    </style>
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="GriddetailsPopup" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Height="500px" Width="800px"
        Style="display: none">
        <div class="body">
            <div class="modal-body">
                <div class="row">
                    <div class="col-xs-12">
                        <span style="font-weight: bold;">Total Records.</span>
                        <asp:Label ID="LblDetails" runat="server" Style="font-weight: bold;"></asp:Label>
                    </div>
                    <div class="col-xs-12">
                        <div id="divGrid" style="overflow: auto; height: 250px">
                            <asp:DataGrid ID="gridDetails" runat="server" CssClass="table table-striped table-bordered table-hover"
                                BorderStyle="None" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Asset Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAstCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssetCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="SerialNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Price") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Sub Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SubCatName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="old Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lbloldLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExistingLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="New Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNewLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NewLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
                <div class="col-md-offset-3 col-md-6">
                    <asp:Button Text=" Export to Excel" runat="server" ID="BtnExportExcel" OnClick="BtnExportExcel_Click"
                        CssClass="btn" />
                    <asp:Button Text=" Close" runat="server" ID="btnClose" OnClientClick="javascript:return HideModalPopup();"
                        CssClass="btn" />
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
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Operation</a> </li>
                    <li><a href="#">Asset Transfer</a> </li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Asset Transfer</h1>
            </div>
            <!-- /.page-header -->
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>
                    <div class="form-horizontal">
                        <div class="clearfix form-actions">
                            <div class="col-xs-12">
                                <label runat="server" id="lblimgbrs" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    Browse File
                                </label>
                                <div class="col-sm-3">
                                    <asp:FileUpload runat="server" ID="productimguploder" class="id-input-file-3" />
                                    <asp:Image ID="mimage" runat="server" Style="width: 200px; width: 150px;" Visible="false" />
                                </div>
                                <asp:Button ID="BtnSendTHR" CssClass="btn" runat="server" Text="Import From THR"
                                    OnClick="BtnGetTHR_Click" OnClientClick="javascript:return validateTHRFile();" />
                                <asp:Button ID="BtnSendTHS" CssClass="btn" runat="server" Text="Import from THS"
                                    OnClick="BtnGetTHS_Click" OnClientClick="javascript:return validateTHSFile();" />
                                <asp:Button ID="BtnDownload" CssClass="btn" runat="server" Text="Download Master"
                                    OnClick="BtnDownloadMaster_Click" Visible="false" />
                            </div>
                            <div class="col-xs-12">
                                <label runat="server" id="lblFromDate" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    From Date:
                                </label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtFrmDate" class="form-control" runat="server"></asp:TextBox>
                                </div>
                                <label runat="server" id="Label11" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    To Date:
                                </label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtToDate" class="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button CssClass="btn" ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" />
                                </div>
                            </div>
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="col-xs-12">
                        <asp:Label ID="lblTotHeader" runat="server" Style="font-weight: bold;">Total Records.</asp:Label>
                        <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:DataGrid ID="gridlist" runat="server" runat="server" AllowPaging="True" AllowSorting="True"
                                CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                                BorderStyle="None" PageSize="10" OnPageIndexChanged="gridlist_PageChanger">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="HdnTrnID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "TransferID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Transaction ID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkTransfer" OnClick="OpenTransferDetails" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TransferCode") %>'
                                                ForeColor="Blue"></asp:LinkButton>
                                            <%--<asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TransferCode") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <%--                                    <asp:TemplateColumn HeaderText="From Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFrmLoc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FromLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>--%>
                                    <asp:TemplateColumn HeaderText="To Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblToLoc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ToLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Date & Time">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CreateDate","{0:MM-dd-yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Transferred By">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransferredBy" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TransferredBY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Reason ">
                                        <ItemTemplate>
                                            <asp:Label ID="Reason" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Reason") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="left" CssClass="GridPager1" Mode="NumericPages" />
                                <PagerStyle BackColor="#F9F9F9" ForeColor="#393939" HorizontalAlign="Center" Mode="NumericPages"
                                    Font-Bold="True" />
                                <HeaderStyle BackColor="#F9F9F9" Font-Bold="True" ForeColor="#393939" Height="25px" />
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
                <%--end main page--%>
            </div>
        </div>
    </div>
</asp:Content>
