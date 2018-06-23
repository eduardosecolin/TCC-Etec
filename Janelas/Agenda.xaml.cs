using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BarberSystem.Controle;
using BarberSystem.Utils;
using BarberSystem.Dados;
using System.Data.Entity.Migrations;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Agenda.xaml
    /// </summary>
    public partial class Agenda {

        AGENDA agendamento = new AGENDA();
        BancoDados conexao = new BancoDados();
        public List<AGENDA> listaAgenda = new List<AGENDA>();
        private Menu janela;

        //Construtor
        public Agenda(Menu window) {
            janela = window;
            InitializeComponent();
            dgAgendamento.RowBackground = null;
            carrgearGrid();
            carregarComboBox();
            carregaComboCodClient();
            carregaPesquisa();
        }

        // metodo para campos vazios
        public void verificaCampos(){
          if(txtCodCliente.Text.Equals(string.Empty)){
                agendamento.codcliente = null;
          }else{
                agendamento.codcliente = int.Parse(txtCodCliente.Text);
          }
        }

        // mostrar agendamento no menu
        private void mostrarAgendamentoMenu(){
            var sql = from a in conexao.AGENDA
                      where a.data == DateTime.Today
                      select new { a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.nome_barbeiro };
            janela.dgAgenda.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
        }
        //Botao de voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            mostrarAgendamentoMenu();
            this.Close();
        }

        //Botao de Novo
        private void btnCadastrar_Click(object sender, RoutedEventArgs e) {
            cbCodCliente.Focus();
            limpaCampos();
        }

        //Metodo para limpar os campos(textBox)
        public void limpaCampos(){
            txtCodigo.Clear();
            cbCodCliente.Text = string.Empty;
            txtCliente.Clear();
            txtDescricao.Clear();
            MtxtHinicio.Clear();
            MtxtHfim.Clear();
            txtCodBarbeiro.Clear();
            cbPesquisar.Text = string.Empty;
            MtxtHinicio.Clear();
            dpData.Text = "";
            cbBarbeiro.Text = "";
            txtCodCliente.Clear();
            btnGravar.IsEnabled = true;
        }

        //Botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                verificaCampos();
                agendamento.cliente = Util.VerificarCamposVazios(txtCliente.Text);
                agendamento.descricao = Util.VerificarCamposVazios(txtDescricao.Text);
                agendamento.hora_inicio = DateTime.Parse(MtxtHinicio.Text);
                agendamento.hora_fim = DateTime.Parse(MtxtHfim.Text);
                agendamento.data = DateTime.Parse(dpData.SelectedDate.ToString());
                agendamento.codbarbeiro = int.Parse(txtCodBarbeiro.Text);
                agendamento.nome_barbeiro = Util.VerificarCamposVazios(cbBarbeiro.Text);

                if (Util.vazio == true) {
                    return;
                }
            
                conexao.AGENDA.Add(agendamento);
                conexao.SaveChanges();

                txtCodigo.Text = agendamento.codigo.ToString();
                carrgearGrid();
                carregaPesquisa();

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
            }catch(Exception a){
                MessageBox.Show(a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
        }

        // carregar a grid
        public void carrgearGrid(){
                listaAgenda = conexao.AGENDA.ToList();
                dgAgendamento.ItemsSource = null;
                dgAgendamento.ItemsSource = listaAgenda.OrderBy(user => user.hora_inicio);
        }

        // pesquisar
        private void BtnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            cbCodCliente.IsEnabled = false;
            try {
                if (cbPesquisar.Text != null) {
                    int codigo = int.Parse(cbPesquisar.Text.Substring(0, 4).Trim());
                    agendamento = conexao.AGENDA.Find(codigo);
                    txtCodCliente.Text = agendamento.codcliente.ToString();
                    txtCodigo.Text = agendamento.codigo.ToString();
                    txtCliente.Text = agendamento.cliente;
                    txtDescricao.Text = agendamento.descricao;
                    MtxtHinicio.Text = DateTime.Parse(agendamento.hora_inicio.ToString()).ToShortTimeString();
                    MtxtHfim.Text = DateTime.Parse(agendamento.hora_fim.ToString()).ToShortTimeString();
                    dpData.Text = agendamento.data.ToString();
                    txtCodBarbeiro.Text = agendamento.codbarbeiro.ToString();
                    cbBarbeiro.Text = agendamento.nome_barbeiro;
                }
                else {
                    MessageBox.Show("Agendamento não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }catch(Exception a){
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.Message, "Erro", MessageBoxButton.OK, 
                                 MessageBoxImage.Exclamation);
                limpaCampos();
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
        }

        // excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            try {
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    agendamento = conexao.AGENDA.Remove(agendamento);
                    limpaCampos();
                    agendamento.cliente = null;
                    agendamento.descricao = null;
                    agendamento.hora_inicio = null;
                    agendamento.hora_fim = null;
                    agendamento.data = null;
                    agendamento.nome_barbeiro = null;
                    conexao.SaveChanges();
                    int? codigo = conexao.AGENDA.Max(a => (int?)a.codigo);
                    Util.redefinirPK_AutoIncremento("AGENDA", codigo);
                    MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    carrgearGrid();
                    limpaCampos();
                    carregaPesquisa();
                }
                else {
                    limpaCampos();
                    return;
                }
                btnGravar.IsEnabled = true;
                cbCodCliente.IsEnabled = true;
            }catch(Exception ex){
                MessageBox.Show("Erro imprevisto ou campos vazios", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        // exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgAgendamento);
        }

        public void carregarComboBox(){
           List<BARBEIROS> listaBarbeiros = conexao.BARBEIROS.ToList();
            cbBarbeiro.ItemsSource = null;
            cbBarbeiro.ItemsSource = listaBarbeiros.OrderBy(user => user.nome);
            cbBarbeiro.DisplayMemberPath = "nome";
        }

        // carregar comboBox com o codigo do cliente
        public void carregaComboCodClient(){
            var sql = from a in conexao.CLIENTES
                      where a.codigo > 0
                      select a.codigo + "    - " + a.nome;
            
            cbCodCliente.ItemsSource = null;
            cbCodCliente.ItemsSource = sql.ToList();
        }

        // carregar comboBox pesquisa
        private void carregaPesquisa(){
         var sql = from a in conexao.AGENDA
                   where a.codigo > 0 && a.data == DateTime.Today
                   select a.codigo + "    - " + a.cliente;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }

        // mostrar cliente automatico
        private void txtCliente_GotFocus(object sender, RoutedEventArgs e) {
            try {
              if(cbCodCliente.SelectedItem != null){
                    int codigo = int.Parse(cbCodCliente.Text.Substring(0, 4).Trim());
                    CLIENTES cliente = new CLIENTES();
                    cliente = conexao.CLIENTES.Find(codigo);
                    txtCliente.Text = cliente.nome;

              }
            }
            catch (Exception a) {
                MessageBox.Show("Código do cliente invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodCliente.Text = "";
                txtCliente.Clear();
                cbCodCliente.Focus();
                Log.logException(a);
                Log.logMessage(a.Message);
            }
        }

        //botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    agendamento.codcliente = int.Parse(txtCodCliente.Text);
                    agendamento.cliente = txtCliente.Text;
                    agendamento.descricao = txtDescricao.Text;
                    agendamento.hora_inicio = DateTime.Parse(MtxtHinicio.Text);
                    agendamento.hora_fim = DateTime.Parse(MtxtHfim.Text);
                    agendamento.data = DateTime.Parse(dpData.SelectedDate.ToString());
                    agendamento.codbarbeiro = int.Parse(txtCodBarbeiro.Text);
                    agendamento.nome_barbeiro = cbBarbeiro.Text;
                    conexao.AGENDA.AddOrUpdate(agendamento);
                    conexao.SaveChanges();
                    MessageBox.Show("Dados alterados com sucesso!", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    carrgearGrid();
                    carregaPesquisa();
                }
                else {
                    MessageBox.Show("Insira um código ou pesquise para alterar", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    return;
                }
            }catch(Exception a){
                MessageBox.Show("Alguns campos não podem ficar vazios" + "\n" + a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                limpaCampos();
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
            btnGravar.IsEnabled = true;
            cbCodCliente.IsEnabled = true;
        }

        // mostrar registros da grid pela data selecionada
        private void dpCurrent_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            var sql = from a in conexao.AGENDA
                      where a.data == dpCurrent.SelectedDate
                      select new {a.codigo, a.codcliente, a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.codbarbeiro, a.nome_barbeiro };
            dgAgendamento.ItemsSource = null;
            dgAgendamento.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);

            atualizaPesquisa();
        }

        // atualizar combo pesquisa
        private void atualizaPesquisa(){
            var sql = from a in conexao.AGENDA
                      where a.data == dpCurrent.SelectedDate
                      select a.codigo + "    - " + a.cliente;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }

        private void txtCodCliente_GotFocus(object sender, RoutedEventArgs e) {
            try {
                if (cbCodCliente.SelectedItem != null) {
                    int codigo = int.Parse(cbCodCliente.Text.Substring(0, 4).Trim());
                    CLIENTES cliente = new CLIENTES();
                    cliente = conexao.CLIENTES.Find(codigo);
                    txtCodCliente.Text = cliente.codigo.ToString();

                }
            }
            catch (Exception a) {
                MessageBox.Show("Código do cliente invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodCliente.Text = "";
                txtCliente.Clear();
                cbCodCliente.Focus();
                Log.logException(a);
                Log.logMessage(a.Message);
            }
        }

        private void txtCliente_LostFocus(object sender, RoutedEventArgs e) {
          if (cbCodCliente.SelectedItem != null) {
                int codigo = int.Parse(cbCodCliente.Text.Substring(0, 4).Trim());
                CLIENTES cliente = new CLIENTES();
                cliente = conexao.CLIENTES.Find(codigo);
                string aux = cliente.nome;
         
            if(txtCliente.Text != aux){
                    txtCliente.Text = string.Empty;
                    txtCliente.Focus();
                    return;
            }
          }
        }


        // mostrar codigo barbeiro automatico
        private void cbBarbeiro_DropDownClosed(object sender, EventArgs e) {
            try {
                if (cbBarbeiro.SelectedItem != null) {
                    var sql = conexao.BARBEIROS.Where(barbeiro => barbeiro.nome == cbBarbeiro.Text);
                    BARBEIROS barber = new BARBEIROS();
                    barber = sql.FirstOrDefault();
                    string resultado = barber.codigo.ToString();
                    txtCodBarbeiro.Text = resultado;
                }
            }
            catch (Exception) {
                MessageBox.Show("Código do barbeiro invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbBarbeiro.Text = "";
                txtCodBarbeiro.Clear();
                cbBarbeiro.Focus();
            }
        }

        // tela fechando
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            mostrarAgendamentoMenu();
        }
    }
}
