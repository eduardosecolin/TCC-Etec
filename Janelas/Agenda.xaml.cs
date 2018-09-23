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
using System.Data.Entity.Core.EntityClient;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Agenda.xaml
    /// </summary>
    public partial class Agenda {

        #region Atributos

        AGENDA agendamento = new AGENDA();
        BancoDados conexao = new BancoDados();
        public List<AGENDA> listaAgenda = new List<AGENDA>();
        private Menu janela;

        #endregion

        #region Construtor

        //Construtor
        public Agenda(Menu window) {
            janela = window;
            InitializeComponent();
            dgAgendamento.RowBackground = null;
            carrgearGrid();
            carregarComboBox();
        }

        #endregion

        #region Metodos

        // metodo para campos vazios
        public void verificaCampos() {
            if (txtCodCliente.Text.Equals(string.Empty)) {
                agendamento.codcliente = null;
            }
            else {
                agendamento.codcliente = int.Parse(txtCodCliente.Text);
            }
        }

        // mostrar agendamento no menu
        private void mostrarAgendamentoMenu() {
            var sql = from a in conexao.AGENDA
                      where a.data == DateTime.Today
                      select new { a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.nome_barbeiro };
            janela.dgAgenda.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
        }

        //Metodo para limpar os campos(textBox)
        public void limpaCampos() {
            txtCodigo.Clear();
            cbClientes.Text = string.Empty;
            txtDescricao.Clear();
            MtxtHinicio.Clear();
            MtxtHfim.Clear();
            txtCodBarbeiro.Clear();
            txtPesquisar.Text = string.Empty;
            MtxtHinicio.Clear();
            dpData.Text = "";
            cbBarbeiro.Text = "";
            txtCodCliente.Clear();
            btnGravar.IsEnabled = true;
        }

        // carregar a grid
        public void carrgearGrid() {
            listaAgenda = conexao.AGENDA.ToList();
            dgAgendamento.ItemsSource = null;
            dgAgendamento.ItemsSource = listaAgenda.OrderBy(user => user.hora_inicio);
        }

        // carregar combo box clientes
        private void CarregaCBClientes() {
            List<string> nomes = conexao.CLIENTES.Select(x => x.nome).ToList();
            nomes = nomes.OrderBy(x => x).ToList();
            cbClientes.ItemsSource = null;
            cbClientes.ItemsSource = nomes;
        }

        public void carregarComboBox() {
            List<BARBEIROS> listaBarbeiros = conexao.BARBEIROS.ToList();
            cbBarbeiro.ItemsSource = null;
            cbBarbeiro.ItemsSource = listaBarbeiros.OrderBy(user => user.nome);
            cbBarbeiro.DisplayMemberPath = "nome";
        }

        #endregion

        #region Eventos

        //Botao de voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            mostrarAgendamentoMenu();
            this.Close();
        }

        //Botao de Novo
        private void btnCadastrar_Click(object sender, RoutedEventArgs e) {
            cbClientes.Focus();
            limpaCampos();
            CarregaCBClientes();
        }

        //Botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
            carrgearGrid();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                verificaCampos();
                agendamento.cliente = Util.VerificarCamposVazios(cbClientes.Text);
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

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
            }
            catch (Exception a) {
                MessageBox.Show("Erro ao gravar!" + "\n" + a.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // pesquisar
        private void BtnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            try {
                if (txtPesquisar.Text != string.Empty) {
                    var sql = conexao.AGENDA.Where(x => x.cliente.Contains(txtPesquisar.Text));

                    dgAgendamento.ItemsSource = null;
                    dgAgendamento.ItemsSource = sql.ToList();

                }
                else {
                    MessageBox.Show("Agendamento não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }
            catch (Exception a) {
                MessageBox.Show("Campo vazio!" + "\n" + a.Message, "Erro", MessageBoxButton.OK,
                                 MessageBoxImage.Exclamation);
                limpaCampos();
            }
        }

        // excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            try {
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    conexao.AGENDA.Attach(agendamento);
                    conexao.AGENDA.Remove(agendamento);
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
                }
                else {
                    limpaCampos();
                    return;
                }
                btnGravar.IsEnabled = true;
            }
            catch (Exception ex) {
                MessageBox.Show("Erro imprevisto ou campos vazios" + "\n" + ex.Message, "Erro",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgAgendamento);
        }

        //botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    verificaCampos();
                    agendamento.codigo = int.Parse(txtCodigo.Text);
                    agendamento.cliente = cbClientes.Text;
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
                }
                else {
                    MessageBox.Show("Pesquise para alterar", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    return;
                }
            }
            catch (Exception a) {
                MessageBox.Show("Alguns campos não podem ficar vazios" + "\n" + a.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                limpaCampos();
            }
            btnGravar.IsEnabled = true;
        }

        // mostrar registros da grid pela data selecionada
        private void dpCurrent_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            var sql = from a in conexao.AGENDA
                      where a.data == dpCurrent.SelectedDate
                      select new { a.codigo, a.codcliente, a.cliente, a.descricao, a.hora_inicio, a.hora_fim, a.data, a.codbarbeiro, a.nome_barbeiro };
            dgAgendamento.ItemsSource = null;
            dgAgendamento.ItemsSource = sql.ToList().OrderBy(user => user.hora_inicio);
        }

        // campo codigo cliente receber valor ao ganhar foco
        private void txtCodCliente_GotFocus(object sender, RoutedEventArgs e) {
            try {
                if (cbClientes.SelectedItem != null) {
                    var sql = conexao.CLIENTES.Where(x => x.nome == cbClientes.Text);
                    CLIENTES cliente = new CLIENTES();
                    cliente = sql.FirstOrDefault();
                    txtCodCliente.Text = cliente.codigo.ToString();
                }
            }
            catch (Exception) {
                MessageBox.Show("Código do cliente invalido!", "Informação",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                cbClientes.Text = string.Empty;
                cbClientes.Focus();
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

        private void dgAgendamento_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                if (dgAgendamento.SelectedItems.Count > 0) {
                    agendamento = dgAgendamento.SelectedItem as AGENDA;
                    if (agendamento != null) {
                        cbClientes.ItemsSource = null;
                        txtCodCliente.Text = agendamento.codcliente.ToString();
                        txtCodigo.Text = agendamento.codigo.ToString();
                        cbClientes.Items.Add(agendamento.cliente);
                        cbClientes.Text = agendamento.cliente;
                        txtDescricao.Text = agendamento.descricao;
                        MtxtHinicio.Text = DateTime.Parse(agendamento.hora_inicio.ToString()).ToShortTimeString();
                        MtxtHfim.Text = DateTime.Parse(agendamento.hora_fim.ToString()).ToShortTimeString();
                        dpData.Text = agendamento.data.ToString();
                        txtCodBarbeiro.Text = agendamento.codbarbeiro.ToString();
                        cbBarbeiro.Text = agendamento.nome_barbeiro;
                    }
                }

            }
            catch (Exception ex) {

                MessageBox.Show("Selecione um registro!" + "\n" + ex.Message, "Aviso",
                                MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        #endregion
    }
}

