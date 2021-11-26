#if UNITY_EDITOR || UNITY_STANDALONE

namespace Toast.Gamebase.Internal.Single.Standalone
{
	public class StandaloneGamebaseTerms : CommonGamebaseTerms
	{
		public StandaloneGamebaseTerms()
		{
			Domain = typeof(StandaloneGamebaseTerms).Name;
		}
	}
}
#endif
