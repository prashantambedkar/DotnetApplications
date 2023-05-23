<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login Page -Tagit </title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans:400,300" />
    <link href="css/ace.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
    <script type="text/javascript">
        function HideModalPopup() {
            $find("mpe").hide();
            return false;
        }
        function showpopup() {
            $find("mpe").show(500);
        }
    </script>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .modalBackgroundN {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        .modalPopupN {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
            height: 65%;
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

    <%--    <style>
       
 div.image { 

  background:url("images/logo_ams.png")  no-repeat ;
  opacity: 0.1;
  position: absolute;
  width: 100%;
  height: 100%;
  background-size: cover;
  background-position: center top;
  
 
}
    </style>--%>
</head>

<body style="background-color: white">

    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="script1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
        </ajax:ToolkitScriptManager>
        <%-- <div class="image responsive"></div>--%>
        <div class="login-layout light-login" style="background-color: #fcfefd">
            <div class="main-container">
                <div class="main-content">
                    <div class="row">
                        <div class="col-sm-10 col-sm-offset-1">
                            <div class="login-container">
                                <div class="space-8"></div>
                                <div class="space-8"></div>
                                <div class="space-8"></div>
                                <div class="center">
                                    <h1>
                                        <img width="80%" src="images/logo_ams.png" />
                                    </h1>

                                </div>

                                <div class="space-8"></div>


                                <div>
                                    <div id="login-box" class="login-box visible widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main" style="background-color: white">
                                                <div class="space-32"></div>
                                                <h4 class="header blue lighter bigger text-center">
                                                    <%--<i class="ace-icon fa fa-coffee green"></i>--%>
                                                    <b>Welcome to Tagit DMS</b>
                                                </h4>

                                                <div class="space-6"></div>

                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:TextBox ID="txtusername" class="form-control" Height="35" placeholder="Username" runat="server" />
                                                        <i class="ace-icon fa fa-user"></i>
                                                    </span>
                                                </label>

                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:TextBox ID="txtpassword" TextMode="Password" Height="35" class="form-control" placeholder="Password" runat="server" />
                                                        <i class="ace-icon fa fa-lock"></i>
                                                    </span>
                                                </label>
                                                <asp:Label ID="lblloginerror" runat="server" Font-Bold="True"
                                                    ForeColor="Red"></asp:Label>

                                                <div class="clearfix">
                                                    <label class="checkbox-inline hidden">
                                                        <asp:CheckBox ID="chkremeberme" runat="server" AutoPostBack="true" />
                                                        <span class="lbl">Remember Me</span>
                                                    </label>
                                                    <label class="inline pull-right"><a href="#" onclick="showpopup()">Forgot Password ?</a></label>

                                                </div>
                                                <div class="clearfix">
                                                    <asp:Button ID="Button1" Height="35" Text="Login" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnlogin_Click" />
                                                </div>
                                                <div class="space-32"></div>


                                                <%--<div class="social-or-login center">
                                                    <span class="bigger-110">Connect With Us</span>
                                                </div>

                                                <div class="space-6"></div>

                                                <div class="social-login center">
                                                    <h5><a href="http://tagitglobal.com/" target="_blank">TAGIT RFID SOLUTIONS</a></h5>
                                                </div>--%>
                                            </div>
                                            <!-- /.widget-main -->


                                        </div>
                                        <!-- /.widget-body -->
                                    </div>
                                    <!-- /.login-box -->

                                    <div id="forgot-box" class="forgot-box widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main">
                                                <h4 class="header red lighter bigger">
                                                    <i class="ace-icon fa fa-key"></i>
                                                    Retrieve Password
                                                </h4>

                                                <div class="space-6"></div>
                                                <p>
                                                    Enter your email and to receive instructions
                                                </p>

                                                <div>

                                                    <label class="block clearfix">
                                                        <span class="block input-icon input-icon-right">
                                                            <asp:TextBox class="form-control" placeholder="Email" ID="txtemail" runat="server" />
                                                            <i class="ace-icon fa fa-envelope"></i>
                                                        </span>
                                                    </label>

                                                    <div class="clearfix">
                                                        <%--<i class="ace-icon fa fa-lightbulb-o"></i>--%>
                                                        <asp:Button Text="Send" runat="server" ID="btnsend" CssClass="width-35 pull-right btn btn-sm btn-danger" />
                                                        <asp:Label ID="Label1" runat="server" Font-Bold="True"
                                                            ForeColor="Red"></asp:Label>
                                                    </div>

                                                </div>
                                            </div>
                                            <!-- /.widget-main -->

                                            <div class="toolbar center">
                                                <a href="#" data-target="#login-box" class="back-to-login-link">Back to login
												<i class="ace-icon fa fa-arrow-right"></i>
                                                </a>
                                                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                            </div>

                                        </div>
                                        <!-- /.widget-body -->
                                    </div>
                                    <!-- /.forgot-box -->

                                </div>
                                <div class="space-4"></div>
                                <div class="center">
                                    <%--<h4 class="blue" id="H1">&copy; Tagit </h4>--%>

                                    <label style="text-align: center; font-size: small; color: dimgray">
                                        <b>© 2022 Tagit RFID Solution DMCC. All rights reserved.</b>
                                    </label>
                                </div>


                            </div>
                        </div>
                        <!-- /.col -->
                    </div>
                    <!-- /.row -->
                </div>
                <!-- /.main-content -->
            </div>
        </div>
        <ajax:ModalPopupExtender ID="DetailsPopup" runat="server" TargetControlID="btnShowPopup"
            PopupControlID="pnlpopup" BackgroundCssClass="modalBackgroundN" BehaviorID="mpe">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlpopup" runat="server" CssClass="panel panel-default">
            <div class="panel-heading">
                <h4>Password will be send to your registered Email ID</h4>
            </div>
            <div class="panel-body">

                <div class="row center">
                    <div class="space-16"></div>
                    <div class="col-sm-10" style="margin-left: 35px; margin-right: 30px">
                        <label class="block">
                            <span class="block input-icon input-icon-right">
                                <asp:TextBox ID="txtUser" CssClass="form-control" Height="35" placeholder="Username" runat="server" />
                                <i class="ace-icon fa fa-user"></i>
                            </span>
                        </label>
                    </div>
                </div>
                <div class="row center">
                    <div class="col-sm-10" style="margin-left: 35px; margin-right: 30px">
                        <label class="block">
                            <span class="block input-icon input-icon-right">
                                <asp:TextBox ID="txtEmailId" CssClass="form-control" Height="35" placeholder="Email" runat="server" />
                                <i class="ace-icon fa fa-envelope"></i>
                            </span>
                        </label>


                    </div>

                </div>
                <div class="row center">
                    <div class="col-sm-12">
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="row center">
                    <div class="col-sm-12">
                        <asp:Button Text="Cancel" runat="server" ID="BtnClose" OnClick="BtnClose_Click"
                            CssClass="btn btn-danger" Height="35" Width="100" />
                        <asp:Button Text="Send" runat="server" ID="btnConfirm" OnClick="btnConfirm_Click"
                            CssClass="btn btn-primary" Height="35" Width="100" CausesValidation="false" />
                    </div>
                </div>

                <div class="space-16"></div>
            </div>

        </asp:Panel>
        <script src="js/jquery.min.js"></script>
        <script type="text/javascript">
            window.jQuery || document.write("<script src='js/jquery.min.js'>" + "<" + "/script>");
        </script>
        <script type="text/javascript">
            if ('ontouchstart' in document.documentElement) document.write("<script src='js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
        </script>
        <!-- inline scripts related to this page -->
        <script type="text/javascript">
            jQuery(function ($) {
                $(document).on('click', '.toolbar a[data-target]', function (e) {
                    e.preventDefault();
                    var target = $(this).data('target');
                    $('.widget-box.visible').removeClass('visible'); //hide others
                    $(target).addClass('visible'); //show target
                });
            });

        </script>

    </form>
</body>
</html>
