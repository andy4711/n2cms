using System;
using System.Web.UI;


namespace N2.Details
{
	/// <summary>Ignore property/detail while updating pages with XMLImporter class.</summary>
	[AttributeUsage(AttributeTargets.Property) ]
	public class XMLTranslateAttribute : Attribute
	{
	}
}


