using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Persistence.Serialization;
using System.IO;
using N2.Xml;
using N2.Engine;
using N2.Edit.Web;
using N2.Edit;
using System.Text;
using N2.Edit.FileSystem;

namespace N2.Management.Content.Globalization
{
	public partial class ItemXmlUpdate : EditUserControl
	{
		public string UploadedFilePath
		{
			get { return (string)(ViewState["UploadedFilePath"] ?? ""); }
			set { ViewState["UploadedFilePath"] = value; }
		}

		public void ContinueWithUpdate(string tempFile)
		{
			UploadedFilePath = tempFile;

			try
			{
				IImportRecord record = Engine.Resolve<Updater>().Read(UploadedFilePath);
				updatedItems.CurrentItem = record.RootItem;
				rptAttachments.DataSource = record.Attachments;
				if (Selection.SelectedItem.GetContentType().Equals(record.RootItem.GetContentType()) != true)
				{
					pnlTypeMissmatch.Visible = true;
				}
				ShowErrors(record);
			}
			catch (WrongVersionException)
			{
				using (Stream s = File.OpenRead(UploadedFilePath))
				{
					N2XmlReader xr = new N2XmlReader(N2.Context.Current);
					updatedItems.CurrentItem = xr.Read(s);
				}
			}

			DataBind();
		}

		protected void btnUpdateUploaded_Click(object sender, EventArgs e)
		{
			Updater updater = Engine.Resolve<Updater>();

			IImportRecord record;
			try
			{
				record = updater.Read(UploadedFilePath);
				ShowErrors(record);
			}
			catch (WrongVersionException)
			{
				N2XmlReader xr = new N2XmlReader(N2.Context.Current);
				ContentItem item = xr.Read(File.OpenRead(UploadedFilePath));
				record = CreateRecord(item);
			}

			Update(updater, record);
		}

		private static IImportRecord CreateRecord(ContentItem item)
		{
			ReadingJournal rj = new ReadingJournal();
			rj.Report(item);
			return rj;
		}

		private void Update(Updater updater, IImportRecord record)
		{
			try
			{

				UpdateOption updateParameter = new UpdateOption();

				if (chkSkipAttachments.Checked)
				{
					updateParameter = updateParameter | UpdateOption.Attachments;
				}

				//if (chkUpdateChilds.Checked)
				//{
				//	updateParameter = updateParameter | UpdateOption.Children;
				//}

				updater.Update(record, Selection.SelectedItem, updateParameter);
				Refresh(Selection.SelectedItem, ToolbarArea.Both);
				
				ShowErrors(record);
			}
			catch (N2Exception ex)
			{
				cvUpdate.ErrorMessage = ex.Message;
				cvUpdate.IsValid = false;
				btnUpdateUploaded.Enabled = false;
			}
			finally
			{
				if (File.Exists(UploadedFilePath))
					File.Delete(UploadedFilePath);
			}
		}

		void ShowErrors(IImportRecord record)
		{
			if (record.Errors.Count > 0)
			{
				StringBuilder errorText = new StringBuilder("<ul>");
				foreach (Exception ex in record.Errors)
				{
					errorText.Append("<li>").Append(ex.Message).Append("</li>");
				}
				errorText.Append("</ul>");

				cvUpdate.IsValid = false;
				cvUpdate.ErrorMessage = errorText.ToString();
			}
		}

		internal void UpdaterNow(HttpPostedFile postedFile)
		{
			Updater updater = Engine.Resolve<Updater>();

			IImportRecord record;
			try
			{
				record = updater.Read(postedFile.InputStream, postedFile.FileName);
			}
			catch (WrongVersionException)
			{
				N2XmlReader xr = new N2XmlReader(N2.Context.Current);
				ContentItem item = xr.Read(postedFile.InputStream);
				record = CreateRecord(item);
				ShowErrors(record);
			}

			Update(updater, record);
		}

		protected string CheckExists(string url)
		{
			if (Engine.Resolve<IFileSystem>().FileExists(url))
				return "(existing file will be overwritten)";
			return string.Empty;
		}
	}
}