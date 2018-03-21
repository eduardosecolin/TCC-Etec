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

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Agenda.xaml
    /// </summary>
    public partial class Agenda : Window {

        AGENDA agendamento;
        public List<AGENDA> listaAgenda = new List<AGENDA>();
        private Menu janela;

        //Construtor
        public Agenda(Menu window) {
            InitializeComponent();
            dgAgendamento.RowBackground = null;
            janela = window;
        }

        //Botao de voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            janela.atualizaForm(listaAgenda);
            this.Close();
        }

        //Botao de cadastrar
        private void btnCadastrar_Click(object sender, RoutedEventArgs e) {
            int codcliente = int.Parse(txtCodCliente.Text);
            string cliente = txtCliente.Text;
            string descricao = txtDescricao.Text;
            DateTime horaInicio = DateTime.Parse(txtHinicio.Text);
            DateTime horaFim = DateTime.Parse(txtHfim.Text);
            DateTime data = DateTime.Parse(dpData.SelectedDate.ToString());
            int codBarbeiro = int.Parse(txtCodBarbeiro.Text);
            string barbeiro = cbBarbeiro.Text;

            agendamento = new AGENDA(codcliente, cliente, descricao, horaInicio, horaFim, data, codBarbeiro, barbeiro);

            txtCodigo.Text = agendamento.codigo.ToString();
            listaAgenda.Add(agendamento);
            dgAgendamento.ItemsSource = listaAgenda.OrderBy(user => user.codigo);
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
        }

        //Botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }
    }
}
