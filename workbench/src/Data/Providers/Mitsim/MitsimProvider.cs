namespace Simetron.Data.Providers.Mitsim
{
	using Simetron.Data.Providers;

	public abstract class MitsimProvider : FileProvider 
	{
		public override ProviderMode Mode {
			get { return ProviderMode.Mitsim; }
		}
	 }
}
