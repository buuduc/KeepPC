using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PowerOff
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Form1Load();
        }
        private void Form1Load()
        {
            numericUpDown2.Value = DateTime.Now.Hour;
            numericUpDown1.Value = DateTime.Now.Minute;
        }
        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton recent = sender as RadioButton;
            if (recent.Checked)
            {
                panel2.Enabled = recent.TabIndex == 1 ? panel1.Enabled : !panel2.Enabled ;
                panel1.Enabled = !panel1.Enabled;
                this.checking = recent.TabIndex;
            }
            
        }
        object checking=0;
        double tick;
        void CaCulator()
        {
            switch (this.checking)
            {
                case 1:
                    {
                        var Now = DateTime.Now;
                        DateTime fur = dateTimePicker1.Value;
                        fur = new DateTime(fur.Year, fur.Month, fur.Day, Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown1.Value), 0);
                        this.tick = fur.Subtract(Now).TotalSeconds;
                        break;
                    }
                case 0:
                    {
                        this.tick = (double)((numericUpDown4.Value * 3600 + numericUpDown5.Value * 60 + numericUpDown6.Value));
                        break;

                    }
            }
            this.tick=System.Math.Round(this.tick);
            
        }
        private DialogResult show_mess(string command)
        {  
            //DateTime TimeRemain = new DateTime().AddSeconds(this.tick);
            TimeSpan TimeRemain = new TimeSpan(0, 0, 0, (int)this.tick);
            if(this.tick<0)
            {
                

                MessageBox.Show("Thời gian bạn nhập là không hợp lệ ! \n", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return DialogResult.Cancel;
            }
            string day = TimeRemain.Days == 0 ? "" : TimeRemain.Days.ToString()+" ngày ";
            string hour = TimeRemain.Hours == 0 ? "" : TimeRemain.Hours.ToString()+" giờ ";
            string minitues = TimeRemain.Minutes == 0 ? "" : TimeRemain.Minutes.ToString() + " phút ";
            string second= TimeRemain.Seconds == 0 ? "" : TimeRemain.Seconds.ToString() + " giây ";
            string Timer;
            if (TimeRemain.TotalSeconds != 0)
            {
                Timer = " sau " + day  + hour + minitues  + second + " nữa";
            }
            else
            {
                Timer = " NGAY BÂY GIỜ !";
            }
            return MessageBox.Show("Thiết bị của bạn sẽ tự động " + command + Timer + "  " +
                "\n Bạn có chắc muốn thực hiện hành động này không ?", "Thông Báo", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }
        private void shutdownbtn_Click(object sender, EventArgs e)
        {
            CaCulator();
            switch (show_mess("SHUTDOWN"))
            {
                case DialogResult.OK:

                    CallDirect("SHUTDOWN");
                    break;

            }

        }
        private void restartBtn_Click(object sender, EventArgs e)
        {
            CaCulator();
            show_mess("RESTART");
        }
        private void AbortBtn_Click(object sender, EventArgs e)
        {
            CallDirect("ABORT");
            MessageBox.Show("Xoá hẹn giờ thành công");
        }
        private EnforceCommand cmd = new EnforceCommand();
        private void CallDirect(string command)
        {
            
            try
            {
                switch (command)
                {
                    case "SHUTDOWN":
                        {
                            cmd.ShutdownByTime(this.tick);
                            break;
                        }
                    case "RESTART":
                        {
                            cmd.RestartByTime(this.tick);
                            break;
                        }
                    case "ABORT":
                        {
                            cmd.IgnoreShutdown();
                            
                            break;
                        }
                }
                if (command== "SHUTDOWN" | command == "RESTART")
                {
                DateTime TimeRemain = DateTime.Now.AddSeconds(this.tick);
                string day = TimeRemain.Day == 0 ? "" : " ngày "+ TimeRemain.Day.ToString();
                string hour =  TimeRemain.Hour.ToString() + " giờ ";
                string minitues = TimeRemain.Minute == 0 ? "" : TimeRemain.Minute.ToString() + " phút ";
                string second = TimeRemain.Second == 0 ? "" : TimeRemain.Second.ToString() + " giây ";
                string month = TimeRemain.Month == 0 ? "" : " tháng "+TimeRemain.Month.ToString() ;
                string year = TimeRemain.Year == 0 ? "" : " năm " + TimeRemain.Year.ToString();
                string Timer = "vào lúc "+hour + minitues + day +month +year;
                
                MessageBox.Show("Thiết bị của bạn sẽ tự động " + command + Timer + "  " +
                    "\n nếu muốn huỷ hành động này, hãy nhấn nút Xoá Cài Đặt !", "Thông Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                throw;
            }
        }

        

        private void ThongTinChung_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.dxdiag();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.system_info();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void UltimateBtn(object sender, EventArgs e)
        {
            try
            {
                DialogResult k = MessageBox.Show("Đây là plan năng lượng " +
                    "được cung cấp sẵn cho các dòng máy trạm và máy gaming hi-end " +
                    "do Microsoft phát triển. " +
                    "Nó cho phép đẩy hiệu năng thiết bị lên mức tối đa, " +
                    "Điều này là không nên với các dòng máy thông thường. Bạn có muốn" +
                    " kích hoạt nó không ?", "Cảnh Báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (k==DialogResult.Yes)
                    cmd.Ultimate();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void WIndowBTN_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.windown_infor();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void RecorveryBTN_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult k = MessageBox.Show("Để truy cập Recovery Mode, thiết bị của bạn sẽ cần khởi động lại" +
                    "\n Kích hoạt ngay bây giờ ?", "Cảnh Báo !", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (k == DialogResult.Yes)
                {
                    MessageBox.Show("Kích hoạt thành công! ");
                    cmd.RecoveryMode();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Process.Start("Control Panel/System and Security/System");
                cmd.DiskCleanup();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void ComManaBTN_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.msconfig();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {   
                cmd.SystemProperties();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var a = sender as CheckBox;
            panel4.Enabled = a.Checked; 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                cmd.DiskCleanup();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.DefragmentDisk();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.MdSched();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.OnScreenKeyBoard();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                cmd.Diskmanger();

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không xác định ! \n" + e.ToString(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
    }
    
}
