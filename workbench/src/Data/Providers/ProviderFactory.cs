//
// ProviderFactory.cs  - A unified factory for Simetron providers
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
// All Rights Reserved
//

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
			Type[] providers = (Type[]) 
				ConfigurationSettings.GetConfig("simetron.data/providers");
			if (providers == null) {
				throw new ConfigurationException (
					"Provider configuration not available!");
			}
			foreach (Type provider in providers) {
				RegisterProvider (provider);
			}
		}

		public static void RegisterProvider (Type provider)
		{
			// discover the underlying type and mode by creating
			// a dummy instance
			IProvider instance = null;
			try {
				instance = NewInstance (provider);
			} catch (Exception e) {
				Logger.Fail (provider + 
					     " was removed: initialization exception");
				Logger.Debug (e.ToString());
				return;
			}
			Type type = instance.Type;
			ProviderMode mode = instance.Mode;

			Hashtable subproviders = (Hashtable) providers[mode];
			if (!providers.Contains (mode)) {
				subproviders = new Hashtable ();
				providers[mode] = subproviders;
			}

			subproviders[type] = provider;
			Logger.Debug ("Registered provider " + provider + 
				      " for " + type + " in mode " + mode);
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
				Logger.Debug ("Set default provider mode to " + 
					      defaultMode);
			}
		}

		public static ICollection GetProviderModes () 
		{
			// providers keys are ProviderMode
			return providers.Keys;
		}

		public static ICollection GetRegisteredTypes (ProviderMode mode)
		{
			Hashtable subproviders = (Hashtable) providers[mode];
			if (subproviders == null)
				return null;
			// subproviders keys are Type
			return subproviders.Keys;
		}

		private static IProvider NewInstance (Type type)
		{
			ConstructorInfo cInfo = type.GetConstructor (Type.EmptyTypes);
			object instance = cInfo.Invoke (null);
			return instance as IProvider;			
		}

		private ProviderFactory () {
		}
	}
}
