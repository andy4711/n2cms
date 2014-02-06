<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Languages.ascx.cs" Inherits="N2.Edit.Globalization.Languages" %>
<asp:Repeater ID="rptLang" runat="server" DataSource='<%# DataSource %>'>
	<ItemTemplate>
		<td class="item">
		    <input type="radio" name="<%# Eval("Language.LanguageCode") %>" value="<%# Eval("ExistingItem.ID") %>" style="display:<%# (bool)Eval("IsNew") || Eval("ExistingItem") == Eval("Language") ? "none" : "inline" %>" />

		    <asp:HyperLink ID="hlEdit" NavigateUrl='<%# Eval("EditUrl") %>' CssClass='<%# GetClass() %>' runat="server" ToolTip='<%# Eval("ExistingItem.Updated") %>' Visible='<%# Eval("IsTranslatable") %>'>
			    <asp:Literal ID="ltCreateNew" runat="server" Text='create new' Visible='<%# (bool)Eval("IsNew") %>' meta:resourceKey="ltCreateNew" />
				
			    <asp:Image ID="imgNew" ImageUrl='<%# Engine.ManagementPaths.ResolveResourceUrl((string)Eval("ExistingItem.IconUrl")) %>' AlternateText="icon" runat="server" Visible='<%# !(bool)Eval("IsNew") && Eval("ExistingItem.IconUrl") != null %>'/>
			    <b class="<%# Eval("ExistingItem.IconClass") %>"></b>
			    <asp:Literal ID="ltExisting" runat="server" Text='<%# Eval("ExistingItem.Title") ?? "(untitled)" %>' meta:resourceKey="ltExisting" Visible='<%# !(bool)Eval("IsNew") %>'/>
		    </asp:HyperLink>

            <asp:HyperLink ID="hlImport" NavigateUrl='<%# Eval("ImportLanguageUrl") %>' CssClass='import' runat="server" ToolTip='' Visible='<%# (bool)Eval("IsNew") && (bool)Eval("IsTranslatable") %>'>
			    <asp:Literal ID="ltImportNew" runat="server" Text='import new' Visible='true' meta:resourceKey="ltImportNew" />
		    </asp:HyperLink>

            <br>

            <asp:HyperLink ID="hlUpdate" NavigateUrl='<%# Eval("UpdateLanguageUrl") %>' CssClass='update' runat="server" ToolTip='' Visible='<%# !(bool)Eval("IsNew") && (bool)Eval("IsTranslatable") %>'>
                    <asp:Literal ID="ltUpdate" runat="server" Text='Update' Visible='true' meta:resourceKey="ltUpdate" />
		    </asp:HyperLink>

            <asp:HyperLink ID="hlExport" NavigateUrl='<%# Eval("ExportLanguageUrl") %>' CssClass='export' runat="server" ToolTip='' Visible='<%# !(bool)Eval("IsNew") && (bool)Eval("IsTranslatable") %>'>
                    <asp:Literal ID="ltExport" runat="server" Text='Export' Visible='true' meta:resourceKey="ltExport" />
		    </asp:HyperLink>
		</td>
	</ItemTemplate>
</asp:Repeater>
