namespace Simetron.GUI.Workbench {
	using System;
	using Gdk;
	using Gtk;
	using GtkSharp;
	using Glade;
	using Simetron.GUI.Commands;
	using Simetron.GUI.Core;

	public class WorkbenchView {
		// load the icon from the default assembly
		private readonly static Gdk.Pixbuf icon = new Pixbuf (null, 
						     "pixmaps/simetron-hicolor-48x48.png");

		private Gtk.Window window;
		private ApplicationBar appbar;
		private Perspective perspective;

		public WorkbenchView (WorkbenchController controller) {
			SplashWindow.Update ("Loading GUI definition");
			XML gxml = new Glade.XML (null,
						  WorkbenchSingleton.GLADEFILE,
						  "Workbench",
						  null);

			// get the widgets from glade
			window = (Gtk.Window) gxml["Workbench"];
			Gtk.MenuBar menubar = (Gtk.MenuBar) gxml["menubar"];
			Gtk.Toolbar toolbar = (Gtk.Toolbar) gxml["toolbar"];		       
			Gtk.VBox vboxWindow = (Gtk.VBox) gxml["vboxWindow"];

			SplashWindow.Update ("Creating widgets");
			// create perspective containing explorer, properties list and editors
			perspective = new Perspective ();
			vboxWindow.PackStart (perspective);

			// finally, add the application status bar
			appbar = new ApplicationBar ();
			vboxWindow.PackEnd (appbar, false, false, 0);

			// TODO: these values should be extracted from persistent state data
			// synchronize the views with the menus
			bool showLeftNotebook = true;
			bool showBottomNotebook = true;

			Gtk.CheckMenuItem menuItem = (Gtk.CheckMenuItem) gxml["explorer_bar"];
			menuItem.Active = showLeftNotebook;
			perspective.LeftNotebookVisible = showLeftNotebook;

			menuItem = (Gtk.CheckMenuItem) gxml["output_bar"];
			menuItem.Active = showBottomNotebook;
			perspective.BottomNotebookVisible = showBottomNotebook;

                        window.Icon = icon;
			this.WindowTitle = "";
			
			SplashWindow.Update ("Displaying widgets");

			// show everything that we have created manually
			// otherwise only glade widgets are shown
			vboxWindow.ShowAll ();
			gxml.Autoconnect (controller);
			SplashWindow.Update ("Workbench is now running!");
		}

		public string WindowTitle {
			set {
				if (value != null && value.Length > 0) {
					window.Title = value + " - ";
				}
				window.Title += "Simetron Workbench";
			}
		}

		public Perspective Perspective {
			get { return perspective; }
		}

		public ApplicationBar ApplicationBar {
			get { return appbar; }
		}

		public Gtk.Window Window {
			get { return window; }
		}
	}
}
