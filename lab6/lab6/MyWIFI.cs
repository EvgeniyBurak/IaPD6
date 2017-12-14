using System;
using System.Collections.Generic;
using SimpleWifi;
using System.Diagnostics;
using System.Windows.Forms;

namespace lab6
{
    class MyWIFI
    {
        private Wifi _wifi;

        /// <summary>
        /// Лист соединений
        /// </summary>
        public List<Connection> Connections { get;  set; }

        /// <summary>
        /// Уникальный номер точки доступа
        /// </summary>
        private string[] Вssids;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public MyWIFI()
        {
            Connections = new List<Connection>();
            _wifi = new Wifi();
        }

        /// <summary>
        /// Количество соединений
        /// </summary>
        public int NumberOfConnections
        {
            get
            {
                return Connections.Count;
            }
        }

        public bool FindAllPoints()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = "cmd",
                    Arguments = @"/C ""netsh wlan show networks mode=bssid | findstr SSID""",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            Вssids = process.StandardOutput.ReadToEnd().Replace(" ", "").Replace("\r", "").Split('\n');
            process.WaitForExit();
            try
            {
                List<Connection> newConnections = new List<Connection>();
                foreach (var accessPoint in _wifi.GetAccessPoints())
                {
                    newConnections.Add(new Connection
                    {
                        AccessPoint = accessPoint,
                        Mac = FindMac(accessPoint)
                    });
                }
                Connections = newConnections;
                return true;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Connections.Clear();
                return false;
            }
        }

        public bool CheckWifi()
        {
            try
            {
                _wifi.GetAccessPoints();
                return true;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return false;
            }
        }

        private string FindMac(AccessPoint accessPoint)
        {
            bool foundMac = false;
            foreach (var bssid in Вssids)
            {
                if (foundMac)
                {
                    return bssid.Substring(bssid.Length - 17, 17);
                }
                foundMac = (bssid.Split(':')[0].Contains("SSID") && accessPoint.Name.Equals(bssid.Split(':')[1]));
            }
            return null;
        }

        /// <summary>
        /// Список имен WIFI
        /// </summary>
        public ListViewItem[] ConnectionsNames
        {
            get
            {
                ListViewItem[] connectionsList = new ListViewItem[NumberOfConnections];
                for (int i = 0; i < NumberOfConnections; i++)
                {
                    connectionsList[i] = new ListViewItem(Connections[i].SSID ?? "Hidden connection");
                }
                return connectionsList;
            }
        }

        /// <summary>
        /// Информация в листе об подключении
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListViewItem[] ConnectionInfo(int index)
        {
            Connection connection = Connections[index];
            return new ListViewItem[] {
                    new ListViewItem($"{connection.SSID ?? "Hidden connection"}"),
                    new ListViewItem($"{connection.AuthType}"),
                    new ListViewItem($"{connection.Mac}"),
                    new ListViewItem($"{connection.SignalStrength}"),
                    new ListViewItem($"{connection.IsSecured}"),
                    new ListViewItem($"{connection.HasProfile}"),
                    new ListViewItem($"{connection.IsConnected}")
                };
        }

        /// <summary>
        /// Подключение к WIFI
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <param name="password">Пароль</param>
        /// <param name="onConnectComplite"></param>
        /// <param name="remember">Запомнить пароль</param>
        public void Connect(int index, string password, Action<bool> onConnectComplite,
            bool remember)
        {
            var connection = Connections[index];
            var authRequest = new AuthRequest(connection.AccessPoint)
            {
                Password = password
            };
            connection.ConnectAsync(authRequest, remember, onConnectComplite);
        }

        /// <summary>
        /// Разъединить WIFI
        /// </summary>
        public void Disconnect()
        {
            _wifi.Disconnect();
        }

    }
}
