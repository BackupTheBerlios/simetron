namespace Simetron.Data {
	using System;
	using System.IO;
	using Simetron.Data.NetworkTopology;
	using Simetron.Logging;

	public class XmlWorkspaceStore : XmlStore {
		public XmlWorkspaceStore () : base (typeof(Workspace)){
		}

		protected override void PostReadTemplate (object obj) {
			Workspace ws = obj as Workspace;
			foreach (Reference reference in ws.Projects.References) {
				IStore store = StoreFactory.Instance.CreateStore ( typeof (Project), 
										   reference.Mode);
				string url = GetURLFromName (reference.Name);
				store.OpenConnection (url);
				Project project = (Project) store.Read ();
				store.CloseConnection ();
				ws.Projects[reference] = project;
			}
		}

		protected override void PostWriteTemplate (object obj) {
			Workspace ws = obj as Workspace;
			foreach (Reference reference in ws.Projects.References) {
				Project project = (Project) ws.Projects[reference];	
				IStore store = StoreFactory.Instance.CreateStore ( typeof (Project), 
										   reference.Mode);
				string url = GetURLFromName (reference.Name);
				store.OpenConnection (url);
				store.Write (project);
				store.CloseConnection ();
			}
		}

		private string GetURLFromName (string name) {
			FileInfo info = new FileInfo (filename);
			string url = info.DirectoryName + 
				Path.DirectorySeparatorChar +
				name +
				Path.DirectorySeparatorChar +
				name +
				".prj";
			Logger.Debug ("URL for " + name + " is " + url);
			return url;				
		}
	}
}
