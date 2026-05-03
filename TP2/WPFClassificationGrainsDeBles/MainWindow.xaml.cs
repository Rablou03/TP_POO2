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

namespace WPFClassificationGrainsDeBles
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.MainWindowViewModel();
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