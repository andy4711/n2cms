<%@ Page MasterPageFile="../Framed.master" Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="N2.Edit.Globalization.Update" meta:resourcekey="PageResource1" %>
<%@ Register Src="ItemXmlUpdate.ascx" TagName="ItemXmlUpdate" TagPrefix="uc1" %>
<%@ Register TagPrefix="edit" Namespace="N2.Edit.Web.UI.Controls" Assembly="N2.Management" %>

<asp:Content ID="CH" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="<%=MapCssUrl("exportImport.css")%>" type="text/css" />
</asp:Content>
<asp:Content ID="CT" ContentPlaceHolderID="Toolbar" runat="server">
    <edit:CancelLink ID="hlCancel" runat="server" meta:resourceKey="hlCancel">Cancel</edit:CancelLink>
</asp:Content>
<asp:Content ID="CC" ContentPlaceHolderID="Content" runat="server">
	<edit:PersistentOnlyPanel ID="popNotSupported" runat="server" meta:resourceKey="popNotSupported">
        <asp:MultiView ID="uploadFlow" runat="server" ActiveViewIndex="0">
		    <asp:View ID="uploadView" runat="server">
		        <asp:CustomValidator id="cvUpdate" runat="server" CssClass="alert alert-error alert-margin" meta:resourceKey="cvUpdate" Display="Dynamic"/>
			    <div class="upload">
				    <div class="cf">
				        <asp:FileUpload ID="fuImport" runat="server" />
				        <asp:RequiredFieldValidator ID="rfvUpload" ControlToValidate="fuImport" runat="server" ErrorMessage="*"  meta:resourceKey="rfvImport"/>
				    </div>
				    <div>
				        <asp:Button ID="btnVerify" runat="server" Text="Upload and examine" OnClick="btnVerify_Click" Display="Dynamic" meta:resourceKey="btnVerify"/>
				    </div>
			    </div>
		    </asp:View>
		    <asp:View ID="preView" runat="server">
				<uc1:ItemXmlUpdate id="xml" runat="server" />				
		    </asp:View>
	    </asp:MultiView>
	</edit:PersistentOnlyPanel>
</asp:Content>
