namespace Simetron.GUI.Core {
	using System;
	using System.IO;
	using Gtk;
	using Simetron.GUI.Workbench;
	using Simetron.Logging;
	using Simetron.GUI.Logging;
	using Simetron.Data;

	public sealed class GSimetronMain {
		private GSimetronMain () {}
		private static string installdir;

		[STAThread()]
		public static void Main (string[] args) {			
			try {
				Application.Init ();			
				Initialize ();
				MainLoop ();
			} catch (Exception e) {
				Logger.Fail ("Fatal exception : " + e);
				
			}
		}

		private static void Initialize () {
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
			System.Diagnostics.Debug.Listeners.Add (new GUIStatckFrameTraceListener ());
			Logger.Debug ("Simetron started");
			SplashWindow.Destroy ();
		}

		private static void MainLoop () {
			bool keepRuning = true;
			while (keepRuning) {
				try { 
					Application.Run ();
					keepRuning = false;
				} catch (Exception e) {
					Logger.Debug (e.ToString());
					MessageDialog md = new MessageDialog (null, 
									      DialogFlags.Modal,
									      MessageType.Error, 
									      ButtonsType.Close,
									      e.Message);
					md.Run ();
					md.Destroy ();
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
