using System;
using Gtk;
using Simetron.Data;
using Simetron.Data.NetworkTopology;

namespace Simetron.GUI.Workbench {
	public class Explorer : TreeView {
		private TreeStore store;

		public Explorer () : base () {
			this.HeadersVisible = false;
			// we need to append the col regardless of whether the headers
			// are visible or not, otherwise the model is not displayed
			this.AppendColumn ("Name", new CellRendererText (), "text", 0);
			
			store = new TreeStore (typeof (string));
			this.Model = store;
			PopulateStore ();
    		}
		
		private void PopulateStore () {
			Workspace ws = WorkbenchSingleton.Instance.Workspace;

			Reference[] references = ws.Projects.References;
			
			foreach (Reference reference in references) {
				TreeIter iter = store.AppendValues (reference.Name);
				Project p = (Project) ws.Projects[reference];
				PopulateProject (iter, p);
			}
		}

		private void PopulateProject (TreeIter parent, Project p) {
			Reference[] references = p.Networks.References;
			foreach (Reference reference in references) {
				TreeIter iter = store.AppendValues (parent, reference.Name);
				PopulateNetwork (iter, (Network) p.Networks[reference]);
			}
		}

		private void PopulateNetwork (TreeIter parent, Network network) {
			TreeIter linkIter = store.AppendValues (parent, "Links");
			foreach (Link link in network.Links) {
				PopulateLink (linkIter, link);
			}
			TreeIter nodeIter = store.AppendValues (parent, "Nodes");
			foreach (Node node in network.Nodes) {
				PopulateNode (nodeIter, node);
			}
		}

		private void PopulateLink (TreeIter parent, Link link) {
			TreeIter nodeIter = 
				store.AppendValues (parent, 
						    "Link (" + link.Label + ")");
			PopulateNode (nodeIter, link.Origin);
			PopulateNode (nodeIter, link.Destination);
		}

		private void PopulateNode (TreeIter parent, Node node) {
			TreeIter iter = 
				store.AppendValues (parent, 
						    "Node (" + node.Label + ")");
			store.AppendValues (iter, "X :" + node.Point.X);
			store.AppendValues (iter, "Y :" + node.Point.Y);
		}
	}
}


			/*
			// REMOVE THIS -- testing
			// create Amsterdam ring network
			Node n1 = new Node (new Point (0,0));
			Node n2 = new Node (new Point (0,1));
			Link l1 = new Link (n1, n2);
			Link l2 = new Link (n2, n1);
			Network n = new Network ();
			n.Add (n1);
			n.Add (n2);
			n.Add (l1);
			n.Add (l2);			
			Project p = new Project ();
			p.Network = n;
			p.NetworkIdentification = "network.xml";

			IStore istore = StoreFactory.CreateStore (StoreType.Project,
						  StoreMode.Xml);
			
			istore.OpenConnection ("project.xml");
			istore.Write (p);
			
			istore.OpenConnection ("project.xml");
			Project p2 = (Project) istore.Read ();
			p2.NetworkIdentification = "network2.xml";

			istore.OpenConnection ("project2.xml");
			istore.Write (p2);
			istore.CloseConnection ();
			
			// END REMOVE
			*/
