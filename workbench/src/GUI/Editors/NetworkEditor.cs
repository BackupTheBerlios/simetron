using System;
using Gtk;
using Simetron.Data.NetworkTopology;

namespace Simetron.GUI.Editors {	
	public class NetworkEditor : Label {
		Network network;

		public NetworkEditor (Network network) : base (network.Label) {
			this.network = network;
		}
	}
}
