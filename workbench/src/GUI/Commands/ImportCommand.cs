//
//  ImportCommand.cs  - Import non-Simetron format file
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

namespace Simetron.GUI.Commands 
{
	using System;
	using System.Collections;
	using System.IO;
	using Gtk;
	using Simetron.Data.Providers;
	using Simetron.Data.NetworkTopology;
	using Simetron.GUI.Core;
	using Simetron.GUI.Dialogs;
	using Simetron.GUI.Workbench;
	using Simetron.Logging;

	public class ImportCommand : ICommand 
	{
		static Hashtable dialogData;

		FileSelection fs;
		ImportDialog dialog;

		static ImportCommand ()
		{
			// determine possible types and formats
			dialogData = new Hashtable ();
			ICollection modes = ProviderFactory.GetProviderModes ();
			foreach (ProviderMode mode in modes) {
				ICollection types = 
					ProviderFactory.GetRegisteredTypes (mode);
				dialogData[mode] = types;
			}
			// remove default (goes through normal Open menu item)
			dialogData.Remove (ProviderFactory.DefaultMode);
		}

		public ImportCommand ()
		{
			// select the file to import
			fs = new FileSelection ("Choose the file to import ...");
			// user selects type and format
			dialog = new ImportDialog (dialogData);			
		}

		public bool Run () 
		{
			int r = dialog.Run ();
			dialog.Hide ();
			if (r != (int) ResponseType.Ok) {
				return false;
			}
			Type type = dialog.Type;
			ProviderMode mode = dialog.Mode;
			// serialization provider
			IProvider provider = ProviderFactory.GetProvider (type, mode);

			bool parse = true;
			object obj = null;
			while (parse) {
				r = fs.Run ();
				fs.Hide ();
				if (r != (int) ResponseType.Ok) {
					return false;
				}
				
				string filename = fs.Filename;
				DirectoryInfo dir = new DirectoryInfo (filename);
				if (dir.Exists) {
					// selection is a directory
					// try again
					continue;
				}
				if (filename == null || filename.Equals("")) {
					return false;
				}

				provider.OpenConnection (filename);
				try {
					obj = provider.Read ();
					parse = false;
				} catch (Exception e) {
					new ErrorDialog (e);
				}
				provider.CloseConnection ();
			}
			
			// if everything ok, add the new object to the Model
			// which will fire events caught by the controllers
			// and the views will be updated
			
			WorkbenchModel model = WorkbenchSingleton.Instance.Model;
			// the filename and name is set to null
			model.AddDocument (new Document (obj, null, null));
			return true;
		}
	}
}
