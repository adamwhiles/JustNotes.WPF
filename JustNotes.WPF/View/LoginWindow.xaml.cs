using JustNotes.WPF.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace JustNotes.WPF.View
{
    /// <summary>
    /// Interaction logic for LoginWindows.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginViewModel VM;
        public LoginWindow()
        {
            InitializeComponent();
            VM = Resources["vm"] as LoginViewModel;

            VM.Authenticated += VM_Authenticated;
        }

        private void VM_Authenticated(object sender, EventArgs e)
        {
            Close();
        }
    }
}
