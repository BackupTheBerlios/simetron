namespace Simetron.Data.Providers
{
	using System;
	using System.IO;
	using Simetron.Logging;

	public abstract class FileProvider : IProvider
	{
		private FileStream fs;

		public FileProvider ()
		{
		}

		public void OpenConnection (string connectionString) 
		{
			Logger.Assert (connectionString != null, "A connection string must be set");
			if (fs != null) {
				CloseConnection ();
			}
			Logger.Debug ("Open connection: " +  connectionString);
			FileInfo finfo = new FileInfo (connectionString);
			DirectoryInfo dinfo = finfo.Directory;
			if (!dinfo.Exists) {
				dinfo.Create ();
				Logger.Debug ("Creating directory " + dinfo);
			}
			fs = new FileStream (connectionString, FileMode.OpenOrCreate);
		}

		public void CloseConnection () 
		{
			if (fs != null) {
				fs.Close ();
				fs = null;
			}
		}

		protected Stream Stream {
			get { return fs; }
		}

		public abstract Type Type {get;}
		public abstract ProviderMode Mode {get;}
		public abstract object Read ();
		public abstract void Write (object obj);
	}
}
