//
//  WorkbenchController.cs  - The workbench controller part of the MVC
//
//  Author:
//    Bruno Fernandez-Ruiz (brunofr@olympum.com)
//
//  Copyright (c) 2003 The Olympum Group,  http://www.olympum.com
//  All Rights Reserved
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//

namespace Simetron.GUI.Workbench 
{
	using System;
	using Gdk;
	using Gtk;
	using GtkSharp;
	using Simetron.Logging;
	using Simetron.GUI.Core;
	using Simetron.GUI.Commands;
	using Simetron.GUI.Editors;

	public class WorkbenchController 
	{
		public WorkbenchController (WorkbenchModel model) 
		{
			model.OnDocumentAdded += new EventHandler (OnDocumentAddedHandler);
			model.OnDocumentRemoved += new EventHandler (OnDocumentRemovedHandler);
		}

		//
		// DOCUMENT HANDLERS
		//
		void OnDocumentAddedHandler (object obj, EventArgs args)
		{
			Document document = (Document) obj;
			Type editorType = document.Object.GetType ();
			IEditor editor = EditorFactory.Instance.GetEditor (editorType);
			editor.Model = document;

			WorkbenchView view = WorkbenchSingleton.Instance.CurrentView;
			EditorNotebook notebook = view.EditorNotebook;
			notebook.AppendEditor (editor);
		}

		void OnDocumentRemovedHandler (object obj, EventArgs args)
		{
		}

		// 
		// WINDOW DELETE HANDLER
		// 
		void OnWorkbenchDeleteEvent(object o, DeleteEventArgs args) 
		{
			SignalArgs sa = (SignalArgs) args;
			WorkbenchSingleton.Instance.DeclareParentViewActive (o);
			ICommand command = CommandFactory.CreateCommand("CloseWindowCommand");
			command.Run ();
			// retval true avoids closing the window
			// the window should have been closed by the command
			// and if no windows left, the application quitted
			sa.RetVal = true;
		}

		//
		// FILE MENU
		//
		void on_new_network_activate (object source, EventArgs args)
		{
		}

		void on_open_activate (object source, EventArgs args)
		{
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("OpenDocumentCommand");
			command.Run ();		
		}

		void on_import_activate (object source, EventArgs args) 
		{
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("ImportCommand");
			command.Run ();
		}

		void on_export_activate (object source, EventArgs args) 
		{
		}

		void on_save_activate (object source, EventArgs args) 
		{
		}

		void on_save_as_activate (object source, EventArgs args) 
		{
		}

		void on_close_activate (object source, EventArgs args) 
		{
		}

		void on_new_project_activate (object source, EventArgs args) 
		{
		}

		void on_open_project_activate (object source, EventArgs args) 
		{
		}

		void on_import_project_activate (object source, EventArgs args) 
		{
		}

		void on_save_project_activate (object source, EventArgs args) 
		{
		}

		void on_close_project_activate (object source, EventArgs args) 
		{
		}

		void on_page_setup_activate (object source, EventArgs args) 
		{
		}

		void on_print_preview_activate (object source, EventArgs args) 
		{
		}

		void on_print_activate (object source, EventArgs args) 
		{
		}

		void on_close_window_activate (object source, EventArgs args) 
		{
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("CloseWindowCommand");
			command.Run ();
		}

		void on_exit_activate (object source, EventArgs args) 
		{
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("QuitCommand");
			command.Run ();
		}

		//
		// EDIT MENU
		//

		void on_undo_activate (object source, EventArgs args) 
		{
		}

		void on_redo_activate (object source, EventArgs args) 
		{
		}

		void on_cut_activate (object source, EventArgs args) 
		{
		}

		void on_copy_activate (object source, EventArgs args) 
		{
		}

		void on_paste_activate (object source, EventArgs args) 
		{
		}

		void on_delete_activate (object source, EventArgs args) 
		{
		}

		void on_select_all_activate (object source, EventArgs args) 
		{
		}

		void on_preferences_activate (object source, EventArgs args) 
		{
		}

		//
		// VIEW MENU
		//
		void on_standard_toolbar_activate (object source, EventArgs args) 
		{
		}

		void on_zoom_toolbar_activate (object source, EventArgs args) 
		{
		}

		void on_execution_toolbar_activate (object source, EventArgs args) 
		{
		}

		void on_search_toolbar_activate (object source, EventArgs args) 
		{
		}

		void on_statusbar_activate (object source, EventArgs args) 
		{
		}

		void on_explorer_activate (object source, EventArgs args) 
		{
		}

		void on_console_activate (object source, EventArgs args) 
		{
		}

		void on_display_grid_activate (object source, EventArgs args) 
		{
		}

		void on_snap_to_grid_activate (object source, EventArgs args) 
		{
		}

		void on_new_window_activate (object source, EventArgs args) 
		{
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = 
				CommandFactory.CreateCommand("NewWindowCommand");
			command.Run ();			
		}

		//
		// PROJECT MENU
		//

		void on_add_new_file_activate (object source, EventArgs args) 
		{
		}

		void on_add_existing_file_activate (object source, EventArgs args) 
		{
		}

		void on_project_properties_activate (object source, 
							     EventArgs args) 
		{
		}

		//
		// DOCUMENTS MENU
		//

		void on_save_all_activate (object source, EventArgs args) 
		{
		}

		void on_close_all_activate (object source, EventArgs args) 
		{
		}

		//
		// HELP MENU
		//
		void on_contents_activate (object source, EventArgs args) 
		{
		}

		void on_about_activate (object source, EventArgs args) 
		{
			WorkbenchSingleton.Instance.DeclareParentViewActive (source);
			ICommand command = CommandFactory.CreateCommand("AboutCommand");
			command.Run ();			
		}

		//
		// STANDARD TOOLBAR
		//

		//
		// ZOOM TOOLBAR
		//

		//
		// EXECUTION TOOLBAR
		//

		//
		// SEARCH TOOLBAR
		//
	}
}
