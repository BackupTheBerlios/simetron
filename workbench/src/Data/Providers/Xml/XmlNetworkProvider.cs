//
// XmlNetworkProvider.cs  - A provider for Simetron Xml network files
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group, Inc.  http://www.olympum.com
// All Rights Reserved
//

namespace Simetron.Data.Providers.Xml
{
	using System;
	using System.Data;
	using Simetron.Data.NetworkTopology;
	using Simetron.Data.Providers;
	using Simetron.Logging;

	public class XmlNetworkProvider : XmlProvider
	{
		public XmlNetworkProvider () : base (typeof (Network))
		{
		}

		protected override void PostReadTemplate (object obj) 
		{
			Logger.Debug ("Completed reading network");
		}

		protected override void PostWriteTemplate (object obj)
		{
			Logger.Debug ("Completed writing network");
		}

		public override Type Type 
		{
			get { return typeof (Network); }
		}
	}
}
