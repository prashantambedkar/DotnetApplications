<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="AddClients.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js?key=AIzaSyD3kNBwtWnxNzyy-JkRepAHIUJaUUNCuUE"></script>
    <link href="css/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .loader {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url('loading.gif') 50% 50% no-repeat rgb(249,249,249);
            background-color: transparent;
        }
    </style>

    <script type="text/javascript">
        function contentHide() {
            $('#ContentPlaceHolder1_divSearch').hide(500);
        };
        var mouseOverActiveElement = false;

        $(document).ready(function () {

            //            $('#hdfImport').val();
            //            alert($('#hdfImport').val());
            ////            divAssetMaster
            $('#ContentPlaceHolder1_txtSearch').click(function () {
                if ($('#ContentPlaceHolder1_txtSearch').val().length > 1) {
                    $('#ContentPlaceHolder1_divSearch').hide(500);
                } else {
                    $('#ContentPlaceHolder1_divSearch').show(500);
                    $('#divUpdateWaranty').hide(500);

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

            $('#ContentPlaceHolder1_GetGrid').click(function () {
                if ($('#ContentPlaceHolder1_txtSearch').val().length == 0) {
                    alert('Enter some text to search');
                    return false;
                }
            });
            $('#ContentPlaceHolder1_btnclear').click(function () {
                $('#ContentPlaceHolder1_ddlCategorySearch').val(0);
                $('#ContentPlaceHolder1_txtAssetCode').val('');
                $('#ContentPlaceHolder1_ddlsubcatSearch').val('0');

                $('#ContentPlaceHolder1_ddllocSearch').val('0');
                $('#ContentPlaceHolder1_ddlbuildSearch').val('0');
                $('#ContentPlaceHolder1_ddlfloorSearch').val('0');
                $('#ContentPlaceHolder1_ddldeptSearch').val('0');
                $('#ContentPlaceHolder1_divSearch').show();
            });
            $('#ContentPlaceHolder1_btnsubmit').click(function () {
                $('#ContentPlaceHolder1_divUpdateFields').hide();
                $('#ContentPlaceHolder1_divWarantyClose').hide();


            });
            $('#ContentPlaceHolder1_txtwarr').keydown(function (e) {

                if (e.keyCode == 32) {
                    if ($('#ContentPlaceHolder1_txtwarr').val().trim() == "") {
                        $(this).val($(this).val() + '');
                        return false;
                    }

                }
            });
            $('#divSearchClose').click(function () {
                $('#ContentPlaceHolder1_divSearch').hide(500);
                $('#divUpdateWaranty').show(500);
            });

            $('#idClose').click(function () {
                $('#ContentPlaceHolder1_divWarantyClose').hide(500);
            });
            $('#IdAssetclose').click(function () {
                $('#ContentPlaceHolder1_divUpdateFields').hide(500);
            });

            $('#btnWarnaty').click(function () {
                //                $('#divWarantyClose').show(500);
                $("#ContentPlaceHolder1_divWarantyClose").attr("style", "");
            });
            //            $('#ContentPlaceHolder1_divBrowse').change(function () {
            //                if ($('#ContentPlaceHolder1_divSelectFile').text() != "") {
            //                    alert('Hi')
            //                }
            //            });

            $(':file').on('change', ':file', function () {
                var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
                input.trigger('fileselect', [numFiles, label]);
            });
        })

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField runat="server" ID="HdnLocation" />
    <div id="div1" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-5">
                                <span style="font-family: Tahoma; font-style: oblique; font-size: x-large">Top 10 Clients Config</span>&nbsp;
                                
                            </div>

                            <div class="col-sm-7">

                                <i style="float: inline-end; font-size: 24px" class="fa">

                                    <%-- <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                                </i>
                                &nbsp;&nbsp;&nbsp;&nbsp;<img src="" alt="" style="float: right;" runat="server" id="CompanyImg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                    <%-- collapse demo--%>
                </div>
                
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2 align-right">
                    <label style="padding-top: 7px; text-align: right" for="name">
                        Select Clients :
                    </label>
                </div>
                <div class="col-md-4 align-left">
                    <asp:DropDownList CssClass="text-control" Width="190px" ID="ddlClients" runat="server"></asp:DropDownList>
                </div>
            </div>
                   <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2"></div>
                <div class="col-md-4">
                    <asp:Button ID="btnSubmit" CssClass="btn btn-info" runat="server" Text="SUBMIT" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnReset" CssClass="btn btn-danger" runat="server" Text="RESET" OnClick="btnReset_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2"></div>
                <div class="col-md-4">
                    <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4">
                </div>
                <div class="col-sm-4">
                     <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                          <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" GridLines="None" CssClass="align-center" OnItemCommand="gv_data_ItemCommand">
                                <ItemStyle HorizontalAlign="Center" Width="100%"></ItemStyle>
                                <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Width="100%" Height="22px"></HeaderStyle>
                                <ClientSettings EnablePostBackOnRowClick="false">
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                    
                                </ClientSettings>
                                <SortingSettings EnableSkinSortStyles="false" />
                                <MasterTableView AllowPaging="True" PageSize="100" AutoGenerateColumns="false" AllowSorting="true"
                                    DataKeyNames="clientListID">
                                    <PagerStyle AlwaysVisible="true" Position="Top" />
                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="SR NO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="clientListID" FilterControlAltText="Filter clientListID column"
                                            HeaderText="List ID" SortExpression="clientListID" UniqueName="clientListID" ReadOnly="true"
                                            AllowSorting="false" AllowFiltering="false" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="clientName" FilterControlAltText="Filter clientName column"
                                            HeaderText="CLIENT NAME" ItemStyle-HorizontalAlign="Center" SortExpression="clientName" UniqueName="clientName" ReadOnly="true"
                                            AllowSorting="false" AllowFiltering="false" Visible="true">
                                        </telerik:GridBoundColumn>
                                       
                                        <telerik:GridButtonColumn CommandName="dels" HeaderText="" ButtonType="ImageButton" UniqueName="dels" ImageUrl="~/images/Delete.png">
                                        </telerik:GridButtonColumn>
<%--                                        <telerik:GridButtonColumn CommandName="dit" HeaderText="" ButtonType="ImageButton" UniqueName="dit" ImageUrl="~/images/pencil.png">
                                        </telerik:GridButtonColumn>--%>
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
    </div>
    s
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

    <!-- /.page-content -->
</asp:Content>
