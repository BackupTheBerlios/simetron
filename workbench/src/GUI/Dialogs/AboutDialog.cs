namespace Simetron.GUI.Dialogs {
	using System;
	using Gtk;
	using Gdk;
	using Glade;
	using Simetron.GUI.Workbench;

	public class AboutDialog {
		private readonly static Gdk.Pixbuf icon = new Pixbuf (null, 
						     "pixmaps/simetron-hicolor-48x48.png");
		private readonly static XML gxml = new Glade.XML (null,
					  WorkbenchSingleton.GLADEFILE,
					  "AboutDialog",
					  null);

		private Gtk.Dialog dialog;
		private Gtk.Label buildLabel;

		public AboutDialog () {
			dialog = (Dialog) gxml["AboutDialog"];
			buildLabel = (Label) gxml["buildLabel"];
			Gtk.Image image = (Gtk.Image) gxml["image"];
			image.Pixbuf = icon;
			gxml.Autoconnect (this);
		}			

		public void Run () {			
			dialog.Run ();
		}

		private void on_closebutton_clicked (object o, EventArgs args) {
			dialog.Hide ();
		}
	}
}
