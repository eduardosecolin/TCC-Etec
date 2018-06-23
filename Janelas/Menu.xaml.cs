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
using System.Diagnostics;
using BarberSystem.Controle;

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
        Navegador janelaNavegador;

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
            try {
                sbData.Content = sbData.Content + " " + DateTime.Now.ToLongDateString();
                sbUsuario.Content = sbUsuario.Content + " " + Login.usuarioLogado;


                BancoDados bd = new BancoDados();
                var sql = from u in bd.USUARIOS where u.nome_usuario == Login.usuarioLogado select u.tipo;
                if (string.Equals(sql.FirstOrDefault(), "admin", StringComparison.OrdinalIgnoreCase)) {
                    return;
                }
                else {
                    esconderBotoes();
                }
            }catch(Exception a){
                Log.logException(a);
                Log.logMessage(a.Message);
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

            // botoes menuItem
            btnPagarMenu.Opacity = .50;
            btnReceberMenu.Opacity = .50;
            btnCaixaMenu.Opacity = .50;
            btnFuncionariosMenu.Opacity = .50;
            btnUsuariosMenu.Opacity = .50;

            btnPagarMenu.IsEnabled = false;
            btnReceberMenu.IsEnabled = false;
            btnCaixaMenu.IsEnabled = false;
            btnFuncionariosMenu.IsEnabled = false;
            btnUsuariosMenu.IsEnabled = false;
        }

        //Botao agenda
        private void btnAgenda_Click(object sender, RoutedEventArgs e) {
            janelaAgenda = new Agenda(this);
            janelaAgenda.Show(); 
        }


        //Popular o dataGrid
        public void carregaGrig() {
            try {
                var sql = from a in conexao.AGENDA
                          where a.data == DateTime.Today
                          select new { a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.nome_barbeiro };
                dgAgenda.ItemsSource = null;
                dgAgenda.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
            }catch(Exception a){
                Log.logException(a);
                Log.logMessage(a.Message);
            }
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
            try {
                var sql = from a in conexao.AGENDA
                          where a.data == calendario.SelectedDate
                          select new { a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.nome_barbeiro };
                dgAgenda.ItemsSource = null;
                dgAgenda.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
            }catch(Exception a){
                Log.logException(a);
                Log.logMessage(a.Message);
            }
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
        private void BtnCaixa_Click(object sender, RoutedEventArgs e) {
            janelaCaixa = new Caixa();
            janelaCaixa.Show();
        }
        // botao caixa menuItem
        private void MenuItem_Click_12(object sender, RoutedEventArgs e) {
            BtnCaixa_Click(sender, e);
        }

        // botao logout menuItem
        private void MenuItem_Click_13(object sender, RoutedEventArgs e) {
            try {
                Login janela = new Login();
                janela.Show();
                if (janelaAgenda != null) {
                    janelaAgenda.Close();
                }
                fecharJanelasAbertas();
                this.Close();
            }catch(Exception a){
                Log.logException(a);
                Log.logMessage(a.Message);
            }
        }

        // metodo para fechar as janelas
        private void fecharJanelasAbertas(){
            if (janelaClientes != null) {
                janelaClientes.Close();
            }
            if (janelaFornecedore != null) {
                janelaFornecedore.Close();
            }
            if (janelaProdutos != null) {
                janelaProdutos.Close();
            }
            if(janelaEstoque != null) {
                janelaEstoque.Close();
            }
            if (janelaPagar != null) {
                janelaPagar.Close();
            }
            if (janelaReceber != null) {
                janelaReceber.Close();
            }
            if (janelaCaixa != null) {
                janelaCaixa.Close();
            }
            if (janelaFuncionarios != null) {
                janelaFuncionarios.Close();
            }
            if (janelaBarbeiros != null) {
                janelaBarbeiros.Close();
            }
            if (janelaUsuario != null) {
                janelaUsuario.Close();
            }
            if (janelaSobre != null) {
                janelaSobre.Close();
            }
            if (janelaNavegador != null) {
                janelaNavegador.Close();
            }
            
        }


        // abrir navegador
        private void btnTeste_Click(object sender, RoutedEventArgs e) {
                    janelaNavegador = new Navegador();
                    janelaNavegador.Show();
        }

        // abrir outlook
        private void btnOutlook_Click(object sender, RoutedEventArgs e) {
            try {
                System.Diagnostics.Process.Start("outlook.exe");
            }catch(Exception ex){
                MessageBox.Show("Sistema não encontrou o outlook!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(ex);
                Log.logMessage(ex.Message);
                return;
            }
        }
    }
}

