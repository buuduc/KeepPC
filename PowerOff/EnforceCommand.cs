
using System.Diagnostics;
using System.Windows.Forms;

namespace PowerOff
{
    class EnforceCommand
    {

        public EnforceCommand()
        {
        }
        private string Command(string Filename, string argument="", bool UseShellExecute=false)
        {
            var cmd = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Filename,
                    Arguments = argument,
                    UseShellExecute = UseShellExecute,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                }
            };

            cmd.Start();
            //cmd.WaitForExit();
            string k = cmd.StandardOutput.ReadToEnd();
            if (cmd.StandardOutput.ReadToEnd() != "")
            {
                MessageBox.Show("Lệnh không hợp lệ \n Mã lỗi: \n" + cmd.StandardOutput.ReadToEnd(),
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cmd.WaitForExit();
            cmd.Close();
            return k;
        }
        public void ShutdownByTime(object time)
        {
            Command("shutdown", "/s /t " + (time).ToString());
        }
        public void IgnoreShutdown()
        {
            Command("shutdown", "/a");
        }
        public void RestartByTime(object time)
        {
            Command("shutdown", "/r /t " + time.ToString());
        }
        public void RecoveryMode()
        {
            Command("shutdown", "/o /r /t 5");
        }
        public void dxdiag()
        {
            Command("dxdiag", "");
        }
        public void msconfig()
        {
            var a=Process.Start("compmgmt.msc");
            a.WaitForExit();
        }
        public void system_info()
        {
            Command("msinfo32", "");
        }
        public void windown_infor()
        {
            Command("winver", "");
        }
        public void Ultimate()
        {
            var txt = Command("powercfg", "/l");
            if (txt.IndexOf("Ultimate Performance")==-1)
            {
                Command("powercfg", "-duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61");
                int cuoi = txt.IndexOf("(Ultimate Performance)");
                string ketqua = txt.Substring(cuoi - 38, 38);
                MessageBox.Show(ketqua);
                Command("powercfg", "/s " + ketqua);
            }
            else
            {
            DialogResult a=MessageBox.Show("Bạn đã có sẵn chế độ này trong thiết bị, bạn có muốn sử dụng nó không ?",
                "Thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (a == DialogResult.Yes)
                {
                int cuoi = txt.IndexOf("(Ultimate Performance)");
                string ketqua=txt.Substring(cuoi-38,38);
                MessageBox.Show(ketqua);
                Command("powercfg", "/s " + ketqua);
                }
           
            }
        }
        public void BatteryReport()
        {
            var cmd = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "<powercfg /batteryreport /output>",
                    WorkingDirectory= "<C:/Windows/System32/cmd.exe>"
                }
            };

            cmd.Start();
           
            MessageBox.Show("test successful");
        }
        public void DiskCleanup()
        {
            Command("cleanmgr");
        }
        public void DefragmentDisk()
        {
            Command("dfrgui");
        }
        public void SystemProperties()
        {
            Process.Start("sysdm.cpl");
        }
        public void MdSched()
        {
            Process.Start("mdsched");
        }
        public void OnScreenKeyBoard()
        {
            Process.Start("osk");
        }
        public void Diskmanger()
        {
            Process.Start("diskmgmt.msc");
        }
    }
}
