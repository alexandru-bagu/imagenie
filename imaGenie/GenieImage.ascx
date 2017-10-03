<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenieImage.ascx.cs" Inherits="imaGenie.imaGenieControl"  ViewStateMode="Enabled"  EnableViewState="true"%>
<div class="image" style="display:inline-block; margin: 5px;">
    <div style="display:inline-block;">
        <div style='<%= "position:relative;display:inline-block; width: " + Width + ";" %>>'>
            <% if (IsAnchor) { %>
            <a href='<%= "Interact.aspx?id=" + ImageId %>'>
            <%} else { %>
            <a href='<%= "Image.aspx?id=" + ImageId %>'>
            <%}%>
                <img id='<%= "img" + ImageId %>' src='<%= "Image.aspx?id=" + ImageId + (Thumbnail ? "&tn=1" : "") %>' width='<%= Width %>' height='<%= Height %>'/>
            </a>
            <div style="position: absolute; top: 0px; width:100%;">
                <div style="display:inline-block; float: left">
                    <asp:Button ID="removeImage" runat="server" Text="Remove" OnClick="removeImage_Click" Visible="false" /><br />
                    <asp:CheckBox ID="isPublicCb" runat="server" Text="Public" ValidationGroup='<%# ImageId %>' Checked='<%# IsPublic %>' OnCheckedChanged="isPublicCb_CheckedChanged" AutoPostBack="true"  /><br />
                    <asp:LoginView ID="loginView" runat="server">
                        <RoleGroups>
                            <asp:RoleGroup Roles="Administrator,Moderator">
                                <ContentTemplate>
                                     <asp:Button ID="removeAll" runat="server" Text="Remove all from user" OnClick="removeAll_Click"/><br />
                                </ContentTemplate>
                            </asp:RoleGroup>
                        </RoleGroups>
                    </asp:LoginView>
                </div>
                <div style="display:inline-block; float: right">
                    <asp:Button ID="editBtn" runat="server" Text="Edit" OnClick="editBtn_Click" Visible="false" /><br />
                </div>
            </div>
        </div>
        <br />
        <% if (!IsAnchor) { %>
            <div style="display: inline-block">
                <span>
                    Added on:<%= Date %>
                </span>
            </div>
        <%}%>
        <div style="float:right; display: inline-block">
            <span style="text-align: right; font-style: italic">
                "<%= Text %>"
            </span>
        </div>
    </div>
</div>