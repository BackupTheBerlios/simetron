//
//  GSimetronMain.cs  - Entry point to the application
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

namespace Simetron.GUI.Core {
	using System;
	using System.IO;
	using Gtk;
	using Simetron.Data;
	using Simetron.GUI.Workbench;
	using Simetron.GUI.Dialogs;
	using Simetron.GUI.Logging;
	using Simetron.Logging;

	public sealed class GSimetronMain 
	{
		// private constructor to avoid instantiation
		GSimetronMain () 
		{
		}

		[STAThread()]
		public static void Main (string[] args) 
		{			
			try {
				Application.Init ();			
				Initialize ();
				MainLoop ();
			} catch (Exception e) {
				Logger.Fail ("Fatal exception : " + e);
				
			}
		}

		static void Initialize () 
		{
			SplashWindow.Update ("Starting Simetron");
			if (!File.Exists (GSimetronMain.MetadataFile)) {
				// create and save empty workspace
				/*Workspace ws = new Workspace ();
				IStore store = StoreFactory.Instance.CreateStore (typeof (Workspace),
										  StoreMode.XML);
				store.OpenConnection (GSimetronMain.MetadataFile);
				store.Write (ws);
				store.CloseConnection ();
				*/
			}

			SplashWindow.Update ("Launching workbench");
			// create one view of the workbench
			WorkbenchSingleton.Instance.CreateWorkbenchView ();
			System.Diagnostics.Debug.Listeners.Add (
				new GUIStackFrameTraceListener ());
			Logger.Debug ("Simetron started");
			SplashWindow.Destroy ();
		}

		static void MainLoop () 
		{
			bool mustRun = true;
			while (mustRun) {
				try {
					Application.Run ();
					mustRun = false;
				} catch (Exception e) {
					new ErrorDialog (e);
				}
			}
		}

 		public readonly static string MetadataFile = Environment.GetEnvironmentVariable("HOME") + 
  			Path.DirectorySeparatorChar + 
  			"simetron" +
  			Path.DirectorySeparatorChar + 
  			"workspace" + 
 			Path.DirectorySeparatorChar + 
 			".metadata.xml";
	}
}
