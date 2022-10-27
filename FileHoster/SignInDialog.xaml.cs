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

namespace FileHoster
{
    /// <summary>
    /// Логика взаимодействия для SignInDialog.xaml
    /// </summary>
    public partial class SignInDialog : Window
    {
        public FileHoster.ViewModels.SignInViewModel ViewModel { get; set; }
        public SignInDialog()
        {
            InitializeComponent();
            ViewModel = new ViewModels.SignInViewModel();
            this.DataContext = ViewModel;
        }
    }
}
