<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="SLA.aspx.cs" Inherits="_Default" %>

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

        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }

        .gvData {
            margin-left: auto !important;
            margin-right: auto !important;
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
    </style>
    <script type="text/javascript">
        function openModal() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal').modal('show');
        }
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


    <script type="text/javascript">



        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }

        function HideMapModalPopup() {
            $find("mpeNew").hide();
            return false;
        }

    </script>
    <style type="text/css">
    </style>

    <style type="text/css">
        @media only screen and (min-width: 480px) and (max-width: 767px) {
            .additionalColumn {
                display: none !important;
            }
        }
    </style>
    <style type="text/css">
      
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
    <script type="text/javascript">
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
    <asp:HiddenField runat="server" ID="HdnLocation" />
    <div id="div1" class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-5">
                                <span style="font-family: 'Calibri'; font-size: x-large">Location & Category SLA Update</span>&nbsp;
                                
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
                <div class="panel-body">
                    <div class="container" id="container1" runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <p class="pull-right" style="color: red; font-size: 12px">
                                    * multiple email id's can be added with comma(,)
                                </p>
                            </div>
                        </div>
                        <div class="row" style="align-content: center">
                            <div class="col-md-3"></div>
                            <div class="col-md-3">
                                <h7>Select Location:</h7>
                                <br />
                                <asp:DropDownList Width="100%" ID="ddlLocation" runat="server">
                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <h7>Enter No. of Days:</h7>
                                <br />
                                <asp:TextBox runat="server" Width="100%" ID="txtnoofdays" type="number" />
                                <asp:RangeValidator runat="server" ControlToValidate="txtnoofdays" ErrorMessage="Invalid No."
                                    Type="Integer" MinimumValue="1" MaximumValue="1000" ForeColor="Red"></asp:RangeValidator>
                            </div>
                        </div>
                        <div class="row">
                             <div class="col-md-3"></div>
                           <div class="col-md-3">
                               <br />
                                <h7> <asp:Label runat="server" ID="Label3">Select Document Category List
                                     </asp:Label></h7>
                                <button type="button" style="width: 25%" class="btn btn-primary form-control text-dark pull-right" data-toggle="modal" data-target="#documentCategoryModal">
                                    <h7><asp:Label runat="server" ForeColor="white"><%--Select Document Category--%><i class="fa fa-list-ul" aria-hidden="true"></i> </asp:Label>                                        
                                    </h7>
                                </button>
                                
                                
                            </div>
                            <div class="col-md-3">
                                <h7>&nbsp;</h7>
                                <br />
                                <asp:TextBox ID="txtcategoryitems" Width="100%" ReadOnly="true" TextMode="MultiLine" runat="server"></asp:TextBox>
                                <br />
                                <div class="modal" id="documentCategoryModal" tabindex="-1" role="dialog">
                                    <div class="modal-dialog modal-sm" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title">Select Document Category
                                                      

                                                    <p class="pull-right"><a href="#" style="font-weight: bold; color: black" data-dismiss="modal">X</a></p>
                                                </h5>
                                            </div>
                                            <div class="modal-body" style="height: 350px; overflow-y: auto;">
                                                <asp:TextBox runat="server" AutoPostBack="true" ID="txtcategoryname" OnTextChanged="txtcategoryname_TextChanged" placeholder="Enter Category Name" Width="100%"></asp:TextBox>
                                                <asp:Repeater runat="server" OnItemDataBound="rptdocumentCategory_ItemDataBound" ID="rptdocumentCategory">
                                                    <ItemTemplate>
                                                        <li style="width: 100%;">
                                                            <input type="checkbox" runat="server" value='<%#Eval("CategoryId")%>' id="chkDisplayTitle" />
                                                            <%#Eval("CategoryName")%></li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="col-md-12">
                                                    <asp:Button ID="btnmark" runat="server" Text="MARK ALL" CssClass="btn btn-success" OnClick="btnmark_Click" />
                                                    <%-- <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>--%>
                                                    <asp:Button ID="btnsavecategoryitems" runat="server" CssClass="btn btn-primary" Text="DONE" OnClick="btnsavecategoryitems_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            
                        </div>
                        <div class="row">
                            <div class="col-md-3"></div>
                            <div class="col-md-3">
                                <h7>Enter TO Email ID/ID's:</h7>
                                <br />
                                <asp:TextBox ID="txttoemailid" Width="100%" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-2" style="text-align: center">
                                <h7> <asp:Label runat="server" ID="Label2">Case Manager</asp:Label></h7>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkcaseManager" OnCheckedChanged="chkcaseManager_CheckedChanged" AutoPostBack="true" CssClass="form-control" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-sm-2" style="text-align: center">
                                <h7> <asp:Label runat="server" ID="Label1">Case Worker</asp:Label></h7>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkcaseWorker" AutoPostBack="true" OnCheckedChanged="chkcaseWorker_CheckedChanged" CssClass="form-control" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-3"></div>
                            <div class="col-md-3">
                                <h7>Enter CC Email ID/ID's:</h7>
                                <br />
                                <asp:TextBox ID="txtccemailid" Width="100%" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-2" style="text-align: center">
                                <h7> <asp:Label runat="server" ID="Label4">Case Manager</asp:Label></h7>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkcaseManager2" AutoPostBack="true" OnCheckedChanged="chkcaseManager2_CheckedChanged" CssClass="form-control" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-sm-2" style="text-align: center">
                                <h7> <asp:Label runat="server" ID="Label5">Case Worker</asp:Label></h7>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkcaseWorker2" AutoPostBack="true" OnCheckedChanged="chkcaseWorker2_CheckedChanged" CssClass="form-control" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-5"></div>
                            <div class="col-md-2">
                                <h7>&nbsp;</h7>
                                <br />
                                <asp:Button Text="SUBMIT" runat="server" ID="btnadd" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-primary pull-left" OnClick="btnadd_Click" />
                            </div>
                        </div>

                    </div>
                    <div class="container" id="container2" runat="server" style="width: auto">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" style="width: auto">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" FilterMenu-EnableAutoScroll="true" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                    OnPageIndexChanged="gvData_PageIndexChanged" ClientSettings-Scrolling-AllowScroll="true" BorderWidth="1" OnInit="gvData_Init"
                                    OnItemCommand="gv_data_ItemCommand">
                                     <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                    <ClientSettings>
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                    </ClientSettings>
                                    <SortingSettings EnableSkinSortStyles="false" />
                                    <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                        DataKeyNames="id">
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
                                            <telerik:GridBoundColumn DataField="id" FilterControlAltText="Filter id column"
                                                HeaderText="id" SortExpression="id" UniqueName="id"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="LocationId" FilterControlAltText="Filter LocationId column"
                                                HeaderText="LocationId" SortExpression="LocationId" UniqueName="LocationId" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column" HeaderText="LOCATION NAME" SortExpression="LocationName" UniqueName="LocationName" ReadOnly="true"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="noOfDays" FilterControlAltText="Filter noOfDays column"
                                                HeaderText="SLA DAYS" SortExpression="noOfDays" UniqueName="noOfDays" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CategoryId" FilterControlAltText="Filter CategoryId column"
                                                HeaderText="CategoryId" SortExpression="CategoryId" UniqueName="CategoryId" ReadOnly="true" Visible="false"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CategoryName" FilterControlAltText="Filter CategoryName column"
                                                HeaderText="CATEGORY NAME" SortExpression="CategoryName" UniqueName="CategoryName" ReadOnly="true"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="toEmailId" FilterControlAltText="Filter toEmailId column"
                                                HeaderText="TO EMAILID" SortExpression="toEmailId" UniqueName="toEmailId" ReadOnly="true"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="casemanageremailstat" FilterControlAltText="Filter casemanageremailstat column"
                                                HeaderText="CASE MANAGER" SortExpression="casemanageremailstat" UniqueName="casemanageremailstat" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="caseworkeremailstat" FilterControlAltText="Filter caseworkeremailstat column"
                                                HeaderText="CASE WORKER" SortExpression="caseworkeremailstat" UniqueName="caseworkeremailstat" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ccEmailId" FilterControlAltText="Filter ccEmailId column"
                                                HeaderText="CC EMAILID" SortExpression="ccEmailId" UniqueName="ccEmailId" ReadOnly="true"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="casemanageremailstat2" FilterControlAltText="Filter casemanageremailstat2 column"
                                                HeaderText="CASE MANAGER" SortExpression="casemanageremailstat2" UniqueName="casemanageremailstat2" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="caseworkeremailstat2" FilterControlAltText="Filter caseworkeremailstat2 column"
                                                HeaderText="CASE WORKER" SortExpression="caseworkeremailstat2" UniqueName="caseworkeremailstat2" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="createdDate" FilterControlAltText="Filter CreatedDate column"
                                                HeaderText="DATE" SortExpression="createdDate" UniqueName="createdDate" ReadOnly="true"
                                                AllowFiltering="false" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn CommandName="edview" HeaderText="EDIT" ButtonType="ImageButton" UniqueName="view"
                                                ImageUrl="~/images/pencil.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn CommandName="dit" HeaderText="Delete" ButtonType="ImageButton" UniqueName="Edit"
                                                ImageUrl="~/images/Delete.png">
                                            </telerik:GridButtonColumn>
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
    <div class="modal fade" id="alertmodal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Location & Category SLA Update</h5>

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
    <div class="modal fade" id="deleteconfirmModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Location & Category SLA Update</h5>

                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <h5>Are you sure you want to delete the data??
                                    <asp:Label ID="Label6" runat="server" Text=""></asp:Label></h5>
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
                                <asp:Button Text="Yes" runat="server" ID="Button2" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-info" OnClick="Button2_Click" />
                            </div>
                            <div class="col-md-6">
                                <asp:Button Text="No" runat="server" ID="Button3" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-warning" />
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
            $('#documentCategoryModal').modal('show');
        }
        function openModal1() {
            $('#deleteconfirmModal').modal('show');
        }
        function openalertModal() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#alertmodal').modal('show');
        }
        function setvalueforlocation(lbllocname) {
            document.getElementById('lbllocname').innerHTML = lbllocname;
            //document.getElementById('lbllocname').innerHTML = lbllocname;
        }
    </script>
</asp:Content>
