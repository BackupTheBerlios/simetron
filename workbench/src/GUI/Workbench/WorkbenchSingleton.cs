//
//  WorkbenchSingleton.cs  - A workbench MVC factory
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
	using System.Collections;
	using Gtk;
	using Simetron.Logging;
	using Simetron.GUI.Core;
	using Simetron.Data;
	using Simetron.Data.Providers;

	public sealed class WorkbenchSingleton 
	{
		public static readonly WorkbenchSingleton Instance = new WorkbenchSingleton ();
		internal const string GLADEFILE = "workbench.glade";

		// many views
		ArrayList views = new ArrayList ();
		// only one view is currently active
		WorkbenchView activeView;
		// one (stateless) controller for all views
		WorkbenchController controller;
		// one model for all views
		WorkbenchModel model;

		// avoid public instantiation
		private WorkbenchSingleton () 
		{
			model = new WorkbenchModel ();
			controller = new WorkbenchController (model);
		}


		public WorkbenchView CurrentView {
			get { return activeView; }
		}

		public WorkbenchModel Model {
			get { return model; }
		}

		public WorkbenchView CreateWorkbenchView () 
		{
			WorkbenchView view = new WorkbenchView (controller);
			views.Add (view);
			activeView = view;
			Logger.Debug ("Created new top level view");
			return view;
		}

		public void RemoveWorkbenchView (WorkbenchView view) 
		{
			if (view == null) {
				Logger.Debug ("Active window is null - cannot remove");
				return;
			}
			if (views.Count == 1) {
				Quit ();
			} else {
				views.Remove (view);
				Logger.Debug ("Removed view " + view.Window.Title);
				view.Window.Destroy ();
				Logger.Debug (views.Count + " views left");

			}
		}
		
		public void Quit ()
		{
			// TODO: must save all documents before closing
			Logger.Debug ("Bye!");
			Application.Quit ();			
		}

		public void DeclareParentViewActive (object obj) 
		{
			activeView = null;
			Widget widget = obj as Widget;
			Logger.Assert (widget != null, "object is not a widget");
			Gtk.Widget window = GetToplevelWidget (widget);
			foreach (WorkbenchView view in views) {
				if (window == view.Window) {
					activeView = view;
					Logger.Debug ("Found active view for " + 
						      obj.GetType() + 
						      " : " + 
						      view.Window.Title);
					return;
				}
			}
			Logger.Debug ("Did not find the active view");
		}

		private Gtk.Widget GetToplevelWidget (Gtk.Widget w) 
		{	
			Gtk.Widget widget = w;
			Gtk.Widget parent = null;
			for (;;) {
				// TODO: Remove as it is fixed in Gtk-sharp
				// calling widget.Toplevel on a menu retrieves a widget
				// that is not the actual toplevel
				if (widget is Gtk.Menu) {
					Gtk.Menu menu = widget as Gtk.Menu;
					parent = menu.AttachWidget;
				} else {
					parent = widget.Parent;
				}
				if (parent == null) {
					break;
				}
				widget = parent;
			}
			return widget;
		}
	}
}
