namespace Simetron.Data.Providers
{
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Reflection;
	using Simetron.Logging;

	public sealed class ProviderFactory
	{
		private readonly static Hashtable providers = new Hashtable ();
		private static ProviderMode defaultMode;

		static ProviderFactory () 
		{
			Type[] providers = 
				(Type[]) ConfigurationSettings.GetConfig("simetron.data/providers");
			if (providers == null) {
				throw new ConfigurationException ("Provider configuration not available" +
								  "- check simetron.config");
			}
			foreach (Type provider in providers) {
				RegisterProvider (provider);
			}
		}

		public static void RegisterProvider (Type provider)
		{
			// discover the underlying type and mode by creating
			// a dummy instance
			IProvider instance = NewInstance (provider);
			Logger.Assert (instance != null, "Type is not a provider");
			Type type = instance.Type;
			ProviderMode mode = instance.Mode;

			Hashtable subproviders = (Hashtable) providers[mode];
			if (!providers.Contains (mode)) {
				subproviders = new Hashtable ();
				providers[mode] = subproviders;
			}

			subproviders[type] = provider;
			Logger.Debug ("Registered provider " + provider + " for " + type + " in mode " + mode);
		}

		public static IProvider GetProvider (Type type, ProviderMode mode)
		{
			Logger.Assert (type != null);
			Type provider = null;
			Hashtable subproviders = (Hashtable) providers[mode];
			if (subproviders != null) {
				provider = (Type) subproviders[type];
			}
			return NewInstance (provider);				
		}

		public static IProvider GetProvider (Type type)
		{
			return GetProvider (type, defaultMode);
		}
	      
		public static ProviderMode DefaultMode {
			get { return defaultMode; }
			set { 
				defaultMode = value;
				Logger.Debug ("Set default provider mode to " + defaultMode);
			}
		}

		private static IProvider NewInstance (Type type)
		{
			ConstructorInfo cInfo = type.GetConstructor (Type.EmptyTypes);
			object instance = cInfo.Invoke (null);
			return instance as IProvider;			
		}
	}
}
