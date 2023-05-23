<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="Rollback.aspx.cs" Inherits="Rollback" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="main-content-inner">

        <div class="page-content">


            <div class="breadcrumbs" id="breadcrumbs">
             
                 
                <ul class="breadcrumb">
                    <li>
                        
                        <a href="#">Master </a>
                    </li>
                     <li>
                        
                        <a href="#">Asset Master </a>
                    </li>
                    <li>
                        <a href="#">Rollback Transaction</a>
                    </li>

                </ul>
            
            </div>
            <div class="page-header">
                <h1>Rollback Transaction</h1>
            </div>
            <!-- /.page-header -->
            
            <div class="row">
                <div class="col-xs-12">
                    <!-- start top menu -->
                    <div class="hidden">
                        <uc1:topmenu runat="server" ID="topmenu" />
                    </div>

                        
                        <hr />
                        

                        <div class="space-4"></div>
                        

                    </div>
                    <%--end main page--%>
                  </div>
                  </div>
</asp:Content>

