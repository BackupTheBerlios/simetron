namespace Simetron.GUI.Dialogs {
	using System;
	using Gtk;
	using Glade;
	using Simetron.GUI.Workbench;
	using Simetron.Logging;
	using Simetron.Data;

	public class NewProjectDialog {
		private readonly static XML gxml = new Glade.XML (null,
					  WorkbenchSingleton.GLADEFILE,
					  "NewProjectDialog",
					  null);

		private Gtk.Dialog dialog;
		private Gtk.Entry projectNameEntry;
		private Gtk.RadioButton fsRadioButton;
		private Gtk.Label messageLabel;
		private Gtk.Button okButton;
		private string[] existingProjectNames;
	        private bool okClicked;

		public NewProjectDialog (string[] _existingProjectNames) {
			existingProjectNames = _existingProjectNames;				
			okClicked = false;
			dialog = (Dialog) gxml["NewProjectDialog"];
			projectNameEntry = (Entry) gxml["projectNameEntry"];
			fsRadioButton = (RadioButton) gxml["fsRadioButton"];
			messageLabel = (Label) gxml["messageLabel"];
			okButton = (Button) gxml["okButton"];
			gxml.Autoconnect (this);
		}			

		public void Run () {
			dialog.Run ();
		}

		public string ProjectName {
			get {
				return projectNameEntry.Text;
			}
		}

		public StoreMode Mode {
			get {
				if (fsRadioButton.Active) {
					return StoreMode.XML;
				} else {
					return StoreMode.DBMS;
				}
			}
		}
		
		public bool OkClicked {
			get {
				return okClicked;
			}
		}

		private void on_content_changed (object o, EventArgs args) {
			messageLabel.Text = "";
			string tempName = projectNameEntry.Text;
			foreach (string name in existingProjectNames) {
				if (tempName.Equals (name)) {
					messageLabel.Markup = 
						"<span foreground=\"red\">Invalid: a project with that name already exists</span>";
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
			projectNameEntry.Text = "";
			okClicked = false;
		}
	}
}
