<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEchart.aspx.cs" Inherits="Test.TestEchart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/echarts.js" type="text/javascript"></script>
    <script type="text/javascript"> 
    echarts{  
    }

    </script>
</head>
<body>
    <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main" style="width: 600px; height: 400px;">
    </div>
    <div id="Pie" style="width: 600px; height: 400px;">
    </div>
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('main'));
        var Data;
        $.post("Data.ashx", "11", function (d) {
            if (d != null) {
                Data = JSON.parse(d); //eval('(' + d + ')');  //
                // 指定图表的配置项和数据
                var option = {
                    title: {
                        
                    },
                    toolbox: {
                        show: true,
                        orient: 'horizontal',
                        itemSize: 15,
                        itemGap: 10,
                        showTitle: true,
                        feature: {

                            magicType: {
                                type: ['line', 'bar']
                            },
                            saveAsImage: {
                                type: 'png'
                            },
                            brush: {
                                type: ['rect', 'polygon'],
                                title: {
                                    rect: '矩形'
                                }
                            }
                        }
                    },
                    dataZoom: [
                    {
                        type: 'inside',
                        start: 0,
                        end: 70
                    }, {
                        start: 0,
                        end: 70,
                        handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
                        handleSize: '80%',
                        handleStyle: {
                            color: '#fff',
                            shadowBlur: 3,
                            shadowColor: 'rgba(0, 0, 0, 0.6)',
                            shadowOffsetX: 2,
                            shadowOffsetY: 2
                        }
                    }],
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        data: ['销量','哈哈']
                    },
                    xAxis: {
                        data: ["衬衫", "羊毛衫", "雪纺衫", "裤子", "高跟鞋", "袜子"]
                    },
                    yAxis: {},
                    series: [{
                        name: '销量',
                        type: 'line',
                        data: Data.d//[5, 20, 36, 10, 10, 20]
                    },
                    {
                        name: '哈哈',
                        type: 'line',
                        data:[10, 20, 40, 10, 15, 17]
                    }]

                };

                // 使用刚指定的配置项和数据显示图表。
                myChart.setOption(option);


            }
        });


        $(function () {
            setInterval(function () {

                $.post("Data.ashx?type=1", "11", function (da) {
                    var Dat;
                    Dat = JSON.parse(da); //eval('(' + d + ')');  //
                    myChart.setOption({
                        series: [{
                            name: '销量',
                            type: 'line',
                            data: Dat.d//[5, 20, 36, 10, 10, 20]
                        }]

                    });
                })

            }, "5000") 
        
        })
       
    </script>
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart2 = echarts.init(document.getElementById('Pie'));

        var option = {
            title: {
                text: '某站点用户访问来源',
                subtext: '纯属虚构',
                x: 'center'
            },
            toolbox: {
                show: true,
                orient: 'horizontal',
                itemSize: 15,
                itemGap: 10,
                showTitle: true,
                feature: {
                    saveAsImage: {
                        type: 'png'
                    }
                }
            },
            tooltip: {
                trigger: 'item'
                //                ,
                //                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                left: 'left',
                data: ['直接访问', '邮件营销', '联盟广告', '视频广告', '搜索引擎']
            },
            series: [
        {
            name: '访问来源',
            type: 'pie',
            radius: '55%',
            center: ['50%', '60%'],
            data: [
                { value: 335, name: '直接访问' },
                { value: 310, name: '邮件营销' },
                { value: 234, name: '联盟广告' },
                { value: 135, name: '视频广告' },
                { value: 1548, name: '搜索引擎' }
            ],
            itemStyle: {
                emphasis: {
                    shadowBlur: 5,
                    shadowOffsetX: 10,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            }
        }
    ]
        };
        // 使用刚指定的配置项和数据显示图表。
        myChart2.setOption(option);
     
    </script>
    
</body>
</html>
