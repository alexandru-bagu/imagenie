<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="imaGenie.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <b>Resize:</b> <br />
    Percentage: <asp:TextBox id="pResize" Text="100%" runat="server"/><asp:Button ID="pResizeBtn" runat="server" Text="Resize" OnClick="pResizeBtn_Click" /><asp:CustomValidator ID="cv1" ControlToValidate="pResize" runat="server" ErrorMessage="Invalid input. Accepted values: 0..100%" OnServerValidate="cv1_ServerValidate"></asp:CustomValidator><br />
    Width x Height: <asp:TextBox id="widthResize" Text="0" runat="server"/>x<asp:TextBox id="heightResize" Text="0" runat="server"/><asp:Button ID="wxhResize" runat="server" Text="Resize" OnClick="wxhResize_Click" /><br />
    <b>Crop:</b> <br />
    Initial X - Initial Y: <asp:TextBox id="initialX" Text="0" runat="server"/>-<asp:TextBox id="initialY" Text="0" runat="server"/><br />
    End X - End Y: <asp:TextBox id="endX" Text="" runat="server"/>-<asp:TextBox id="endY" Text="" runat="server"/> <br />
    <asp:Button ID="cropBtn" runat="server" Text="Crop" OnClick="cropBtn_Click" /><br />
    <asp:Image ImageAlign="Middle" runat="server" ID="image"/>
</asp:Content>
