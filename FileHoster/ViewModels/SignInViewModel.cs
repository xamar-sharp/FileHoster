using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using FileHoster.Commands;
using System.ComponentModel;
namespace FileHoster.ViewModels
{
    public sealed class SignInViewModel
    {
        private string _userName;
        public ICommand SignInCommand { get; set; }
        public string UserName { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public SignInViewModel()
        {
            SignInCommand = new Command((obj) =>
            {
                App.UserName = UserName;
                App.ApplicationState = Resource.Signed;
                (obj as System.Windows.Window).Close();
            }, (obj) => !string.IsNullOrEmpty(UserName));
        }
    }
}
