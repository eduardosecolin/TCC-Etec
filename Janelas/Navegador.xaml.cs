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
using BarberSystem.Controle;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Navegador.xaml
    /// </summary>
    public partial class Navegador : Window {

        public Navegador() {
            InitializeComponent();
        }

        // voltar pagina
        private void btnAtras_Click(object sender, RoutedEventArgs e) {
            try {
                Browser.GoBack();
            }
            catch (Exception) {
                MessageBox.Show("Nenhuma pagina para voltar", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // avancar pagina
        private void btnFrente_Click(object sender, RoutedEventArgs e) {
            try {
                Browser.GoForward();
            }
            catch (Exception) {
                MessageBox.Show("Nenhuma pagina para avançar", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // ir para pagina
        private void btnGo_Click(object sender, RoutedEventArgs e) {
            try {

             if(txtSearch.Text.Equals(string.Empty)){
                    MessageBox.Show("Digite um endereço de web valido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
             }
                string verifica = txtSearch.Text.Substring(0, 3);
                if (verifica.Equals("htt")) {
                    Browser.Source = new Uri(txtSearch.Text);
                }
                if(verifica.Equals("www")){
                    Browser.Source = new Uri("http://" + txtSearch.Text);
                }
            }catch(Exception a){
                MessageBox.Show("Erro inesperado ou Endereço invalido" + "\n" + a.StackTrace, "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // atualizar pagina
        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            try {
                Browser.Refresh();
            }
            catch (Exception a) {
                MessageBox.Show(a.Message, "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
