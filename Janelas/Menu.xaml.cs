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
        Clientes janelaClientes;
        Fornecedores janelaFornecedore;
        Produtos janelaProdutos;
        Estoque janelaEstoque;
        ContasPagar janelaPagar;
        ContasReceber janelaReceber;
        Caixa janelaCaixa;
        Funcionarios janelaFuncionarios;
        Barbeiros janelaBarbeiros;
        Configurações_de_Usuários janelaUsuario;
        Sobre janelaSobre;

        Agenda janelaAgenda;
        
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
                App.Current.Shutdown();
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
            btnPagar.Opacity = .50;
            btnReceber.Opacity = .50;
            btnCaixa.Opacity = .50;
            btnFuncionarios.Opacity = .50;
            btnConfig.Opacity = .50;

            btnPagar.IsEnabled = false;
            btnReceber.IsEnabled = false;
            btnCaixa.IsEnabled = false;
            btnFuncionarios.IsEnabled = false;
            btnConfig.IsEnabled = false;
        }

        //Botao agenda
        private void btnAgenda_Click(object sender, RoutedEventArgs e) {
            janelaAgenda = new Agenda(this);
            janelaAgenda.Show(); 
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
            janelaBarbeiros = new Barbeiros();
            janelaBarbeiros.Show();
        }

        // botao Clientes
        private void btnClientes_Click(object sender, RoutedEventArgs e) {
            janelaClientes = new Clientes();
            janelaClientes.Show();
        }

        // botao config. usuarios
        private void btnConfig_Click(object sender, RoutedEventArgs e) {
            janelaUsuario = new Configurações_de_Usuários();
            janelaUsuario.Show();           
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
        private void BtnPagar_Click(object sender, RoutedEventArgs e) {
            janelaPagar = new ContasPagar();
            janelaPagar.Show();
        }

        // botao menuitem contas pagar
        private void MenuItem_Click_5(object sender, RoutedEventArgs e) {
            BtnPagar_Click(sender, e);
        }

        // botao contas a receber
        private void btnReceber_Click(object sender, RoutedEventArgs e) {
            janelaReceber = new ContasReceber();
            janelaReceber.Show();
        }

        // botao contas a receber menuItem
        private void MenuItem_Click_6(object sender, RoutedEventArgs e) {
            btnReceber_Click(sender, e);
        }

        // botao fornecedores
        private void btnFornecedores_Click(object sender, RoutedEventArgs e) {
            janelaFornecedore = new Fornecedores();
            janelaFornecedore.Show();
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
            janelaFuncionarios = new Funcionarios();
            janelaFuncionarios.Show();
        }

        // botao funcionarios menuItem
        private void MenuItem_Click_8(object sender, RoutedEventArgs e) {
            btnFuncionarios_Click(sender, e);
        }

        // botao produtos
        private void btnProdutos_Click(object sender, RoutedEventArgs e) {
            janelaProdutos = new Produtos();
            janelaProdutos.Show();
        }
        // botao menuItem produtos
        private void MenuItem_Click_9(object sender, RoutedEventArgs e) {
            btnProdutos_Click(sender, e);
        }

        // botao estoque
        private void btnEstoque_Click(object sender, RoutedEventArgs e) {
            janelaEstoque = new Estoque();
            janelaEstoque.Show();
        }
        // botao estoque menuItem
        private void MenuItem_Click_10(object sender, RoutedEventArgs e) {
            btnEstoque_Click(sender, e);
        }

        // botao sobre
        private void MenuItem_Click_11(object sender, RoutedEventArgs e) {
            janelaSobre = new Sobre();
            janelaSobre.Show();
        }

        // botao caixa
        private void btnCaixa_Click(object sender, RoutedEventArgs e) {
            janelaCaixa = new Caixa();
            janelaCaixa.Show();
        }
        // botao caixa menuItem
        private void MenuItem_Click_12(object sender, RoutedEventArgs e) {
            btnCaixa_Click(sender, e);
        }

        // botao logout menuItem
        private void MenuItem_Click_13(object sender, RoutedEventArgs e) {
            Login janela = new Login();
            janela.Show();
            if (janelaAgenda != null) {
                janelaAgenda.Close();
            }
            fecharJanelasAbertas();
            this.Close();
        }

        // metodo para fechar as janelas
        private void fecharJanelasAbertas(){
            janelaClientes.Close();
            janelaFornecedore.Close();
            janelaProdutos.Close();
            janelaEstoque.Close();
            janelaPagar.Close();
            janelaReceber.Close();
            janelaCaixa.Close();
            janelaFuncionarios.Close();
            janelaBarbeiros.Close();
            janelaUsuario.Close();
        }
    }
}

