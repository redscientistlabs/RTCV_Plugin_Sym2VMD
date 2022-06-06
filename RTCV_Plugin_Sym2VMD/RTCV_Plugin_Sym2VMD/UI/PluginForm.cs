namespace RTCV_Plugin_Sym2VMD.UI
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using NLog;
    using RTCV.CorruptCore;
    using RTCV.NetCore;
    using RTCV.Common;
    using RTCV.UI;
    using static RTCV.CorruptCore.RtcCore;
    using RTCV.Vanguard;
    using System.IO;
    using System.Text.RegularExpressions;

    public partial class PluginForm : Form
    {
        public RTCV_Plugin_Sym2VMD plugin;

        public SymbolMapFormat format;
        public GenerationMode generationMode;
        public string MemoryDomainName;

        public volatile bool HideOnClose = true;

        Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public PluginForm(RTCV_Plugin_Sym2VMD _plugin)
        {
            plugin = _plugin;

            this.InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.PluginForm_FormClosing);


            this.Text = RTCV_Plugin_Sym2VMD.CamelCase(nameof(RTCV_Plugin_Sym2VMD).Replace("_", " ")) + $" - Version {plugin.Version.ToString()}"; //automatic window title

            var formats = Enum.GetNames(typeof(SymbolMapFormat));
            foreach (var format in formats)
            {
                cbSymbolFormat.Items.Add(format);
            }
            var modes = Enum.GetNames(typeof(GenerationMode));
            foreach (var mode in modes)
            {
                cbGenMode.Items.Add(mode);
            }
        }



        private void PluginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(HideOnClose)
            {
                e.Cancel = true;
                this.Hide();
            }    
        }

        private void PluginForm_Load(object sender, EventArgs e)
        {

        }

        private void cbSymbolFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            format = (SymbolMapFormat)Enum.Parse(typeof(SymbolMapFormat), cbSymbolFormat.SelectedItem.ToString());
        }

        private void cbGenMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            generationMode = (GenerationMode)Enum.Parse(typeof(GenerationMode), cbGenMode.SelectedItem.ToString());
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = Application.ExecutablePath;
                dlg.Filter = "linker maps (*.map)|*.map";
                dlg.FilterIndex = 0;
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    TextReader reader = new StreamReader(dlg.FileName);
                    var vmds = SymbolMapFileReader.GenerateVMDs(reader, format, generationMode, MemoryDomainName, tbFilter.Text);
                    if (vmds == null)
                    {
                        MessageBox.Show("No VMDs could be made.");
                        return;
                    }
                    foreach (var vmd in vmds)
                    {
                        LocalNetCoreRouter.Route(Ep.RTC_SIDE, Commands.MAKEAVMD, (object)vmd, true);
                    }
                }
            }
        }

        private void tbMDName_TextChanged(object sender, EventArgs e)
        {
            MemoryDomainName = tbMDName.Text;
        }
    }
}
