namespace Simetron.Data.Providers
{
	using System;

	public enum ProviderMode {
		Db,
		Xml,
		Mitsim
	}

	public interface IProvider
	{
		Type Type {get;}
		ProviderMode Mode {get;}
		void OpenConnection (string connectionString);
		object Read ();
		void Write (object obj);
		void CloseConnection ();
	}
}
