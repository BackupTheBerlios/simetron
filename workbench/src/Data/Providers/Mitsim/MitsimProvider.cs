//
// MitsimProvider.cs  - An abstract provider for MITSIM
//
// Author:
//   Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
// Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
// All Rights Reserved
//

namespace Simetron.Data.Providers.Mitsim
{
	using Simetron.Data.Providers;

	public abstract class MitsimProvider : FileProvider 
	{
		public override ProviderMode Mode {
			get { return ProviderMode.Mitsim; }
		}
	 }
}
