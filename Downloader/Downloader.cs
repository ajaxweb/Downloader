/*  This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/    
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Downloader
{
    class Downloader
    {
        private String installerExe;
        private String startExe;
        private String installDir;

        public Downloader(String _installerExe, String _startExe, String _installDir)
        {
            installerExe = _installerExe;
            startExe = _startExe;
            installDir = _installDir;
        }

        public void start()
        {
            installDosBox();
            Thread t = new Thread(new ThreadStart(installGame));
            t.Start();
        }

        private void installDosBox()
        {
            //download and install dosbox
            try {
                System.Net.WebClient webc = new System.Net.WebClient();
                webc.DownloadFile("https://gamestream.ml/dosbox_inst.exe", "dosbox_inst.exe");

                Process process = Process.Start("dosbox_inst.exe");
                process.WaitForExit();
            }catch(Exception e) {
                MessageBox.Show(e.Message);
                Application.ExitThread();
                Application.Exit();
            }
        }

        private void installGame()
        {
            //download and unpack game   
            try
            {
                String filename = installerExe.Substring(0, installerExe.Length - 4);
                if (!System.IO.Directory.Exists(getDosBoxPath() + "\\" + filename))
                {
                    System.Net.WebClient webc = new System.Net.WebClient();
                    webc.DownloadFile(new Uri("https://gamestream.ml/games/" + installerExe), getDosBoxPath() + installerExe);
                    Process process1 = Process.Start(getDosBoxPath() + installerExe);
                    process1.WaitForExit();
                    startDosBox();
                }
                else
                {
                    startDosBox();
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                Application.ExitThread();
                Application.Exit();
            }
        }

        private void startDosBox()
        {
            //start dosbox
            try
            {                
                String FileToStart = getDosBoxPath() + startExe;
                ProcessStartInfo pi = new ProcessStartInfo();
                pi.Arguments = FileToStart;
                pi.WorkingDirectory = getDosBoxPath();
                pi.FileName = "dosbox.exe";
                Process process2 = Process.Start(pi);
            }catch(Exception e) {
                MessageBox.Show(e.Message);
                Application.ExitThread();
                Application.Exit();
            }
        }

        private String getDosBoxPath()
        {
            //get env path
            String path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            if (System.IO.File.Exists(path + "\\DosBox\\dosbox.exe"))
                path = path + "\\DosBox\\";
            else
                path = path + " (x86)\\DosBox\\";            
            return path;
        }
    }
}
