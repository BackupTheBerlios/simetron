namespace Simetron.GUI.Editors {	
	using System;

	public sealed class EditorFactory
	{
		public static readonly EditorFactory Instance = new EditorFactory ();

		private EditorFactory ()
		{
		}

		public IEditor GetEditor (Type type)
		{
			return new NetworkEditor ();
		}
	}
}
