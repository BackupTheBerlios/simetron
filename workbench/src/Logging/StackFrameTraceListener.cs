using System;
using System.Diagnostics;
using System.IO;

namespace Simetron.Logging {
	public class StackFrameTraceListener : TextWriterTraceListener {
		public StackFrameTraceListener (TextWriter writer) : base (writer) {}
		
		public override void Write (string message) {
			StackFrame sf = new StackFrame (6, true);
			string fileName = sf.GetFileName();
			int lineNumber = sf.GetFileLineNumber();
			string now = DateTime.Now.ToLongTimeString();
			string prefix = "[" + now + " - " + fileName + ":" + lineNumber + "] ";
			base.Write ( prefix + message);
		}
	}
}
