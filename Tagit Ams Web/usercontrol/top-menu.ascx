<%@ Control Language="C#" AutoEventWireup="true" CodeFile="top-menu.ascx.cs" Inherits="adminx_usercontrols_top_menu" %>
<asp:Button ID="btnErrorPopup" runat="server" Style="display: none" />
<button data-target="#sidebar2" data-toggle="collapse" type="button" class="pull-left navbar-toggle collapsed">
    <span class="sr-only">Toggle sidebar</span> <span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
</button>
<meta name="viewport" content="width=device-width, initial-scale=1">
<style>
    ul {
        list-style-type: none;
        margin: 0;
        padding: 0;
        overflow: hidden;
        background-color: #595959;
    }

    .rcbList {
        background-color: white;
    }

    li {
        float: left;
    }

        li.cent {
            float: none;
        }

        li a {
            display: block;
            color: white;
            text-align: center;
            padding: 14px 16px;
            text-decoration: none;
        }

            li a:hover {
                background-color: #F5F5F5;
            }

    ul li ul.dropdown {
        /*min-width: 100%;*/ /* Set width of the dropdown */
        background: #595959;
        display: none;
        position: absolute;
        z-index: 999;
        left: 0;
        overflow: hidden;
    }

        ul li ul.dropdown li ul.dropdown1 {
            background: #595959;
            display: none;
            position: fixed;
            z-index: 999;
            left: 0;
        }



    ul li:hover ul.dropdown {
        display: block; /* Display the dropdown */
    }

        ul li:hover ul.dropdown li:hover ul.dropdown1 {
            display: block; /* Display the dropdown */
        }

    ul li ul.dropdown li {
        display: block;
    }

    ul li ul.dropdown1 li {
        display: block;
    }

    .button5 {
        border-radius: 50%;
        background-color: Red;
    }
</style>
<script>
    function myFunction() {
        location.replace("DocumentRequestt.aspx")
    }
</script>
<%--new UI--%>
<div class="panel panel-default">
    <div id="sidebar2" class="panel-heading" style="background-color: white; font-family: Calibri;">
        <ul>
            <li><a href="Home.aspx"><i class="fa fa-home" aria-hidden="true"></i><span
                class="menu-text">&nbsp;Dashboard</span> </a><b class="arrow"></b></li>

            <li>

                <a href="#"><i class="fa fa-database" aria-hidden="true"></i>&nbsp;Master &#9662;</a>
                <ul class="dropdown" style="width: 13%; margin-left: 121px;">

                    <li style="width: 100%;">
                        <a href="#" style="text-align: left"><i class="menu-icon fa fa-caret-right" aria-hidden="true"></i>&nbsp;Company</a>
                        <ul class="dropdown1" style="top: 129px; width: 11%; margin-left: 20%;">
                            <li style="width: 100%;"><a style="text-align: left" href="LocationMaster.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Major Location Master</a></li>

                            <br />
                            <li style="width: 100%;"><a style="text-align: left" href="BuildingMaster.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Minor Location Master</a></li>

                            <br />
                            <li style="width: 100%;"><a style="text-align: left" href="FloorMaster.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Minor Sub Master</a></li>

                            <%--<br />
                            <li style="width: 100%;"><a style="text-align: left" href="DepartmentMaster.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Department Master</a></li>--%>

                            <br />
                            <li style="width: 100%;"><a style="text-align: left" href="CustodianMaster.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Custodian Master</a></li>

                            <%-- <br />
                            <li style="width: 100%;"><a style="text-align: left" href="SuplierMaster.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Supplier Master</a></li>--%>
                        </ul>
                    </li>
                    <br />
                    <br />
                    <li style="width: 100%;">
                        <a href="CategoryMaster.aspx" style="text-align: left"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Category Master</a>
                    </li>

                    <br />
                    <br />
                    <li style="width: 100%;"><a href="test.aspx" style="text-align: left"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Document</a>
                    </li>
                    <br />
                    <br />
                    <li style="width: 100%;"><a href="TestMaster.aspx" style="text-align: left"><i class="menu-icon fa fa-caret-right"></i>&nbsp;User Management</a>
                    </li>
                    <br />
                    <br />
                    <%if (Session["UserType"].ToString() == "1")
                        { %>
                    <li style="width: 100%;"><a href="PdfReportConfig.aspx" style="text-align: left"><i class="menu-icon fa fa-caret-right"></i>&nbsp;PDF Config</a>
                    </li>
                    <br />
                    <br />
                    <%} %>

                    <li style="width: 100%;"><a href="ViewDocumentRequestt.aspx" style="text-align: left"><i class="menu-icon fa fa-caret-right"></i>&nbsp;<%--Approve Document Request--%>Tagging Approve Request</a>
                    </li>
                    <br />
                    <br />
                    <li style="width: 100%;"><a href="SLA.aspx" style="text-align: left"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Location & Category SLA Update</a>
                    </li>


                </ul>
            </li>
            <li><a href="#"><i class="fa fa-tags" aria-hidden="true"></i>&nbsp;Label &#9662;</a>
                <ul class="dropdown" style="margin-left: 218px; width: 13%">
                    <li style="width: 100%;"><a style="text-align: left" href="LabelPrint.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Print</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="LabelReprint.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Reprint</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="LabelConfig.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Label Config</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="EncodeTSB.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Encode</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="AssetsTaging.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Tagging</a>
                    </li>
                </ul>
            </li>
            <li><a href="#"><i class="fa fa-tasks" aria-hidden="true"></i>&nbsp;Transfer &#9662;</a>
                <ul class="dropdown" style="margin-left: 313px; width: 13%;">
                    <li style="width: 100%;"><a style="text-align: left" href="TransferAssets.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Manual Transfer</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="TransferAssetsTSB.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;TSB</a>
                    </li>
                </ul>
            </li>
            <li><a href="#"><i class="fa fa-area-chart" aria-hidden="true"></i>&nbsp;Statistics &#9662;</a>
                <ul class="dropdown" style="margin-left: 421px; width: 13%;">
                    <li style="width: 100%;"><a style="text-align: left" href="Assethistory.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Document Verification Report</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="RAssetMovement.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Document Transfer Report</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="AssetMovement.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Document Movement History</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="REncodedlabels.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Encoded Label Report</a>
                    </li>
                    <br />
                    <%--<li visible="false" style="width: 100%;" runat="server" id="ddWarranty"><a style="text-align: left" href="AMCWarrentyReport.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Warranty/AMC Report</a>
                    </li>
                    <br />
                    <li visible="false" style="width: 100%;" runat="server" id="Li1"><a style="text-align: left" href="AMCWarrentyReport.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Warranty/AMC Report</a>
                    </li>
                    <br />--%>
                    <li style="width: 100%;"><a style="text-align: left" href="RTaggedAssets.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Tagged Document Report</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="REmptyLocation.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Empty Location Report</a>
                    </li>
                    <br />
                    <li style="width: 100%;"><a style="text-align: left" href="topClients.aspx"><i class="menu-icon fa fa-caret-right"></i>&nbsp;Top Clients Report</a>
                    </li>
                </ul>
            </li>

            <li>
                <button type="button" onclick="myFunction();" class="btn btn-primary navbar-btn" style="margin: 3px auto; border-radius: 8px;">
                    Document Tagging Request
                </button>
            </li>
        </ul>
    </div>
</div>
