using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TextBox[] AdrBits;

        private void button4_Click(object sender, EventArgs e)
        {
            if (AdrBits == null)
            {
                AdrBits = new TextBox[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25 };
            }
            int adr = 0;
            var str = address.Text.ToUpper();
            try
            {
                if (adr_hex.Checked)
                {
                    adr = lab3.Converter.Str16ToInt(str);
                }
                else if (adr_dec.Checked)
                {
                    adr = lab3.Converter.Str10ToInt(str);
                }
                else
                {
                    adr = lab3.Converter.Str2ToInt(str);
                }
            }
            catch (lab3.Converter.ConvertException)
            {
                MessageBox.Show("Неккоректные символы в адресе");
                return;
            }

            adr = adr & 0x3FFFFFF;
            for (var i = 0; i < AdrBits.Length; ++i)
            {
                AdrBits[i].Text = (adr & (int)Math.Pow(2, i)) != 0 ? "1" : "0";
            }

            device_adr_check.Checked = (adr >= 64944820 && adr <= 64944823);
            UpdateController();
        }

        private void UpdateAdrCheck()
        {
            adr_reg_check.Checked = (we_checkbox.Checked && device_adr_check.Checked && a1.Text == "0" && a0.Text == "0");
        }

        private void UpdateDataCheck()
        {
            data_reg_check.Checked = (we_checkbox.Checked && device_adr_check.Checked && a1.Text == "0" && a0.Text == "1");
        }

        private void UpdateWriteCheck()
        {
            write_check.Checked = (we_checkbox.Checked && device_adr_check.Checked && a1.Text == "1" && a0.Text == "0");
        }

        private void UpdateReadCheck()
        {
            read_check.Checked = (re_checkbox.Checked && device_adr_check.Checked && a1.Text == "1" && a0.Text == "1");
        }

        private void UpdateController()
        {
            write_enabled.Text = we_checkbox.Checked ? "1" : "0";
            read_enabled.Text = re_checkbox.Checked ? "1" : "0";

            UpdateAdrCheck();
            UpdateDataCheck();
            UpdateWriteCheck();
            UpdateReadCheck();

            if (write_check.Checked)
                // Write to flash
                try
                {
                    var address = int.Parse(adr_reg_content.Text);
                    var data = int.Parse(data_reg_content.Text);
                    try
                    {
                        FlashMemory.Add(int.Parse(adr_reg_content.Text), int.Parse(data_reg_content.Text));
                    }
                    catch (System.Exception)
                    {
                        FlashMemory[int.Parse(adr_reg_content.Text)] = int.Parse(data_reg_content.Text);
                    }
                }
                catch (System.Exception)
                {

                    return;
                }

            if (read_check.Checked)
                // Read from flash
                try
                {
                    var address = int.Parse(adr_reg_content.Text);
                    try
                    {
                        data_reg_content.Text = FlashMemory[address].ToString();
                    }
                    catch (System.Exception)
                    {
                        data_reg_content.Text = "0";
                    }
                }
                catch (System.Exception)
                {

                    return;
                }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateController();
        }

        private void re_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateController();
        }

        private Dictionary<int, int> FlashMemory = new Dictionary<int, int>();

        TextBox[] DataBits;

        private void button1_Click(object sender, EventArgs e)
        {
            if (DataBits == null)
            {
                DataBits = new TextBox[] { d0, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15 };
            }
            int data = 0;
            var str = data_text.Text.ToUpper();
            if (data_hex.Checked)
            {
                for (var i = 0; i < str.Length; ++i)
                {
                    var ch = str[str.Length - i - 1];
                    int p2 = (int)Math.Pow(16, i);
                    if (ch >= '0' && ch <= '9')
                    {
                        data += p2 * (ch - '0');
                    }
                    else if (ch >= 'A' && ch <= 'F')
                    {
                        data += p2 * (ch - 'A' + 10);
                    }
                    else
                    {
                        MessageBox.Show("Неккоректные символы: " + ch);
                        return;
                    }
                }
            }
            else if (data_dec.Checked)
            {
                for (var i = 0; i < str.Length; ++i)
                {
                    var ch = str[str.Length - i - 1];
                    int p2 = (int)Math.Pow(10, i);
                    if (ch >= '0' && ch <= '9')
                    {
                        data += p2 * (ch - '0');
                    }
                    else
                    {
                        MessageBox.Show("Неккоректные символы: " + ch);
                        return;
                    }
                }
            }
            else
            {
                for (var i = 0; i < str.Length; ++i)
                {
                    var ch = str[str.Length - i - 1];
                    int p2 = (int)Math.Pow(2, i);
                    if (ch >= '0' && ch <= '1')
                    {
                        data += p2 * (ch - '0');
                    }
                    else
                    {
                        MessageBox.Show("Неккоректные символы: " + ch);
                        return;
                    }
                }
            }
            data = data & 0xFFFF;
            for (var i = 0; i < DataBits.Length; ++i)
            {
                DataBits[i].Text = (data & (int)Math.Pow(2, i)) != 0 ? "1" : "0";
            }

            if (adr_reg_check.Checked)
            {
                adr_reg_content.Text = data.ToString();
            }
            else if (data_reg_check.Checked)
            {
                data_reg_content.Text = data.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FlashMemory.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(lab3.FlashReader.ReadFlash(FlashMemory), "Flash", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
