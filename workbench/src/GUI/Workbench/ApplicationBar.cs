using System;
using Gtk;

namespace Simetron.GUI.Workbench {
	public class ApplicationBar : HBox {
		private Gtk.Statusbar statusbarLeft;
		private Gtk.Statusbar statusbarCenter;
		private Gtk.Statusbar statusbarRight;
		
		public ApplicationBar () : base (false, 0) {
			statusbarLeft = new Statusbar ();
			statusbarLeft.HasResizeGrip = false;
			statusbarCenter = new Statusbar ();
			statusbarCenter.HasResizeGrip = false;
			statusbarRight = new Statusbar ();
			this.PackStart (statusbarLeft);
			this.PackStart (statusbarCenter);
			this.PackStart(statusbarRight);
		}	      		
	}
}
