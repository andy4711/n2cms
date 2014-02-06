<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemXmlUpdate.ascx.cs" Inherits="N2.Management.Content.Globalization.ItemXmlUpdate" %>
<%@ Register Src="../AffectedItems.ascx" TagName="AffectedItems" TagPrefix="uc1" %>

<asp:CustomValidator id="cvUpdate" runat="server" CssClass="alert alert-error alert-margin" meta:resourceKey="cvUpdate" Display="Dynamic"/>

<asp:Panel ID="pnlTypeMissmatch" runat="server" CssClass="formField" Visible="false">
	<asp:Label ID="lblTypeMissmatch" runat="server" meta:resourceKey="lblTypeMissmatch" Text="New name" />
</asp:Panel>
<div>
	<asp:CheckBox ID="chkSkipAttachments" runat="server" Text="Skip attachments" ToolTip="Checking this options cause the attachments not to be updated" meta:resourceKey="chkSkipAttachments" />
    <%--<asp:CheckBox ID="chkUpdateChilds" runat="server" Text="Update children" ToolTip="Replaces all children from file" meta:resourceKey="chkUpdateChilds" />--%>
</div>
<asp:Button ID="btnUpdateUploaded" runat="server" Text="Update" OnClick="btnUpdateUploaded_Click"  meta:resourceKey="btnUpdateUploaded"/>
<n2:h4 runat="server" Text="Updated Items" meta:resourceKey="updatedItems" />
<uc1:AffectedItems id="updatedItems" runat="server" />
<n2:h4 runat="server" Text="Attachments" meta:resourceKey="attachments" />
<asp:Repeater ID="rptAttachments" runat="server">
	<ItemTemplate>
		<div class="file"><asp:Image ID="Image1" runat="server" ImageUrl="../../Resources/icons/page_white.png" alt="file" /><%# Eval("Url") %> <span class="warning"><%# CheckExists((string)Eval("Url")) %></span></div>
	</ItemTemplate>
</asp:Repeater>