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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileHoster.ViewModels;
namespace FileHoster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PageViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            var title = new Paragraph(new Run(Resource.FileHosterAbout)) { FontWeight = FontWeights.Bold };
            var desc = new Paragraph(new Run(Resource.FileHosterDescription)) { FontStyle = FontStyles.Italic };
            entering.Blocks.Add(title);
            entering.Blocks.Add(desc);
            ViewModel = new PageViewModel();
            ViewModel.GetFilesCommand.Execute(0);
            this.DataContext = ViewModel;
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ViewModel.GetFilesCommand.Execute(null);
            if (ViewModel._currentModel != null)
            {
                if (innerData.Blocks.Count > 1)
                {
                    innerData.Blocks.Clear();
                }
                innerData.Blocks.Add(new Paragraph(new Run(Encoding.UTF8.GetString(ViewModel._currentModel.Data))));
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.GetFileCommand.Execute(string.IsNullOrEmpty((sender as ComboBox).Text) ? e.AddedItems[0] : (sender as ComboBox).Text);
        }
    }
    public class LocalizerExtension : System.Windows.Markup.MarkupExtension
    {
        public LocalizerExtension()
        {

        }
        public LocalizerExtension(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public override object ProvideValue(IServiceProvider provider)
        {
            return Resource.ResourceManager.GetString(Name, Resource.Culture);

        }
    }
}
