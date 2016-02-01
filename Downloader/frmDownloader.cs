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
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Web;
using System.Windows.Forms;

namespace Downloader
{
    public partial class frmDownloader : Form
    {
        public frmDownloader()
        {
            InitializeComponent();
        }                         

        private void frmDownloader_Load(object sender, EventArgs e)
        {
            NameValueCollection nvc = this.GetQueryStringParameters();
            String installer = nvc.Get("game");
            String path = nvc.Get("path");
            string startfile = nvc.Get("exe");

            try
            {
                Downloader d = new Downloader(installer, startfile, path);
                d.start(); 
            }
            catch (Exception e1) {}
  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar2.Value < 100)
                progressBar2.Value += 10;
        }

        private NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try
                {
                    string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;                 
                    nameValueTable = HttpUtility.ParseQueryString(queryString);
                }
                catch (Exception e) { }
            }

            return (nameValueTable);
        }
    }
}
