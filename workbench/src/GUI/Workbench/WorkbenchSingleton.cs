namespace Simetron.GUI.Workbench {
	using System;
	using System.Collections;
	using Gtk;
	using Simetron.Logging;
	using Simetron.GUI.Core;
	using Simetron.Data;
	using Simetron.Data.NetworkTopology;
	using Simetron.Data.Providers;

	public sealed class WorkbenchSingleton {
		internal const string GLADEFILE = "workbench.glade";

		private ArrayList views = new ArrayList ();
		private WorkbenchView activeView;
		private WorkbenchController controller;
		//private Workspace model;

		private WorkbenchSingleton () {
			ProviderFactory.GetProvider (typeof (Network));
			// read metadata.xml using StoreFactory
			/*
			IStore store = StoreFactory.Instance.CreateStore (typeof (Workspace),
									  StoreMode.XML);
			store.OpenConnection (GSimetronMain.MetadataFile);
			model = (Workspace) store.Read ();
			store.CloseConnection ();
			*/
			controller = new WorkbenchController ();
		}

		public static readonly WorkbenchSingleton Instance = new WorkbenchSingleton ();

		public WorkbenchView ActiveWorkbenchView {
			get { return activeView; }
		}

		//public Workspace Workspace {
		//	get { return model; }
		//}

		public WorkbenchView CreateWorkbenchView () {
			WorkbenchView view = new WorkbenchView (controller);
			views.Add (view);
			Logger.Debug ("Created new top level view");
			return view;
		}

		public void RemoveWorkbenchView (WorkbenchView view) {
			if (view == null) {
				Logger.Debug ("Active window is null - cannot remove");
				return;
			}
			views.Remove (view);
			Logger.Debug ("Removed a top level view " + view.Window.Title);
			if (views.Count == 0) {
				Logger.Debug ("No views left - quitting");
				Application.Quit ();
			} else {
				Logger.Debug ("There are " + views.Count + " views left");
			}
		}

		public void DeclareParentViewActive (object obj) {
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

		private Gtk.Widget GetToplevelWidget (Gtk.Widget w) {	
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
					// TODO: Remove as it is fixed in Gtk-sharp
					// calling Parent on a toplevel window throws an 
					// exception
					// This hack won't work if we use windows
					// that are not toplevel
					if (widget is Gtk.Window) {
						break;
					}
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
