namespace Simetron.Data.NetworkTopology {
	using System;
	using Simetron.Data;

	public class XmlNetworkStore : XmlStore {
		public XmlNetworkStore () : base (typeof (Network)) {
		}

		protected override void PostReadTemplate (object obj) {
			Network network = obj as Network;
			network.Connect ();
		}

		protected override void PostWriteTemplate (object obj) {
		}
	}
}
