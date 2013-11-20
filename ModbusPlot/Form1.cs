using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Modbus;
using Modbus.Device;

namespace ModbusPlot
{
    public partial class Form1 : Form
    {
        private ModbusSerialMaster mMaster;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                serialPort1.BaudRate = 19200;
                serialPort1.Parity = System.IO.Ports.Parity.None;
                serialPort1.StopBits = System.IO.Ports.StopBits.One;
                serialPort1.DataBits = 8;
                serialPort1.PortName = "COM4";
                mMaster = ModbusSerialMaster.CreateRtu(serialPort1);
                serialPort1.Open();
                timer1.Start();
            }
            catch (Exception ex)
                {

                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var BUF = ReadFloat(mMaster, 1, 102);
            chart1.Series[0].Points.Add(BUF);
            BUF = ReadFloat(mMaster, 1, 104);
            chart1.Series[1].Points.Add(BUF);
            chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points.Count-100;
            chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + 100;
        }

        public static float ReadFloat(IModbusMaster master, byte slaveId, ushort address)
        {
            ushort[] registers = master.ReadInputRegisters(slaveId, address, sizeof(float) / sizeof(ushort));
            var result = new byte[registers.Length * sizeof(ushort)];
            Buffer.BlockCopy(registers, 0, result, 0, result.Length);
            return BitConverter.ToSingle(result, 0);
        }
    }
}
