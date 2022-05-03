namespace NetworkUtils
{
    using System.Net.NetworkInformation;
	using System.Management;
	using System.Text.RegularExpressions;
	using System.Runtime.Versioning;

	/// <summary>
	/// ネットワークの有効・無効切り替え機能を提供する
	/// </summary>
	/// <remarks>
	/// このライブラリは、Windows のみサポートしている
	/// </remarks>
	[SupportedOSPlatform("Windows")]
	public class NetworkCapabilities
    {
		/// <summary>
		/// 正規表現での検証に使用する イーサーネットを表す識別子
		/// </summary>
		const string RegixPatternIndex = "[イーサネット]";

		/// <summary>
		/// ネットワークを有効化する
		/// </summary>
		public void EnableNetwork()
			=> IssueNetworkQuery(true);

		/// <summary>
		/// ネットワークを無効化する
		/// </summary>
		public void DisableNetwork()
			=> IssueNetworkQuery(false);

		/// <summary>
		/// ネットワークが有効化どうかを示す値を取得
		/// </summary>
		/// <returns>true:ネットワークは有効.false:ネットワークは無効</returns>
		public bool IsEnableNetwork()
			=> NetworkInterface.GetIsNetworkAvailable();

		/// <summary>
		/// ネットワークの有効・無効のクエリを発行
		/// </summary>
		/// <param name="enable">true:ネットワークを有効.false:ネットワークを無効</param>
		void IssueNetworkQuery(bool enable)
		{
			// NICの検索
			var wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
			using (var searcher = new ManagementObjectSearcher(wmiQuery)) {
				foreach (var item in searcher.Get().OfType<ManagementObject>()) {
					if (!Regex.IsMatch((string)item["NetConnectionId"], RegixPatternIndex))
						continue;

					using (item) {
						item.InvokeMethod(enable ? "Enable" : "Disable", null);
					}
				}
			}
		}
	}
}