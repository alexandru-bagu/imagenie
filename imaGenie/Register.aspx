<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="imaGenie.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding: 20px 20px 20px 20px;">
        <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" OnCreatedUser="CreateUserWizard1_CreatedUser" UserNameLabelText="Username:" UserNameRequiredErrorMessage="Username is required." CreateUserButtonText="Register" ForeColor="Black">
            <WizardSteps>
                <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server" Title="Register a new account">
                </asp:CreateUserWizardStep>
                <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                </asp:CompleteWizardStep>
            </WizardSteps>
        </asp:CreateUserWizard>
    </div>
</asp:Content>
