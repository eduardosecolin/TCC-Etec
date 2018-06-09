using BarberSystem.Janelas;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using BarberSystem.Controle;
using BarberSystem.Dados;
using System.Data.Entity;
using log4net;

namespace BarberSystem {
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public BancoDados conexao = new BancoDados();

        private const int TEMP = 700;
        public MainWindow() {
            InitializeComponent();
            //createDataBase();
            createDataBaseEF();
            if (!SqlServer.existeDados()) {
                SqlServer.acesso();
            }
            carregarprogressBar();
            log4net.Config.XmlConfigurator.Configure();
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
                Log.logException(a);
                Log.logMessage(a.Message);
            }
        }

        private void createDataBase(){
            SqlServer.createIfNotExists("Data Source=" + SqlServer.getServer() + "\\" + SqlServer.getInstance() + ";Initial Catalog=BARBER_DATABASE;Integrated Security=True");
            SqlServer.existeTabela("dbo.AGENDA");
            if (!SqlServer.existeDados()) {
                SqlServer.acesso();
            }
        }

        private void createDataBaseEF(){
            if (!conexao.Database.Exists()) {
                conexao.Database.CreateIfNotExists();
            }
        }
    }
}
