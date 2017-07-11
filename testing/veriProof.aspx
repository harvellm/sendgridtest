<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="veriProof.aspx.cs" Inherits="testing.veriProof" %>
<%@ Register TagPrefix="uc" TagName="proofing" Src="~/Proofing.ascx"%>
<%@ Register TagPrefix="uc" TagName="chat" Src="~/chat.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .container {
            background-color: #99CCFF;
            border: thick solid #808080;
            padding: 20px;
            margin: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc:chat id="chatting" runat="server" />
        </div>
    </form>
</body>
</html>
