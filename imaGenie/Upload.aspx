<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="imaGenie.Upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LoginView ID="LoginView1" runat="server">
        <AnonymousTemplate>
            You have to login first.
        </AnonymousTemplate>
    </asp:LoginView>
    <asp:Panel ID="uploadPanel" runat="server">
        <asp:FileUpload ID="fileUpload" runat="server"/><asp:RequiredFieldValidator ID="rfv1" ControlToValidate="fileUpload" runat="server" ErrorMessage="You must select an image first."/>
        <asp:Label runat="server" Text="" ID="fileUploadError"></asp:Label>
        <br />
        <asp:Label ID="messageLabel" runat="server" Text="Description:"></asp:Label>
        <asp:TextBox ID="messageInput" runat="server" TextMode="MultiLine" MaxLength="1024" Width="50%"/><asp:RequiredFieldValidator ID="rfv2" ControlToValidate="messageInput" runat="server" ErrorMessage="You must input a description."/>
        <br />
        <asp:CheckBox ID="publicImage" runat="server" Text="Public" Checked="true" />
        <br />
        Category: <asp:CheckBoxList ID="categoryList" runat="server" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="Id" BorderWidth="1" BorderStyle="Dotted"/><asp:CustomValidator ID="cv1" runat="server" ErrorMessage="You must select at least one category." OnServerValidate="cv1_ServerValidate"></asp:CustomValidator>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [Name], [Id] FROM [Category]"></asp:SqlDataSource>
        <br />
        Album:<asp:CheckBoxList ID="albumList" runat="server" DataTextField="Name" DataValueField="Id" BorderWidth="1" BorderStyle="Dotted"/>
        <br />
        <br />
        <asp:Button ID="uploadButton" runat="server" Text="Upload" OnClick="uploadButton_Click" />
        <asp:CustomValidator ID="customValidatorUpload" runat="server" ErrorMessage="" ControlToValidate="fileUpload" ClientValidationFunction="setUploadButtonState();"></asp:CustomValidator>
        <script type="text/javascript">
            function setUploadButtonState() {
                var aspFileUpload = document.getElementById('ContentPlaceHolder1_fileUpload');
                var uploadButton = document.getElementById('ContentPlaceHolder1_uploadButton');
                var fileUploadError = document.getElementById('ContentPlaceHolder1_fileUploadError');
                var fileName = aspFileUpload.value;
                uploadButton.setAttribute("disabled", "true");
                var ext = fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();
                if (!(ext == "jpeg" || ext == "jpg" || ext == "png")) {
                    fileUploadError.innerHTML = "Invalid image file, must select a *.jpeg, *.jpg, or *.png file.";
                    return false;
                }
                uploadButton.removeAttribute("disabled");
                return true;
            }
        </script>
    </asp:Panel>
</asp:Content>
