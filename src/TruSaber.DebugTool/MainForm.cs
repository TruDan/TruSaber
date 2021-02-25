using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TruSaber.Debugging;

namespace TruSaber.DebugTool
{
    public partial class MainForm : Form
    {
        private readonly IDebugService _debugService;
        public static MainForm Instance { get; private set; }
        
        public MainForm(IDebugService debugService)
        {
            _debugService = debugService;
            Instance = this;
            InitializeComponent();
            var props =  _debugService.Properties;
            propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter((IDictionary)props);
            props.CollectionChanged += PropsOnCollectionChanged;
        }

        private void PropsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            propertyGrid1.Refresh();
        }

        private void cameraView2_Load(object sender, EventArgs e)
        {

        }

        private void cameraView1_Load(object sender, EventArgs e)
        {

        }
    }
}