<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="DownloadMasters.aspx.cs" Inherits="DownloadMasters" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="breadcrumbs" id="breadcrumbs">
                <ul class="breadcrumb">
                    <li><a href="#">Operations</a> </li>
                    <li><a href="#">Download Masters</a> </li>
                </ul>
            </div>
            <div class="page-header">
                <h1>
                    Download Masters</h1>
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
                            <div class="col-md-offset-3 col-md-9">
                                <asp:Button runat="server" ID="btnDownloadMasters_THR" CssClass="btn" 
                                    Text="Download Masters - THR" onclick="btnDownloadMasters_THR_Click"  />
                                <asp:Button ID="btnDownloadMasters_THS" CssClass="btn" runat="server" 
                                    Text="Download Masters - THS" onclick="btnDownloadMasters_THS_Click" />                               
                            </div>
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
