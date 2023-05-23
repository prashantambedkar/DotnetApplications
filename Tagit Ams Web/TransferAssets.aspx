<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="TransferAssets.aspx.cs" Inherits="TransferAssets" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart {
            display: none;
        }
    </style>
    <%-- <script type="text/javascript">
        var x = document.getElementById('<%=txtloc.ClientID %>');
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            }
            else { x.innerHTML = "Geolocation is not supported by this browser."; }
        }
        function showPosition(position) {
            var latlondata = position.coords.latitude + "," + position.coords.longitude;
            //var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:="  +position.coords.longitude;  
            var latlon = position.coords.latitude + "," + position.coords.longitude;
            alert(latlon)
            //document.getElementById("mapholder").innerHTML = latlon;
            document.getElementById('<%=txtloc.ClientID %>').value = latlon;
            document.getElementById('<%=txtloc.ClientID%>').style.display = "none";
        }
        function showError(error) {
            if (error.code == 1) {
                x.innerHTML = "User denied the request for Geolocation."
            }
            else if (err.code == 2) {
                x.innerHTML = "Location information is unavailable."
            }
            else if (err.code == 3) {
                x.innerHTML = "The request to get user location timed out."
            }
            else {
                x.innerHTML = "An unknown error occurred."
            }
        }
    </script>--%>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script type="text/javascript">
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (p) {
                var cur_loc = (p.coords.latitude + "," + p.coords.longitude);

                var indiagate = ("28.6109" + "," + "77.2344");//cordinates if India Gate
                var com_cord = indiagate + ":" + cur_loc;
                var indcord = com_cord.split(":");
                for (var i = 0; i < indcord.length; i++) {
                    var ncord_cord = (indcord[i]);
                    var split_fincord = ncord_cord.split(",");
                    var finalcordinates = "(" + parseFloat(split_fincord[0]) + "," + parseFloat(split_fincord[1]) + ")";
                    var data = indcord[i];
                    var LatLng = new google.maps.LatLng(parseFloat(split_fincord[0]), parseFloat(split_fincord[1]));
                    var mapOptions = {
                        center: LatLng,
                        zoom: 13,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };



                }
                //alert(cur_loc);
              
                //document.getElementById('txtloc').value = cur_loc;
                setCookie('locationparam', cur_loc);
                // alert(getCookie('locationparam'));
                var newlocstring = com_cord.replace(":", ",");

                var new_pts = newlocstring.split(",");

                var d = getDistanceFromLatLonInKm(new_pts[0], new_pts[1], new_pts[2], new_pts[3]);

               <%-- document.getElementById('<%=lbl1.ClientID%>').textContent = d.toFixed(2) + "Km";--%>
                calcRoute(new_pts[0], new_pts[1], new_pts[2], new_pts[3]);
            });



        } else {
            alert('Geo Location feature is not supported in this browser.');
        }
        function setCookie(name, value) {
            var expiry = new Date();
            expiry.setMinutes(expiry.getMinutes() + 15);
            document.cookie = name + "=" + (value) + "; path=/; expires=" + expiry.toGMTString();
        }
        function getCookie(name) {
            let cookie = {};
            document.cookie.split(';').forEach(function(el) {
                let [k,v] = el.split('=');
                cookie[k.trim()] = v;
            })
            return cookie[name];
        }
        function getDistanceFromLatLonInKm(lat1, lon1, lat2, lon2) {

            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(parseFloat(lat2) - parseFloat(lat1));  // deg2rad below

            var dLon = deg2rad(parseFloat(lon2) - parseFloat(lon1));

            var a =
              Math.sin(dLat / 2) * Math.sin(dLat / 2) +
              Math.cos(deg2rad(parseFloat(lat1))) * Math.cos(deg2rad(parseFloat(lat2))) *
              Math.sin(dLon / 2) * Math.sin(dLon / 2);

            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        function deg2rad(deg) {
            return deg * (Math.PI / 180)
        }

        function calcRoute(lat1, lon1, lat2, lon2) {
            var start = new google.maps.LatLng(parseFloat((lat1)), parseFloat(lon1));


            var end = new google.maps.LatLng(parseFloat(lat2), parseFloat(lon2));

            var request = {
                origin: start,
                destination: end,
                travelMode: google.maps.TravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {

                if (status == google.maps.DirectionsStatus.OK) {

                    directionsDisplay.setDirections(response);

                }

            });
        };

    </script>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="css/Site.css" />
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
                //var hidden = document.getElementById('#ContentPlaceHolder1_HiddenField3');
                var checkBox = document.getElementById(document.getElementById("<%=HiddenField3.ClientID%>").value);
                checkBox.checked = false;
            }
        }
    </script>
    <script type="text/javascript">
        var mouseOverActiveElement = false;

        $(document).ready(function () {


            $('#ContentPlaceHolder1_txtSearch').click(function () {


                if ($('#ContentPlaceHolder1_txtSearch').val().length > 1) {
                    $('#ContentPlaceHolder1_divSearch').hide(500);
                } else {
                    $('#ContentPlaceHolder1_divSearch').show(500);
                }
            })
            //            $('#ContentPlaceHolder1_btnRefresh').click(function () {

            //                $('#ContentPlaceHolder1_ddlproCategory').val(0);
            //                $('#ContentPlaceHolder1_txtAssetCode').val('');
            //                $('#ContentPlaceHolder1_ddlsubcat').val('0');

            //                $('#ContentPlaceHolder1_ddlloc').val('0');
            //                $('#ContentPlaceHolder1_ddlbuild').val('0');
            //                $('#ContentPlaceHolder1_ddlfloor').val('0');
            //                $('#ContentPlaceHolder1_ddldept').val('0');
            //                $('#ContentPlaceHolder1_ddlCustodian').val('0');
            //                
            //                $('#ContentPlaceHolder1_divSearch').show();

            //            });
            $('#ContentPlaceHolder1_GetGrid').click(function () {
                if ($('#ContentPlaceHolder1_txtSearch').val().length == 0) {
                    alert('Enter some text to search');
                    return false;
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

            $('#divSearchClose').click(function () {
                $('#ContentPlaceHolder1_divSearch').hide(500);
            });


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

        var lat = document.getElementById("<%=HdnLatValue.ClientID%>");
        var logn = document.getElementById("<%=HdnLogValue.ClientID%>");
        var x = document.getElementById("<%=HdnError.ClientID %>");
        getLocation();

        function getLocation() {

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError, { maximumAge: 600000, timeout: 30000, enableHighAccuracy: false });
            }
            else { x.value = "Geolocation is not supported by this browser."; }
        }

        function showPosition(position) {
            //debugger;
            var latlondata = position.coords.latitude + "," + position.coords.longitude;
            var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:=" + position.coords.longitude;

            document.getElementById("<%=HdnLatValue.ClientID%>").value = position.coords.latitude;
            document.getElementById("<%=HdnLogValue.ClientID%>").value = position.coords.longitude;
            //alert(latlon);
        }

        function showError(error) {
            if (error.code == 1) {
                x.value = "User denied the request for Geolocation."
            }
            else if (err.code == 2) {
                x.value = "Location information is unavailable."
            }
            else if (err.code == 3) {
                x.value = "The request to get user location timed out."
            }
            else {
                x.value = "An unknown error occurred."
            }
        }

    </script>
    <%--    <style type="text/css">
        div.RadGrid .rgPager .rgAdvPart
        {
            display: none;
        }
    </style>--%>
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

        .focus {
            background-color: Red !important;
            color: White !important;
            text-decoration: blink;
        }

        .targetButton {
            border-color: Green !important;
            color: White !important;
            text-decoration: blink;
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
        //var STab = document.getElementById('#ContentPlaceHolder1_HdnTab');
        var ActiveTab = "";
        $(document).ready(function () {
            ActiveTab = document.getElementById('<%=HdnTab.ClientID %>').value;
            if (ActiveTab == "ByCustodian") {
                $('#ContentPlaceHolder1_divbyCustodian').show(500);
                $('#ContentPlaceHolder1_divbyLocation').hide(500);
                $('#BtnByCustodian').addClass("targetButton");
                $('#BtnByLocation').removeClass("targetButton");
            }
            else if (ActiveTab == "ByLocation") {
                $('#ContentPlaceHolder1_divbyCustodian').hide(500);
                $('#ContentPlaceHolder1_divbyLocation').show(500);
                $('#BtnByCustodian').removeClass("targetButton");
                $('#BtnByLocation').addClass("targetButton");
            } else {
                $('#ContentPlaceHolder1_divbyCustodian').hide(500);
                $('#ContentPlaceHolder1_divbyLocation').hide(500);
                $('#BtnByCustodian').removeClass("targetButton");
                $('#BtnByLocation').removeClass("targetButton");
            }

            $('#divCloseCustodian').click(function () {
                $('#ContentPlaceHolder1_divbyCustodian').hide(500);
                document.getElementById('<%=HdnTab.ClientID %>').value = "";
                $('#BtnByCustodian').removeClass("targetButton");
            });

            $('#divCloseLocation').click(function () {
                $('#ContentPlaceHolder1_divbyLocation').hide(500);
                //ActiveTab = "";
                document.getElementById('<%=HdnTab.ClientID %>').value = "";
                $('#BtnByLocation').removeClass("targetButton");
            });

            $('#BtnByLocation').click(function () {
                $('#ContentPlaceHolder1_divbyLocation').show(500);
                $('#ContentPlaceHolder1_divbyCustodian').hide(500);
                document.getElementById('<%=HdnTab.ClientID %>').value = "ByLocation";
            });

            $('#BtnByCustodian').click(function () {
                $('#ContentPlaceHolder1_divbyCustodian').show(500);
                $('#ContentPlaceHolder1_divbyLocation').hide(500);
                document.getElementById('<%=HdnTab.ClientID %>').value = "ByCustodian";
            });

            $('#txtSearch').click(function () {
                $('#ContentPlaceHolder1_divSearch').show(500);
            });

            $('#divSearchClose').click(function () {
                $('#ContentPlaceHolder1_divSearch').hide(500);
            });

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
        $(document).ready(function () {

            $('#BtnByLocation').click(function () {
                $(this).addClass("targetButton");
                $('#BtnByCustodian').removeClass("targetButton");
            });
            $('#BtnByCustodian').click(function () {
                $(this).addClass("targetButton");
                $('#BtnByLocation').removeClass("targetButton");
            });
        });

    </script>
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
            <%--<p id="demo">Click the button to get your position:</p>
            <asp:Button Text="fetchgps" runat="server" ID="fetchgpsbtn" Height="37px" OnClick="fetchgpsbtn_Click"
                CssClass="btn btn-primary" Style="font-size: 12px" />
            <button onclick="getLocation()">Get your Location</button>
            <div id="mapholder"></div>--%>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row">
                        <div class="form-inline">
                            <div style="padding-left: 10px" class="input-group input-group-unstyled">
                                <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="GetGrid" Style="height: 34px" runat="server" OnClick="GetGrid_Click"
                                        class="btn btn-default" type="button">
                                         <i class="fa fa-search"></i></asp:LinkButton></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div id="divSearch" runat="server" style="width: 100%;" class="panel-group col-md-9 form_wrapper">
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
                                                        <asp:Label runat="server" ID="lblcattype"> <%#Assets %> Code</asp:Label>
                                                        <asp:TextBox ID="txtAssetCode" runat="server" class="form-control"
                                                            placeholder="Enter the value" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label9"> <%#Category %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlproCategory" OnSelectedIndexChanged="OnSelectedIndexChangedCategory"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>

                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label11"> <%#Location %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlloc" OnSelectedIndexChanged="OnSelectedIndexChangedLocation"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label12"> <%#Building %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlbuild" OnSelectedIndexChanged="OnSelectedIndexChangedBuilding"
                                                                    AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="row" style="width: 100%">
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label13"><%#Floor %></asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlfloor" AutoPostBack="true" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-0">
                                                        <asp:Label runat="server" Visible="false" ID="Label14">Department</asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel6" Visible="false" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddldept" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:Label runat="server" ID="Label15">Custodian</asp:Label>
                                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList runat="server" ID="ddlCustodian" class="form-control">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label17">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="SEARCH" runat="server"
                                                            ID="btnsearchsubmit" OnClick="btnsubmit_Click" class="btn btn-primary form-control" />
                                                    </div>
                                                    <div class="col-sm-1">
                                                        <asp:Label runat="server" ID="Label18">&nbsp;</asp:Label><br />
                                                        <asp:Button Text="CLEAR" runat="server"
                                                            ID="btnRefresh" class="btn btn-danger form-control" OnClick="btnClearCustodianTransfer_Click" />
                                                    </div>
                                                </div>
                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /.page-header -->
                            <div class="row">
                                <div class="col-xs-12">
                                    <!-- start top menu -->
                                    <div class="hidden">
                                        <uc1:topmenu runat="server" ID="topmenu" />
                                    </div>
                                </div>
                            </div>
                            <!-- /.col -->
                        </div>
                    </div>
                    <div class="page-header">
                        <h1 style="font-family: 'Calibri'; font-size: x-large; color: black;">
                            <%#Assets %> Transfer</h1>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <input type="button" value="TRANSFER BY LOCATION" id="BtnByLocation"
                                class="btn btn-primary form-control" />
                            <%--OnClick="BtnByLocation_Click"--%>
                            <%-- <asp:Button ID="Button2" runat="server" Text="fetch" OnClick="Button2_Click" />--%>
                        </div>
                        <div class="col-sm-3">
                            <input type="button" value="TRANSFER BY CUSTODIAN" id="BtnByCustodian" height="37px"
                                class="btn btn-primary form-control" />
                            <%--OnClick="BtnByCustodian_Click"--%>
                        </div>
                        <%--<div class="col-sm-8">
                            <p id="demo">Click the button to get your position:</p>
                            <asp:Button runat="server" ID="BtnGetLocation" Text="Get Location" OnClick="BtnGetLocation_Click" />
                            <div id="mapholder"></div>
                        </div>--%>
                    </div>
                    <br />
                    <div id="divbyCustodian" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="form-inline">
                                        <div class="col-sm-2" style="padding-left: 10px" class="input-group input-group-unstyled">
                                            Transfer By Custodian
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divCloseCustodian" href="#"><span id="span1" style="top: 0px;"
                                                class="badge"><b>Close</b></span></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="row" style="width: 100%">
                                            <div class="col-sm-3">
                                                <asp:Label runat="server" ID="Label1"> From Custodian :</asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="cboFromCustodian" AutoPostBack="true" class="form-control"
                                                            OnSelectedIndexChanged="cboFromCustodian_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Label runat="server" ID="Label19">To Custodian :</asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="cboToCustodian" AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Label runat="server" ID="Label5">Reason :</asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtReasonCusChange" MaxLength="100" class="form-control" runat="server"
                                                            TextMode="MultiLine" placeholder="Enter the value" AutoPostBack="false"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label runat="server" ID="Label20">&nbsp;</asp:Label><br />
                                                <asp:Button Text="TRANSFER" runat="server" ID="btnCustodianTransfer"
                                                    CssClass="btn btn-primary form-control" OnClick="btnCustodianTransfer_Click" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label runat="server" ID="Label21">&nbsp;</asp:Label><br />
                                                <asp:Button Text="CLEAR" runat="server" ID="btnClearCustodianTransfer"
                                                    class="btn btn-danger form-control" OnClick="btnClearCustodianTransfer_Click" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="divbyLocation" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <div class="row">
                                    <div class="form-inline">
                                        <div class="col-sm-2" style="padding-left: 10px" class="input-group input-group-unstyled">
                                            Transfer By Location
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divCloseLocation" href="#"><span id="span2" style="top: 0px;"
                                                class="badge"><b>Close</b></span></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="row" style="width: 100%">
                                            <div class="col-sm-3">
                                                <asp:Label runat="server" ID="Label7">To <%#Location %> :</asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="cboLoc" AutoPostBack="true" class="form-control"
                                                            OnSelectedIndexChanged="OnSelectedIndexChangedToLocation">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Label runat="server" ID="Label3"> To <%#Building %> :</asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="cboBuild" AutoPostBack="true" class="form-control"
                                                            OnSelectedIndexChanged="OnSelectedIndexChangedToBuilding">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Label runat="server" ID="Label22"> To <%#Floor %> :</asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList runat="server" ID="cboFloor" AutoPostBack="true" class="form-control">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-3">
                                                <asp:Label runat="server" ID="Label2">Reason :</asp:Label>
                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtReasonLocChange" MaxLength="100" class="form-control" runat="server"
                                                            TextMode="MultiLine" placeholder="Enter the value" AutoPostBack="false"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Label runat="server" ID="Label4">&nbsp;</asp:Label><br />
                                                <asp:Button Text="TRANSFER" runat="server" ID="Button1" Height="37px" OnClick="Button1_Click"
                                                    CssClass="btn btn-primary form-control" />
                                            </div>
                                        </div>




                                        <div class="col-sm-1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-- <asp:TextBox runat="server" ID="txtloca"></asp:TextBox>
                    <input type="text" id="txtloc" runat="server" />--%>
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvData" runat="server" Width="100%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" FilterMenu-Width="100%" FilterItemStyle-HorizontalAlign="Center" Skin="Telerik" GridLines="None" AllowFilteringByColumn="true" CssClass="gvData"
                                    OnPageIndexChanged="gvData_PageIndexChanged" BorderWidth="1" OnInit="gvData_Init" OnDataBinding="gvData_DataBinding"
                                    OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">
                                     <GroupingSettings CaseSensitive="false"></GroupingSettings>
                                    <%-- <telerik:RadGrid ID="gvData" runat="server" Width="98%" OnNeedDataSource="gvData_NeedDataSource"
                                    CellSpacing="0" GridLines="None" CssClass="gvData" OnDataBinding="gvData_DataBinding" OnItemDataBound="gvData_ItemDataBound" OnItemCreated="gvData_ItemCreated">--%>
                                    <%----%>
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
                                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Select">
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
                                            <telerik:GridBoundColumn DataField="AssetCode" FilterControlAltText="Filter ID column"
                                                HeaderText="ASSETCODE" SortExpression="AssetCode" UniqueName="AssetCode" ReadOnly="true"
                                                AllowSorting="true" AllowFiltering="false">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Category" FilterControlAltText="Filter Category column"
                                                HeaderText="CATEGORY" SortExpression="Category" UniqueName="Category" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SubCategory" FilterControlAltText="Filter SubCategory column"
                                                HeaderText="SUBCATEGORY" SortExpression="SubCategory" Visible="false" UniqueName="SubCategory"
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
                                            <telerik:GridBoundColumn DataField="Custodian" FilterControlAltText="Filter Custodian column"
                                                HeaderText="CUSTODIAN" SortExpression="Custodian" UniqueName="Custodian" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Department" FilterControlAltText="Filter Department column"
                                                HeaderText="DEPARTMENT" SortExpression="Department" Visible="false" UniqueName="Department" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DeliveryDate" FilterControlAltText="Filter DeliveryDate column"
                                                HeaderText="DELIVERY DATE" SortExpression="DeliveryDate" UniqueName="DeliveryDate"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SerialNo" FilterControlAltText="Filter SerialNo column"
                                                HeaderText="SERIAL NO" SortExpression="SerialNo" Visible="false" UniqueName="SerialNo" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                                HeaderText="DESCRIPTION" Visible="false" SortExpression="Description" UniqueName="Description"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Price" FilterControlAltText="Filter Price column"
                                                HeaderText="PRICE" Visible="false" SortExpression="Price" UniqueName="Price" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Status" FilterControlAltText="Filter Status column"
                                                HeaderText="STATUS" SortExpression="Status" UniqueName="Status" ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column1" FilterControlAltText="Filter Column1 column"
                                                HeaderText="Column1" SortExpression="Column1" UniqueName="Column1"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column2" FilterControlAltText="Filter Column2 column"
                                                HeaderText="Column2" SortExpression="Column2" UniqueName="Column2"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column3" FilterControlAltText="Filter Column3 column"
                                                HeaderText="Column3" SortExpression="Column3" UniqueName="Column3"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column4" FilterControlAltText="Filter Column4 column"
                                                HeaderText="Column4" SortExpression="Column4" UniqueName="Column4"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column5" FilterControlAltText="Filter Column5 column"
                                                HeaderText="Column5" SortExpression="Column5" UniqueName="Column5"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column6" FilterControlAltText="Filter Column6 column"
                                                HeaderText="Column6" SortExpression="Column6" UniqueName="Column6"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column7" FilterControlAltText="Filter Column7 column"
                                                HeaderText="Column7" SortExpression="Column7" UniqueName="Column7"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>

                                            <telerik:GridBoundColumn DataField="Column8" FilterControlAltText="Filter Column8 column"
                                                HeaderText="Column8" SortExpression="Column8" UniqueName="Column8"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column9" FilterControlAltText="Filter Column9 column"
                                                HeaderText="Column9" SortExpression="Column9" UniqueName="Column9"
                                                ReadOnly="true">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Column10" FilterControlAltText="Filter Column10 column"
                                                HeaderText="Column10" SortExpression="Column10" UniqueName="Column10"
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
            <div class="page-header">
            </div>
        </div>
    </div>
    <!-- /.page-content -->
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
                    <label id="Label8" style="font-size: large" runat="server">&nbsp;Tagit&nbsp;<%#_Ams %></label></td>
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
    <asp:HiddenField runat="server" ID="HdnLatValue" />
    <asp:HiddenField runat="server" ID="HdnLogValue" />
    <asp:HiddenField runat="server" ID="HiddenField3" />
    <asp:HiddenField runat="server" ID="HdnLocation" />
    <asp:HiddenField runat="server" ID="HdnLastLocation" />
    <asp:HiddenField runat="server" ID="HdnError" />
    <asp:HiddenField runat="server" ID="HdnTab" Value="" />
    <script type="text/javascript">
        function openModal() {
            $('#myModal').modal('show');
        }
        function setvalueforlocation(lbllocname) {
            document.getElementById('lbllocname').innerHTML = lbllocname;
        }
    </script>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Transfer Assets</h5>

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

                            <div class="col-md-12" style="align-content: center;">
                                <%-- <asp:Button Text="No" runat="server" ID="Button2" Style="height: 100%; font-size: 16px; border-radius: 5px; padding: 0; width: 100%;"
                                    class="btn btn-warning" />--%>
                                <button type="button" class="btn btn-warning" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
