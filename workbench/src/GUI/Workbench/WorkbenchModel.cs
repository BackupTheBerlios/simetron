//
//  WorkbenchModel.cs  - The M in the workbench MVC
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
	using Simetron.GUI.Core;

	public sealed class WorkbenchModel
	{
		ArrayList documents;
		
		public WorkbenchModel ()
		{
			documents = new ArrayList ();
		}

		// a collection of Document
		public ICollection Documents {
			get { 
				return documents; 
			}
		}

		public void AddDocument (Document document)
		{
			if (document == null) {
				throw new ArgumentException ("Null document");
			}
			documents.Add (document);
			if (OnDocumentAdded != null) {
				OnDocumentAdded (document, EventArgs.Empty);
			}
		}
		
		public void RemoveDocument (Document document)
		{
			if (document == null) {
				throw new ArgumentException ("Null document");
			}
			documents.Remove (document);
			if (OnDocumentRemoved != null) {
				OnDocumentRemoved (document, EventArgs.Empty);
			}
		}

		public event EventHandler OnDocumentAdded;
		public event EventHandler OnDocumentRemoved;
	}
}
