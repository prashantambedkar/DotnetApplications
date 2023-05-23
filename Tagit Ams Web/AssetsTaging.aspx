<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="AssetsTaging.aspx.cs" Inherits="AssetsTaging" %>


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
    <script type="text/javascript">
 
        <%--function unCheckHeader(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == false) {
                var hidden = document.getElementById("HiddenField3");
                var checkBox = document.getElementById(hidden.value);
                checkBox.checked = false;
            }
        }--%>
    </script>
    <style type="text/css">
        .drpwidth {
            width: 200px;
        }
    </style>
    <%--            div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }--%>

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
            height: 65%;
        }
    </style>
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
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        function HideModalPopup() {
            $find("mpe").hide();
            txt2 = document.getElementById('<%= txtAreaScannedItems.ClientID%>');
            txt2.value = "";
            txt1 = document.getElementById('<%= hdnScanned.ClientID%>');
            txt1.value = "";
            StopScanning();
            return false;
        }
        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
    </script>
    <script type="text/javascript">


        function CheckAll(id) {
            debugger;
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    if (masterTable.get_dataItems()[i].findElement("cboxSelect").disabled != true)
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
                //var hidden = document.getElementById("HiddenField3");
                var checkBox = document.getElementById(document.getElementById("<%=HiddenField3.ClientID%>").value);
                checkBox.checked = false;
            }
        }

    </script>
    <style type="text/css">
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }
    </style>
    <object id="ctr1" classid="CLSID:4CF51716-6C2C-3F23-AA9C-FE69888AA646" codebase="http://tagitglobal.com/Tagit.cab"
        hidden="hidden">
        <param name="Interval" value="1000">
        <param name="Enabled" value="1">
    </object>
    <script type="text/javascript">
        var v = "";
        var stat = new Boolean();
        var Read1Status = new Boolean();
        Read1Status = false;
        ts = document.getElementById("ctr1");
        try {
            var tg = "1"
            function ConfirmBox() {
                if (tg == "1") {
                    try {
                        stat = ts.LoginToTSBV2("COM3", 10);
                        document.getElementById('<%=hfConfirmValue.ClientID %>').value = stat;
                    }
                    catch (err) {
                        document.getElementById('<%=hfConfirmValue.ClientID %>').value = "false";
                    }
                }
            }
        }
        catch (err) {
            document.getElementById('<%=hfConfirmValue.ClientID %>').value = "false";
        }
    </script>
    <script language="javascript" type="text/javascript">
        var v = "";
        var status = new Boolean();
        var EncodeStatus = new Boolean();
        EncodeStatus = false;
        ts = document.getElementById("ctr1");

        function EncodeTag() {
            var cell = "";
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (row.length > 0) {
                for (var i = 0; i < row.length; i++) {
                    if (masterTable.get_dataItems()[i].findElement("ChkEncode").checked == true) // for checking the checkboxes
                    {
                        cell = masterTable.get_dataItems()[i].findElement("hdbCustCode").value;
                        break;
                    }
                }
            } else {
                alert("No Records to Encode..!!");
                return false;
            }
            if (cell == "") {
                alert("Select any item..!!");
                return false;
            }
            ConfirmBox();
            if (document.getElementById('<%=hfConfirmValue.ClientID %>').value == "false") {
                alert("Connection to TSB failed..!!");
                return false;
            }
            EncodeStatus = ts.EncodeTheTagTSBV2(cell);
            if (EncodeStatus == false) {
                alert("Encode Failed..!!");
                ts.LogOutTSBV2();
                return false;
            }
            else {
                //alert("Encoded successfully..!!");
                ts.LogOutTSBV2();
                return true;
            }
        }
        function ScanTag() {
            ConfirmBox();
            if (document.getElementById('<%=hfConfirmValue.ClientID %>').value == "false") {
                alert("Connection to TSB failed..!!");
                return false;
            }
            txt2 = document.getElementById('<%=txtAreaScannedItems.ClientID %>');
            txt1 = document.getElementById('<%=hdnScanned.ClientID %>');
            txt2.value = "";
            txt1.value = "";
            var code = document.getElementById('<%=hdnClientCode.ClientID %>').value;
            //alert(chosen); 
            ts.StartReadTSBV2(code);
            btn = document.getElementById('<%=btnConfirm.ClientID %>');
            lbl = document.getElementById('<%=lblCount.ClientID %>');
            lbl.textContent = "0";
            btn.disabled = true;
            $find("mpe").show();
            InitializeTimer();
        }

        function StopScanning() {
            secs = 0;
            ts.StopReadTSBV2();
            ts.LogOutTSBV2();
            btn = document.getElementById('<%=btnConfirm.ClientID %>');
            btn.disabled = false;

        }
        function add() {
            txt2 = document.getElementById('<%=txtAreaScannedItems.ClientID %>');
            txt1 = document.getElementById('<%=hdnScanned.ClientID %>');
            v = ts.TagData;
            txt2.value = v;
            txt1.value = v;
            if (v != "") {
                var res = v.split(",");
                lbl = document.getElementById('<%=lblCount.ClientID %>');
                lbl.textContent = res.length;
            }
        }

        //------Timmer--------
        var secs
        var timerID = null
        var timerRunning = false
        var delay = 1


        function InitializeTimer() {
            secs = 1
            StopTheClock()
            StartTheTimer()
        }

        function StopTheClock() {
            if (timerRunning)
                clearTimeout(timerID)
            timerRunning = false
        }

        function StartTheTimer() {
            if (secs == 0) {
                StopTheClock()
            }
            else {
                add();
                timerRunning = true
                timerID = self.setTimeout("StartTheTimer()", delay)
            }
        }
        //--------------------------
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
    <div class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group">
                                <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="GetGrid" Style="height: 34px" runat="server" OnClick="btnSearch_Click"
                                        class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton>
                                </span>
                            </div>
                            <div class="btn-group pull-right">
                                <asp:Button ID="btnTag" class="btn btn-primary" OnClick="btnTag_Click" runat="server" Text="TAG"
                                    Style="font-size: 12px; border-radius: 5px" />
                            </div>
                            <div class="btn-group pull-right">
                                <input type="button" id="btnScan" class="btn btn-primary" onclick="ScanTag()" runat="server" value="SCAN"
                                    style="font-size: 12px; border-radius: 5px" />
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
                                                                <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLcocation"
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
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label11">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="SEARCH" runat="server"
                                                            ID="btnsearchsubmit" OnClick="btnsearchsubmit_Click" class="btn btn-primary form-control" />
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label12">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="CLEAR" runat="server"
                                                            ID="btnRefresh" class="btn btn-danger form-control" OnClick="btnRefresh_Click" />
                                                    </div>
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
                    </div>
                    <div class="page-header">
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">Tag Label</h1>
                    </div>

                    <div>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                    OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                    OnItemDataBound="gvData_ItemDataBound" OnDataBinding="gvData_DataBinding" OnItemCreated="gvData_ItemCreated">

                                    <%--<telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" GridLines="None" CssClass="gvData" OnPageIndexChanged="gvData_PageIndexChanged"
                                OnDataBinding="gvData_DataBinding" OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">--%>
                                    <%----%>
                                     <GroupingSettings CaseSensitive="false"></GroupingSettings>
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
                                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Select">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cboxSelect" runat="server" onclick="unCheckHeader(this)" />
                                                    <asp:HiddenField ID="hdnAstID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AssetId") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                                                SortExpression="ID" UniqueName="ID" ReadOnly="true" AllowSorting="false" AllowFiltering="false">
                                            </telerik:GridBoundColumn>
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
                                            <telerik:GridBoundColumn DataField="Description" Visible="false" FilterControlAltText="Filter Description column"
                                                HeaderText="DESCRIPTION" SortExpression="Description" UniqueName="Description"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Price" Visible="false" FilterControlAltText="Filter Price column"
                                                HeaderText="PRICE" SortExpression="Price" UniqueName="Price" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                                                HeaderText="DOCUMENT CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubCategory" FilterControlAltText="Filter SubCategory column"
                                                HeaderText="SUBCATEGORY" Visible="false" SortExpression="SubCategory" UniqueName="SubCategory"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                                                HeaderText="LOCATION" SortExpression="Location" UniqueName="Location" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Building" FilterControlAltText="Filter Building column"
                                                HeaderText="BUILDING" SortExpression="Building" UniqueName="Building" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Floor" FilterControlAltText="Filter Floor column"
                                                HeaderText="FLOOR" SortExpression="Floor" UniqueName="Floor" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                                                HeaderText="DEPARTMENT" Visible="false" SortExpression="Department" UniqueName="Department" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Custodian" FilterControlAltText="Filter Custodian column"
                                                HeaderText="CUSTODIAN" SortExpression="Custodian" UniqueName="Custodian" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                                                HeaderText="DELIVERY DATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="Column1" FilterControlAltText="Filter Column1 column"
                                                HeaderText="Column1" SortExpression="Column1" UniqueName="Column1"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column2" FilterControlAltText="Filter Column2 column"
                                                HeaderText="Column2" SortExpression="Column2" UniqueName="Column2"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column3" FilterControlAltText="Filter Column3 column"
                                                HeaderText="Column3" SortExpression="Column3" UniqueName="Column3"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column4" FilterControlAltText="Filter Column4 column"
                                                HeaderText="Column4" SortExpression="Column4" UniqueName="Column4"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column5" FilterControlAltText="Filter Column5 column"
                                                HeaderText="Column5" SortExpression="Column5" UniqueName="Column5"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column6" FilterControlAltText="Filter Column6 column"
                                                HeaderText="Column6" SortExpression="Column6" UniqueName="Column6"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column7" FilterControlAltText="Filter Column7 column"
                                                HeaderText="Column7" SortExpression="Column7" UniqueName="Column7"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="Column8" FilterControlAltText="Filter Column8 column"
                                                HeaderText="Column8" SortExpression="Column8" UniqueName="Column8"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column9" FilterControlAltText="Filter Column9 column"
                                                HeaderText="Column9" SortExpression="Column9" UniqueName="Column9"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column10" FilterControlAltText="Filter Column10 column"
                                                HeaderText="Column10" SortExpression="Column10" UniqueName="Column10"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column11" FilterControlAltText="Filter Column11 column"
                                                HeaderText="Column11" SortExpression="Column11" UniqueName="Column11"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column12" FilterControlAltText="Filter Column12 column"
                                                HeaderText="Column12" SortExpression="Column12" UniqueName="Column12"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column13" FilterControlAltText="Filter Column13 column"
                                                HeaderText="Column13" SortExpression="Column13" UniqueName="Column13"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column14" FilterControlAltText="Filter Column14 column"
                                                HeaderText="Column14" SortExpression="Column14" UniqueName="Column14"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column15" FilterControlAltText="Filter Column15 column"
                                                HeaderText="Column15" SortExpression="Column15" UniqueName="Column15"
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <!-- /.page-header -->
            <div class="row">
                <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                <asp:HiddenField runat="server" ID="hfConfirmValue" />
                <asp:HiddenField runat="server" ID="hdnClientCode" />
                <asp:HiddenField runat="server" ID="hdnScanned" />
                <asp:HiddenField runat="server" ID="HiddenField3" />
                <%--end main page--%>
            </div>
        </div>
    </div>
    <ajax:ModalPopupExtender ID="GriddetailsPopup" runat="server" TargetControlID="btnShow"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Height="400px" Width="50%"
        Style="display: none">
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
                        <label runat="server">
                            <b>SCAN&nbsp;ITEMS&nbsp;</b>&nbsp;&nbsp;&nbsp;&nbsp;</label>
                    </div>

                    <br />
                    <br />
                    <br />
                    <div class="col-sm-8">
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="Label1" Text="Count :" runat="server"></asp:Label>
                        <asp:Label ID="lblCount" Text="0" runat="server"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:Label ID="lbl1" Text="Scanned Items :" runat="server"></asp:Label>
                    </div>

                    <div class="col-sm-6">
                        <asp:TextBox ID="txtAreaScannedItems" Enabled="false" TextMode="MultiLine" Height="200px" Width="400px" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-sm-offset-4">
                <input type="button" id="btnStopScanning" class="btn btn-primary" onclick="StopScanning()" runat="server" value="Stop Scaning"
                    style="font-size: 12px; border-radius: 5px" />
                <%--<input type="button" value=" Stop Scaning" runat="server" ID="btnStopScanning" onclick="StopScanning()"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />--%>
                <asp:Button Text=" Close" runat="server" ID="Button2" OnClientClick="javascript:return HideModalPopup();"
                    CssClass="btn btn-primary" Style="font-size: 12px; border-radius: 5px" />
                <asp:Button Text=" Confirm" runat="server" ID="btnConfirm" OnClick="btnConfirm_Click"
                    CssClass="btn btn-primary" Enabled="false" Style="font-size: 12px; border-radius: 5px" />
            </div>
        </div>
    </asp:Panel>
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
                    <asp:Button ID="btnClose" runat="server" CssClass="bgimage" />
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


    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Style="display: none" />
</asp:Content>

