﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Test.Templates.SharpKit.WebSite.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Test.Templates.SharpKit.WebSite</title>
    <script src="res/jquery-1.8.2.min.js"></script>
    <script src="res/jsclr-4.1.0.js"></script>
    <script src="res/StringWriter.js"></script>
    <script src="res/MagicWare.Client.ObjectBuilder.js"></script>
    <script src="res/Neptuo.Templates.js"></script>
    <script src="res/Neptuo.Templates.Components.js"></script>
    <script src="res/Test.Templates.js"></script>
    <script src="res/View.js"></script>
    <script src="res/Default.js"></script>
    <script>

        JsRuntime.Start();

    </script>
</head>
<body>
    <button onclick="btnTest_click(event);">Create edit address form</button>

    <div id="viewContent"></div>
</body>
</html>
