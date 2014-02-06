using System;

namespace N2.Persistence.Serialization
{
	[Flags]
	public enum UpdateOption
	{
		/// <summary>Update children .</summary>
		Children = 0,
		/// <summary>Import attachment overwriting any existing files.</summary>
		Attachments = 1
	}
}
