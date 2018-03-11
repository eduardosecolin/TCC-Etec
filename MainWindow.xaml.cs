using BarberSystem.Janelas;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;


namespace BarberSystem {
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window {
         public MainWindow() {
            InitializeComponent();
        }
        private delegate void ProgressBarDelegate();

        private void criarConstrucao() {
            PB.IsIndeterminate = false;
            PB.Maximum = 2500;
            PB.Value = 0;

            for (int i = 0; i < 2500; i++) {
                PB.Dispatcher.Invoke(new ProgressBarDelegate(UpdateProgress), DispatcherPriority.Background);
            }
        }
        private void UpdateProgress() {
            PB.Value += 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            int cont = 0;
            while (cont < 5) {
                criarConstrucao();
                cont++;
            }
            try {
                Login janela = new Login();
                janela.Show();
                this.Hide();
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception a) {
                MessageBox.Show(a.Message);
            }
        }
    }
}
