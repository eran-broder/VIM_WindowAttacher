using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIAutomationClient;
using Vigor.Windows.Native;

namespace WindowResizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Perform();
            }
        }
        
        private void Perform()
        {
            //var windowHandle = Native.FindWindow(null, TextWindowName.Text);
            //var client = new CUIAutomationClass();
            //var condition = client.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Vim Button1");
            //var condition = client.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, TextWindowName.Text);
            //var element = client.GetRootElement().FindFirst(TreeScope.TreeScope_Descendants, condition);
            //if (element == null)
            {
//                MessageBox.Show("No window");
            }
  //          else
            {
                var width = int.Parse(TextWidth.Text);
                var heigth = int.Parse(TextHeight.Text);
                Native.SetWindowPos(new IntPtr(int.Parse(TextWindowName.Text)), IntPtr.Zero, 0, 0, width, heigth,
                    Native.SetWindowPosFlags.DoNotReposition);
            }
            
        }
    }
}
