namespace Simetron.Data {
	using System;
	using System.IO;
	using Simetron.Data.NetworkTopology;
	using Simetron.Logging;

	public class XmlProjectStore : XmlStore {
		public XmlProjectStore () : base (typeof(Project)){
		}

		protected override void PostReadTemplate (object obj) {
			Project project = obj as Project;
			foreach (Reference reference in project.Networks.References) {
				IStore store = StoreFactory.Instance.CreateStore (typeof (Network), 
										  reference.Mode);
				
				string url = GetURLFromName (reference.Name);
				store.OpenConnection (url);			
				Network network = (Network) store.Read ();
				store.CloseConnection ();
				project.Networks[reference] = network;
			}
		}

		protected override void PostWriteTemplate (object obj) {
			Project project = obj as Project;
			foreach (Reference reference in project.Networks.References) {
				Network network = (Network) project.Networks[reference];
				IStore store = StoreFactory.Instance.CreateStore ( typeof (Network), 
										   reference.Mode);
				string url = GetURLFromName (reference.Name);
				store.OpenConnection (url);			
				store.Write (network);
				store.CloseConnection ();
			}
		}

		private string GetURLFromName (string name) {
			FileInfo info = new FileInfo (filename);
			string url = info.DirectoryName +
				Path.DirectorySeparatorChar +
				name +
				".net";
			Logger.Debug ("URL for " + name + " is " + url);
			return url;				
		}
	}
}
