namespace Simetron.GUI.Workbench {
	using System;
	using Gdk;
	using Gtk;
	using GtkSharp;
	using Simetron.Logging;
	using Simetron.GUI.Commands;

	public class WorkbenchController {
		public WorkbenchController () {}

		private void OnWorkbenchDeleteEvent(object o, DeleteEventArgs args) {
			SignalArgs sa = (SignalArgs) args;
			WorkbenchSingleton.Instance.DeclareParentViewActive (o);
			ICommand command = CommandFactory.CreateCommand("QuitCommand");
			if (command.Run ()) {
				sa.RetVal = true;
			} else {
				sa.RetVal = false; // avoid closing window
			}
		}

		// menu items handlers

		
		private void on_project_activate (object source, EventArgs args) {
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("NewProjectCommand");
			command.Run ();			
		}

		private void on_network_activate (object source, EventArgs args) {
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("NewNetworkCommand");
			command.Run ();			
		}

		private void on_save_activate (object source, EventArgs args) {
		}

		private void on_save_all_activate (object source, EventArgs args) {
		}

		private void on_close_activate (object source, EventArgs args) {
		}

		private void on_close_all_activate (object source, EventArgs args) {
		}

		private void on_import_activate (object source, EventArgs args) {
		}

		private void on_print_activate (object source, EventArgs args) {
		}

		private void on_close_window_activate (object source, EventArgs args) {
		}

		private void on_exit_activate (object source, EventArgs args) {
		}

		private void on_explorer_bar_activate (object source, EventArgs args) {
		}

		private void on_output_bar_activate (object source, EventArgs args) {
		}

		private void on_new_window_activate (object source, EventArgs args) {
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("NewWindowCommand");
			command.Run ();			
		}

		private void on_about_activate (object source, EventArgs args) {
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("AboutCommand");
			command.Run ();			
		}


		// View event handlers
		private void OnLeftPaneMenuItemToggled (object o, EventArgs args) {
//			workbench.Perspective.LeftNotebookVisible = 
//				!workbench.Perspective.LeftNotebookVisible;
		}
			
		private void OnBottomPaneMenuItemToggled (object o, EventArgs args) {
//			workbench.Perspective.BottomNotebookVisible = 
//				!workbench.Perspective.BottomNotebookVisible;
		}


		private void OnCloseButtonClicked (object o, EventArgs args) {
			WorkbenchSingleton.Instance.DeclareParentViewActive (o);
			ICommand command = CommandFactory.CreateCommand("QuitCommand");
			command.Run ();
		}
			

		private void OnQuitActivate (object o, EventArgs args) {
			WorkbenchSingleton.Instance.DeclareParentViewActive (o);
			ICommand command = CommandFactory.CreateCommand("QuitCommand");
			command.Run ();
		}
			
		private void OnNewButtonClicked (object o, EventArgs args) {
			ICommand command = CommandFactory.CreateCommand("NewDocumentCommand");
			command.Run ();			
		}
			
		private void OnOpenButtonClicked (object o, EventArgs args) {
			ICommand command = CommandFactory.CreateCommand("OpenDocumentCommand");
			command.Run ();
		}
			
		private void OnSaveButtonClicked (object o, EventArgs args) {
			ICommand command = CommandFactory.CreateCommand("SaveDocumentCommand");
			command.Run ();
		}
			
		private void OnNewActivate (object o, EventArgs args) {
			ICommand command = CommandFactory.CreateCommand("NewDocumentCommand");
			command.Run ();
		}
			
		private void OnSaveActivate (object o, EventArgs args) {
			ICommand command = CommandFactory.CreateCommand("SaveDocumentCommand");
			command.Run ();
		}
			
		private void OnOpenActivate (object o, EventArgs args) {
			ICommand command = CommandFactory.CreateCommand("OpenDocumentCommand");
			command.Run ();
		}
			
		private void OnCloseActivate (object o, EventArgs args) {
			ICommand command = CommandFactory.CreateCommand("CloseDocumentCommand");
			command.Run ();
		}		

		// Document event handlers
		public void OnDocumentAdded (object o, EventArgs args) {
			Logger.Debug ("User has selected an object of type " +  o.GetType ());
//			workbench.Perspective.OpenActiveDocument (o);		
		}
	}
}
