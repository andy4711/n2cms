<%@ Page Title="" Language="C#" MasterPageFile="~/N2/Content/Framed.master" AutoEventWireup="true" CodeBehind="Export.aspx.cs" Inherits="N2.Management.Content.Globalization.Export" meta:resourcekey="PageResource1" %>
<%@ Register Src="../AffectedItems.ascx" TagName="AffectedItems" TagPrefix="uc1" %>
<%@ Register TagPrefix="edit" Namespace="N2.Edit.Web.UI.Controls" Assembly="N2.Management" %>

<asp:Content ID="CH" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="<%=MapCssUrl("exportImport.css")%>" type="text/css" />
</asp:Content>
<asp:Content ID="ct" ContentPlaceHolderID="Toolbar" runat="server">
    <edit:CancelLink ID="hlCancel" runat="server" meta:resourceKey="hlCancel">Cancel</edit:CancelLink>
</asp:Content>
<asp:Content ID="cc" ContentPlaceHolderID="Content" runat="server">
	<edit:PersistentOnlyPanel ID="popNotSupported" runat="server" meta:resourceKey="popNotSupported">
		<div>
		    <asp:Button ID="btnExport" runat="server" CssClass="command" OnCommand="btnExport_Command" CausesValidation="false" meta:resourceKey="btnExport" Text="Export these items" />
		</div>
		<div>
		    <asp:CheckBox ID="chkPageTitle" runat="server" Text="Don't translate page title"  meta:resourceKey="chkPageTitle" />
		</div>
		<div>
		    <asp:CheckBox ID="chkPageName" runat="server" Text="Don't translate page name"  meta:resourceKey="chkPageName" />
		</div>
        <div>
		    <asp:CheckBox ID="chkPageURL" runat="server" Text="Don't translate page URL"  meta:resourceKey="chkPageURL" />
		</div>
<%--        <div>
		    <asp:CheckBox ID="chkDetailTitle" runat="server" Text="Don't translate detail title"  meta:resourceKey="chkDetailTitle" />
		</div>
        <div>
		    <asp:CheckBox ID="chkDetailName" runat="server" Text="Don't translate detail name"  meta:resourceKey="chkDetailName" />
		</div>
        <div>
		    <asp:CheckBox ID="chkPartName" runat="server" Text="Don't translate part name"  meta:resourceKey="chkPartName" />
		</div>--%>
        <div>
		    <asp:CheckBox ID="chkPartTitle" runat="server" Text="Don't translate part title"  meta:resourceKey="chkPartTitle" />
		</div>
		<n2:h4 runat="server" Text="Exported items" meta:resourceKey="exportedItems" />
		<uc1:AffectedItems id="exportedItems" runat="server" />		
	</edit:PersistentOnlyPanel>	
</asp:Content>
