using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using N2.Details;
using N2.Edit.FileSystem;
using N2.Engine;
using N2.Edit.Versioning;
using N2.Engine.Globalization;

namespace N2.Persistence.Serialization
{
	public class Updater
	{
		private Engine.Logger<Importer> logger; // TODO: Figure out how/where this gets initialized.
		private readonly IPersister _persister;
		private readonly IItemXmlReader _reader;
		private readonly IFileSystem _fs;
	
		public Updater(IPersister persister, IItemXmlReader reader, IFileSystem fs)
		{
			_persister = persister;
			_reader = reader;
			_fs = fs;
		}

		public IPersister Persister
		{
			get { return _persister; }
		} 

		public virtual IImportRecord Read(string path)
		{
			using (Stream input = File.OpenRead(path))
			{
				return Read(input, Path.GetFileName(path));
			}
		}

		public virtual IImportRecord Read(Stream input, string filename)
		{
			return Read(new StreamReader(input));
		}

		public virtual IImportRecord Read(TextReader input)
		{
			XPathNavigator navigator = CreateNavigator(input);

			navigator.MoveToRoot();
			if (!navigator.MoveToFirstChild())
				throw new DeserializationException("Expected root node 'n2' not found");

			int version = ReadExportVersion(navigator);
			if (2 != version)
				throw new WrongVersionException("Invalid export version, expected 2 but was " + version);

			return _reader.Read(navigator);
		}

		protected virtual XPathNavigator CreateNavigator(TextReader input)
		{
			return new XPathDocument(input).CreateNavigator();
		}

		protected virtual int ReadExportVersion(XPathNavigator navigator)
		{
			return int.Parse(navigator.GetAttribute("exportVersion", string.Empty));
		}
				
		public virtual void Update(IImportRecord record, ContentItem destination, UpdateOption options)
		{
			
			RemoveReferences(record.ReadItems, record.RootItem);

			AssociateLanguageKeys(record.ReadItems);

			UpdateContentItem(destination, record.RootItem);
			UpdateParts(destination, record.RootItem);
			

			if ((options & UpdateOption.Children) == UpdateOption.Children)
			{
				//Could be implemented for future use...
			}
		

			if ((options & UpdateOption.Attachments) == UpdateOption.Attachments)
			{
				foreach (Attachment a in record.Attachments)
				{
					try
					{
						a.Import(_fs);
					}
					catch (Exception ex)
					{
						logger.Warn(ex);
						record.FailedAttachments.Add(a);
					}
				}
			}

			destination.DontReOrderSave = true;
			_persister.SaveRecursive(destination);
			N2.Context.Current.Resolve<N2.Edit.Trash.ITrashHandler>().PurgeAll();
	
		}


		protected virtual void RemoveReferences(IEnumerable<ContentItem> items, ContentItem referenceToRemove)
		{
			foreach (ContentItem item in items)
			{
				RemoveDetailReferences(referenceToRemove, item);
				RemoveReferencesInCollections(referenceToRemove, item);
			}
		}

		protected virtual void RemoveDetailReferences(ContentItem referenceToRemove, ContentItem item)
		{
			List<string> keys = new List<string>(item.Details.Keys);
			foreach (string key in keys)
			{
				ContentDetail detail = item.Details[key];
				if (detail.ValueType == typeof(ContentItem))
				{
					if (detail.LinkedItem == referenceToRemove)
					{
						item.Details.Remove(key);
					}
				}
			}
		}

		protected virtual void RemoveReferencesInCollections(ContentItem referenceToRemove, ContentItem item)
		{
			foreach (DetailCollection collection in item.DetailCollections.Values)
			{
				for (int i = collection.Details.Count - 1; i >= 0; --i)
				{
					ContentDetail detail = collection.Details[i];
					if (detail.ValueType == typeof(ContentItem))
					{
						if (detail.LinkedItem == referenceToRemove)
						{
							collection.Remove(referenceToRemove);
						}
					}
				}
			}
		}

	
		protected virtual void ReplaceDetailsEnclosingItem(ContentItem newEnclosingItem, Collections.IContentList<ContentDetail> item)
		{
			List<string> keys = new List<string>(item.Keys);
			foreach (string key in keys)
			{
				item[key].EnclosingItem = newEnclosingItem;
			}
		}

		protected virtual void ReplaceDetailCollectionsEnclosingItem(ContentItem newEnclosingItem, Collections.IContentList<DetailCollection> item)
		{
			List<string> keys = new List<string>(item.Keys);
			foreach (string key in keys)
			{
				item[key].EnclosingItem = newEnclosingItem;
			}


		}

		private void UpdateContentItem(ContentItem destination, ContentItem source)
		{
			destination.AlteredPermissions = source.AlteredPermissions;
			destination.Expires  = source.Expires;
			destination.Name  = source.Name;
			destination.State = source.State;
			destination.Title = source.Title;
			destination.Visible = source.Visible;

			UpdateDetails(destination.Details, source.Details, destination);
			UpdateDetailCollections(destination.DetailCollections, source.DetailCollections, destination);
		
		}

		protected virtual void UpdateDetails(Collections.IContentList<ContentDetail> target, Collections.IContentList<ContentDetail> source, ContentItem targetItem)
		{
			ReplaceDetailsEnclosingItem(targetItem, source);
			
			List<string> keys = new List<string>(target.Keys);
			foreach (string key in keys)
			{
				
				ContentDetail detail = target[key];
				
				
				//Don't update Parent and Versionkey
				if (target[key].Name == "ParentID" || target[key].Name == "VersionKey")
					continue;

				
				//Update Values 
				if (source.Keys.Contains(key))
				{

						//Check Ignore Attribute
						var prop = targetItem.GetType().GetProperty(key);

						//Details left in db not used as property anymore check
						if (prop != null)
						{
							if (Attribute.IsDefined(prop, typeof(N2.Details.XMLUpdaterIgnoreAttribute)))
								continue;
						}

						target[key].ObjectValue = source[key].BoolValue;
						target[key].DateTimeValue = source[key].DateTimeValue;
						target[key].DoubleValue = source[key].DoubleValue;
						target[key].IntValue = source[key].IntValue;
						target[key].Meta = source[key].Meta;
						target[key].ObjectValue = source[key].ObjectValue;
						target[key].StringValue = source[key].StringValue;
						target[key].Value = source[key].Value;
				
				}
				else
				{
					target.Remove(detail);					
				}
			}

			//Add none existing Details from source
			keys = new List<string>(source.Keys);
			foreach (string key in keys)
			{
				ContentDetail detail = source[key];

				//Ignore Parent and Versionkey
				if (detail.Name == "ParentID" && detail.Name == "VersionKey")
					continue;

				if (target.Keys.Contains(key) == false)
				{
					detail.ID = 0;
					target.Add(detail);
				}
			}
		}
		
		protected virtual void UpdateDetailCollections(Collections.IContentList<DetailCollection> target, Collections.IContentList<DetailCollection> source, ContentItem targetItem)
		{
			ReplaceDetailCollectionsEnclosingItem(targetItem, source);
			

			List<string> keys = new List<string>(target.Keys);
			foreach (string key in keys)
			{
				DetailCollection detailCollection = target[key];

				if (detailCollection.Name == "TrackedLinks")
					continue;

				if (source.Keys.Contains(key))
				{
					UpdateDetails(detailCollection.Details as Collections.IContentList<ContentDetail>, source[key].Details as Collections.IContentList<ContentDetail>, targetItem);
				}
				else
					target.Remove(detailCollection);
				
			}
			
			keys = new List<string>(source.Keys);
			foreach (string key in keys)
			{
				DetailCollection detailCollection = source[key];

				if (detailCollection.Name == "TrackedLinks")
					continue;

				if (target.Keys.Contains(key) == false)
				{
					detailCollection.ID = 0;
					target.Add(detailCollection);
				}
			}

		}

		protected virtual void UpdateParts(ContentItem target, ContentItem source)
		{

			List<string> keysTarget = new List<string>(target.GetChildren(new Collections.PartFilter()).Keys);
			List<string> keysSource = new List<string>(source.GetChildren(new Collections.PartFilter()).Keys);
			System.Collections.Generic.Dictionary<int, ContentItem> toCopy = new System.Collections.Generic.Dictionary<int, ContentItem>();

			bool bDelete = true;
			bool bAdd = true;


			foreach (string keyTarget in keysTarget)
			{
				ContentItem childTarget = target.Children[keyTarget];


				foreach (string keySource in keysSource)
				{
					ContentItem childSource = source.Children[keySource];

					if (childSource.TranslationKey == childTarget.TranslationKey)
					{
						UpdateContentItem(childTarget, childSource);
						childTarget.SortOrder = childSource.SortOrder;
						bDelete = false;
					}

				}

				if (bDelete)
				{
					_persister.Delete(childTarget);
					//N2.Context.Current.Resolve<N2.Edit.Trash.ITrashHandler>().Purge(childTarget);
				}
				else
					bDelete = true;
			}


			keysTarget = new List<string>(target.GetChildren(new Collections.PartFilter()).Keys);

			foreach (string keySource in keysSource)
			{
				ContentItem childSource = source.Children[keySource];

				foreach (string keyTarget in keysTarget)
				{
					ContentItem childTarget = target.Children[keyTarget];

					if (childSource.TranslationKey == childTarget.TranslationKey)
					{
						bAdd = false;
					}
				}

				if (bAdd)
				{
					childSource.ID = 0;
					childSource.Parent = null;
					toCopy.Add(source.Children.IndexOf(childSource), childSource);
				}
				else
					bAdd = true;

			}

			ContentItem copyChild;
			foreach (int key in toCopy.Keys)
			{
				if (toCopy.TryGetValue(key, out copyChild))
				{
					copyChild.InsertAt(target, key);
				}
			}

		}

		protected virtual void AssociateLanguageKeys(IEnumerable<ContentItem> items)
		{
			foreach (ContentItem item in items)
			{
				if (item.TranslationKey == null)
				{
					item.TranslationKey = item.ID;
					ContentItem currentItem = _persister.Get(item.ID);
					
					if (currentItem != null)
					{
						currentItem.TranslationKey = item.ID;
						_persister.Save(currentItem);
					}
				}
			}
		}
		
	}
}
