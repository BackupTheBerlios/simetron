//
//  WorkbenchView.cs  - The workbench view part of the MVC
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
	using Glade;
	using Simetron.GUI.Commands;
	using Simetron.GUI.Core;

	public class WorkbenchView 
	{
		// load the icon from the default assembly
		readonly static Gdk.Pixbuf icon = new Pixbuf(null, 
							     "simetron-hicolor-48x48.png");

		Gtk.Window window;
		Gtk.TextView console;
		EditorNotebook editorNotebook;

		public WorkbenchView (WorkbenchController controller) 
		{
			SplashWindow.Update ("Creating GUI");
			XML gxml = new Glade.XML (null,
						  WorkbenchSingleton.GLADEFILE,
						  "Workbench",
						  null);

			window = (Gtk.Window) gxml["Workbench"];
			Gtk.VPaned splitter = (Gtk.VPaned) gxml["mainPane"];
			
			editorNotebook = new EditorNotebook ();
			splitter.Pack1 (editorNotebook, true, false);

			ScrolledWindow sw = new ScrolledWindow ();
			console = new TextView ();
			console.Editable = false;
			console.WrapMode = WrapMode.Word;
			sw.Add (console);
			
			Notebook bottomNotebook = new Notebook ();
			bottomNotebook.AppendPage (sw, new Label ("Console"));
			splitter.Pack2 (bottomNotebook, true, false);

                        window.Icon = icon;
			this.WindowTitle = "";		
			gxml.Autoconnect (controller);
			bottomNotebook.ShowAll ();
			editorNotebook.ShowAll ();
			SplashWindow.Update ("Simetron is ready!");
		}

		public string WindowTitle {
			set {
				if (value != null && value.Length > 0) {
					window.Title = value + " - ";
				}
				window.Title += "Simetron Workbench";
			}
		}

 		public Gtk.Window Window {
 			get { return window; }
 		}

		public Gtk.TextView Console {
			get {
				return console;
			}
		}

		public EditorNotebook EditorNotebook {
			get {
				return editorNotebook;
			}
		}
	}
}
