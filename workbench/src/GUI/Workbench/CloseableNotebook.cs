using System;
using System.Collections;
using Gtk;
using Simetron.Logging;

namespace Simetron.GUI.Workbench {
	public delegate void RemovingPageHandler (object page, RemovingPageEventArgs args);

	public class RemovingPageEventArgs : EventArgs {
		private bool retVal;

		public RemovingPageEventArgs () : base () {
			// by default 
			retVal = true;
		}

		public bool Removable {
			get {
				return retVal;
			}
			set {
				retVal = value;
			}
		}
	}


	public class CloseableNotebook : NonCloseableNotebook {
		ArrayList buttons;
			      
		public event RemovingPageHandler OnRemovingPage;

		public CloseableNotebook () : base () {
			this.TabPos = PositionType.Top;
			buttons = new ArrayList ();
		}

		protected override Widget GetLabel (string label) {
			HBox widget = new HBox (false, 10);
			widget.PackStart (new Label (label));
			Button close = new Button("x");
			close.Relief = ReliefStyle.Half;
			close.Clicked += new EventHandler (OnCloseButtonClicked);
			widget.PackStart (close, false, false, 0);
			buttons.Add (close);
			widget.ShowAll ();
			return widget;
		}

		private void OnCloseButtonClicked (object obj, EventArgs args) {
			Button b = (Button) obj;
			int page = buttons.IndexOf (b);
			bool canBeRemoved = true;
			if (OnRemovingPage != null) {
				ScrolledWindow sw = this.GetNthPage (page) as ScrolledWindow;
				Logger.Assert (sw != null);
				foreach (Widget w1 in sw.Children) {
					Viewport vp = w1 as Viewport;
					if (vp != null) {
						foreach (Widget w2 in vp.Children) {
							RemovingPageEventArgs eArgs = new RemovingPageEventArgs ();
							OnRemovingPage (w2, eArgs);
							canBeRemoved &= eArgs.Removable;
						}
					} else {
						RemovingPageEventArgs eArgs = new RemovingPageEventArgs ();
						OnRemovingPage (w1, eArgs);
						canBeRemoved &= eArgs.Removable;
					}
				}
			}
			if (canBeRemoved) {
				this.RemovePage (page);
				buttons.Remove (b);
			}
		}
	}       
}
