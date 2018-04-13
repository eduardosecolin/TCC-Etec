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
            MessageBoxResult resultado = MessageBox.Show("Deseja realmente sair do sitema?", "Sair", 
                                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
           if(resultado == MessageBoxResult.Yes){
                Close();
           }else{
                return;
           }          
        }

        //Quando form carregado validar usuario(admin ou user) e mostrar items do statusBar
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            
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
            var sql = from a in conexao.AGENDA where a.data == DateTime.Today 
            select new { a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.nome_barbeiro };
            dgAgenda.ItemsSource = null;
            dgAgenda.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
        }


        // botao Barbeiros
        private void btnBarbeiros_Click(object sender, RoutedEventArgs e) {
            Barbeiros janela = new Barbeiros();
            janela.Show();
        }

        // botao Clientes
        private void btnClientes_Click(object sender, RoutedEventArgs e) {
            Clientes janela = new Clientes();
            janela.Show();
        }

        // botao config. usuarios
        private void btnConfig_Click(object sender, RoutedEventArgs e) {
            Configurações_de_Usuários janela = new Configurações_de_Usuários();
            janela.Show();           
        }

        // botao menuitem agenda
        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {
            btnAgenda_Click(sender, e);
        }

        // botao menuitem usuarios
        private void MenuItem_Click_2(object sender, RoutedEventArgs e) {
            btnConfig_Click(sender, e);
        }

        // botao menuitem clientes
        private void MenuItem_Click_3(object sender, RoutedEventArgs e) {
            btnClientes_Click(sender, e);
        }

        // botao menuitem barbeiros
        private void MenuItem_Click_4(object sender, RoutedEventArgs e) {
            btnBarbeiros_Click(sender, e);
        }

        // botao contas pagar
        private void btnPagar_Click(object sender, RoutedEventArgs e) {
            ContasPagar janela = new ContasPagar();
            janela.Show();
        }

        // botao menuitem contas pagar
        private void MenuItem_Click_5(object sender, RoutedEventArgs e) {
            btnPagar_Click(sender, e);
        }

        // botao contas a receber
        private void btnReceber_Click(object sender, RoutedEventArgs e) {
            ContasReceber janela = new ContasReceber();
            janela.Show();
        }

        // botao contas a receber menuItem
        private void MenuItem_Click_6(object sender, RoutedEventArgs e) {
            btnReceber_Click(sender, e);
        }

        // botao fornecedores
        private void btnFornecedores_Click(object sender, RoutedEventArgs e) {
            Fornecedores janela = new Fornecedores();
            janela.Show();
        }

        // botao fornecedores menuItem
        private void MenuItem_Click_7(object sender, RoutedEventArgs e) {
            btnFornecedores_Click(sender, e);
        }

        // selecionar a data e mostrar no datagrid
        private void calendario_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) {
            var sql = from a in conexao.AGENDA
                      where a.data == calendario.SelectedDate
                      select new { a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.nome_barbeiro };
            dgAgenda.ItemsSource = null;
            dgAgenda.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
        }

        // botao funcionarios
        private void btnFuncionarios_Click(object sender, RoutedEventArgs e) {
            Funcionarios janela = new Funcionarios();
            janela.Show();
        }

        // botao funcionarios menuItem
        private void MenuItem_Click_8(object sender, RoutedEventArgs e) {
            btnFuncionarios_Click(sender, e);
        }

        // botao produtos
        private void btnProdutos_Click(object sender, RoutedEventArgs e) {
            Produtos janela = new Produtos();
            janela.Show();
        }
        // botao menuItem produtos
        private void MenuItem_Click_9(object sender, RoutedEventArgs e) {
            btnProdutos_Click(sender, e);
        }
    }
}

