namespace Simetron.GUI.Editors {	
	using System;
	using Gtk;
	using Simetron.GUI.Core;
	using Simetron.Data.NetworkTopology;

	public class NetworkEditor : Label, IEditor {
		Document document;
		
		public NetworkEditor () : base ("Test") {
		}

		public Document Model { 
			get {
				return document;
			}
			set {
				document = value;
			}
		}		
	}
}
