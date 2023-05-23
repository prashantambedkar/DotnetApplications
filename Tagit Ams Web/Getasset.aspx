<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="Getasset.aspx.cs" Inherits="Getasset" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Operation</a> </li>
                    <li><a href="#">Asset Verification - Get Asset</a> </li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Asset Verification - Get Asset</h1>
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
                            </div>
                            <div class="col-xs-12">
                                <label runat="server" id="lblcattype" class="col-sm-1 control-label no-padding-right"
                                    for="form-field-1">
                                    Status
                                </label>
                                <div class="col-sm-2">
                                    <asp:DropDownList runat="server" ID="ddlStatus" class="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-6">
                                    <asp:Button CssClass="btn" ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" />
                                    <asp:Button CssClass="btn" ID="btnSaveTHR" Text="SaveTHR" runat="server" OnClick="btnSaveTHR_Click" />
                                    <asp:Button CssClass="btn" ID="BtnSaveTHS" Text="SaveTHS" runat="server" OnClick="BtnSaveTHS_Click" />
                                    <asp:Button CssClass="btn" ID="btmRefresh" Text="Refresh" runat="server" OnClick="btmRefresh_Click" />
                                    <asp:Button CssClass="btn" ID="BtnAccept" Text="Accept" runat="server" OnClick="BtnAccept_Click" Visible="false" />
                                    <asp:Button CssClass="btn" ID="BtnReject" Text="Reject" runat="server" OnClick="BtnReject_Click" Visible="false"/>
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
                         <div id="divGrid" style="overflow: auto; height: 250px">
                            <asp:DataGrid ID="gridlist" runat="server" AllowSorting="True" CssClass="table table-striped table-bordered table-hover"
                                AutoGenerateColumns="False" BorderStyle="None" AllowPaging="true" PageSize="250" OnPageIndexChanged="myDataGrid_PageChanger">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Asset Id">
                                        <ItemTemplate>
                                            <asp:Label ID="AssetCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssetCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Serial No">
                                        <ItemTemplate>
                                            <asp:Label ID="SerialNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Category">
                                        <ItemTemplate>
                                            <asp:Label ID="CategoryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Category") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="AssetId" Enabled="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AssetId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Sub Category">
                                        <ItemTemplate>
                                            <asp:Label ID="subCategoryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SubCategory") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="Location" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Building">
                                        <ItemTemplate>
                                            <asp:Label ID="Building" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Building") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Floor">
                                        <ItemTemplate>
                                            <asp:Label ID="Floor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Floor") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Asset Desc">
                                        <ItemTemplate>
                                            <asp:Label ID="Department" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Price">
                                        <ItemTemplate>
                                            <asp:Label ID="Price" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Price") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="PeviousLocation">
                                        <ItemTemplate>
                                            <asp:Label ID="PreLoc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FromLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="NewLocation">
                                        <ItemTemplate>
                                            <asp:Label ID="NewLoc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ToLocation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Status" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'></asp:Label>
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
                </div>
                <%--end main page--%>
            </div>
        </div>
    </div>
</asp:Content>
