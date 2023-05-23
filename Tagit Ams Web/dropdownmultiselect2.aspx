<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="dropdownmultiselect2.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.3/css/select2.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.3/js/select2.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".js-example-placeholder-single").select2({
                placeholder: "Select",
                allowClear: false
            });
            $('#ddl1').on('change', function () {
                $('#<%=hfSelected.ClientID%>').val($(this).val());
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DropDownList ID="ddl1" Width="300px" runat="server" multiple="multiple" CssClass="form-control js-example-placeholder-single"
        ToolTip="Select ">
    </asp:DropDownList>
    <asp:HiddenField ID="hfSelected" runat="server" />
    <asp:Button Text="Selected Country" runat="server" OnClick="GetSelected" />
</asp:Content>

