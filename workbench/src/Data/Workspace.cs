namespace Simetron.Data {
	using System;
	using System.IO;
	using System.Xml.Serialization;

	public class Workspace {
		private Container projects;

		public Workspace () {
			projects = new Container ();
		}

		public Container Projects {
			get { return projects; }
			set { projects = value;	}
		}
 	}
}
