<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestLangue.aspx.cs" Inherits="Test.TestLangue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        onload = function () { 
            load();
        };
        function load() {
            var test = document.getElementsByTagName('html')[0].innerHTML;
            test = result.replaceAll('中文', 'Chinese');
            test = result.replaceAll('翻译', 'translate');
            test = result.replaceAll('成', 'turn into');
            test = result.replaceAll('英文', 'English');
            document.getElementsByTagName('html')[0].innerHTML = test;
        } 
</script> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        中文翻译成英文
    </div>
    </form>
</body>
</html>
