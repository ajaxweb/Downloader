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
