using System;

namespace Simetron.Data {
	public enum StoreMode {
		XML = 0,
		DBMS
	}

	public interface IStore {
		StoreMode Mode { get; }
		void OpenConnection (string connectionString);
		object Read ();
		void Write (object obj);
		void CloseConnection ();
	}
}
