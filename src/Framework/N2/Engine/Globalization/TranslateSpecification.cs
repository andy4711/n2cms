using N2.Definitions;
using N2.Edit;
using N2.Web;
using System.Linq;

namespace N2.Engine.Globalization
{
	/// <summary>
	/// Information used by the edit interface about an existing or potential translation.
	/// </summary>
	public class TranslateSpecification
	{
		public TranslateSpecification(string editUrl, string importLanguageUrl, string updateLanguageUrl, string exportLanguageUrl, ILanguage language, ContentItem existingItem, ItemDefinition definition, Site site)
		{
			EditUrl = editUrl;
			ImportLanguageUrl = importLanguageUrl;
			UpdateLanguageUrl = updateLanguageUrl;
			ExportLanguageUrl = exportLanguageUrl;
			Language = language;
			ExistingItem = existingItem;
			Definition = definition;
			Site = site;
			IsTranslatable = true;
		}

		public bool IsTranslatable { get; set; }

		public ItemDefinition Definition { get; set; }

		public ILanguage Language { get; set; }

		public bool IsNew
		{
			get { return ExistingItem == null; }
		}

		public string EditUrl { get; set; }

		public string ImportLanguageUrl { get; set; }

		public string UpdateLanguageUrl { get; set; }

		public string ExportLanguageUrl { get; set; }

		public ContentItem ExistingItem { get; set; }

		public Site Site { get; set; }
	}
}
