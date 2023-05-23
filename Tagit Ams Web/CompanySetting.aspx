<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/adminMasterPage.master"
    CodeFile="CompanySetting.aspx.cs" Inherits="CompanySetting" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Setings</a> </li>
                    <li><a href="#">Data Settings</a> </li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Data Settings</h1>
            </div>
            <!-- /.page-header -->
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>
                    <div class="col-xs-12">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="col-xs-12">
                                    <label runat="server" id="lblsubcattype" class="col-sm-2 control-label no-padding-right"
                                        for="form-field-1-1">
                                        Asset Type:
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:RadioButton ID="RdoImport" Text="Import" TextAlign="Left" GroupName="Type" runat="server" Checked="true" />

                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="RdoManual" Text="Manual" TextAlign="Left" GroupName="Type" runat="server"/>

                                    </div>
                                </div>
                                <div class="col-xs-12">
                                    <label runat="server" id="Label1" class="col-sm-2 control-label no-padding-right"
                                        for="form-field-1-1">
                                        Quantity:
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:RadioButton ID="RdoQty" Text="Quantity" TextAlign="Left" GroupName="Qty" runat="server" Checked="true" runat="server" />
                                        &nbsp;&nbsp;
                                    <asp:RadioButton ID="RdoNoQty" Text="No Quantity" TextAlign="Left" GroupName="Qty" runat="server" />
                                       
                                    </div>
                                </div>
                                <hr />
                                
                                 <div class="col-md-offset-1 col-md-9">
                                 <asp:Button Text="Submit" runat="server" ID="btnsubmit" CssClass="btn" OnClick="btnsubmit_Click" />
                                  </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <%--  <hr />
                        

                        <div class="space-4"></div>--%>
                </div>
                <%--end main page--%>
            </div>
        </div>
    </div>
</asp:Content>
