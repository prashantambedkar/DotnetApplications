<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true" CodeFile="ViewDocumentRequestt.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ScrollToTop() {
            window.scrollTo(500, 0);
        }
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    ScrollToTop();
                }
            });
        };
    </script>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js?key=AIzaSyD3kNBwtWnxNzyy-JkRepAHIUJaUUNCuUE"></script>
    <link href="css/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function contentHide() {
            $('#ContentPlaceHolder1_divSearch').hide(500);
        };
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
        })

    </script>
    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () {
                var body = $('[Id*=txtName]').val();
                //$("#MyPopup .modal-body").html("Welcome to " + body);
                $.ajax({
                    type: "POST",
                    url: "Home.aspx/SendParameters",
                    data: "{name: '" + $("#txtName").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        $("#MyPopup").modal("show");
                    }
                });
            });
            $("#btnClosePopup").click(function () {
                $("#MyPopup").modal("hide");
            });
        });
    </script>


    <script type="text/javascript">



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

    </script>
    <style type="text/css">
        .loader {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url('loading.gif') 50% 50% no-repeat rgb(249,249,249);
            background-color: transparent;
        }

        .gvData {
            margin-left: auto !important;
            margin-right: auto !important;
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
    </style>
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
    <script type="text/javascript">
        function contentHide() {
            $('#ContentPlaceHolder1_divSearch').hide(500);
        };
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
        })

    </script>
    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () {
                var body = $('[Id*=txtName]').val();
                //$("#MyPopup .modal-body").html("Welcome to " + body);
                $.ajax({
                    type: "POST",
                    url: "Home.aspx/SendParameters",
                    data: "{name: '" + $("#txtName").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        $("#MyPopup").modal("show");
                    }
                });
            });
            $("#btnClosePopup").click(function () {
                $("#MyPopup").modal("hide");
            });
        });
    </script>


    <script type="text/javascript">



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

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="div1" class="main-content-inner" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-6">
                                <span style="font-family: 'Calibri'; font-size: x-large">Document Tagging Approve Request</span>&nbsp;
                            </div>
                            <div class="col-sm-6">
                                <i style="float: inline-end; font-size: 24px" class="fa"></i>
                                &nbsp;&nbsp;&nbsp;&nbsp;<img src="" alt="" style="float: right;" runat="server" id="CompanyImg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="container" id="container2" runat="server" style="width: auto">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" style="width: auto">
                            <ContentTemplate>

                                <%--   <telerik:RadGrid ID="RadGrid1" runat="server" Width="90%" OnNeedDataSource="gvData_NeedDataSource"
                                CellSpacing="0" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init"
                                OnItemCommand="gv_data_ItemCommand">--%>

                                <telerik:RadGrid ID="gvData" runat="server" OnItemDataBound="gvData_ItemDataBound" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" FilterItemStyle-HorizontalAlign="Center" FilterItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                    Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                    OnItemCommand="gv_data_ItemCommand" OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init" OnItemCreated="gvData_ItemCreated">
                                     <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <ItemStyle HorizontalAlign="Center" Wrap="false"></ItemStyle>
                                    <AlternatingItemStyle HorizontalAlign="Center"></AlternatingItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" ForeColor="Black" Wrap="false" Height="22px"></HeaderStyle>
                                    <ClientSettings>
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                    </ClientSettings>
                                    <SortingSettings EnableSkinSortStyles="false" />
                                    <MasterTableView AllowPaging="True" PageSize="250" AutoGenerateColumns="false" AllowSorting="true"
                                        DataKeyNames="CaseID">
                                        <PagerStyle AlwaysVisible="true" Position="Top" />
                                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="SR NO" AllowFiltering="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblserial" Text='<%# Container.ItemIndex + 1%>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="excelid" FilterControlAltText="Filter excelid column"
                                                HeaderText="excelid" SortExpression="excelid" UniqueName="excelid" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="locationId" FilterControlAltText="Filter locationId column"
                                                HeaderText="locationId" SortExpression="locationId" UniqueName="locationId" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="id" FilterControlAltText="Filter id column"
                                                HeaderText="id" SortExpression="id" UniqueName="id" ReadOnly="true"
                                                AllowSorting="false" AllowFiltering="false" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CaseID" FilterControlAltText="Filter CaseID column"
                                                HeaderText="CaseID" SortExpression="CaseID" UniqueName="CaseID" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="CustodianName" FilterControlAltText="Filter CustodianName column"
                                                HeaderText="Custodian Name" SortExpression="CustodianName" UniqueName="CustodianName" ReadOnly="true" AllowSorting="false"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Filter LocationName column"
                                                HeaderText="Location Name" SortExpression="LocationName" UniqueName="LocationName" ReadOnly="true"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="AssigneeName" FilterControlAltText="Filter AssigneeName column"
                                                HeaderText="Assignee Name" SortExpression="AssigneeName" UniqueName="AssigneeName" ReadOnly="true"
                                                AllowFiltering="true" Visible="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="OrganisationName" FilterControlAltText="Filter OrganisationName column"
                                                HeaderText="Organisation Name" SortExpression="OrganisationName" UniqueName="OrganisationName" ReadOnly="true"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="BuildingName" FilterControlAltText="Filter BuildingName column"
                                                HeaderText="Building Name" SortExpression="BuildingName" UniqueName="BuildingName" ReadOnly="true"
                                                AllowFiltering="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="FloorName" FilterControlAltText="Filter FloorName column"
                                                HeaderText="Floor Name" SortExpression="FloorName" UniqueName="FloorName" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter Name column"
                                                HeaderText="Tag Type" SortExpression="Name" UniqueName="Name" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="CreatedDate" FilterControlAltText="Filter CreatedDate column"
                                                HeaderText="CreatedDate" SortExpression="CreatedDate" UniqueName="CreatedDate" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="                           Status          " FilterControlAltText="Filter Status column"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"
                                                HeaderText="Status" SortExpression="Status" UniqueName="Status" ReadOnly="true"
                                                AllowFiltering="true" Visible="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn CommandName="dit" HeaderText="View" ButtonType="ImageButton" UniqueName="Edit"
                                                ImageUrl="~/images/view.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn CommandName="tup" HeaderText="Approve" ButtonType="ImageButton" UniqueName="Approve"
                                                ImageUrl="~/images/thumbsup.png">
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn CommandName="tdn" HeaderText="Reject" ButtonType="ImageButton" UniqueName="Reject"
                                                ImageUrl="~/images/thumbsdown.png">
                                            </telerik:GridButtonColumn>
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
                <div class="panel-body">
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12" style="background-color: white">
                <!-- start top menu -->
                <div class="hidden">
                    <uc1:topmenu runat="server" ID="topmenu" />
                </div>
                <%--end top menu--%>
                <%--start main page--%>
            </div>
            <%--end main page--%>
        </div>
    </div>
    <script type="text/javascript">
        function openModal() {
            $('#myModal').modal('show');
        }

    </script>
    <div class="modal fade" id="myModal1" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Document Request Details</h5>

                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <h5>Request Approved!!</h5>
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
                                <button type="button" class="btn btn-warning" data-dismiss="modal">Ok</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function openModal1() {
            <%--var name =  document.getElementById('<%=lblcat.ClientID%>').innerHTML;
            console.log(name);
            document.getElementById('<%= txtCategoryName.ClientID %>').value = name;--%>
            $('#myModal1').modal('show');
        }

    </script>
</asp:Content>

