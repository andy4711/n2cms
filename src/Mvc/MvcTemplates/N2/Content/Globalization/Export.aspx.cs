using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Persistence.Serialization;
using N2.Edit.Web;
using N2.Edit;
using N2.Definitions;

namespace N2.Management.Content.Globalization
{
	public partial class Export : EditPage
	{
		protected AffectedItems exportedItems;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (Selection.SelectedItem is IFileSystemNode)
			{
				Response.Redirect("../../Files/FileSystem/Export.aspx?path=" + Server.UrlEncode(Selection.SelectedItem.Path) + "#ctl00_ctl00_Frame_Content_tpExport");
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			exportedItems.CurrentItem = Selection.SelectedItem;
			exportedItems.DataBind();
		}

		protected void btnExport_Command(object sender, CommandEventArgs e)
		{
			ExportOptions options = ExportOptions.Default;

			if (chkPageTitle.Checked==false)
				options |= ExportOptions.TranslatePageTitle;

			if (chkPageURL.Checked == false)
				options |= ExportOptions.TranslatePageURL;

			if (chkPageName.Checked == false)
				options |= ExportOptions.TranslatePageName;

			//if (chkDetailName.Checked == false)
			//	options |= ExportOptions.TranslateDetailName;

			//if (chkDetailTitle.Checked == false)
			//	options |= ExportOptions.TranslateDetailTitle;

			if (chkPartTitle.Checked == false)
				options |= ExportOptions.TranslatePartTitle;

				options |= ExportOptions.ExcludeAttachments;
				options |= ExportOptions.ExcludePages;

			Engine.Resolve<Exporter>().Export(Selection.SelectedItem, options, Response);
		}
	}
}