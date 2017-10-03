<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Interact.aspx.cs" Inherits="imaGenie.Interact" %>
<%@ Register src="~/GenieImage.ascx" tagName="Image" tagPrefix="genie" %>
<%@ Register src="~/GenieComment.ascx" tagName="Comment" tagPrefix="genie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display:inline-block; width:100%;">
        <div style="display: table;
    margin: 0 auto; "><genie:Image runat="server" ID="genieImage" IsAnchor="false"/></div>
        <div style="clear:both;"></div>
        <div style="display:block;">
            Comments:
            <asp:LoginView runat="server" ID="newComment">
                <LoggedInTemplate>
                    <div class="comment">
                        Add a new comment: <br />
                        <div><asp:TextBox runat="server" ID="commentBox" width="100%"/></div>
                        <div align="right"><asp:Button runat="server" ID="commentButton" Text="Post" OnClick="commentButton_Click" /></div>
                    </div>
                </LoggedInTemplate>
            </asp:LoginView>
            <asp:Repeater runat="server" ID="genieComments">
                <ItemTemplate>
                    <genie:Comment CommentId='<%# Eval("id") %>' ImageOwner='<%# genieImage.Owner %>' runat="server"/>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
