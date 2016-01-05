using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dyd.BaseService.TaskManager.Core;
using Dyd.BaseService.TaskManager.Domain.Model;
using SharpCompress.Archive.Rar;
using SharpCompress.Common;
using SharpCompress.Reader;
using XXF.ProjectTool;

namespace Dyd.BaseService.TaskManager.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //this.ShowInTaskbar = false;
            //this.WindowState = FormWindowState.Minimized;
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
           //var o =  new CryptoHelper().Encrypt("cofnig");
           //var m = new CryptoHelper().Decrypt(o);
           //var a = 1;
            //CompressHelper.UnCompress(@"E:\111.zip",@"E:\111");


           //SqlHelper.ExcuteSql("server=192.168.17.201;Initial Catalog=dyd_bs_task;User ID=sa;Password=Xx~!@#;", (c) =>
           //{
           //    Domain.Dal.tb_version_dal versiondal = new Domain.Dal.tb_version_dal();
           //    versiondal.Edit(c, new tb_version_model()
           //    {
           //        id = 6,
           //        taskid = 8,
           //        version = 1,
           //        versioncreatetime = DateTime.Now,
           //        zipfile = System.IO.File.ReadAllBytes(@"E:\8.rar"),
           //        zipfilename = "8.rar"
           //    });
           //});

            XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo tai = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            tai.Add("ConfigConnectString", @"server=192.168.17.205;Initial Catalog=dyd_new_config;User ID=sa;Password=Xx~!@#; ");
            string o = new XXF.Serialization.JsonHelper().Serializer(tai);
            //int a = 1;

            //EmailHelper email = new EmailHelper();
            //email.mailFrom = "fengyeguigui@163.com";
            //email.mailPwd = "472790378@";
            //email.mailSubject = "11";
            //email.mailBody = "111";
            //email.isbodyHtml = true;    //是否是HTML
            //email.host = "smtp.163.com";//如果是QQ邮箱则：smtp:qq.com,依次类推
            //email.mailToArray = new string[] { "472790378@qq.com" };//接收者邮件集合
            //email.mailCcArray = new string[] { };//抄送者邮件集合
            //if (email.Send())
            //{
              

            //}
            //else
            //{
                
            //}
        }
    }
}
