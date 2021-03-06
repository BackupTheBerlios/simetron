//
//  GUIStackFrameTraceListener.cs - Console output to GUI
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

namespace Simetron.GUI.Logging 
{
	using System;
	using System.Diagnostics;
	using Gtk;
	using Simetron.GUI.Workbench;

	public class GUIStackFrameTraceListener : TraceListener 
	{
		public GUIStackFrameTraceListener () 
		{
		}

		public override void Write (string message) 
		{
			WorkbenchView currentView = 
				WorkbenchSingleton.Instance.CurrentView;

			TextBuffer buffer = currentView.Console.Buffer; 
			string now = DateTime.Now.ToLongTimeString();
			string prefix = "[" + now + "] ";
			string prefixedMessage = prefix + message;
			buffer.Text += prefixedMessage;
		}
			
		public override void WriteLine (string message) 
		{
			Write (message + '\n');
		}
	}
}
