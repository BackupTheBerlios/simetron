namespace Simetron.GUI.Commands {
	using System;
	using Gtk;
	using Simetron.Data;
	using Simetron.Logging;
	using Simetron.GUI.Workbench;

	public class OpenDocumentCommand : ICommand {
		private FileSelection fs;

		public OpenDocumentCommand () {
			fs = new FileSelection ("Choose a file");
		}

		public bool Run () {
			fs.Run ();
			fs.Hide ();
			string filename = fs.Filename;
			if (!"".Equals(filename)) {
				Logger.Debug ("Filename is " + filename);
// 				IStore istore = StoreFactory.Instance.CreateStore (typeof (Project),
// 									  StoreMode.XML);
// 				istore.OpenConnection (filename);
// 				Project project = (Project) istore.Read ();
// 				istore.CloseConnection ();
// 				WorkbenchSingleton.Instance.DocumentStore[filename] = project;
			}
			return true;
		}
	}
}
