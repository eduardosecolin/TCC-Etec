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
using System.Globalization;
using BarberSystem.Dados;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Agenda.xaml
    /// </summary>
    public partial class Agenda : Window {

        AGENDA agendamento = new AGENDA();
        BancoDados conexao = new BancoDados();
        public List<AGENDA> listaAgenda=new List<AGENDA>();
        private Menu janela;

        //Construtor
        public Agenda(Menu window) {
            janela = window;
            InitializeComponent();
            dgAgendamento.RowBackground = null;
            carrgearGrid();
        }

        //Botao de voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            janela.dgAgenda.ItemsSource = listaAgenda;
            this.Close();
        }

        //Botao de Novo
        private void btnCadastrar_Click(object sender, RoutedEventArgs e) {
            txtCliente.Focus();
            limpaCampos();
        }

        //Metodo para limpar os campos(textBox)
        public void limpaCampos(){
            txtCodigo.Clear();
            txtCodCliente.Clear();
            txtCliente.Clear();
            txtDescricao.Clear();
            txtHinicio.Clear();
            txtHfim.Clear();
            txtCodBarbeiro.Clear();
            txtPesquisar.Clear();
        }

        //Botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }

        private void btnGravar_Click(object sender, RoutedEventArgs e) {
                //agendamento.codcliente = int.Parse(txtCodCliente.Text);
                agendamento.cliente = txtCliente.Text;
                agendamento.descricao = txtDescricao.Text;
                agendamento.hora_inicio = DateTime.Parse(txtHinicio.Text);
                agendamento.hora_fim = DateTime.Parse(txtHfim.Text);
                agendamento.data = DateTime.Parse(dpData.SelectedDate.ToString());
                //agendamento.codbarbeiro = int.Parse(txtCodBarbeiro.Text);
                agendamento.nome_barbeiro = cbBarbeiro.Text;

                conexao.AGENDA.Add(agendamento);
                conexao.SaveChanges();

                txtCodigo.Text = agendamento.codigo.ToString();
                carrgearGrid();
          
                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
            limpaCampos();
        }

        public void carrgearGrid(){
                listaAgenda = conexao.AGENDA.ToList();
                dgAgendamento.ItemsSource = null;
                dgAgendamento.ItemsSource = listaAgenda.OrderBy(user => user.hora_inicio);
        }

        // pesquisar
        private void BtnPesquisar_Click(object sender, RoutedEventArgs e) {
            agendamento.codigo = int.Parse(txtPesquisar.Text);
          if(txtPesquisar.Text != ""){
                agendamento = conexao.AGENDA.Find(int.Parse(txtPesquisar.Text));
                txtCliente.Text = agendamento.cliente;
                txtDescricao.Text = agendamento.descricao;
                txtHinicio.Text = agendamento.hora_inicio.ToString();
                txtHfim.Text = agendamento.hora_fim.ToString();
                dpData.Text = agendamento.data.ToString();
                cbBarbeiro.Text = agendamento.nome_barbeiro;
          }else{
                MessageBox.Show("Agendamento não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
          }
        }

        // excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
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
                MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                carrgearGrid();
            }else{
                limpaCampos();
                return;
            }
        }
    }
}
