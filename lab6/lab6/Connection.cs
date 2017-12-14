using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleWifi;

namespace lab6
{
    class Connection
    {


        /// <summary>
        /// Название Wi-Fi сети; 
        /// (Service Set Identifier) – идентификатор беспроводной сети
        /// </summary>
        public string SSID
        {
            get
            {
                return this.AccessPoint.Name.Equals("") ? null : this.AccessPoint.Name;
            }
        }

        /// <summary>
        /// Точка доступа
        /// </summary>
        public AccessPoint AccessPoint { get; set; }

        /// <summary>
        /// MAC-адрес
        /// </summary>
        public string Mac { get; set; }


        /// <summary>
        /// Сила сигнала
        /// </summary>
        public int SignalStrength
        {
            get
            {
                return (int)this.AccessPoint.SignalStrength;
            }
        }

        public bool IsSecured
        {
            get
            {
                return this.AccessPoint.IsSecure;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.AccessPoint.IsConnected;
            }
        }

        /// <summary>
        /// Если компьютер имеет профиль подключения, сохраненный для этой точки доступа
        /// </summary>
        public bool HasProfile
        {
            get
            {
                return this.AccessPoint.HasProfile;
            }
        }

        /// <summary>
     /// Тип авторизации
     /// </summary>
        public string AuthType
        {
            get
            {
                var CipherAlgorithm = this.AccessPoint.ToString().Split()[10];
                var authAlgorithm = this.AccessPoint.ToString().Split()[6];

                switch (CipherAlgorithm)
                {
                    case "None":
                        return "Open";
                    case "Wep":
                        return "Wep";
                    case "CCMP":
                    case "TKIP":
                        return (authAlgorithm.Equals("RSNA") ? "WPA2-Enterprise-PEAP-MSCHAPv2" : "WPA2-PSK");
                    default:
                        return "Unknown";
                }
            }
        }

        /// <summary>
        /// Connect asynchronous to the access point
        /// </summary>
        /// <param name="authRequest"></param>
        /// <param name="onConnectComplete"></param>
        /// <param name="remember"></param>
        public void ConnectAsync(AuthRequest authRequest, Action<bool> onConnectComplete, bool remember)
        {
            this.AccessPoint.ConnectAsync(authRequest, !remember, onConnectComplete);
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Connection()
        {

        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="AccessPoint">Точка доступа</param>
        /// <param name="Mac">MAC-адрес</param>
        public Connection(AccessPoint AccessPoint, string MAC)
        {
            this.AccessPoint = AccessPoint;
            this.Mac = MAC;
        }

    }
}
