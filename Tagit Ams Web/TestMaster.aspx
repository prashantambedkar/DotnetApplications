<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="TestMaster.aspx.cs" Inherits="TestMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">






    <script src="js/jquery.min.js" type="text/javascript"></script>



    <script type="text/javascript">

        function changebuttonname(lbllocname) {
            document.getElementById('<%=btnsubmit.ClientID%>').value = lbllocname;
        }
    </script>
    <script type="text/javascript">
        function OnClientItemsRequestedHandler(sender, eventArgs) {
            //set the max allowed height of the combo  
            var MAX_ALLOWED_HEIGHT = 220;
            //this is the single item's height  
            var SINGLE_ITEM_HEIGHT = 22;

            var calculatedHeight = sender.get_items().get_count() * SINGLE_ITEM_HEIGHT;

            var dropDownDiv = sender.get_dropDownElement();

            if (calculatedHeight > MAX_ALLOWED_HEIGHT) {
                setTimeout(
                    function () {
                        dropDownDiv.firstChild.style.height = MAX_ALLOWED_HEIGHT + "px";

                    }, 20
                );
            }
            else {
                setTimeout(
                    function () {
                        dropDownDiv.firstChild.style.height = calculatedHeight + "px";
                    }, 20
                );
            }
        }
    </script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }

        .rcbItem {
            width: 100%;
        }

        .RadListBox1 {
            left: 37px;
            top: 84px;
        }
    </style>
    <style type="text/css">
        @media only screen and (min-width: 480px) and (max-width: 767px) {
            .additionalColumn {
                display: none !important;
            }
        }
    </style>
    <style type="text/css">
        .gvData {
            margin-left: auto !important;
            margin-right: auto !important;
            width: 100%;
        }

        .ddllocabgcolor {
            background-color: white;
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Button ID="btnErrorPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnErrorPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackground" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopup" Height="140px" Width="400px"
        Style="display: none">
        <div class="headerModal">
            Confirmation
        </div>
        <div class="body">
            <asp:Label ID="lblmodmsg" runat="server" Text="You are not authorized to view this page." />
        </div>
        <div align="center">
            <asp:Button ID="btnYes" runat="server" Text="Ok" OnClick="btnYes_Click" CssClass="yes" />
        </div>
    </asp:Panel>
    <script type="text/javascript">
        try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
    </script>
    <style type="text/css">
        .mycheckbox input[type="checkbox"] {
            margin-right: 5px;
            font-family: Cambria;
            font-size: large;
        }


        .DivClose {
            display: none;
            position: absolute;
            width: 250px;
            height: 220px;
            border-style: solid;
            border-color: Gray;
            border-width: 1px;
            background-color: #99A479;
        }

        .LabelClose {
            vertical-align: text-top;
            position: absolute;
            bottom: 0px;
            font-family: Verdana;
        }

        .DivCheckBoxList {
            display: none;
            background-color: White;
            width: 250px;
            position: absolute;
            height: 200px;
            overflow-y: auto;
            overflow-x: hidden;
            border-style: solid;
            border-color: Gray;
            border-width: 1px;
        }

        .CheckBoxList {
            position: relative;
            width: 250px;
            height: 10px;
            overflow: scroll;
            font-size: small;
        }
    </style>
    <script type="text/javascript">
        var timoutID;

        //This function shows the checkboxlist
        function ShowMList() {
            var divRef = document.getElementById("divCheckBoxList");
            divRef.style.display = "block";
            var divRefC = document.getElementById("divCheckBoxListClose");
            divRefC.style.display = "block";
        }

        //This function hides the checkboxlist
        function HideMList() {
            document.getElementById("divCheckBoxList").style.display = "none";
            document.getElementById("divCheckBoxListClose").style.display = "none";
        }

        //This function finds the checkboxes selected in the list and using them,
        //it shows the selected items text in the textbox (comma separated)
        function FindSelectedItems(sender, textBoxID) {
            var cblstTable = document.getElementById(sender.id);
            var checkBoxPrefix = sender.id + "_";
            var noOfOptions = cblstTable.rows.length;
            var selectedText = "";
            for (i = 0; i < noOfOptions ; ++i) {
                if (document.getElementById(checkBoxPrefix + i).checked) {
                    if (selectedText == "")
                        selectedText = document.getElementById
                                           (checkBoxPrefix + i).parentNode.innerText;
                    else
                        selectedText = selectedText + "," +
                         document.getElementById(checkBoxPrefix + i).parentNode.innerText;
                }
            }
            document.getElementById(textBoxID.id).value = selectedText;
        }
    </script>

    <script language="javascript" type="text/javascript">

        function Validate() {


            var chck = '<%=chkstatus.ClientID %>';
            var pass = '<%=txtboxpas.ClientID %>';

            var txtcat = '<%=txtboxname.ClientID %>';

            var e = document.getElementById('<%=ddtype.ClientID %>');
            var strUser = e.options[e.selectedIndex].text;

            if (document.getElementById(txtcat).value == '') {
                //alert("Please Enter The User Name");

                document.getElementById(txtcat).focus();

                return false;

            }

            if (document.getElementById(pass).value == '') {
                //alert("Please Enter Password");

                document.getElementById(pass).focus();

                return false;

            }

            if (strUser == '--Select--') {
                //alert("Please Select Type");

                e.focus();

                return false;

            }

            if (document.getElementById('<%=btnsubmit.ClientID %>').value == 'Update') {
                if (document.getElementById(chck).checked == false) {
                    return confirm('Do you really want to deactivate this user?');

                }
            }


            return true;
        }

    </script>


    <div class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <label style="font-family: 'Calibri'; font-size: x-large; color: black">
                        User Management</label>
                </div>
                <div class="panel-body">
                    <div>
                        <div class="col-sm-3">
                            <label runat="server" id="Label3" class="col-sm-2 control-label no-padding-right"
                                for="form-field-1-1">
                                Permission
                            </label>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:ListBox ID="lstPermission" runat="server" SelectionMode="Multiple" Style="height: 190px"></asp:ListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <%--                        <div class="col-sm-1" style="border-left: thick solid #ff0000;">
                        </div>--%>
                        <div>
                            <div class="row">
                                <div class="col-sm-9">
                                    <div class="form-horizontal">
                                        <div class="col-sm-12" style="text-align: center">
                                            <asp:Label runat="server" ID="lblSuccessMessage" Style="color: green" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </div>
                                        <div class="form-group">
                                            <div class="container" style="width: 100%">
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-12">
                                                        <h7><asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                                            ForeColor="Red"></asp:Label>
                                                        <asp:Label ID="lblImgUp" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label></h7>
                                                        <asp:Label runat="server" ID="showerror" Style="color: Red"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                        <h7> <asp:Label runat="server" ID="lblcattype"> User Name</asp:Label>
                                                        <span style="color: Red">*</span> :</h7>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtboxname" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7> <asp:Label runat="server" ID="Label11"> Password</asp:Label>
                                                        <span style="color: Red">*</span> :</h7>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtboxpas" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7> <asp:Label runat="server" ID="Label1"> Email ID</asp:Label>
                                                        <span style="color: Red">*</span> :</h7>
                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtEmailID" Width="100%" runat="server"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="regexEmailid" runat="server" ControlToValidate="txtEmailID" ErrorMessage="EmailID Format Invalid" ForeColor="Red" ValidationExpression="^[a-zA-Z0-9_\-\.]+@[a-z]+\.[a-z]{2,3}"></asp:RegularExpressionValidator>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7><asp:Label runat="server" ID="lblsubcattype">Type</asp:Label>
                                                        <span style="color: Red">*</span> :</h7>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddtype" AutoPostBack="true" class="form-control"
                                                                    OnSelectedIndexChanged="ddtype_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <h7><asp:Label runat="server" ID="Label10">Location</asp:Label>
                                                        <span style="color: Red">*</span> :</h7>
                                                        <asp:UpdatePanel ID="UpdatePanel8demo" runat="server">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox CssClass="form-control" BackColor="white" EnableScreenBoundaryDetection="false" runat="server" ID="ddllocation" EnableLoadOnDemand="true" CheckBoxes="true"
                                                                    EnableCheckAllItemsCheckBox="true" CheckedItemsTexts="DisplayAllInInput" EmptyMessage="--Select--" Width="100%"
                                                                    OnClientItemsRequested="OnClientItemsRequestedHandler" OnClientDropDownOpening="OnClientItemsRequestedHandler">
                                                                </telerik:RadComboBox>
                                                                <%--<asp:ListBox runat="server" ID="lstLocation" AutoPostBack="false" class="form-control" SelectionMode="Multiple"></asp:ListBox>--%>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                </div>
                                                <div class="row" style="width: 100%">

                                                    <div class="col-sm-3">
                                                        <h7> <asp:Label runat="server" ID="Label7">Custodian</asp:Label>
                                                        <span style="color: Red">*</span> :</h7>
                                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                            <ContentTemplate>
                                                                <telerik:RadComboBox EnableScreenBoundaryDetection="true" runat="server" ID="ddlCustodian" EnableLoadOnDemand="true" CheckBoxes="true"
                                                                    EnableCheckAllItemsCheckBox="true" Width="100%" CheckedItemsTexts="DisplayAllInInput" CssClass="form-control" EmptyMessage="--Select--"
                                                                    OnClientItemsRequested="OnClientItemsRequestedHandler" OnClientDropDownOpening="OnClientItemsRequestedHandler" ZIndex="6000">
                                                                </telerik:RadComboBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <h7><asp:Label runat="server" ID="Label8">Status</asp:Label></h7>
                                                        <%--<span style="color: Red">*</span> :--%>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkstatus" CssClass="form-control" runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                    <div class="col-sm-1">
                                                        <h7> <asp:Label runat="server" ID="Label2">Controller</asp:Label></h7>
                                                        <%-- <span style="color: Red">*</span>--%>
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkIsDirector" CssClass="form-control" runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <h7> <asp:Label runat="server" ID="Label12">Approve</asp:Label></h7>
                                                        <%-- <span style="color: Red">*</span> :--%>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkApprove" CssClass="form-control" runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <h7> <asp:Label runat="server" ID="Label13">Delete</asp:Label></h7>
                                                        <%--  <span style="color: Red">*</span> :--%>
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="chkDelete" CssClass="form-control" runat="server" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label runat="server" ID="Label4">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="SUBMIT" runat="server" ID="btnsubmit" OnClick="btnsubmit_Click"
                                                            OnClientClick="this.value = 'Submit';"
                                                            class="btn btn-primary form-control" />
                                                    </div>
                                                    <div class="col-sm-2" style="visibility: hidden">
                                                        <br />
                                                        <button type="button" style="width: 100%" class="btn btn-danger form-control text-dark" data-toggle="modal" data-target="#custodianModal">
                                                            <h7><asp:Label runat="server" ForeColor="white">CUSTODIAN</asp:Label></h7>
                                                        </button>
                                                        <div class="modal fade" id="custodianModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                                                            <div class="modal-dialog modal-dialog-centered modal-sm  modal-dialog-scrollable" role="document">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <h5 class="modal-title" id="exampleModalLongTitle">Select Location Details</h5>

                                                                    </div>
                                                                    <div class="modal-body" style="height: 500px; overflow-y: auto;">
                                                                        <div class="card-body">

                                                                            <asp:Repeater runat="server" OnItemDataBound="custodianrepeater_ItemDataBound" ID="custodianrepeater">
                                                                                <ItemTemplate>
                                                                                    <li style="width: 100%;">
                                                                                        <input type="checkbox" runat="server" value='<%#Eval("CustodianId")%>' id="chkcustodianID" />
                                                                                        <%#Eval("CustodianName")%></li>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                        <div>
                                                                            <br />
                                                                            <asp:Button ID="Button3" CssClass="btn btn-primary" OnClick="Button1_Click" runat="server" Text="Save Changes" />

                                                                            <asp:Button ID="Button4" CssClass="btn btn-danger pull-right" OnClick="Button4_Click" runat="server" Text="    Mark      " />
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3" style="visibility: hidden">
                                                        <br />
                                                        <button type="button" style="width: 100%" class="btn btn-danger form-control text-dark" data-toggle="modal" data-target="#locationModal">
                                                            <h7><asp:Label runat="server" ForeColor="white">LOCATIONS</asp:Label></h7>
                                                        </button>
                                                        <div class="modal fade" id="locationModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                                                            <div class="modal-dialog modal-dialog-centered modal-sm modal-dialog-scrollable" role="document">
                                                                <div class="modal-content">
                                                                    <div class="modal-header">
                                                                        <h5 class="modal-title" id="exampleModalLongTitle">Select Location Details</h5>

                                                                    </div>
                                                                    <div class="modal-body" style="height: 500px; overflow-y: auto;">
                                                                        <div class="card-body">

                                                                            <asp:Repeater runat="server" OnItemDataBound="rptPerson_ItemDataBound" ID="rptPerson">
                                                                                <ItemTemplate>
                                                                                    <li style="width: 100%;">
                                                                                        <input type="checkbox" runat="server" value='<%#Eval("LocationId")%>' id="chkDisplayTitle" />
                                                                                        <%#Eval("LocationName")%></li>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                        <div>
                                                                            <br />
                                                                            <asp:Button ID="Button1" CssClass="btn btn-primary" OnClick="Button1_Click" runat="server" Text="Save Changes" />

                                                                            <asp:Button ID="Button2" CssClass="btn btn-danger pull-right" OnClick="Button2_Click" runat="server" Text="    Mark      " />
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>







                                            <%-- //-new section---------%>



                                            <%--//------------code by prashant end--%>

                                            <div class="col-sm-3 align-center" style="padding: 5px">
                                                <%--                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>--%>
                                                <%--  <asp:Button Text="Submit" runat="server" ID="btnsubmit" OnClick="btnsubmit_Click"
                                                    Style="height: 35px; border-radius: 5px; padding: 0; width: 75%" OnClientClick="javascript:return Validate();"
                                                    class="btn btn-primary"Style="height: 35px; border-radius: 5px; padding: 0" />--%>


                                                <%--                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                            </div>
                                            <asp:HiddenField ID="hiduserid" runat="server" />
                                            <div class="col-xs-12">
                                                <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"
                                                    Width="100%"></asp:Label>
                                                <span style="font-weight: bold;"></span>
                                                <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;" Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtPageCount" runat="server" Visible="False"></asp:TextBox>
                                                <asp:Label ID="lblSort" runat="server" Visible="False"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div>
                                    <div class="col-sm-2">
                                    </div>
                                    <div class="col-sm-9">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                                    CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                                    OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                                    OnItemCommand="gv_data_ItemCommand">
                                                    <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                                    <%-- <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                                    CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data_ItemCommand">--%>
                                                    <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                                                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                                    <ClientSettings EnablePostBackOnRowClick="false">
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                                        <ClientEvents OnGridCreated="GridCreated" />
                                                    </ClientSettings>
                                                    <SortingSettings EnableSkinSortStyles="false" />
                                                    <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                                        DataKeyNames="UserId">
                                                        <PagerStyle AlwaysVisible="true" Position="Top" />
                                                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                                        </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridButtonColumn CommandName="dit" ButtonType="ImageButton" UniqueName="Edit"
                                                                ImageUrl="~/images/pencil.png">
                                                            </telerik:GridButtonColumn>
                                                            <telerik:GridTemplateColumn HeaderText="SR NO" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="UserId" FilterControlAltText="Filter UserId column"
                                                                HeaderText="UserId" SortExpression="UserId" UniqueName="UserId" ReadOnly="true"
                                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column"
                                                                HeaderText="Type" SortExpression="Type" UniqueName="Type" ReadOnly="true" AllowSorting="false"
                                                                AllowFiltering="true" Visible="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="UserName" FilterControlAltText="Filter UserName column"
                                                                HeaderText="USER NAME" SortExpression="UserName" UniqueName="UserName" ReadOnly="true"
                                                                AllowFiltering="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="PASSWORD" FilterControlAltText="Filter PASSWORD column"
                                                                HeaderText="PASSWORD" SortExpression="PASSWORD" UniqueName="PASSWORD" ReadOnly="true"
                                                                AllowFiltering="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="EmailID" FilterControlAltText="Filter EmailID column"
                                                                HeaderText="EMAIL ID" SortExpression="EmailID" UniqueName="EmailID" ReadOnly="true"
                                                                AllowFiltering="true">
                                                            </telerik:GridBoundColumn>
                                                            <%-- <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                                                                HeaderText="LOCATION" SortExpression="Location" UniqueName="Location" ReadOnly="true"
                                                                AllowFiltering="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CustodianName" FilterControlAltText="Filter Location column"
                                                                HeaderText="CUSTODIAN-NAME" SortExpression="CustodianName" UniqueName="CustodianName" ReadOnly="true"
                                                                AllowFiltering="false">
                                                            </telerik:GridBoundColumn>--%>
                                                            <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter CreatedBy column"
                                                                HeaderText="CREATED BY" SortExpression="CreatedBy" UniqueName="CreatedBy" ReadOnly="true"
                                                                AllowFiltering="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                                                HeaderText="LAST MODIFIED DATE" SortExpression="CreatedDate" UniqueName="CreatedDate"
                                                                ReadOnly="true" AllowFiltering="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                                                HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true"
                                                                AllowFiltering="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Approve" FilterControlAltText="Filter Approve column"
                                                                HeaderText="APPROVE" SortExpression="Approve" UniqueName="Approve" ReadOnly="true"
                                                                AllowFiltering="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="IsDirector" FilterControlAltText="Filter IsDirector column"
                                                                HeaderText="DOCUMENT CONTROLLER" SortExpression="IsDirector" UniqueName="IsDirector" ReadOnly="true"
                                                                AllowFiltering="true">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="CanDelete" FilterControlAltText="Filter CanDelete column"
                                                                HeaderText="DELETE" SortExpression="CanDelete" UniqueName="CanDelete" ReadOnly="true"
                                                                AllowFiltering="true">
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
                            <div class="row">
                                <div class="col-xs-12">
                                    <!-- start top menu -->
                                    <div class="hidden">
                                        <uc1:topmenu runat="server" ID="topmenu" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div>
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
        <%--CssClass="modalPopup" border: 1px solid Gray; --%>
        <table style="width: 100%">
            <tr style="height: 25px;" id="trheader" runat="server">
                <td colspan="1">
                    <label id="Label5" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label></td>
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
                    <%--ImageUrl="~/images/Success.png"--%>
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
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">User Management Details</h5>

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
                                <%--<asp:Button Text="OK" runat="server" ID="Button1" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-warning pull-center" />--%>
                                <button type="button" class="btn btn-warning" data-dismiss="modal">OK</button>
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
            $('#myModal').modal('show');
        }
        function setvalueforlocation(lbllocname) {
            document.getElementById('lbllocname').innerHTML = lbllocname;
            //document.getElementById('lbllocname').innerHTML = lbllocname;
        }
    </script>
</asp:Content>
