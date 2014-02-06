using System;

namespace N2.Details
{
    /// <summary>Class applicable attribute used for xml translation expor.</summary>
    /// <example>
	/// [N2.Details.WithEditableTranslateAttribute("Display in navigation", 11)]
    /// public abstract class AbstractBaseItem : N2.ContentItem 
    /// {
    ///	}
    /// </example>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class WithEditableTranslateAttribute : EditableCheckBoxAttribute
    {
		/// <summary>
		/// Creates a new instance of the WithEditableTranslateAttribute class with default values.
		/// </summary>
		public WithEditableTranslateAttribute()
			: this("Don't translate", 0)
		{
		}

		/// <summary>
		/// Creates a new instance of the WithEditableTranslateAttribute class with default values.
		/// </summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public WithEditableTranslateAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			Name = "DontTranslate";
		}
	}
}
