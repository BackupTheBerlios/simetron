namespace Simetron.Data {
	using System;
	using System.Collections;
	using System.IO;
	using Simetron.Data.NetworkTopology;
	using Simetron.Logging;


	public sealed class StoreFactory {
		public readonly static StoreFactory Instance = new StoreFactory ();
		private Hashtable[] stores;

		private StoreFactory () {
			stores = new Hashtable [2];
			stores[(int)StoreMode.XML] = new Hashtable ();
			stores[(int)StoreMode.DBMS] = new Hashtable ();
			// TODO: put this in the config file for this assembly
			stores[(int)StoreMode.XML][typeof (Workspace)] = new XmlWorkspaceStore ();
			stores[(int)StoreMode.XML][typeof (Project)] = new XmlProjectStore ();
			stores[(int)StoreMode.XML][typeof (Network)] = new XmlNetworkStore ();
			// END TODO
		}

		public void RegisterStore (Type type, IStore store) {
			StoreMode mode = store.Mode;
			stores[(int)mode][type] = store;
		}

		public IStore CreateStore (Type type, StoreMode mode) {
			return (IStore) stores[(int)mode][type];
		}
	}
}
