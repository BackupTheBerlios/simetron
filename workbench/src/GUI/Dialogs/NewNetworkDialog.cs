namespace Simetron.GUI.Dialogs {
	using System;
	using System.Collections;
	using Gtk;
	using Glade;
	using Simetron.GUI.Workbench;
	using Simetron.Logging;
	using Simetron.Data;

	public class NewNetworkDialog {
		private readonly static XML gxml = new Glade.XML (null,
					  WorkbenchSingleton.GLADEFILE,
					  "NewNetworkDialog",
					  null);

		private Gtk.Dialog dialog;
		private Gtk.Combo parentProjectCombo;
		private Gtk.Entry networkNameEntry;
		private Gtk.Label messageLabel;
		private Gtk.Button okButton;
	        private bool okClicked;
		private Hashtable names;

		public NewNetworkDialog (Hashtable names) {
			this.names = names;
			okClicked = false;
			dialog = (Dialog) gxml["NewNetworkDialog"];
			networkNameEntry = (Entry) gxml["networkNameEntry"];
			parentProjectCombo = (Combo) gxml["parentProjectCombo"];
			messageLabel = (Label) gxml["messageLabel"];
			okButton = (Button) gxml["okButton"];

			// populate combo box

			ICollection keyCollection = names.Keys;
			int count = keyCollection.Count;
			string[] projects = new string[count];
			keyCollection.CopyTo (projects, 0);
			
			parentProjectCombo.SetPopdownStrings (projects);

			// end populate

			gxml.Autoconnect (this);
		}			

		public void Run () {			
			dialog.Run ();
		}

		public string NetworkName {
			get {
				return networkNameEntry.Text;
			}
		}

		public string ProjectName {
			get {
				return parentProjectCombo.Entry.Text;
			}
		}
		
		public bool OkClicked {
			get {
				return okClicked;
			}
		}

		private void on_content_changed (object o, EventArgs args) {
			messageLabel.Text = "";
			string projectName = parentProjectCombo.Entry.Text;
			Logger.Debug ("Project name : " + projectName);
			string[] existingNetworkNames = (string[]) names[projectName];
			if (existingNetworkNames == null) {
				return;
			}
			string networkName = networkNameEntry.Text;
			foreach (string name in existingNetworkNames) {
				if (networkName != null && networkName.Equals (name)) {
					messageLabel.Markup = 
						"<span foreground=\"red\">Invalid: a network with that name already exists</span>";
					okButton.Sensitive = false;
					return;
				}
			}
			okButton.Sensitive = true;
		}
		
		private void on_okbutton_clicked (object o, EventArgs args) {
			dialog.Hide ();
			okClicked = true;
		}
		
		private void on_cancelbutton_clicked (object o, EventArgs args) {
			dialog.Hide ();
			networkNameEntry.Text = "";
			okClicked = false;
		}
	}
}
