using System;
using System.Collections;
using Gtk;

namespace Simetron.GUI.Workbench {
	public class NonCloseableNotebook : Notebook{
		public NonCloseableNotebook () : base () {
			this.EnablePopup = false;
			this.Homogeneous = false;
			this.Scrollable = true;
			this.ShowBorder = true;
			this.ShowTabs = true;
			this.TabPos = PositionType.Bottom;
		}

		public void AppendPage (Widget child, string label) {
			// add by a default a scrolled window
			ScrolledWindow sw = new ScrolledWindow ();
			// TODO: find if child is scrollable
			// I am afraid this is messing up for widgets which
			// are scrollable
			bool isScrollable = child.SetScrollAdjustments (null, null);
			if (isScrollable) {
				sw.Add (child);
			} else {
				sw.AddWithViewport (child);
			}
			this.AppendPage (sw, GetLabel (label));
			this.SetTabLabelPacking (sw, true, true, PackType.Start);
		}

		protected virtual Widget GetLabel (string label) {
			return new Label (label);
		}
	}       
}
