//
//  ImportDialog.cs  - Dialog for importing files
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

namespace Simetron.GUI.Dialogs {
	using System;
	using System.Collections;
	using Gtk;
	using Glade;
	using Simetron.GUI.Workbench;
	using Simetron.Logging;
	using Simetron.Data;
	using Simetron.Data.Providers;

	public class ImportDialog {
		// only one dialog instance
		static XML gxml;
		static Gtk.Dialog importDialog;
		static Gtk.Combo typeCombo;
		static Gtk.Combo formatCombo;
		static Gtk.Button okButton;
		
		static ImportDialog ()
		{
			gxml = new Glade.XML (null,	
					      WorkbenchSingleton.GLADEFILE,
					      "ImportDialog", 
					      null);
			importDialog = (Dialog) gxml["ImportDialog"];
			typeCombo = (Combo) gxml["typeCombo"];
			formatCombo = (Combo) gxml["formatCombo"];
			okButton = (Button) gxml["okButton"];
		}

		// instance data
		Hashtable dialogData;

		public ImportDialog (Hashtable dialogData) {			
			gxml.Autoconnect (this);
			this.dialogData = dialogData;
			// populate format combo
			ICollection modes = dialogData.Keys;
			string[] modeNames = new string[modes.Count];
			int i = 0;
			foreach (ProviderMode mode in modes) {
				modeNames[i++] = Enum.GetName (typeof (ProviderMode), 
							       mode);
			}
			formatCombo.SetPopdownStrings (modeNames);			
			
			PopulateTypeCombo ();
		}			

		public Type Type {
			get {
				if (dialogData == null)
					return null;
				string selectedTypeName = typeCombo.Entry.Text;
				ICollection types = (ICollection) dialogData[this.Mode];
				foreach (Type type in types) {
					int nslen = type.Namespace.Length;
					string typeName = 
						type.FullName.Substring (nslen + 1);
					if (typeName.Equals (selectedTypeName)) {
						return type;
					}
				}
				return null;
			}
		}

		public ProviderMode Mode {
			get {
				string modeName = formatCombo.Entry.Text;
				ProviderMode mode = 
					(ProviderMode) Enum.Parse (typeof (ProviderMode), 
								   modeName);
				return mode;
			}
		}
		
		public int Run () {
			return importDialog.Run ();
		}
		
		public void Hide () {
			importDialog.Hide ();
		}
						
		void PopulateTypeCombo ()
		{
			if (dialogData == null) 
				return;
			ICollection types = (ICollection) dialogData[this.Mode];
			string[] typeNames = new string[types.Count];
			int i = 0;
			foreach (Type type in types) {
				int nslen = type.Namespace.Length;
				typeNames[i++] = type.FullName.Substring (nslen + 1);
			}
			typeCombo.SetPopdownStrings (typeNames);
		}

		void on_formatEntry_changed (object o, EventArgs args)
		{
			PopulateTypeCombo ();
		}
	}
}
