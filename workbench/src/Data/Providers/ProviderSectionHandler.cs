namespace Simetron.Data.Providers
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Xml;
 
	public class ProviderSectionHandler : IConfigurationSectionHandler
	{
		public object Create (object parent, object configContext, XmlNode section)
		{
			ArrayList providers = new ArrayList ();

			foreach (XmlElement node in section.ChildNodes) {
				string name = GetStringValue(node,"name",true);
				Type type = Type.GetType (name);
				providers.Add(type);
			}

			string mode = section.Attributes["default-mode"].Value;
			ProviderMode defaultMode = (ProviderMode) Enum.Parse (typeof (ProviderMode), mode);
			ProviderFactory.DefaultMode = defaultMode;

			return providers.ToArray (typeof (Type));
		}
		
		private string GetStringValue(XmlNode _node, string _attribute, bool required)
		{
			XmlNode a = _node.Attributes.RemoveNamedItem(_attribute);
			if (a==null) {
				if (required) {
					throw new ConfigurationException("Attribute required: " + 
									  _attribute);
				} else {
					return null;
				}
			}
			return a.Value;
		}
	}
}
