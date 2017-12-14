using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    public partial class Form1 : Form
    {

        private MyWIFI _MyWIFI;
        private Timer _timer;
        private int _selectedIndex;

        public Form1()
        {
            InitializeComponent();
            _MyWIFI = new MyWIFI();
            _selectedIndex = -1;
            Load += (s, e) =>
            {
                RepaintUI();
                _timer = new Timer
                {
                    Interval = 5000
                };
                _timer.Tick += new EventHandler(Timer_Tick);
                _timer.Start();
            };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RepaintUI();
        }
        private bool IsValidSelection
        {
            get
            {
                if ((_selectedIndex != -1) && (_selectedIndex < _MyWIFI.NumberOfConnections))
                {
                    return true;
                }
                else
                {
                    _selectedIndex = -1;
                    return false;
                }
            }
        }

        private void RepaintUI()
        {
            _timer?.Stop();

            ShowConnections(_MyWIFI.FindAllPoints());
            if (IsValidSelection)
            {
                connectionsList.Items[_selectedIndex].Selected = true;
                ShowSelectedInfo();
            }

            _timer?.Start();
        }

        private void ShowConnections(bool turnedOn)
        {
            connectionsList.Items.Clear();
            if (turnedOn)
                connectionsList.Items.AddRange(_MyWIFI.ConnectionsNames);
            else
                connectionsList.Items.Add("Wi-Fi отключен");
        }
        private void ShowSelectedInfo()
        {
            infoList.Items.Clear();
            infoList.Items.AddRange(_MyWIFI.ConnectionInfo(_selectedIndex));
        }

        private void connectButton_Click(object sender, EventArgs e)
        {

        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {

        }
    }
}
