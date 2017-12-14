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
        /// Точка доступа
        /// </summary>
        public AccessPoint AccessPoint { get; set; }

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

        /// <summary>
        /// Подключен ли к данному WIFI
        /// </summary>
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
        /// Тип авторизации/шифрования
        /// </summary>
        public string AuthType
        {
            get
            {
                var CipherAlgorithm = this.AccessPoint.ToString().Split()[10];
                var authAlgorithm = this.AccessPoint.ToString().Split()[6];

                switch (CipherAlgorithm)
                {
                    //Открытая сеть WIFi
                    case "None":
                        return "Open";
                    //Wired Equivalent Privacy    
                    case "Wep":
                        return "Wep";
                    //Counter Mode with Cipher Block Chaining Message Authentication Code Protocol
                    case "CCMP":
                    //Temporal Key Integrity Protocol
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
        public void ConnectAsync(AuthRequest authRequest, bool remember, Action<bool> onConnectComplete)
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
