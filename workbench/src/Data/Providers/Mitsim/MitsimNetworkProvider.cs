//
// MitsimNetworkProvider.cs  - A provider for MITSIM network files
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group, Inc.  http://www.olympum.com
// All Rights Reserved
//

namespace Simetron.Data.Providers.Mitsim
{
	using System;
	using Simetron.Data.NetworkTopology;
	using Simetron.Data.Providers;

	public class MitsimNetworkProvider : MitsimProvider
	{
		public override Type Type 
		{
			get { return typeof (Network); }
		}

		public override object Read () 
		{
			return null;
		}

		public override void Write (object obj) 
		{
		}
	}
}
