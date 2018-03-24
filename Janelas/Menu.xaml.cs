using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using BarberSystem.Dados;

namespace BarberSystem.Janelas
{
    /// <summary>
    /// Lógica interna para Menu.xaml
    /// </summary>
    public  partial class Menu : Window
    {
        BancoDados conexao = new BancoDados();
        public Menu()
        {
            InitializeComponent();
            dgAgenda.RowBackground = null;
            carregaGrig();
        }

        //Botao de sair menuitem
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        //Quando form carregado validar usuario(admin ou user) e mostrar items do statusBar
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            
            sbHora.Content = sbHora.Content + " " + DateTime.Now.ToShortTimeString();
            sbData.Content = sbData.Content + " " + DateTime.Now.ToLongDateString();
            sbUsuario.Content = sbUsuario.Content + " " + Login.usuarioLogado;


            BancoDados bd = new BancoDados();
            var sql = from u in bd.USUARIOS where u.nome_usuario == Login.usuarioLogado select u.tipo;
            if(sql.FirstOrDefault() == "admin"){
                return;
            }else{
                esconderBotoes();
            }
        }

        //Metodo para validar usuario deixando inacessivel os botoes para user
        public void esconderBotoes(){
            btnPagar.IsEnabled = false;
            btnReceber.IsEnabled = false;
            btnCaixa.IsEnabled = false;
            btnFuncionarios.IsEnabled = false;
            btnConfig.IsEnabled = false;
        }

        //Botao agenda
        private void btnAgenda_Click(object sender, RoutedEventArgs e) {
            Agenda janela = new Agenda(this);
            janela.Show(); 
        }


        //Popular o dataGrid
        public void carregaGrig() {
            List<AGENDA> listaAgenda = conexao.AGENDA.ToList();
            dgAgenda.ItemsSource = null;
            dgAgenda.ItemsSource = listaAgenda.OrderBy(user => user.hora_inicio);
        }
    }
}

