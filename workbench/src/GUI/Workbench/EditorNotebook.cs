//
//  EditorNotebook.cs  - Specialized notebook for editors
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

namespace Simetron.GUI.Workbench 
{
	using System;
	using System.Collections;
	using Gtk;
	using Simetron.GUI.Editors;
	using Simetron.GUI.Core;
	using Simetron.Logging;

	public class EditorNotebook : Notebook
	{
		ArrayList buttons;
			      
		public event RemovingPageHandler OnRemovingPage;

		public EditorNotebook () : base ()
		{
			this.EnablePopup = false;
			this.Homogeneous = false;
			this.Scrollable = true;
			this.ShowBorder = true;
			this.ShowTabs = true;
			this.TabPos = PositionType.Top;
			buttons = new ArrayList ();
		}

		public void AppendEditor (IEditor editor)
		{
			Widget child = (Widget) editor;
			Document doc = editor.Model;
			string label = doc.Name;
			Logger.Debug ("Adding editor " + label);

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
			this.SetTabLabelPacking (sw, false, false, PackType.Start);
			this.ShowAll ();
		}

		Widget GetLabel (string label) {
			HBox widget = new HBox (false, 10);
			widget.PackStart (new Label (label));
			Button close = new Button ();
			close.Relief = ReliefStyle.None;
			Gtk.Image image = new Image ();
			image.SetFromStock (Stock.Close, IconSize.Menu);
			close.Add (image);
			close.Clicked += new EventHandler (OnCloseButtonClicked);
			widget.PackStart (close, false, false, 0);
			buttons.Add (close);
			widget.ShowAll ();
			return widget;
		}

		void OnCloseButtonClicked (object obj, EventArgs args) {
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
}
