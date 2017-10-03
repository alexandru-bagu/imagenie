<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenieComment.ascx.cs" Inherits="imaGenie.GenieComment" %>

<div class="comment">
    <div style="display:inline; ">
        <span>
            <b> <%= Owner %></b> at  <%= Date %> <br />
        </span>
       
        <asp:PlaceHolder ID="textHolder" runat="server">
            <asp:Label ID="labelText" runat="server" CssClass="paddedtext" Text='<%# Message %>'/>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="editHolder" runat="server" Visible ="false">
            <div><asp:TextBox runat="server" ID="commentText" Text='<%# Message %>' Width="100%"></asp:TextBox></div>
            <div align="right"><asp:Button runat="server" ID="commentSave" Text="Save" OnClick="commentSave_Click" /></div>
        </asp:PlaceHolder>
    </div>
    <div style="position:absolute; top:0px; right:0px">
        <asp:CheckBox runat="server" ID="approveCb" Text="Approved" Checked='<%# Approved %>' OnCheckedChanged="approveCb_CheckedChanged" AutoPostBack="true" Visible="false" />
        <asp:Button runat="server" ID="editBtn" Text="Edit" OnClick="editBtn_Click" Visible ="false"/>
        <asp:Button runat="server" ID="deleteBtn" Text="Delete" OnClick="deleteBtn_Click" Visible ="false"/>
    </div>
</div>