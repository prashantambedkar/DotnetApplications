<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductDetailsCS.ascx.cs" Inherits="usercontrol_ProductDetailsCS" %>

<style type="text/css">
    .mg {
    margin-left:50px;
  }
</style>
<table runat="server" style="width: 100%;z-index:9999;" class="center"  id="ProductWrapper" border="0" cellpadding="2"
    cellspacing="0" > 
    <tr>
        <td style="text-align: center;">
            <asp:FormView ID="ImageView" DataKeyNames="AssetId"
                runat="server" OnDataBound="ImageView_DataBound"> <%--DataSourceID="ProductDataSource" --%>
                <ItemTemplate>
                     <b><asp:Label CssClass="title" Visible="false" ID="Category" runat="server" Font-Size="Large"><%# Eval("Category")%></asp:Label></b>                   
                     <br />
                    <asp:Image ID="image" Width="200" Height="200" runat="server" CssClass="mg" ImageUrl='<%# Eval("ImageName", "/AssetImage/{0}") %>'  />                  
                </ItemTemplate>
            </asp:FormView>
        </td>
    </tr>
</table>

<%--<asp:SqlDataSource ID="ProductDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
    ProviderName="System.Data.SqlClient" SelectCommand="Select * from AssetMasterIdentifiedAndroid WHERE ([ImageName] = @ImageName)">
    <SelectParameters>
        <asp:Parameter Name="ImageName" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>--%>