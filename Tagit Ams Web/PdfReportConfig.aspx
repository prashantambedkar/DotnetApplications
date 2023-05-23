<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="PdfReportConfig.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
    <script src="js/jquery.min.js" type="text/jalbllocnamevascript"></script>
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
        function GridCreated(sender, args) {
            var scrollArea = sender.GridDataDiv;
            var dataHeight = sender.get_masterTableView().get_element().clientHeight; if (dataHeight < 350) {
                scrollArea.style.height = dataHeight + 17 + "px";
            }
        }

        $(document).ready(function () {
            $('#ContentPlaceHolder1_chkstatus').prop("checked", true);
            //            $('#ContentPlaceHolder1_chkstatus').click(function () {

            //                
            //            });
        });
    </script>
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
    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () {
                var body = $('[Id*=txtName]').val();
                //$("#MyPopup .modal-body").html("Welcome to " + body);
                $.ajax({
                    type: "POST",
                    url: "Home.aspx/SendParameters",
                    data: "{name: '" + $("#txtName").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        $("#MyPopup").modal("show");
                    }
                });
            });
            $("#btnClosePopup").click(function () {
                $("#MyPopup").modal("hide");
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField runat="server" ID="HdnLocation" />
    <div id="div1" class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-5">
                                <span style="font-family: 'Calibri'; font-size: x-large; color: black">Pdf Report Configuration</span>&nbsp;                                
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
                        Select Location :
                    </label>
                </div>

                <div class="col-md-4 align-left">
                    <asp:DropDownList CssClass="text-control" Width="190px" ID="ddlLocation" runat="server"></asp:DropDownList>
                </div>

            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2 align-right">
                    <label style="padding-top: 7px; text-align: right" for="name">
                        Address :
                    </label>
                </div>

                <div class="col-md-4 align-left">
                    <asp:TextBox ID="txtAddress" Width="190px" MaxLength="500" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>

            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2 align-right">
                    Select Company Logo :
                </div>

                <div class="col-md-2 align-left">
                    <asp:FileUpload ID="fileuploadCompanyLogo" runat="server" /><br />
                    <asp:Button ID="btnUpload" Width="190px" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                    <%--<asp:RegularExpressionValidator ID="revbtnUpload" ControlToValidate="btnUpload" ValidationExpression="(.*\.([Gg][Ii][Ff])|.*\.([Jj][Pp][Gg])|.*\.([Bb][Mm][Pp])|.*\.([pP][nN][gG])|.*\.([tT][iI][iI][fF])$)" runat="server" ErrorMessage="Error Uploading File!"></asp:RegularExpressionValidator>--%>
                    <asp:Label ID="lblimagepath" Visible="false" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-4 align-left">
                    <asp:Image ID="Imgcompanylogo" ImageUrl="~/images/noimage.jpg" Width="190px" Height="130px" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2"></div>
                <div class="col-md-1">
                    <asp:Button ID="btnSubmit" CssClass="btn btn-primary form-control" runat="server" Text="" OnClick="btnSubmit_Click" />
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnReset" CssClass="btn btn-danger form-control" runat="server" Text="RESET" OnClick="btnReset_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2"></div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-2"></div>
                <div class="col-md-4">
                    <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                </div>
                <div class="col-sm-7" style="margin-left: 10px">
                    <input id="inpHide" type="hidden" runat="server" />
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <%--<telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" GridLines="None" CssClass="align-center" OnItemCommand="gv_data_ItemCommand">--%>



                            <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik"
                                GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                OnItemCommand="gv_data_ItemCommand" OnItemDataBound="gvData_ItemDataBound">
                                 <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                <ItemStyle HorizontalAlign="Center" Wrap="false" Width="100%"></ItemStyle>
                                <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Width="100%" Wrap="false" Height="22px"></HeaderStyle>
                                <ClientSettings EnablePostBackOnRowClick="false">
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                    <ClientEvents OnGridCreated="GridCreated" />
                                </ClientSettings>
                                <SortingSettings EnableSkinSortStyles="false" />
                                <MasterTableView AllowPaging="True" PageSize="100" AutoGenerateColumns="false" AllowSorting="true"
                                    DataKeyNames="pdfconfigid">
                                    <PagerStyle AlwaysVisible="true" Position="Top" />
                                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridButtonColumn CommandName="dels" HeaderText="ACTION" ButtonType="ImageButton" UniqueName="dels" ImageUrl="~/images/Delete.png">
                                        </telerik:GridButtonColumn>
                                        <telerik:GridTemplateColumn HeaderText="SR NO" AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserial" Width="100%" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="pdfconfigid" FilterControlAltText="Filter pdfconfigid column"
                                            HeaderText=" CONFIG ID" SortExpression="pdfconfigid" UniqueName="pdfconfigid" ReadOnly="true"
                                            AllowSorting="false" AllowFiltering="false" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column"
                                            HeaderText=" LOCATION NAME" SortExpression="LocationName" UniqueName="LocationName"
                                            AllowSorting="false" AllowFiltering="true" Visible="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="LocationCode" FilterControlAltText="Filter LocationCode column"
                                            HeaderText="  LOCATION CODE" SortExpression="LocationCode" UniqueName="LocationCode" ReadOnly="true"
                                            AllowSorting="false" AllowFiltering="false" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="address" FilterControlAltText="Filter address column"
                                            HeaderText="  ADDRESS" SortExpression="address" UniqueName="address" ReadOnly="true"
                                            AllowSorting="false" AllowFiltering="true" Visible="true">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridImageColumn HeaderText="  IMAGE" AllowFiltering="false" DataImageUrlFields="imgPath" ImageWidth="50px" ImageHeight="50px"></telerik:GridImageColumn>
                                        <telerik:GridBoundColumn DataField="ActiveStatus" FilterControlAltText="Filter ActiveStatus column"
                                            HeaderText="  STATUS" SortExpression="ActiveStatus" UniqueName="ActiveStatus" ReadOnly="true"
                                            AllowSorting="false" AllowFiltering="false" Visible="false">
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

    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">PDF Config Details</h5>

                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <h5>Are you sure you want to delete the data for
                                    <label id="lbllocname"></label>
                                    ??</h5>
                                <%--<asp:TextBox ID="txtid" Visible="false" Width="100%" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtCategoryName" ReadOnly="true" Width="100%" runat="server"></asp:TextBox>--%>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button Text="Yes" runat="server" ID="btnupdates" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-info" OnClick="btnupdates_Click" />
                            </div>
                            <div class="col-md-6">
                                <asp:Button Text="No" runat="server" ID="btnclose" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-warning" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- /.page-header -->
    <div class="row">
         <div class="col-xs-12" style="background-color: white">
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
      <div class="modal fade" id="myModalz1" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">PDF Report Config</h5>

                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <h5>
                                    <label id="lbllocnamez1"></label>
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
        function openModalz1() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModalz1').modal('show');
        }
        function setvalueforlocationz1(lbllocname) {
            document.getElementById('lbllocnamez1').innerHTML = lbllocname;
            //document.getElementById('lbllocname').innerHTML = lbllocname;
        }
    </script>
    <!-- /.page-content -->
</asp:Content>

