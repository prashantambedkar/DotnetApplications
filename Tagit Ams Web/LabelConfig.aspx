<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LabelConfig.aspx.cs" Inherits="LabelConfig"
    MasterPageFile="~/adminMasterPage.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //$(document).ready(function () {



        //$("#ContentPlaceHolder1_UpdatePanel1").keypress(function (e) {

        //                var PrintStatus = $('#ContentPlaceHolder1_UpdatePanel1').find('.myclass').parent('td')[0].innerHTML;
        //                var Postion = $('#ContentPlaceHolder1_UpdatePanel1').find('.myclass').parent('td')[1].innerHTML

        //                outerData = outerData[0].innerHTML;
        //                outerData = outerData.toString();
        //                var StatusindexPos = PrintStatus.toString().indexOf("Print Status");
        //                var PrintindexPos = PrintStatus.toString().indexOf("Print Status");
        //                if (indexPos > 1) {
        //                    if ($('#ContentPlaceHolder1_UpdatePanel1').find('.myclass').attr('class') == "myclass") {
        //                        var code;
        //                        if (window.event) {
        //                            code = e.keyCode;
        //                        }
        //                        if (code == 13)
        //                        { return false }
        //                        var char = keychar = String.fromCharCode(code);
        //                        char = char + $('#ContentPlaceHolder1_UpdatePanel1').find('.myclass').val();
        //                        if (char.trim() == "" || char.trim() == "0" || char.trim() == "1") {

        //                            return true;
        //                        } else {
        //                            alert('Allow only (0,1)')
        //                            return false;
        //                        }

        //                    }
        //                }



        //                else if ($('#ContentPlaceHolder1_UpdatePanel1').find('.myclass1').attr('class') == "myclass1") {


        //                    var arr = "0123456789,";

        //                    var code;
        //                    if (window.event) {
        //                        code = e.keyCode;
        //                    }
        //                    else
        //                        code = e.which;
        //                    var char = keychar = String.fromCharCode(code);
        //                    if (arr.indexOf(char) == -1) {
        //                        alert("Only numbers and comma(,) only allow")
        //                        return false;
        //                    }
        //                }

        //            });

        //             });

    </script>
    <style type="text/css">
        .bgimage {
            width: 21px;
            height: 21px;
            background: url("images/CloseGray.jpg");
            border: 0;
            display: inline-block;
            text-transform: uppercase;
            margin-right: 5px;
        }

        .changePosition {
            margin-right: 10px;
            border: 0;
        }

        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td {
            padding: 2px !important;
        }

        .remBorderddl {
            border-style: none;
            width: 190px !important;
            margin-left: -16px;
        }
    </style>
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
    <div class="main-content-inner" style="font-family: Calibri;font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="panel panel-default">
                <div class="panel-heading">


                    <label style="font-family: 'Calibri'; font-size: x-large;color:black;">Label Configuration</label>
                    <span style="padding-left: 10px" class="small ">(Position Should(x,y) axis values required)</span>
                    <div class="col-sm-2 btn-group pull-right">
                        <asp:Button ID="btnPreview" class="btn btn-primary" runat="server" Text="PREVIEW"
                            Style="font-size: 12px; border-radius: 5px" OnClick="btnPreview_Click" />
                    </div>
                </div>
                <!-- start top menu -->
                <div class="hidden">
                    <uc1:topmenu runat="server" ID="topmenu" />
                </div>

                <!-- /.page-header -->
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-12">
                            
                            <label runat="server" id="lblsubcattype" class="col-sm-1 control-label no-padding-right"
                                for="form-field-1" style="padding-top: 7px;">
                                Tag Type
                            </label>

                            <div class="col-sm-2">
<%--                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:DropDownList runat="server" ID="ddlTagType" AutoPostBack="true" OnSelectedIndexChanged="ddlTagType_SelectedIndexChanged"
                                            Width="180px">
                                        </asp:DropDownList>
<%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </div>

                        </div>
                    </div>
                    <%-- </div>--%>

                    <div class="row">
                        <div class="col-sm-12">
                            <asp:Label ID="lblCompany" runat="server" Text="" Visible="false"></asp:Label>
                            <label runat="server" id="Label2" class="col-sm-1 control-label no-padding-right"
                                for="form-field-1" style="padding-top: 7px">
                                Company 
                            </label>

                            <div class="col-sm-2">
                                <telerik:RadTextBox ID="txtCompany" runat="server" EmptyMessage="Name" Height="30px" ToolTip="Name"
                                    Width="180px">
                                </telerik:RadTextBox>
                            </div>

                            <div class="col-sm-2" style="margin-left: 20px;">
                                <telerik:RadTextBox ID="txtPosCompany" runat="server" EmptyMessage="Position" Height="30px" ToolTip="Position"
                                    Width="180px">
                                </telerik:RadTextBox>
                            </div>
                            <div class="col-sm-2" style="margin-left: 20px;">
<%--                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>--%>
                                        <asp:DropDownList runat="server" ID="ddlCompayOrient" AutoPostBack="true"
                                            Width="180px">
                                        </asp:DropDownList>
<%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </div>
                            <div class="col-sm-2" style="margin-left: 20px;">
<%--                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>--%>

                                        <asp:DropDownList runat="server" ID="ddlFontCompany" AutoPostBack="true"
                                            Width="180px">
                                        </asp:DropDownList>
<%--                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </div>
                            <div class="col-sm-2" style="margin-left: 20px;">
                                <telerik:RadTextBox ID="txtCompanyFontSize" runat="server" EmptyMessage="FontSize" Height="30px" ToolTip="FontSize"
                                    Width="180px">
                                </telerik:RadTextBox>
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-xs-12">
                             <asp:Label ID="lblBarcode" runat="server" Text="" Visible="false"></asp:Label>
                            <label runat="server" id="Label4" class="col-sm-1 control-label no-padding-right"
                                for="form-field-1" style="padding-top: 7px">
                                Barcode 
                            </label>
                            <div class="col-sm-2">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlbarcode" AutoPostBack="true"
                                            Width="180px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-sm-2" style="margin-left: 20px;">
                                <telerik:RadTextBox ID="txtPosbarcode" runat="server" EmptyMessage="Position" Height="30px"
                                    Width="180px">
                                </telerik:RadTextBox>
                            </div>
                            <div class="col-sm-2" style="margin-left: 20px;">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="ddlOrientation_barcode" AutoPostBack="true"
                                            Width="180px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:Label ID="lblLogo" runat="server" Text="" Visible="false"></asp:Label>
                            <label runat="server" id="Label3" class="col-sm-1 control-label no-padding-right"
                                for="form-field-1" style="padding-top: 7px">
                                Logo 
                            </label>

                            <div class="col-sm-2">
                                <telerik:RadTextBox ID="txtLogo" runat="server" EmptyMessage="Name" Height="30px"
                                    Width="180px">
                                </telerik:RadTextBox>
                            </div>
                            <div class="col-sm-2" style="margin-left: 20px;">
                                <telerik:RadTextBox ID="txtPosLogo" runat="server" EmptyMessage="Position" Height="30px"
                                    Width="180px">
                                </telerik:RadTextBox>
                            </div>
                            <div class="col-sm-2" style="margin-left: 20px;">

                                <asp:Button ID="BtnSave"  runat="server" Text="Save"
                                    Height="30px" Style="color: White;background-color:#448AC9;border-radius: 5px" OnClick="btnSave_Click" /> 
                               <%-- class="btn btn-primary" Style="font-size: 12px; border-radius: 5px" --%>
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-hover"
                                        DataKeyNames="id" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                                        OnRowUpdating="OnRowUpdating" EmptyDataText="No records has been added." AutoGenerateEditButton="true"
                                        AutoGenerateColumns="false" OnRowDataBound="GridView_RowDataBound" Font-Size="Smaller">
                                        <RowStyle HorizontalAlign="left"></RowStyle>
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="Id" ReadOnly="true" />
                                            <asp:BoundField DataField="FieldName" HeaderText="Field Name" ReadOnly="true" />
                                            <asp:BoundField DataField="MappingName" HeaderText="Mapping Name" ReadOnly="true" />
                                            <asp:BoundField DataField="PrintStatus" HeaderText="Print Status" ControlStyle-CssClass="myclass" Visible="false" />
                                            <asp:BoundField DataField="Position" HeaderText="Position" ControlStyle-CssClass="myclass" />
                                            <asp:BoundField DataField="BarCode" HeaderText="BarCode" ControlStyle-CssClass="myclass" Visible="false" />
                                            <asp:BoundField DataField="Prefix" HeaderText="Prefix" Visible="false" />
                                            <%--<asp:BoundField DataField="Font" HeaderText="Font" />--%>
                                            <asp:TemplateField HeaderText="Font">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFont" runat="server" Text='<%# Eval("FONT") %>' Visible="false" />
                                                    <asp:DropDownList ID="ddlFont" runat="server" Height="25px">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FontSize" HeaderText="Font Size" />
                                            <%--                                                <asp:BoundField DataField="Orientation" HeaderText="Orientation" />--%>
                                            <asp:TemplateField HeaderText="Orientation">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrientation" runat="server" Text='<%# Eval("Orientation") %>' Visible="false" />
                                                    <asp:DropDownList ID="ddlOrientation" runat="server" Height="25px">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TagType" HeaderText="TagType" Visible="false" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("IsSelected") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <%--end main page--%>
                </div>
            </div>
        </div>
    </div>
    </div>
    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="Panel22"
        TargetControlID="btnShow" CancelControlID="btnClose">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel22" runat="server" align="center" Style="display: none" CssClass="modalPopup"
        Height="132px" Width="250px">
        <table style="width: 100%">
            <tr style="height: 25px;" id="trheader" runat="server">
                <td colspan="1">
                     <label ID="Label1" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label>
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
        <table style="width: 100%">
            <tr style="height: 25px;" id="trheaderPrview" runat="server">
                <td>
                    <asp:Label Style="font-size: large" Text="&nbsp;Label&nbsp;Preview" runat="server"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btnClosePreview" runat="server" CssClass="bgimage" Text="X"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" style="border: none; width: 20%;" colspan="2">
                    <asp:Image ID="Image1" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" style="margin-right: 10px; border: 0px; padding-top: 65px;">
                    <asp:Button ID="btnCloseinnerPreview" runat="server" Text="OK" Width="50px" BackColor="152, 192, 218"
                        CssClass="changePosition" Visible="false"/>
                </td>
            </tr>
            <tr style="height: 0px;" id="trfooterPreview" runat="server">
                <td colspan="2" style="margin-right: 10px;"></td>
            </tr>
        </table>
    </asp:Panel>


</asp:Content>


