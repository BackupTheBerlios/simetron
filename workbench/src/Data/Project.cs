namespace Simetron.Data {
	using System.Xml.Serialization;

	public class Project {
		private Container networks;

		public Project () {
			networks = new Container ();
		}
		
		public Container Networks {
			get { return networks; }
			set { networks = value;	}
		}
	}
}
