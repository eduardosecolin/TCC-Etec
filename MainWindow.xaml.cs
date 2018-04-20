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

        private const int TEMP = 700;
         public MainWindow() {
            InitializeComponent();
            carregarprogressBar();
        }
        private delegate void ProgressBarDelegate();

        private void criarConstrucao() {
            PB.IsIndeterminate = false;
            PB.Maximum = TEMP;
            PB.Value = 0;

            for (int i = 0; i < TEMP; i++) {
                PB.Dispatcher.Invoke(new ProgressBarDelegate(UpdateProgress), DispatcherPriority.Background);
            }
        }
        private void UpdateProgress() {
            PB.Value += 1;
        }


        private void carregarprogressBar(){
            int cont = 0;
            while (cont < 5) {
                if (cont >= 1) {
                    lblCarregar.Content = lblCarregar.Content + ".";
                }
                criarConstrucao();
                cont++;
            }
            try {
                Login janela = new Login();
                janela.Show();
                this.Hide();
                Close();
            }
            catch (Exception a) {
                MessageBox.Show(a.Message);
            }
        }
    }
}
