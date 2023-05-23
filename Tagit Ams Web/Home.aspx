<%@ Page Title="" Language="C#" MasterPageFile="~/adminMasterPage.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="adminx_demodashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Src="~/usercontrol/top-menu.ascx" TagPrefix="uc1" TagName="topmenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js?key=AIzaSyD3kNBwtWnxNzyy-JkRepAHIUJaUUNCuUE"></script>
    <link href="css/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function setvaluenpass(locname, type) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/setvalueofmonthdata',
                data: 'locname:' + locname,
                success: function (response) {
                    window.location.href = "Asset.aspx";

                },

                error: function () {

                }
            });
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

        .googleChartTitle {
            font: bold 11px Arial;
            text-align: center;
            position: absolute;
            width: 100%;
            padding-top: 8px;
        }

        g text {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['bar'] });
        google.charts.load("current", { packages: ["corechart"] });
        $(function () {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/LoadTop10Charts',
                //url: 'Home.aspx/GetBarchartData',
                data: '{}',
                success: function (response) {

                    var dataArray = [['clientName', 'countClientData']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.clientName, item.countClientData]);
                    });


                    google.charts.setOnLoadCallback(drawChart);
                    var chart, data;
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            // title: 'Top 10 Client Stock',
                            //chartArea: { width: 350, height: 300 },
                            chartArea: { width: '50%' },
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            },
                            colors: ['#008000'],
                            //hAxis: {
                            //    title: 'Total Count',
                            //    minValue: 0
                            //},

                            //pieHole: 0,

                            //sliceVisibilityThreshold: .0,
                            ////is3D: true,
                            //legend: 'none',
                            //bar: { groupWidth: '95%' },
                            //vAxis: { gridlines: { count: 10 }, width: '55px' },


                            //hAxis: {

                            //    //slantedText: true
                            //}
                        };


                        if (dataArray.length > 1) {
                            chart = new google.visualization.BarChart(document.getElementById('chart_div4'));
                            // chart.draw(data, google.charts.Bar.convertOptions(options));

                            chart.draw(data, options);

                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelectedtop10);
                        } else { $('#lblmsg4').removeClass("hidden"); }
                    }
                    //--------------------
                    function onAreaSliceSelectedtop10() {
                        console.log('hi');
                        console.log(data.getValue(chart.getSelection()[0].row, 0));
                        console.log('------------');
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({
                            type: "POST",
                            url: "Home.aspx/Dashboard_Filtered_Location",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: "Home.aspx/Dashboard_Filtered_Location",
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////


                            success: function (response) {
                                // alert("Successs");
                                //console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...top10........");
                    //console.log(response);
                    $('#divLoader').hide(500);
                }
            });//Top 10 Client Stock graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetGeoLocationWiseStockV2',
                //data: JSON.stringify({ UserId: Session["userid"], Location: LocationID, Building: BuildingID, Type: Stock, FloorId: FloorId, Column1: Column1FCNumber, Column2: Column2AssigneeName, Column3: Column3ClientName, CustodianId: CustodianId }),////
                data: {},
                success: function (response) {
                    // console.log(response);
                    var dataArray = [['Country', 'Stock']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.LocationName, item.Stock]);
                    });
                    google.charts.setOnLoadCallback(drawChart);
                    var chart, data;
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            // title: 'Analytics Based on Custodian & Location Based Report                    \n',
                            chartArea: { width: 300, height: 250 },
                            pieHole: 0,
                            sliceVisibilityThreshold: .0,
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            }
                            //is3D: true
                        };

                        if (dataArray.length > 1) {
                            chart = new google.visualization.PieChart(document.getElementById('chart_geo'));
                            chart.draw(data, options);
                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelected);
                            //window.stop();
                        } else { $('#lblmsg2').removeClass("hidden"); }
                    };

                    function onAreaSliceSelected() {
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({
                            type: "POST",
                            url: "Home.aspx/Dashboard_Filtered_LocationV2",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",


                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: 'Home.aspx/Dashboard_Filtered_LocationV2',
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////


                            success: function (response) {
                                // alert("Successs");
                                //console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Major Location Wise Stock graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetBarchartDataV2_',
                data: '{}',
                success: function (response) {

                    var dataArray = [['StockDate', 'Found', 'Missing', 'Mismatch', 'Extra']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.StockDate, item.Found, item.Missing, item.MissMatch, item.Extra]);
                    });

                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                        var data = google.visualization.arrayToDataTable(dataArray);

                        var options = {
                            chart: {
                                //title: 'Stock Check',
                                //title: 'Stock Check',
                                legend: {
                                    position: 'bottom', textStyle: { fontSize: 9 },
                                    width: '100%', maxLines: 3
                                }
                            },

                        };

                        if (dataArray.length > 1) {
                            var chart = new google.charts.Bar(document.getElementById('chart_divstock'));
                            chart.draw(data, options);
                        } else { $('#lblmsg1').removeClass("hidden"); }



                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Date Wise Stock Check Details graph        
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetTagVsPrintV2_',
                //data: JSON.stringify({ Location: null }),
                data: {},
                success: function (response) {

                    var dataArray = [['Description', 'Count', { role: 'style' }]];

                    $.each(response.d, function (i, item) {
                        var color = "";
                        if (i == 0) {
                            color = '#b87333';
                        }
                        if (i == 1) {
                            color = 'gold';
                        }
                        dataArray.push([item.StringColumn, item.dataCount, color]);
                    });
                    //                    if (dataArray.length > 0) {
                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                        var data = google.visualization.arrayToDataTable(dataArray);
                        var view = new google.visualization.DataView(data);
                        view.setColumns([0, 1,
                           {
                               calc: "stringify",
                               sourceColumn: 1,
                               type: "string",
                               role: "annotation"
                           },
                           2]);
                        var options = {
                            //title: "Printed Vs Tagged",
                            width: "80%",
                            height: "100%",
                            bar: { groupWidth: "95%" },
                            legend: { position: "none", maxLines: 3 },
                            vAxis: { minValue: 0 }
                        };

                        if (dataArray.length > 1) {
                            var chart = new google.visualization.ColumnChart(document.getElementById('chart_tagged'));
                            chart.draw(data, options);
                        } { $('#lblmsg3').removeClass("hidden"); }
                        $('#divLoader').hide(500);
                    };


                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Printed Vs Tagged graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetTodayHealthAnalysis',
                data: '{}',
                success: function (response) {

                    //var dataArray = [['DATA', 'Active', 'Inprogress', 'Closed', 'Inactive']];

                    //$.each(response.d, function (i, item) {
                    //    dataArray.push([item.DATA, item.Active, item.Inprogress, item.Closed, item.Inactive]);
                    //});
                    var dataArray = [['Data', 'counthealthData']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.Data, item.counthealthData]);
                    });

                    // google.charts.setOnLoadCallback(drawChart);

                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            //  title: 'Analytics Based on Document in office Vs Document Current Status',
                            chartArea: { width: 300, height: 250 },
                            pieHole: 0,
                            sliceVisibilityThreshold: .0,
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            },
                            colors: ['#800080', '#FF0000', '#ffd700', '#109618'],
                            //is3D: true
                        };

                        if (dataArray.length > 1) {
                            chart = new google.visualization.PieChart(document.getElementById('chart_divTodayHealthAnalysis'));


                            chart.draw(data, options);
                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelectedhealthdata);
                        } else { $('#lblTodayHealthAnalysis').removeClass("hidden"); }
                    }
                    function onAreaSliceSelectedhealthdata() {
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({

                            type: "POST",
                            url: "Home.aspx/createSessionofHealthData",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: 'Home.aspx/createSessionofHealthData',
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////


                            success: function (response) {
                                // alert("Successs");
                                // console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Todays Health Analysis Report graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/CaseManagerwiseData',
                //data: JSON.stringify({ UserId: Session["userid"], Location: LocationID, Building: BuildingID, Type: Stock, FloorId: FloorId, Column1: Column1FCNumber, Column2: Column2AssigneeName, Column3: Column3ClientName, CustodianId: CustodianId }),////
                data: {},
                success: function (response) {
                    // console.log(response);
                    var dataArray = [['Country', 'Stock']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.CaseManager, item.documentCount]);
                    });
                    google.charts.setOnLoadCallback(drawChart);
                    var chart, data;
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            //title: 'Analytics Based on Custodian & Location Based Report                    \n',
                            chartArea: { width: 300, height: 250 },
                            pieHole: 0,
                            sliceVisibilityThreshold: .0,
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            }
                            //is3D: true
                        };

                        if (dataArray.length > 1) {
                            chart = new google.visualization.PieChart(document.getElementById('containerdemograph'));


                            chart.draw(data, options);
                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelected);
                            //window.stop();
                        } else { $('#lblmsg2').removeClass("hidden"); }
                    };

                    // console.log(response);

                    function onAreaSliceSelected() {
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({
                            type: "POST",
                            url: "Home.aspx/Dashboard_Filtered_CaseManager",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: 'Home.aspx/Dashboard_Filtered_CaseManager',
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////


                            success: function (response) {
                                // alert("Successs");
                                //console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Case Manager Wise graph

        });
    </script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['bar'] });
        google.charts.load("current", { packages: ["corechart"] });
        function defaultContentsofPage() {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/LoadTop10Charts',
                //url: 'Home.aspx/GetBarchartData',
                data: '{}',
                success: function (response) {


                    var dataArray = [['clientName', 'countClientData']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.clientName, item.countClientData]);
                    });


                    google.charts.setOnLoadCallback(drawChart);
                    var chart, data;
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            // title: 'Top 10 Client Stock',
                            //chartArea: { width: 350, height: 300 },
                            chartArea: { width: '50%' },
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            },
                            colors: ['#008000'],
                            //hAxis: {
                            //    title: 'Total Count',
                            //    minValue: 0
                            //},

                            //pieHole: 0,

                            //sliceVisibilityThreshold: .0,
                            ////is3D: true,
                            //legend: 'none',
                            //bar: { groupWidth: '95%' },
                            //vAxis: { gridlines: { count: 10 }, width: '55px' },


                            //hAxis: {

                            //    //slantedText: true
                            //}
                        };


                        if (dataArray.length > 1) {
                            chart = new google.visualization.BarChart(document.getElementById('chart_div4'));

                            //chart.draw(data, google.charts.Bar.convertOptions(options));

                            chart.draw(data, options);

                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelectedtop10);
                        } else { $('#lblmsg4').removeClass("hidden"); }
                    }
                    //--------------------
                    function onAreaSliceSelectedtop10() {
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({
                         
                            type: "POST",
                            url: "Home.aspx/Dashboard_Filtered_Location",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: "Home.aspx/Dashboard_Filtered_Location",
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////




                            success: function (response) {
                                // alert("Successs");
                                //console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...top10........");
                    //console.log(response);
                    $('#divLoader').hide(500);
                }
            });//Top 10 Client Stock graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetGeoLocationWiseStockV2',
                //data: JSON.stringify({ UserId: Session["userid"], Location: LocationID, Building: BuildingID, Type: Stock, FloorId: FloorId, Column1: Column1FCNumber, Column2: Column2AssigneeName, Column3: Column3ClientName, CustodianId: CustodianId }),////
                data: {},
                success: function (response) {
                    // console.log(response);
                    var dataArray = [['Country', 'Stock']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.LocationName, item.Stock]);
                    });
                    google.charts.setOnLoadCallback(drawChart);
                    var chart, data;
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            //title: 'Analytics Based on Custodian & Location Based Report                    \n',
                            chartArea: { width: 300, height: 250 },
                            pieHole: 0,
                            sliceVisibilityThreshold: .0,
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            }
                            //is3D: true
                        };

                        if (dataArray.length > 1) {
                            chart = new google.visualization.PieChart(document.getElementById('chart_geo'));


                            chart.draw(data, options);
                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelected);
                            //window.stop();
                        } else { $('#lblmsg2').removeClass("hidden"); }
                    };

                    function onAreaSliceSelected() {
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({
                            type: "POST",
                            url: "Home.aspx/Dashboard_Filtered_LocationV2",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: 'Home.aspx/Dashboard_Filtered_LocationV2',
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////


                            success: function (response) {
                                // alert("Successs");
                                //console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Major Location Wise Stock graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetBarchartDataV2_',
                data: '{}',
                success: function (response) {

                    var dataArray = [['StockDate', 'Found', 'Missing', 'Mismatch', 'Extra']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.StockDate, item.Found, item.Missing, item.MissMatch, item.Extra]);
                    });

                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                        var data = google.visualization.arrayToDataTable(dataArray);

                        var options = {
                            chart: {
                                //title: 'Stock Check',
                                // title: 'Stock Check',
                                legend: {
                                    position: 'bottom', textStyle: { fontSize: 9 },
                                    width: '100%', maxLines: 3
                                }
                            },

                        };

                        if (dataArray.length > 1) {
                            var chart = new google.charts.Bar(document.getElementById('chart_divstock'));
                            chart.draw(data, options);
                        } else { $('#lblmsg1').removeClass("hidden"); }



                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Date Wise Stock Check Details graph        
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetTagVsPrintV2_',
                //data: JSON.stringify({ Location: null }),
                data: {},
                success: function (response) {

                    var dataArray = [['Description', 'Count', { role: 'style' }]];

                    $.each(response.d, function (i, item) {
                        var color = "";
                        if (i == 0) {
                            color = '#b87333';
                        }
                        if (i == 1) {
                            color = 'gold';
                        }
                        dataArray.push([item.StringColumn, item.dataCount, color]);
                    });
                    //                    if (dataArray.length > 0) {
                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                        var data = google.visualization.arrayToDataTable(dataArray);
                        var view = new google.visualization.DataView(data);
                        view.setColumns([0, 1,
                           {
                               calc: "stringify",
                               sourceColumn: 1,
                               type: "string",
                               role: "annotation"
                           },
                           2]);
                        var options = {
                            // title: "Printed Vs Tagged",
                            width: "80%",
                            height: "100%",
                            bar: { groupWidth: "95%" },
                            legend: { position: "none", maxLines: 3 },
                            vAxis: { minValue: 0 }
                        };

                        if (dataArray.length > 1) {
                            var chart = new google.visualization.ColumnChart(document.getElementById('chart_tagged'));
                            chart.draw(data, options);
                        } { $('#lblmsg3').removeClass("hidden"); }
                        $('#divLoader').hide(500);
                    };


                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Printed Vs Tagged graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/GetTodayHealthAnalysis',
                data: '{}',
                success: function (response) {

                    //var dataArray = [['DATA', 'Active', 'Inprogress', 'Closed', 'Inactive']];

                    //$.each(response.d, function (i, item) {
                    //    dataArray.push([item.DATA, item.Active, item.Inprogress, item.Closed, item.Inactive]);
                    //});
                    var dataArray = [['Data', 'counthealthData']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.Data, item.counthealthData]);
                    });

                    // google.charts.setOnLoadCallback(drawChart);

                    google.charts.setOnLoadCallback(drawChart);
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            //title: 'Analytics Based on Document in office Vs Document Current Status',
                            chartArea: { width: 300, height: 250 },
                            pieHole: 0,
                            sliceVisibilityThreshold: .0,
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            },
                            colors: ['#800080', '#FF0000', '#ffd700', '#109618'],
                            //is3D: true

                        };

                        if (dataArray.length > 1) {
                            chart = new google.visualization.PieChart(document.getElementById('chart_divTodayHealthAnalysis'));


                            chart.draw(data, options);
                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelectedhealthdata);
                        } else { $('#lblTodayHealthAnalysis').removeClass("hidden"); }
                    }
                    function onAreaSliceSelectedhealthdata() {
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({

                            type: "POST",
                            url: "Home.aspx/createSessionofHealthData",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: 'Home.aspx/createSessionofHealthData',
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////


                            success: function (response) {
                                // alert("Successs");
                                // console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Todays Health Analysis Report graph
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Home.aspx/CaseManagerwiseData',
                //data: JSON.stringify({ UserId: Session["userid"], Location: LocationID, Building: BuildingID, Type: Stock, FloorId: FloorId, Column1: Column1FCNumber, Column2: Column2AssigneeName, Column3: Column3ClientName, CustodianId: CustodianId }),////
                data: {},
                success: function (response) {
                    // console.log(response);
                    var dataArray = [['Country', 'Stock']];

                    $.each(response.d, function (i, item) {
                        dataArray.push([item.CaseManager, item.documentCount]);
                    });
                    google.charts.setOnLoadCallback(drawChart);
                    var chart, data;
                    function drawChart() {
                        data = google.visualization.arrayToDataTable(dataArray);
                        var options = {
                            //title: 'Analytics Based on Custodian & Location Based Report                    \n',
                            chartArea: { width: 300, height: 250 },
                            pieHole: 0,
                            sliceVisibilityThreshold: .0,
                            legend: {
                                position: 'bottom', textStyle: { fontSize: 9 },
                                width: '100%', maxLines: 3
                            }
                            //is3D: true
                        };

                        if (dataArray.length > 1) {
                            chart = new google.visualization.PieChart(document.getElementById('containerdemograph'));


                            chart.draw(data, options);
                            google.visualization.events.addListener(chart, 'select', onAreaSliceSelected);
                            //window.stop();
                        } else { $('#lblmsg2').removeClass("hidden"); }
                    };

                    // console.log(response);

                    function onAreaSliceSelected() {
                        //alert(data.getValue(chart.getSelection()[0].row, 0));
                        $.ajax({
                            type: "POST",
                            url: "Home.aspx/Dashboard_Filtered_CaseManager",
                            data: '{name: "' + data.getValue(chart.getSelection()[0].row, 0) + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            type: 'POST',
                            dataType: 'json',
                            contentType: 'application/json',
                            url: 'Home.aspx/Dashboard_Filtered_CaseManager',
                            data: JSON.stringify({ 'name': data.getValue(chart.getSelection()[0].row, 0) }),////


                            success: function (response) {
                                // alert("Successs");
                                //console.log(data);
                                window.location.href = "Asset.aspx";
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                                //console.log('failed');
                                //console.log(response);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                                //console.log('error');
                                //console.log(response);
                            }
                        });


                    }
                },

                error: function () {
                    alert("Error loading data...........");
                    $('#divLoader').hide(500);
                }
            });//Case Manager Wise graph
        }
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


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField runat="server" ID="HdnLocation" />
    <div id="div1" style="font-family: Calibri; font-size: 10pt;" class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-sm-2">

                                <span style="font-family: 'Calibri'; font-size: x-large">Dashboard</span>&nbsp;
                                <a style="float: inline-end; font-size: 24px" onclick="return (event.keyCode!=13);" runat="server" id="txtSearch"><i class="fa fa-filter"></i></a>
                            </div>

                            <div class="col-sm-10">

                                <i style="float: inline-end; font-size: 24px" class="fa">

                                    <%-- <asp:TextBox runat="server" ID="txtSearch" placeholder="Search" class="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                                </i>
                                &nbsp;&nbsp;&nbsp;&nbsp;<img src="" alt="" style="float: right;" runat="server" id="CompanyImg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                    </div>
                    <%-- collapse demo--%>

                    <div class="row">
                        <div id="divSearch" runat="server" class="panel-group col-md-12 form_wrapper">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <label>
                                            </label>
                                        </div>
                                        <div class="col-sm-10 pull-right">
                                            <a class="ex1" id="divSearchClose" href="#"><span id="spanSerch" style="top: 0px;"
                                                class="badge"><i class="fa fa-times" aria-hidden="true"></i></span></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-md-3">
                                                Major Location:
                                                <asp:DropDownList ID="ddlMajorLocation" AutoPostBack="true" OnSelectedIndexChanged="ddlMajorLocation_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                Minor Location:
                                                <asp:DropDownList ID="ddlMinorLocation" AutoPostBack="true" Style="width: 100%;" OnSelectedIndexChanged="ddlMinorLocation_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                Minor-Sub Location:
                                                <asp:DropDownList ID="ddlMinorSubLocation" AutoPostBack="true" OnSelectedIndexChanged="ddlMinorSubLocation_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                FC Number:
                                                <asp:DropDownList ID="ddlFCNumber" AutoPostBack="true" OnSelectedIndexChanged="ddlFCNumber_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                Case Manager:
                                                 <asp:DropDownList ID="ddlCaseManager" AutoPostBack="true" OnSelectedIndexChanged="ddlCaseManager_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                Custodian:
                                                <asp:DropDownList ID="ddlCustodian" AutoPostBack="true" OnSelectedIndexChanged="ddlCustodian_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                Client Name:
                                                <asp:DropDownList ID="ddlClientName" AutoPostBack="true" OnSelectedIndexChanged="ddlClientName_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                Assignee Name:
                                                <asp:DropDownList ID="ddlAssigneeName" AutoPostBack="true" OnSelectedIndexChanged="ddlAssigneeName_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-3">
                                                Case Worker 1:
                                                <asp:DropDownList ID="ddlCaseWorker1" AutoPostBack="true" OnSelectedIndexChanged="ddlCaseWorker1_SelectedIndexChanged" Style="width: 100%;" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--  collapse start--%>
                </div>

                <%--collapse end--%>
                <div class="panel-body" style="padding: 2px;">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-4 col-lg-4" style="border: 1px solid #C0C0C0; border-radius: 15px;">
                                <label style="text-align: center; width: 100%; font-family: Calibri; font-size: 10pt; font-weight: bold">Analytics Based on Document in office Vs Document Current Status</label>
                                <div class="" style="height: 450px;" id="chart_divTodayHealthAnalysis">
                                    <div id="chart1MsgTodayHealthAnalysis">
                                        <label id="lblTodayHealthAnalysis" runat="server" class="hidden" style="font-family: Calibri; font-size: 10pt;">
                                            No data found</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-lg-4" style="border: 1px solid #C0C0C0; border-radius: 15px;">
                                <label style="text-align: center; width: 100%; font-family: Calibri; font-size: 10pt; font-weight: bold">Analytics Based on Custodian Location Based Report</label>
                                <div class="" style="height: 450px;" id="chart_geo">
                                    <div id="chart2Msg">
                                        <label id="lblmsg2" runat="server" class="hidden" style="font-family: Calibri; font-size: 10pt;">
                                            No data found</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-lg-4" style="border: 1px solid #C0C0C0; border-radius: 15px; align-items: center; text-align: center; align-content: center; vertical-align: middle;">
                                <label style="text-align: center; width: 100%; font-family: Calibri; font-size: 10pt; font-weight: bold">Analytics Based on Team Custodian Location Based Report</label>
                                <div id="containerdemograph" style="height: 450px">
                                    <div id="chart3Msg">
                                        <label class="hidden" id="lblmsgcasemg" runat="server" style="font-family: Calibri; font-size: 10pt;">
                                            No data found</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-sm-4 col-lg-4" style="border: 1px solid #C0C0C0; border-radius: 15px;">
                                <label style="text-align: center; width: 100%; font-family: Calibri; font-size: 10pt; font-weight: bold">Printed Vs Tagged</label>
                                <div class="" style="height: 450px;"
                                    id="chart_tagged">
                                    <div id="chart3Msg">
                                        <label class="hidden" id="lblmsg3" runat="server" style="font-family: Calibri; font-size: 10pt;">
                                            No data found</label>
                                    </div>
                                </div>

                            </div>

                            <div class="col-sm-4 col-lg-4" style="border: 1px solid #C0C0C0; height: 100%; border-radius: 15px;">
                                <label style="text-align: center; width: 100%; font-family: Calibri; font-size: 10pt; font-weight: bold">Month Wise Report for Document Request</label>
                                <br />
                                <div class="" style="height: 450px; overflow: scroll;">
                                    <asp:Literal ID="ltTable" runat="server" />
                                </div>
                                <%--<table class="table">
                                    <thead class="thead-light" align="center">

                                        <tr align="center">
                                            <th scope="col" align="center">#</th>
                                            <th scope="col" style="font-family: Calibri; font-size: 10pt; text-align: center" align="center">&nbsp;&nbsp;&nbsp;&nbsp;LocationName</th>
                                            <th scope="col" style="font-family: Calibri; font-size: 10pt; text-align: center" align="center">&nbsp;&nbsp;&nbsp;&nbsp;NewRequest</th>
                                            <th scope="col" style="font-family: Calibri; font-size: 10pt; text-align: center" align="center">&nbsp;&nbsp;&nbsp;&nbsp;Approved</th>
                                            <th scope="col" style="font-family: Calibri; font-size: 10pt; text-align: center" align="center">&nbsp;&nbsp;&nbsp;&nbsp;Rejected</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                      
                                        <tr align="center">
                                            <th scope="row" style="font-family: Calibri; font-size: 10pt; text-align: center">1</th>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">DIC-DC</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">20</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">15</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">5</td>
                                        </tr>
                                        <tr align="center">
                                            <th scope="row" style="font-family: Calibri; font-size: 10pt; text-align: center">2</th>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">DIFC-DC</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">10</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">10</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">0</td>
                                        </tr>
                                        <tr align="center">
                                            <th scope="row" style="font-family: Calibri; font-size: 10pt; text-align: center">3</th>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">AUH-DC</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">20</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">19</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">1</td>
                                        </tr>
                                        <tr align="center" style="font-weight: 900">
                                            <th scope="row"></th>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">TOTAL</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">50</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">44</td>
                                            <td align="center" style="font-family: Calibri; font-size: 10pt; text-align: center">6</td>
                                        </tr>
                                    </tbody>
                                </table>--%>
                            </div>
                            <div class="col-sm-4 col-lg-4" style="border: 1px solid #C0C0C0; border-radius: 15px;">
                                <label style="text-align: center; width: 100%; font-family: Calibri; font-size: 10pt; font-weight: bold">Stock Check</label>
                                <div class="" style="height: 450px;"
                                    id="chart_divstock">
                                    <div id="chart1Msg">
                                        <label id="lblmsg1" runat="server" class="hidden" style="font-family: Calibri; font-size: 10pt;">
                                            No data foundlblmsg1</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-sm-12 col-lg-12" style="border: 1px solid #C0C0C0; border-radius: 15px;">
                                <label style="text-align: center; width: 100%; font-family: Calibri; font-size: 10pt; font-weight: bold">Top 10 Client Stock</label>

                                <div class="" style="height: 450px;"
                                    id="chart_div4">
                                    <div id="chart4Msg" style="align-items: center;">
                                        <label id="lblmsg4" runat="server" class="hidden" style="font-family: Calibri; font-size: 10pt;">
                                            No data foundlblmsg1</label>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div>
            </div>
        </div>
        <!-- /.page-header -->
        <div class="row">
            <div class="col-xs-12" style="background-color: white">
                <div class="hidden">
                    <uc1:topmenu runat="server" ID="topmenu" />
                </div>
            </div>
        </div>
        <!-- /.col -->
    </div>
    <%--  <script language="JavaScript">
        function drawChart() {
            // Define the chart to be drawn.
            var data = google.visualization.arrayToDataTable([
               ['Year', 'Asia', { role: 'annotation' }, 'Europe', { role: 'annotation' }],
               ['RAGESH', 900, '900', 390, '390'],
               ['NEHA', 1000, '1000', 400, '400'],
               ['SUCHITRA', 1170, '1170', 440, '440'],
               ['SONITHA', 1250, '1250', 480, '480'],
               ['PRASHANT', 1530, '1530', 540, '540']
            ]);

            var options = {
                chartArea: { width: 300, height: 250 }, isStacked: 'percent', legend: {
                    position: 'bottom', textStyle: { fontSize: 9 },
                    width: '100%'
                }
            };

            // Instantiate and draw the chart.
            var chart = new google.visualization.PieChart
            (document.getElementById('containerdemograph'));
            chart.draw(data, options);
        }
        google.charts.setOnLoadCallback(drawChart);
    </script>--%>
    <!-- /.page-content -->
</asp:Content>
