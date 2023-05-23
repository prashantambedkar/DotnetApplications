<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dropdownmultiselect.aspx.cs" Inherits="dropdownmultiselect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="ddl1" Width="300px" runat="server" multiple="multiple" CssClass="form-control js-example-placeholder-single"
                ToolTip="Select ">
            </asp:DropDownList>
            <asp:HiddenField ID="hfSelected" runat="server" />
            <asp:Button Text="Selected Country" runat="server" OnClick="GetSelected" />
        </div>
    </form>
</body>
</html>
