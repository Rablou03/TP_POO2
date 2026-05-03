// Views/MainWindow.xaml.cs
using System.Windows;
using ClassificationGrainsDeBle_WPF.ViewModels;

namespace ClassificationGrainsDeBle_WPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void ShowAboutDialog(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Classification de Grains de Blé\n" +
                "Algorithme k-NN (k plus proches voisins)\n\n" +
                "Développé dans le cadre du cours POO2\n" +
                "Université du Québec à Rimouski (UQAR)\n\n" +
                "© 2026",
                "À propos",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}