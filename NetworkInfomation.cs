
namespace NetworkUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;

    /// <summary>
    /// ネットワークに関する情報を提供する
    /// </summary>
    public static class NetworkInfomation
    {
        /// <summary>
        /// ネットワークに接続中の一番最初に見つかった NIC の情報を取得
        /// </summary>
        /// <returns></returns>
        public static NetworkInterface? GetNetworkInterfaceFirst()
            => NetworkInterface.GetAllNetworkInterfaces().Where(x => IsConnectNetwork(x)).FirstOrDefault();

        /// <summary>
        /// 無線接続かどうか
        /// </summary>
        /// <returns>true:無線接続.false:有線接続</returns>
        public static bool IsWired()
            => NetworkInterface.GetAllNetworkInterfaces().Where(x => x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211).Any();

        /// <summary>
        /// 有線接続かどうか
        /// </summary>
        /// <returns>true:有線接続.false:無線接続</returns>
        public static bool IsNonWired()
            => NetworkInterface.GetAllNetworkInterfaces().Where(x => x.NetworkInterfaceType == NetworkInterfaceType.Ethernet).Any();

        /// <summary>
        /// 指定のネットワークインターフェースがネットワークに接続中かどうかを示す値を取得
        /// </summary>
        /// <param name="netInterface">NetworkInterface</param>
        /// <remarks>
        /// ネットワーク接続中かどうか以下条件で、判定する
        /// 1. ネットワークインターフェースが稼働している (OperationalStatus.Up)
        /// 2. ネットワークインターフェースの種類がトンネル接続でもループバック (NetworkInterfaceType.Tunnel かつ Loopback) でない
        /// </remarks>
        /// <returns>true:ネットワークに接続中.false:ネットワークに未接続</returns>
        static bool IsConnectNetwork(NetworkInterface netInterface)
            => netInterface.OperationalStatus == OperationalStatus.Up
            && !(netInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback || netInterface.NetworkInterfaceType == NetworkInterfaceType.Tunnel);

    }
}
