//
//  AboutDialog.cs  - Dialog for the common "About"
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

namespace Simetron.GUI.Dialogs 
{
	using System;
	using Gtk;
	using Gdk;
	using Glade;
	using Simetron.GUI.Workbench;

	public class AboutDialog 
	{
		static Gtk.Dialog dialog;
		static Gtk.Label buildLabel;

		static AboutDialog () 
		{
			XML gxml = new Glade.XML (null,
						  WorkbenchSingleton.GLADEFILE,
						  "AboutDialog",
						  null);
			dialog = (Dialog) gxml["AboutDialog"];
			buildLabel = (Label) gxml["buildLabel"];
			Gtk.Image image = (Gtk.Image) gxml["image"];
			Gdk.Pixbuf icon = new Pixbuf (null, 
						      "simetron-hicolor-48x48.png");
			image.Pixbuf = icon;
			gxml.Autoconnect (typeof (AboutDialog));
		}

		static void on_closebutton_clicked (object o, EventArgs args) 
		{
			dialog.Hide ();
		}

		public AboutDialog ()
		{
		}

		public void Run () 
		{
			dialog.Run ();
		}
	}
}
