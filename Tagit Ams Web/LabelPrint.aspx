<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="LabelPrint.aspx.cs" Inherits="LabelPrint" %>

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
                //var hidden = document.getElementById("HiddenField3");
                var checkBox = document.getElementById(document.getElementById("<%=HiddenField3.ClientID%>").value);
                checkBox.checked = false;
            }
        }
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
            return false;
        }
        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
    </script>
    <script type="text/javascript">
        function checkAll(gvExample, colIndex) {
            var GridView = gvExample.parentNode.parentNode.parentNode;
            for (var i = 1; i < GridView.rows.length - 1; i++) {
                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                chb.checked = gvExample.checked;
            }
        }

        function checkItem_All(objRef, colIndex) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var selectAll = GridView.rows[0].cells[colIndex].getElementsByTagName("input")[0];
            if (!objRef.checked) {
                selectAll.checked = false;
            }
            else {
                var checked = true;
                for (var i = 1; i < GridView.rows.length - 1; i++) {
                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                    if (!chb.checked) {
                        checked = false;
                        break;
                    }
                }
                selectAll.checked = checked;
            }
        }

    </script>
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
                        stat = ts.TestActivex();
                        document.getElementById('<%=hfConfirmValue.ClientID %>').value = "true";
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
        var Read1Status = new Boolean();
        Read1Status = false;
        ts = document.getElementById("ctr1");

        function PrintTags(print) {
            status = ts.PrintTags(print);
            if (status == false) {
                alert("Print Failed..!!");
                return;
            }
            else {
                alert("Item sent to printer successfully..!!");
                return;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .FixedHeader {
            position: absolute;
            font-weight: bold;
        }
    </style>
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
                                <asp:Button ID="btnConfirmPrint" class="btn btn-primary" runat="server" Text="PRINT"
                                    OnClick="btnConfirmPrint_Click" OnClientClick="ConfirmBox()" />
                            </div>
                            <div class="btn-group pull-right">
                                <asp:Button ID="btnPreview" class="btn btn-primary" runat="server" Text="PREVIEW"
                                    OnClick="btnPreview_Click" />
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


                                        <div class="col-sm-12 align-center">
                                        </div>
                                        <%--<div class="col-sm-3 pu">
                                              
                                            </div>--%>
                                        <%--OnClick="btnsubmit_Click"--%>
                                        <%-- OnClick="btnclear_Click"--%>
                                    </div>
                                    <%--                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                AssetCode</label>
                                            <div class="col-sm-3">                                                
                                                <asp:TextBox ID="txtAssetCode" runat="server" Style="width: 200px" class="form-control"
                                                    placeholder="AssetCode"></asp:TextBox>
                                            </div>
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                Category</label>
                                            <div class="col-sm-4">                                                
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlproCategory" OnSelectedIndexChanged="OnSelectedIndexChangedCategory"
                                                            AutoPostBack="true" class="form-control drpwidth">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                SubCategory</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlsubcat" AutoPostBack="true" class="form-control drpwidth">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                Location</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLcocation"
                                                            AutoPostBack="true" class="form-control drpwidth">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                Building</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlbuild" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding"
                                                            AutoPostBack="true" class="form-control drpwidth">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                Floor</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlfloor" AutoPostBack="true" class="form-control drpwidth">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                                Department</label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddldept" class="form-control drpwidth">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-2">
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Button Text="Clear" runat="server" Style="height: 35px" ID="btnRefresh" class="'btn btn-primary"
                                                    OnClick="btnRefresh_Click" />
                                                <asp:Button Text="Search" runat="server" Style="height: 35px" ID="btnsearchsubmit"
                                                    OnClick="btnsearchsubmit_Click" class="'btn btn-primary" />
                                            </div>
                                        </div>
                                    </div>--%>
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
                        <h1><span style="font-family: 'Calibri'; font-size: x-large; color: black;">Print Label</span></h1>
                    </div>
                    <div class="row">
                        <div class="Col-sm-1">
                            <%--                            <label runat="server" id="lblsubcattype" class="col-sm-1 control-label no-padding-right"
                                for="form-field-1" style="padding-top: 7px;margin-left:10px" Font-Names="Calibri" Font-Size="Large">
                                Tag Type  :</label>--%>
                            <%--  <asp:Label ID="Label2" runat="server" class="col-sm-1 control-label no-padding-right"
                                for="form-field-1"
                                Text="Tag Type  :" Style="padding-top: 3px; margin-left: 10px" Font-Names="Calibri" Font-Size="Large" />--%>
                        </div>
                        <div class="col-sm-3" style="">
                            <h7>Tag Type  :</h7>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList runat="server" ID="ddlTagType" AutoPostBack="true" class="form-control"
                                        OnSelectedIndexChanged="ddlTagType_SelectedIndexChanged">
                                    </asp:DropDownList><%----%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div style="margin-left: 6px;">
                        <h7>Encoding:</h7>
                        <%--<asp:Label ID="Label3" runat="server" Text="Encoding:" Font-Names="Calibri" Font-Size="Large" />--%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton RenderMode="Lightweight" ID="RadYes" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            AutoPostBack="false" BorderWidth="0" BackColor="transparent" GroupName="Radio"
                            Text="Yes" Skin="Metro" />
                        <telerik:RadButton RenderMode="Lightweight" ID="RadNo" runat="server" ButtonType="ToggleButton" ToggleType="Radio"
                            AutoPostBack="false" BorderWidth="0" BackColor="transparent" GroupName="Radio"
                            Text="No" Skin="Metro" />

                    </div>

                    <div>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                    OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init" OnDataBinding="gvData_DataBinding"
                                    OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">
                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>

                                    <%--<telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" GridLines="None" CssClass="gvData" OnPageIndexChanged="gvData_PageIndexChanged"
                                    OnDataBinding="gvData_DataBinding" OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">--%>
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
                                                HeaderText="SERIALNO" SortExpression="SerialNo" UniqueName="SerialNo" Visible="false" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                                HeaderText="DESCRIPTION" SortExpression="Description" UniqueName="Description" Visible="false"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                                                HeaderText="PRICE" SortExpression="Price" UniqueName="Price" ReadOnly="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                                                HeaderText="DOCUMENT CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubCategory" FilterControlAltText="Filter SubCategory column"
                                                HeaderText="SUBCATEGORY" SortExpression="SubCategory" UniqueName="SubCategory" Visible="false"
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
                                            <telerik:GridBoundColumn DataField="Department" Visible="false" FilterControlAltText="Filter Department column"
                                                HeaderText="DEPARTMENT" SortExpression="Department" UniqueName="Department" ReadOnly="true">
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
                                                ReadOnly="true" Visible="false">
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
                <%--end main page--%>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenField3" runat="server" />

    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel22"
        TargetControlID="btnShow" CancelControlID="btnClose">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel22" runat="server" align="center" Style="display: none" CssClass="modalPopup"
        Height="132px" Width="250px">
        <table style="width: 100%">
            <tr style="height: 25px;" id="trheader" runat="server">
                <td colspan="1">
                    <label id="Label1" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label>
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

    <asp:Button ID="btnShowPreview" runat="server" Text="Show Modal Popup" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender22" runat="server" PopupControlID="Panel222"
        TargetControlID="btnShowPreview" CancelControlID="btnClose" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel222" runat="server" align="center" Style="display: none" CssClass="modalPopup"
        Height="420px" Width="700px" BackColor="LightGray">
        <table style="width: 100%;">
            <tr style="height: 25px;" id="trheaderPrview" runat="server">
                <td colspan="1">
                    <asp:Label Style="font-size: large" Text="&nbsp;Label&nbsp;Preview" runat="server"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btnClosePreview" runat="server" Text="X" CssClass="bgimage" />
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="border: none; width: 20%;">
                    <asp:Image ID="Image1" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" style="margin-right: 10px; border: 0px; padding-top: 65px;">
                    <asp:Button ID="btnCloseinnerPreview" runat="server" Text="OK" Width="50px" BackColor="152, 192, 218"
                        CssClass="changePosition" Visible="false" />
                </td>
            </tr>
            <%--<tr style="height: 0px;" id="trfooterPreview" runat="server">
                <td colspan="2" style="margin-right: 10px;"></td>
            </tr>--%>
        </table>
    </asp:Panel>

</asp:Content>
