<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="DepartmentMaster.aspx.cs" Inherits="DepartmentMaster" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var mouseOverActiveElement = false;

        $(document).ready(function () {


            $('#txtserchbox').click(function () {
                //  $('#ContentPlaceHolder1_divSearch').show(500);
            })

            $('#divSearchClose').click(function () {
                // $('#ContentPlaceHolder1_divSearch').hide(500);
            });

            $(':file').on('change', ':file', function () {
                var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });
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
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnErrorPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackground" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopup" Height="140px" Width="400px"
        Style="display: none">
        <div class="headerModal">
            Confirmation
        </div>
        <div class="body">
            <asp:Label ID="lblmodmsg" runat="server" Text="You are not authorized to view this page." />
        </div>
        <div align="center">
            <asp:Button ID="btnYes" runat="server" Text="Ok" OnClick="btnYes_Click" CssClass="yes" />
        </div>
    </asp:Panel>
    <script type="text/javascript">
        try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
    </script>
    <script language="javascript" type="text/javascript">
        function Validate() {

            var txtcat = '<%=txtFullName.ClientID %>';

            if (document.getElementById(txtcat).value == '') {
                //alert("Please Enter The Department");

                document.getElementById(txtcat).focus();

                return false;

            }
            //                        else {
            //                            var isValid = false;
            //                           var regex = /^[a-zA-Z0-9 ]*$/;
            //                            isValid = regex.test(document.getElementById(txtcat).value);
            //                            if (isValid == false) {
            //                                alert('Special character not allowed');
            //                                document.getElementById(txtcat).focus();
            //                                return false;
            //                            }
            //                        }

            return true;
        }

    </script>
    <div class="main-content-inner" style="font-family: Calibri;font-size: 10pt;" class="main-content-inner">
        <div class="">
            <uc1:topmenu runat="server" ID="topmenu" />
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="form-inline">
                        <div style="padding-left: 10px" class="input-group">
                            <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnSearchInfo" Style="height: 34px" runat="server" OnClick="btnSearchInfo_Click"
                                    class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div id="divSearch" runat="server" class="panel-group col-md-7 form_wrapper">
                        <div class="panel panel-default">
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
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label style="padding-top: 7px; text-align: right" class="col-sm-3" for="name">
                                            Department Name:</label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtSearchbox" CssClass="text-control" runat="server" onkeydown="return (event.keyCode!=13);" />&nbsp;&nbsp;
                                        </div>
                                        <div class="col-sm-5 pull-right">
                                            <asp:Button ID="btnSearch" CssClass="btn btn-primary" Text="search" runat="server"
                                                OnClick="btnSearch_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <label style="font-size: x-large">Department Master</label>
                    </div>
                    <div class="panel-body">
                        <div class="col-xs-12">
                            <!-- start top menu -->

                            <%--end top menu--%>
                            <%--start main page--%>
                            <div class="form-horizontal">
                                <div class="col-sm-12" style="text-align: center">
                                    <asp:Label runat="server" ID="lblSuccessMessage" Style="color: green" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                        ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lblImgUp" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    <asp:Label runat="server" ID="showerror" Style="color: Red"></asp:Label>
                                    <label runat="server" id="lblcattype" class="col-sm-2 control-label no-padding-right"
                                        for="form-field-1">
                                        Department Name <span style="color: Red">*</span> :
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtFullName" MaxLength="100" class="form-control" runat="server"
                                            placeholder="Enter the value" AutoPostBack="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    </div>
                                    <label style="padding-top: 7px; text-align: right" class="col-sm-1" for="name">
                                        Active :</label>
                                    <div class="col-sm-1" style="padding-top: 7px;">
                                        <asp:CheckBox ID="chkActive" runat="server" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button Text="Submit" Style="border-radius: 5px; padding: 0" runat="server" ID="btnsubmit"
                                            OnClick="btnsubmit_Click" OnClientClick="javascript:return Validate();" class="btn btn-primary" />
                                        <asp:HiddenField ID="hdncatidId" runat="server" />
                                        <asp:HiddenField ID="hidcatcode" runat="server" />

                                        <asp:Button ID="btnreset" Style="border-radius: 5px; padding: 0" class="btn btn-primary"
                                            runat="server" Text="Reset" OnClick="btnreset_Click" />
                                    </div>
                                </div>
                            </div>
                            <%--end main page--%>
                            <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                        </div>
                        <!-- /.col -->
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"
                                    Width="100%"></asp:Label>
                                <asp:TextBox ID="txtPageCount" runat="server" Visible="False"></asp:TextBox>
                                <asp:Label ID="lblSort" runat="server" Visible="False"></asp:Label>
                                <asp:HiddenField ID="hdnOldDept" runat="server" />
                            </div>
                            <!-- /.span -->
                        </div>
                        <hr />
                        <div>
                           <%--  <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                            CellSpacing="0" GridLines="None" AllowPaging="True" PageSize="50" BorderStyle="Solid"
                            OnItemCommand="GvData_ItemCommand">--%>
                            <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                OnPageIndexChanged="gvData_PageIndexChanged" OnItemCommand="GvData_ItemCommand" BorderWidth="1" OnInit="gvData_Init">
                                <SortingSettings EnableSkinSortStyles="false"  />
                                <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                                <AlternatingItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" />
                                <MasterTableView AllowPaging="True" AutoGenerateColumns="false" DataKeyNames="DepartmentId"
                                    AllowSorting="true" CellSpacing="2">
                                    <PagerStyle AlwaysVisible="true" Position="Top" />
                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridButtonColumn CommandName="dit" ButtonType="ImageButton" UniqueName="column1"
                                            ImageUrl="~/images/pencil.png" HeaderStyle-Width="40px" HeaderText="EDIT">
                                            <ItemStyle Width="60px" />
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridTemplateColumn HeaderText="SR.NO." AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" />
                                            <HeaderStyle Width="60px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="DepartmentCode" FilterControlAltText="Filter DepartmentCode column"
                                            HeaderText="DEPARTMENT ID" SortExpression="DepartmentCode" UniqueName="DepartmentCode"
                                            ReadOnly="true" AllowSorting="true" AllowFiltering="false">
                                            <ItemStyle Width="100px" />
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DepartmentName" FilterControlAltText="Filter DepartmentName column"
                                            HeaderText="DEPARTMENT NAME" SortExpression="DepartmentName" UniqueName="DepartmentName"
                                            ReadOnly="true">
                                            <ItemStyle Width="100px" />
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DepartmentId" FilterControlAltText="Filter DepartmentId column"
                                            HeaderText="DepartmentId" SortExpression="DepartmentId" UniqueName="DepartmentId"
                                            ReadOnly="true" Visible="false">
                                            <ItemStyle Width="120px" />
                                            <HeaderStyle Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter CreatedBy column"
                                            HeaderText="CREATED BY" SortExpression="CreatedBy" UniqueName="CreatedBy" ReadOnly="true">
                                            <ItemStyle Width="100px" />
                                            <HeaderStyle Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                            HeaderText="CREATED DATE" SortExpression="CreatedDate" UniqueName="CreatedDate"
                                            ReadOnly="true">
                                            <ItemStyle Width="130px" />
                                            <HeaderStyle Width="130px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                            HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true">
                                            <ItemStyle Width="90px" />
                                            <HeaderStyle Width="90px" />
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

                <!-- /.page-header -->


            </div>
            <!-- /.row -->
        </div>

        <!-- /.page-content -->
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
</asp:Content>
