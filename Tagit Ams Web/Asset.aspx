<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Asset.aspx.cs" MasterPageFile="~/adminMasterPage.master"
    Inherits="Asset" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
   
    <script type="text/javascript">
        $(function () {
            $("[id$=txtstartdate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
            });
            $("[id$=txtenddate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
            });

            //$('[id*=lstBoxTest]').multiselect({
            //    includeSelectAllOption: true
            //});

        });
    </script>
    <script type="text/javascript" language="javascript">
        function Validate() {
            var z = document.getElementById('<%=ddlsupplier.ClientID %>');
            var zuser = z.options[z.selectedIndex].text;
            var txtwarr = '<%=txtwarr.ClientID %>';

            if (zuser == '--Select--') {
                alert("Please Select Supplier");

                z.focus();

                return false;

            }
            if (document.getElementById(txtwarr).value == '') {
                alert("Please Enter The AMC Type");

                document.getElementById(txtwarr).focus();

                return false;
            }

            return true;
        }

    </script>
    <script type="text/javascript">

        function validatefile() {

            var array = ['xls', 'xlsx'];

            var xyz = document.getElementById('<%=productimguploder.ClientID %>');

            var Extension = xyz.value.substring(xyz.value.lastIndexOf('.') + 1).toLowerCase();

            if (array.indexOf(Extension) <= -1) {

                alert("Please Upload only .xls or .xlsx extension file");
                document.getElementById('<%=productimguploder.ClientID %>').focus();
                return false;

            }
        }

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
        .gvData {
            margin-left: auto !important;
            margin-right: auto !important;
        }
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
    <script type="text/javascript">


        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            var grid = $find("<%=gvData.ClientID %>");
            var masterTable = grid.get_masterTableView();
            var number = 0;
            for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                var gridItemElement = masterTable.get_dataItems()[i].findElement("cboxSelect");
                if (gridItemElement.checked) {
                    number++;
                }
            }



            if (number == 0) {
                alert("Please select items from grid");
                return false;
            }



            //            if (confirm("Do you want to update these assets ?")) {
            //                confirm_value.value = "Yes";
            //            } else {
            //                confirm_value.value = "No";
            //            }
            confirm_value.value = "Yes";
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = true; // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    masterTable.get_dataItems()[i].findElement("cboxSelect").checked = false; // for unchecking the checkboxes
                }
            }
        }
        function unCheckHeader(id) {
            var masterTable = $find("<%= gvData.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == false) {
                //var hidden = document.getElementById("HiddenField3");
                var checkBox = document.getElementById(document.getElementById("<%=HiddenField3.ClientID%>").value);
                checkBox.checked = false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdfImport" runat="server" />
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
    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup"
        PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" CssClass="modalPopupN" Height="181px" Width="400px"
        Style="display: none; text-align: justify;">
        <div class="headerN" style="text-align: center">
            Confirmation
            <asp:ImageButton ID="btnHide" Visible="false" runat="server" ImageUrl="~/images/Close.gif"
                Style="border: 0px" align="right" OnClientClick="return HideModalPopup()" />
        </div>
        <br />
        <div class="body">
            <asp:Label ID="lblmodmsg" runat="server" />
        </div>
        <br />
        <div align="center">
            <asp:Button ID="btnYes" runat="server" Text="Yes" Visible="false" OnClick="btnYes_Click"
                CssClass="yesN" />
            <asp:Button ID="btnNo" runat="server" Text="Close" OnClick="btnNo_Click" CssClass="noN" />
        </div>
    </asp:Panel>
    <asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowPopup1"
        PopupControlID="pnlpopupNew" BackgroundCssClass="modalBackground" BehaviorID="mpeNew">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopupNew" runat="server" CssClass="modalPopup" Width="850px" Height="600px"
        ScrollBars="Vertical" Style="display: none">
        <div class="modal-dialog" style="width: 850px;">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 id="h1" runat="server" class="modal-title">Map xls Columns with DataBase Columns
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <label><%#Category %> Name</label>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl1" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <label><%#Building %> Name</label>
                                    <%--<asp:Literal runat="server" ID="ltlBuildingName" Text="Building Name"></asp:Literal>--%>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl10" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <label><%#Floor %> Name</label>
                                    <%--<asp:Literal runat="server" ID="ltlFloorName" Text="Floor Name"></asp:Literal>--%>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl5" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <label><%#Location %> Name</label>
                                    <%--<asp:Literal runat="server" ID="ltlLocationName" Text="Location Name"></asp:Literal>--%>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl3" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="LtlDeptName" Text="Department Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl12" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlCustodianName" Text="Custodian Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl14" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlSupplierName" Text="Supplier Name"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl16" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlSerialNo" Text="Serial No"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl18" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlDecription" Text="Decription"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl19" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlQuantity" Text="Quantity"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl20" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlPrice" Text="Price"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl21" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlDeliveryDate" Text="Delivery Date"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl22" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlAssignDate" Text="AssignDate"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl23" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="llActive" Text="Active"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl30" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="Literal1" Text="Image"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl300" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn1" Text="Column1"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl24" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn2" Text="Column2"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl4" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn3" Text="Column3"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl11" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn4" Text="Column4"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl17" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn5" Text="Column5"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl6" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn6" Text="Column6"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl2" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn7" Text="Column7"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl8" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn8" Text="Column8"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl9" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn9" Text="Column9"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl13" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn10" Text="Column10"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl15" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn11" Text="Column11"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl25" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn12" Text="Column12"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl26" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn13" Text="Column13"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl27" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn14" Text="Column14"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl28" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">
                                    <asp:Literal runat="server" ID="ltlColumn15" Text="Column15"></asp:Literal>
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList runat="server" ID="ddl29" CssClass="form-control" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                &nbsp;
                            </div>
                            <div class="form-group">
                                <div class="col-sm-6">
                                    <div>
                                        <asp:Button Text="Submit" runat="server" ID="btnMapping" OnClientClick="javascript:return MappingValidate();"
                                            CssClass="btn" OnClick="btnMapping_Click" />
                                        <asp:Button Text="Close" runat="server" ID="BtnClose" OnClientClick="return HideMapModalPopup()"
                                            CssClass="btn" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </asp:Panel>
    <div class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <%-- <div class="breadcrumbs" id="breadcrumbs">
                <script type="text/javascript">
                    try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
                </script>

                <ul class="breadcrumb">
                   <li>
                        <i class="ace-icon fa fa-home home-icon"></i>
                        <a href="#">Asset</a>
                    </li>
                </ul>
                
            </div>--%>
            <div class="panel panel-default">
                <div style="height: 50px;" class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div class="col-sm-5">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="GetGrid" Style="height: 34px" runat="server" OnClick="GetGrid_Click"
                                            class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton>
                                    </span>
                                </div>
                            </div>

                            <div class="btn-group pull-right">
                                <div>
                                    <div class="input-group input-file">

                                        <div id="divSelectFile" runat="server" class="form-control">
                                            <a style="width: 36px" target="_blank">select file....</a>
                                        </div>
                                        <span id="divBrowse" runat="server" class="input-group-addon"><a style="font-size: 11px; padding-bottom: 3px"
                                            class='btn btn-primary' href='javascript:;'>Browse
                                            <%--<input type="file" name="field_name" onchange="$(this).parent().parent().parent().find('.form-control').html($(this).val());" />--%>
                                            <asp:FileUpload runat="server" ID="productimguploder" onchange="$(this).parent().parent().parent().find('.form-control').html($(this).val());" />
                                        </a></span>
                                    </div>
                                    <asp:Button Text="Upload" runat="server" ID="btnImport" OnClick="btnImport_Click"
                                        OnClientClick="javascript:return validatefile();" Style="height: 35px; border: 0; font-size: 11px; border-radius: 5px;"
                                        type="button" class="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="row" id="divUpdateWaranty">
                        <div class="form-inline">
                            <div class="col-sm-12">
                                <div class="btn-group pull-left">
                                    <div class="container" style="width: 100%">
                                        <div class="row" style="width: 100%">
                                            <div class="col-sm-5">
                                                <asp:Button ID="btnUpdateInfo" runat="server" class="btn btn-primary form-control"
                                                    Text="Update" OnClick="btnUpdateInfo_Click" />
                                            </div>

                                            <div class="col-sm-5">
                                                <asp:Button ID="btnExport" class="btn btn-primary form-control" runat="server"
                                                    Text="Export" OnClick="btnExportExcel_Click" />
                                            </div>
                                            <div class="col-sm-5">
                                                <asp:Button ID="btnWarnaty" class="btn btn-primary" OnClick="btnWarnaty_Click"
                                                    runat="server" Text="Warranty/AMC" />
                                            </div>
                                        </div>
                                    </div>


                                </div>
                                <div class="btn-group pull-right">
                                    <asp:Button ID="btnDelete" class="btn btn-danger" BackColor="Red" OnClick="btnDelete_Click"
                                        Style="height: 35px; border-radius: 5px; font-size: 11px" Width="60px"
                                        runat="server" Text="Delete" OnClientClick="return confirm('Are you sure, to delete?');return false;" />
                                </div>
                            </div>
                            <%--<div class="col-sm-6">

                                <asp:DropDownList runat="server" ID="lstBoxTest" SelectionMode="Multiple"  ShowCheckbox="true">
                                    <asp:ListItem Text="Red" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Green" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                              
                            </div>--%>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="hidden">
                            <uc1:topmenu runat="server" ID="topmenu" />
                        </div>
                    </div>
                </div>
                <div class="page-header">
                    <div class="panel-body">
                        <div class="row">
                            <div id="divSearch" runat="server" class="panel-group col-md-9 form_wrapper" style="width: 100%;">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <label>
                                                    Search</label>
                                            </div>
                                            <div class="col-sm-10 pull-right">
                                                <a class="ex1" id="divSearchClose" href="#"><span id="spanSerch" style="top: 0px;"
                                                    class="badge">X</span></a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <div class="container" style="width: 100%">
                                                    <div class="row" style="width: 100%">
                                                        <div class="col-sm-3">
                                                            <asp:Label runat="server" ID="Label23"> <%#Assets %> Code</asp:Label>
                                                            <asp:TextBox ID="txtAssetCode" class="form-control" placeholder="Enter the value"
                                                                runat="server" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <h7><asp:Label runat="server" ID="Label24"> <%#Category %></asp:Label></h7>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList runat="server" ID="ddlCategorySearch" OnSelectedIndexChanged="OnSelectedIndexChangedCategory_Search"
                                                                        AutoPostBack="true" class="form-control">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <div class="col-sm-3">
                                                            <h7><asp:Label runat="server" ID="Label26"><%#Location %></asp:Label></h7>
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList runat="server" ID="ddllocSearch" OnSelectedIndexChanged="OnSelectedIndexChangedLocation_Search"
                                                                        AutoPostBack="true" class="form-control">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <h7><asp:Label runat="server" ID="Label27"><%#Building %></asp:Label></h7>
                                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList runat="server" ID="ddlbuildSearch" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding_Search"
                                                                        AutoPostBack="true" class="form-control">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="width: 100%">
                                                        <div class="col-sm-3">
                                                            <h7><asp:Label runat="server" ID="Label28"><%#Floor %></asp:Label><h7>
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList runat="server" ID="ddlfloorSearch" AutoPostBack="true" class="form-control">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-sm-0">
                                                            <h7><asp:Label runat="server" Visible="false" ID="Label29">Department</asp:Label></h7>
                                                            <asp:UpdatePanel ID="UpdatePanel6" Visible="false" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList runat="server" ID="ddldeptSearch" class="form-control">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <h7><asp:Label runat="server" ID="Label30">Custodian</asp:Label></h7>
                                                            <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList runat="server" ID="ddlCustodian" class="form-control">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:Label runat="server" ID="Label31">&nbsp;</asp:Label><br />
                                                            <asp:Button Text="SEARCH" runat="server" ID="btnsubmit" class="btn btn-primary form-control"
                                                                OnClick="btnsubmit_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:Label runat="server" ID="Label32">&nbsp;</asp:Label><br />
                                                            <asp:Button Text="CLEAR" runat="server" ID="btnclear" class="btn btn-danger form-control"
                                                                OnClick="btnClear_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divUpdateFields" runat="server" class="panel panel-default" runat="server">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label>
                                            <%#Assets %> Update</label>
                                    </div>
                                    <div class="col-sm-10 pull-right">
                                        <a id="IdAssetclose" href="#"><span style="top: 0px" class="badge">X</span></a>
                                        <%-- background-color: Red!important;--%>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizantal">
                                    <div class="form-group">
                                        <div class="container" style="width: 100%">
                                            <div class="row" style="width: 100%">
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label33">Choose Field:</asp:Label>
                                                    <asp:DropDownList runat="server" ID="cboField" AutoPostBack="true" class="form-control"
                                                        OnSelectedIndexChanged="cboField_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label34">Value:</asp:Label>
                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="form-control"></asp:TextBox>

                                                    <asp:DropDownList runat="server" ID="ddlstatus" class="form-control">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="Active">Active</asp:ListItem>
                                                        <asp:ListItem Value="Inprogress">Inprogress</asp:ListItem>
                                                        <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label5">&nbsp;</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="DropDownList2" AutoPostBack="true" class="form-control"
                                                                Visible="false">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <%--  Visible="false"--%>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label runat="server" ID="Label35">&nbsp;</asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList runat="server" ID="cboValue" AutoPostBack="true" class="form-control"
                                                                Visible="false">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <%--  Visible="false"--%>
                                                </div>

                                            </div>
                                            <div class="row" style="width: 100%">
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnUpdate" runat="server" Text="UPDATE" class="btn btn-primary form-control"
                                                        ValidationGroup="a" OnClick="btnUpdate_Click"
                                                        OnClientClick="javascript:return Confirm()" /><%--  OnClientClick="Confirm()" OnClick="btnPrint_Click"--%>
                                                </div>
                                            </div>
                                        </div>



                                    </div>
                                </div>
                            </div>
                            <div>
                            </div>
                        </div>
                        <div id="divWarantyClose" runat="server" class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label>
                                            Warranty/ AMC Info</label>
                                    </div>
                                    <div class="col-sm-10 pull-right">
                                        <a id="idClose" href="#"><span style="top: 0px" class="badge">Close</span></a>
                                    </div>
                                    <%--background-color: Red!important; --%>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div id="divWarantyAmc">
                                    <!-- /.page-header -->
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <!-- start top menu -->
                                            <%--                                        <div class="hidden">
                                            <uc1:topmenu runat="server" ID="topmenu2" />                                            
                                        </div>--%>
                                            <%--end top menu--%>
                                            <%--start main page--%>
                                            <div class="form-horizontal">
                                                <div class="col-sm-12" style="text-align: center">
                                                    <asp:Label runat="server" ID="Label7" Style="color: green" Font-Bold="true" Font-Size="Medium"
                                                        Visible="false"></asp:Label>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" EnableViewState="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                    <asp:Label ID="Label10" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label>
                                                    <asp:Label runat="server" ID="Label12" Style="color: Red"></asp:Label>
                                                    <label runat="server" id="Label13" class="col-sm-2 control-label no-padding-right"
                                                        for="form-field-1">
                                                        Supplier Name
                                                        <asp:Label ID="Label17" Text="*" ForeColor="Red" runat="server" />
                                                        :
                                                    </label>
                                                    <div class="col-sm-3">
                                                        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlsupplier" AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <label runat="server" id="Label18" class="col-sm-2 control-label no-padding-right"
                                                        for="form-field-1">
                                                        Warranty/AMC Type <span style="color: Red">*</span> :
                                                    </label>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtwarr" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                                    </div>
                                                    <label runat="server" id="Label19" class="col-sm-2 control-label no-padding-right"
                                                        for="form-field-1">
                                                        AMC Start Date <span style="color: Red">*</span> :
                                                    </label>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtstartdate" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                                    </div>
                                                    <label runat="server" id="Label20" class="col-sm-2 control-label no-padding-right"
                                                        for="form-field-1">
                                                        AMC End Date <span style="color: Red">*</span> :
                                                    </label>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtenddate" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                                    </div>
                                                    <label runat="server" id="Label21" class="col-sm-2 control-label no-padding-right"
                                                        for="form-field-1">
                                                        Remark
                                                    </label>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtrmk" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div class="col-md-offset-5 col-md-12">
                                                        <%--border-radius: 10px;--%>
                                                        <asp:Button Text="Submit" Style="height: 35px; border: 0; font-size: 11px;" class="btn btn-primary"
                                                            runat="server" ID="btnSubmit_Warranty" OnClick="btnSubmit_Warranty_Click" OnClientClick="javascript:return Validate();" />
                                                        <asp:Button ID="btnreset" Style="height: 35px; border: 0; font-size: 11px;" class="btn btn-primary"
                                                            runat="server" Text="Reset" OnClick="btnreset_Click" />
                                                        <%-- border-radius: 10px;--%>
                                                        <%--                                                    <asp:Button Text="Export" Style="height: 35px; border: 0; font-size: 11px; border-radius: 10px;"
                                                        class="btn btn-primary" runat="server" ID="exptxl" />--%>
                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--end main page--%>
                                        <%-- <span style="font-weight: bold;">Total Records.</span>
                                    <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="divAssetMaster" runat="server">
                            <div class="page-header">
                                <h1><%#Assets %> Master</h1>
                            </div>
                            <!-- /.page-header -->
                            <div class="row">
                                <div class="col-xs-12">
                                    <!-- start top menu -->
                                    <div class="hidden">
                                        <uc1:topmenu runat="server" ID="topmenu1" />
                                    </div>
                                    <%--end top menu--%>
                                    <%--start main page--%>
                                    <div class="form-horizontal">
                                        <div class="col-sm-12" style="text-align: center">
                                            <asp:Label runat="server" ID="lblSuccessMessage" Style="color: green" Font-Bold="true"
                                                Font-Size="Medium" Visible="false"></asp:Label>
                                        </div>
                                        <div class="form-group" id="SearchDiv" runat="server">
                                            <asp:Label ID="lblImgUp" runat="server" EnableViewState="False" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            <asp:Label runat="server" ID="showerror" Style="color: Red"></asp:Label>
                                            <label runat="server" id="lblcattype" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Category Type
                                                <asp:Label ID="Label4" Text="*" ForeColor="Red" runat="server" />
                                                :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlproCategory" OnSelectedIndexChanged="OnSelectedIndexChangedCategory"
                                                            AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <%--<hr />--%>
                                            <%--<div class="form-group">--%>
                                            <label runat="server" id="Label1" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Select Location
                                                <asp:Label ID="Label2" Text="*" ForeColor="Red" runat="server" />
                                                :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLocation"
                                                            AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label runat="server" id="lblcomposition" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Select Building :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlbuild" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding"
                                                            AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <br />
                                            <label runat="server" id="Label8" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Select Floor :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlfloor" AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <%--</div>--%>
                                            <%--<div class="space-4"></div>

                        <div class="space-4"></div>--%>
                                            <%--<div class="form-group">--%>
                                            <label runat="server" id="Label6" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Select Department :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" OnSelectedIndexChanged="OnSelectedIndexChangedDepartment"
                                                            ID="ddldept" AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <%--</div>--%>
                                            <%--<div class="space-4"></div>--%>
                                            <%--<div class="form-group">--%>
                                            <label runat="server" id="Label11" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Select Custodian :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlcust" AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <label runat="server" id="Label3" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Select Supplier :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="DropDownList1" AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <%--</div>--%>
                                            <%--<div class="space-4"></div>--%>
                                            <%--<div class="form-group">--%>
                                            <label runat="server" id="lblsr" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Serial No.:
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtserail" class="form-control" runat="server" MaxLength="20" placeholder="Sr. No."
                                                    AutoPostBack="false"></asp:TextBox>
                                                <%-- <asp:HiddenField ID="hdnSerialNo" runat="server" />--%>
                                            </div>
                                            <label runat="server" id="lblqty" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Quantity :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtquant" onkeypress="javascript:return isNumber(event)" MaxLength="10000"
                                                    class="form-control" runat="server" placeholder="Quantity!" AutoPostBack="false"></asp:TextBox>
                                            </div>
                                            <label runat="server" id="lbldeldate" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Delivery Date:
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtdeldate" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                            </div>
                                            <%--</div>--%>
                                            <%--<div class="form-group">--%>
                                            <label runat="server" id="lblassdt" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Assign Date:
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtass" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                            </div>
                                            <label runat="server" id="Label14" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Price :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtprice" class="form-control" runat="server" AutoPostBack="false"></asp:TextBox>
                                            </div>
                                            <label runat="server" id="lbldesc" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1">
                                                Asset Description :
                                            </label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtdesc" class="form-control" MaxLength="100" TextMode="MultiLine"
                                                    runat="server" placeholder="Description!" AutoPostBack="false"></asp:TextBox>
                                            </div>
                                            <label runat="server" id="Label15" class="col-sm-2 control-label no-padding-right"
                                                for="form-field-1-1">
                                                Active :
                                            </label>
                                            <div class="col-sm-3">
                                                <div class="checkbox">
                                                    <label>
                                                        <asp:CheckBox ID="chkstatus" runat="server" Class="ace" />
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div>
                                            <asp:Button Text="Submit" runat="server" ID="btnManualEdition" OnClick="btnManualEdition_Click"
                                                OnClientClick="javascript:return Validate();" Class="'btn btn-primary" />
                                            <asp:HiddenField ID="hdncatidId" runat="server" />
                                            <asp:HiddenField ID="hidcatcode" runat="server" />
                                        </div>
                                        <asp:Label ID="lblMessage" runat="server" EnableViewState="False" Font-Bold="True"
                                            ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <%-- <span style="font-weight: bold;">Total Records:</span>
                <asp:Label ID="lblcnt" runat="server" Style="font-weight: bold;"></asp:Label>--%>
                            </div>
                        </div>
                        <div class="row">
                            <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                <ContentTemplate>
                                    <%--<telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                                        CellSpacing="0" GridLines="None" CssClass="gvData" OnItemCommand="gv_data_ItemCommand" OnPageIndexChanged="gvData_PageIndexChanged"
                                        OnDataBinding="gvData_DataBinding" OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">--%>
                                    <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                        CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                        OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                        OnItemDataBound="gvData_ItemDataBound" OnItemCommand="gv_data_ItemCommand" OnItemCreated="gvData_ItemCreated">
                                        <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                        <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                                        <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                        <ClientSettings EnablePostBackOnRowClick="false">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px" />
                                            <ClientEvents OnGridCreated="GridCreated" />
                                        </ClientSettings>
                                        <SortingSettings EnableSkinSortStyles="false" />
                                        <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true">
                                            <PagerStyle AlwaysVisible="true" Position="Top" />
                                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <%--                <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                    SortExpression="ID" UniqueName="ID" ReadOnly="true" AllowSorting="false" AllowFiltering="false"
                    Visible="false">
                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn UniqueName="Select" AllowFiltering="false">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cboxSelect" runat="server" onclick="unCheckHeader(this)" />
                                                        <asp:HiddenField ID="hdnAstID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AssetId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="AssetId" FilterControlAltText="Filter AssetId column"
                                                    HeaderText="AssetId" SortExpression="AssetId" UniqueName="AssetId" ReadOnly="true"
                                                    AllowSorting="false" AllowFiltering="true" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridButtonColumn CommandName="dit" ButtonType="ImageButton" UniqueName="Edit"
                                                    ImageUrl="~/images/pencil.png">
                                                </telerik:GridButtonColumn>
                                                <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter ID column"
                                                    HeaderText="ASSETCODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                                                    AllowFiltering="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                                                    HeaderText="DOCUMENT CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SubCategory" FilterControlAltText="Filter SubCategory column"
                                                    HeaderText="SUBCATEGORY" Visible="false" SortExpression="SubCategory" UniqueName="SubCategory"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                                                    HeaderText="LOCATION" SortExpression="Location" UniqueName="Location" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Building" FilterControlAltText="Filter Building column"
                                                    HeaderText="BUILDING" SortExpression="Building" UniqueName="Building" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Floor" FilterControlAltText="Filter Floor column"
                                                    HeaderText="FLOOR" SortExpression="Floor" UniqueName="Floor" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                                                    HeaderText="DEPARTMENT" Visible="false" SortExpression="Department" UniqueName="Department" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Custodian" FilterControlAltText="Filter Custodian column"
                                                    HeaderText="CUSTODIAN" SortExpression="Custodian" UniqueName="Custodian" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SupplierName" FilterControlAltText="Filter SupplierName column"
                                                    HeaderText="SUPPLIER" SortExpression="SupplierName" UniqueName="SupplierName"
                                                    ReadOnly="true" AllowSorting="true" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                                                    HeaderText="SERIAL NO" SortExpression="SerialNo" UniqueName="SerialNo" ReadOnly="true" Visible="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                                    HeaderText="DESCRIPTION" SortExpression="Description" UniqueName="Description" Visible="false"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column"
                                                    HeaderText="QUANTITY" SortExpression="Quantity" UniqueName="Quantity" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                                                    HeaderText="PRICE" SortExpression="Price" UniqueName="Price" ReadOnly="true" Visible="false">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                                                    HeaderText="DELIVERY DATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ASSIGNDATE" FilterControlAltText="Filter AssignDate column"
                                                    HeaderText="ASSIGN DATE" SortExpression="AssignDate" UniqueName="AssignDate" Visible="false" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                                    HeaderText="STATUS" Visible="false" SortExpression="Status" UniqueName="Status" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="TagType" FilterControlAltText="Filter TagType column"
                                                    HeaderText="TAG TYPE" SortExpression="TagType" UniqueName="TagType" ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column1" FilterControlAltText="Filter Column1 column"
                                                    HeaderText="FC Number" SortExpression="Column1" UniqueName="Column1"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column2" FilterControlAltText="Filter Column2 column"
                                                    HeaderText="Case Assignee Name" SortExpression="Column2" UniqueName="Column2"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column3" FilterControlAltText="Filter Column3 column"
                                                    HeaderText="Client Name" SortExpression="Column3" UniqueName="Column3"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column4" FilterControlAltText="Filter Column4 column"
                                                    HeaderText="Document Controller Name" SortExpression="Column4" UniqueName="Column4"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column5" FilterControlAltText="Filter Column5 column"
                                                    HeaderText="Case Manager Full Name" SortExpression="Column5" UniqueName="Column5"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column6" FilterControlAltText="Filter Column6 column"
                                                    HeaderText="Case Manager Email" SortExpression="Column6" UniqueName="Column6"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column7" FilterControlAltText="Filter Column7 column"
                                                    HeaderText="Case Worker 1 Name" SortExpression="Column7" UniqueName="Column7"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="Column8" FilterControlAltText="Filter Column8 column"
                                                    HeaderText="Case Worker 1 Email" SortExpression="Column8" UniqueName="Column8"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column9" FilterControlAltText="Filter Column9 column"
                                                    HeaderText="Case Status" SortExpression="Column9" UniqueName="Column9"
                                                    ReadOnly="true" Visible="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column10" FilterControlAltText="Filter Column10 column"
                                                    HeaderText="Case Person Association" SortExpression="Column10" UniqueName="Column10"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column11" FilterControlAltText="Filter Column11 column"
                                                    HeaderText="Column11" SortExpression="Column11" UniqueName="Column11"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column12" FilterControlAltText="Filter Column12 column"
                                                    HeaderText="Column12" SortExpression="Column12" UniqueName="Column12"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column13" FilterControlAltText="Filter Column13 column"
                                                    HeaderText="Column13" SortExpression="Column13" UniqueName="Column13"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column14" FilterControlAltText="Filter Column14 column"
                                                    HeaderText="Column14" SortExpression="Column14" UniqueName="Column14"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Column15" FilterControlAltText="Filter Column15 column"
                                                    HeaderText="Column15" SortExpression="Column15" UniqueName="Column15"
                                                    ReadOnly="true">
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
            </div>
        </div>
    </div>
    <!-- /.page-content -->

    <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Style="display: none" />
    <ajax:ModalPopupExtender ID="ModalPopupExtender4" runat="server" PopupControlID="Panel22"
        TargetControlID="btnShow" CancelControlID="btnClose">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Panel22" runat="server" align="center" Style="display: none" CssClass="modalPopup"
        Height="132px" Width="250px">
        <%--CssClass="modalPopup" border: 1px solid Gray; --%>
        <table style="width: 100%">
            <tr style="height: 25px;" id="trheader" runat="server">
                <td colspan="1">
                    <label id="Label22" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label></td>
                <td align="right" style="margin-right: 10px;">
                    <asp:Button ID="Button1" runat="server" CssClass="bgimage" />
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
    <asp:HiddenField runat="server" ID="HiddenField3" />
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Asset Master</h5>

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
