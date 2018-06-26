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
using BarberSystem.Dados;
using System.Data.Entity.Migrations;
using BarberSystem.Controle;
using BarberSystem.Utils;


namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para ContasReceber.xaml
    /// </summary>
    public partial class ContasReceber {

        BancoDados conexao = new BancoDados();
        CONTAS_RECEBER cr = new CONTAS_RECEBER();
        private List<CONTAS_RECEBER> listaReceber = new List<CONTAS_RECEBER>();
      
        public ContasReceber() {
            InitializeComponent();
            dgReceber.RowBackground = null;
            carregaGrid();
            carregaPesquisa();
        }

        // metodo para limpar os campos
        public void limpaCampos(){
            txtCodigo.Clear();
            txtDescricao.Clear();
            txtUnitario.Clear();
            cbPesquisar.Text = string.Empty;
            dpPagto.Text = "";
            dpVencto.Text = "";
            lblTotal.Content = "0";
            btnGravar.IsEnabled = true;
        }

        // verificar campos vazios
        public void verificaVazios() {
            if (txtDescricao.Text == "") {
                MessageBox.Show("O campo descrição não pode ser vazio!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                return;
            }
            else {
                cr.descricao = txtDescricao.Text;
            }
            if (dpPagto.SelectedDate.ToString() == "") {
                cr.data_pagto = null;
            }
            else {
                cr.data_pagto = DateTime.Parse(dpPagto.SelectedDate.ToString());
            }
            if (dpVencto.SelectedDate.ToString() == "") {
                cr.data_vencto = null;
            }
            else {
                cr.data_vencto = DateTime.Parse(dpVencto.SelectedDate.ToString());
            }
            if(txtUnitario.Text == ""){
                cr.vl_unitario = null;
            }else{
                cr.vl_unitario = double.Parse(txtUnitario.Text);
            }
        }

        // metodo para carregar o dataGrid
        public void carregaGrid(){
            listaReceber = conexao.CONTAS_RECEBER.ToList();
            dgReceber.ItemsSource = null;
            dgReceber.ItemsSource = listaReceber.OrderBy(user => user.codigo);
        }

        // calcular valor total e mostrar na Label
        public void calculaValorTotal() {
            cr.vl_total = 0.0;
            foreach (CONTAS_RECEBER item in listaReceber) {
                cr.vl_total += item.vl_unitario;
            }
            lblTotal.Content = cr.vl_total.ToString();
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtDescricao.Focus();
            limpaCampos();
        }

        // botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    verificaVazios();
                    if (txtDescricao.Text == "") {
                        return;
                    }
                    cr.vl_total = cr.vl_unitario;
                    double? temp = 0.0;
                    foreach (CONTAS_RECEBER item in listaReceber) {
                        item.vl_total = temp;
                        item.vl_total += item.vl_unitario;
                        temp = item.vl_total;
                    }
                    MessageBox.Show("Dados alterados com sucesso!", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    carregaGrid();
                    carregaPesquisa();
                }
                else {
                    MessageBox.Show("Insira um código ou pesquise para alterar", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    return;
                }
                btnGravar.IsEnabled = true;
            }catch(Exception){
                MessageBox.Show("Erro ao tentar alterar!", "erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // botao pesquisar
        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            try {
                if (cbPesquisar.Text != null) {
                    int codigo = int.Parse(cbPesquisar.Text.Substring(0, 4).Trim());
                    cr = conexao.CONTAS_RECEBER.Find(codigo);
                    txtCodigo.Text = cr.codigo.ToString();
                    txtDescricao.Text = cr.descricao;
                    dpPagto.Text = cr.data_pagto.ToString();
                    dpVencto.Text = cr.data_vencto.ToString();
                    txtUnitario.Text = cr.vl_unitario.ToString();
                    lblTotal.Content = cr.vl_total.ToString();
                }
                else {
                    MessageBox.Show("Registro não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }
            catch (Exception a) {
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.StackTrace, "Erro", MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                limpaCampos();
            }
        }

        // botao excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            try {
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro ? ", "Excluir",
                                                                MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    cr = conexao.CONTAS_RECEBER.Remove(cr);
                    cr.descricao = null;
                    cr.data_pagto = null;
                    cr.data_vencto = null;
                    cr.vl_unitario = null;
                    cr.vl_total = null;
                    conexao.SaveChanges();
                    int? codigo = conexao.CONTAS_RECEBER.Max(a => (int?)a.codigo);
                    Util.redefinirPK_AutoIncremento("CONTAS_RECEBER", codigo);
                    MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    carregaGrid();
                    limpaCampos();
                    carregaPesquisa();
                }
                else {
                    limpaCampos();
                    return;
                }
                btnGravar.IsEnabled = true;
            }catch(Exception ex){
                MessageBox.Show("Erro imprevisto ou campos vazios", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                calculaValorTotal();
                verificaVazios();
                if (txtDescricao.Text == "") {
                    return;
                }
                cr.vl_total += cr.vl_unitario;

                conexao.CONTAS_RECEBER.Add(cr);
                conexao.SaveChanges();

                txtCodigo.Text = cr.codigo.ToString();

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                carregaGrid();
                limpaCampos();
                carregaPesquisa();
            }catch(Exception a){
                MessageBox.Show("Erro ao gravar!" + "\n" + a.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }

        // botao voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        // botao calcular valor total
        private void btnCalcularValorTotal_Click(object sender, RoutedEventArgs e) {
            calculaValorTotal();
        }

        // mostrar a calculadora do windows
        private void btnCalcWindows_Click(object sender, RoutedEventArgs e) {
            try {
                System.Diagnostics.Process.Start("calc.exe");
            }
            catch (Exception) {
                MessageBox.Show("Sistema não encontrou a calculadora!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // botao excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Utils.Util.exportarExcel(dgReceber);
        }

        // carregar comboBox pesquisa
        private void carregaPesquisa() {
            DateTime mes = DateTime.Now;
            var sql = from cr in conexao.CONTAS_RECEBER
                      where cr.codigo > 0 && cr.data_vencto.Value.Month == mes.Month
                      select cr.codigo + "    - " + cr.descricao;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }

        // pesquisar por data
        private void dpCurrent_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e) {
            atualizaPesquisa();
            calcularPorData();
        }
        private void atualizaPesquisa() {
            var sql = from cr in conexao.CONTAS_RECEBER
                      where cr.data_vencto.Value.Month == dpCurrent.SelectedDate.Value.Month
                      select cr.codigo + "    - " + cr.descricao;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }

        // calcular valor total por data selecionada
        private void calcularPorData() {
            listaReceber = conexao.CONTAS_RECEBER.Where(x => x.data_vencto.Value.Month == dpCurrent.SelectedDate.Value.Month).ToList();
            cr.vl_total = 0.0;
            foreach (CONTAS_RECEBER item in listaReceber) {
                cr.vl_total += item.vl_unitario;
            }
            lblTotal.Content = cr.vl_total.ToString();
        }
    }
}
