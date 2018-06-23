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
using BarberSystem.Janelas;
using BarberSystem.Dados;
using System.Data.Entity.Migrations;
using BarberSystem.Controle;
using BarberSystem.Utils;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para ContasPagar.xaml
    /// </summary>
    public partial class ContasPagar {

        CONTAS_PAGAR cp = new CONTAS_PAGAR();
        BancoDados conexao = new BancoDados();
        private List<CONTAS_PAGAR> listaPagar = new List<CONTAS_PAGAR>();

        public ContasPagar() {
            InitializeComponent();
            dgPagar.RowBackground = null;
            carregaGrid();
            carregaPesquisa();
        }

        // limpar os campos(textBox)
        public void limpaCampos(){
            txtCodigo.Clear();
            txtDescricao.Clear();
            cbPesquisar.Text = string.Empty;
            txtUnitario.Clear();
            lblTotal.Content = "0";
            dpPagto.Text = "";
            dpVencto.Text = "";
            btnGravar.IsEnabled = true;
        }

        //carregar o dataGrid
        public void carregaGrid(){
            listaPagar = conexao.CONTAS_PAGAR.ToList();
            dgPagar.ItemsSource = null;
            dgPagar.ItemsSource = listaPagar.OrderBy(user => user.codigo);
        }

        // verificar campos vazios
        public void verificaVazios() {
            if (txtDescricao.Text == "") {
                MessageBox.Show("O campo descrição não pode ser vazio!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                return;
            }
            else {
                cp.descricao = txtDescricao.Text;
            }
            if (dpPagto.SelectedDate.ToString() == "") {
                cp.data_pagto = null;
            }
            else {
                cp.data_pagto = DateTime.Parse(dpPagto.SelectedDate.ToString());
            }
            if (dpVencto.SelectedDate.ToString() == "") {
                cp.data_vencto = null;
            }
            else {
                cp.data_vencto = DateTime.Parse(dpVencto.SelectedDate.ToString());
            }
            if (txtUnitario.Text == ""){
                cp.vl_unitario = null;
            }else{
                cp.vl_unitario = double.Parse(txtUnitario.Text);
            }
        }


        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtDescricao.Focus();
            limpaCampos();
        }

        // calcular valor total e mostrar na Label
        public void calculaValorTotal(){
            cp.vl_total = 0.0;
            foreach (CONTAS_PAGAR item in listaPagar) {
                cp.vl_total += item.vl_unitario;
            }
            lblTotal.Content = cp.vl_total.ToString();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                calculaValorTotal();
                verificaVazios();
                if (txtDescricao.Text == "") {
                    return;
                }
                cp.vl_total += cp.vl_unitario;

                conexao.CONTAS_PAGAR.Add(cp);
                conexao.SaveChanges();


                txtCodigo.Text = cp.codigo.ToString();
                carregaGrid();
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

        // botao limpar
        private void btnLimpar_Click(object sender, RoutedEventArgs e) {
            limpaCampos();
        }

        // botao voltar
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        // botao pesquisar
        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            try {
             if(cbPesquisar.Text != null){
                    int codigo = int.Parse(cbPesquisar.Text.Substring(0, 4).Trim());
                    cp = conexao.CONTAS_PAGAR.Find(codigo);
                    txtCodigo.Text = cp.codigo.ToString();
                    txtDescricao.Text = cp.descricao;
                    dpPagto.Text = cp.data_pagto.ToString();
                    dpVencto.Text = cp.data_vencto.ToString();
                    txtUnitario.Text = cp.vl_unitario.ToString();
                    lblTotal.Content = cp.vl_total.ToString();
             }else {
                    MessageBox.Show("Registro não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }
            catch (Exception a) {
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.Message, "Erro", MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                limpaCampos();
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
        }

        // botao excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            try {
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir",
                                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    cp = conexao.CONTAS_PAGAR.Remove(cp);
                    cp.descricao = null;
                    cp.data_pagto = null;
                    cp.data_vencto = null;
                    cp.vl_unitario = null;
                    cp.vl_total = null;
                    limpaCampos();
                    conexao.SaveChanges();
                    int? codigo = conexao.AGENDA.Max(a => (int?)a.codigo);
                    Util.redefinirPK_AutoIncremento("CONTAS_PAGAR", codigo);
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
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        // botao calcular valor total
        private void btnCalcularValorTotal_Click(object sender, RoutedEventArgs e) {
            calculaValorTotal();
        }

        // exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Utils.Util.exportarExcel(dgPagar);
        }

        // mostrar a calculadora do windows
        private void btnCalcWindows_Click(object sender, RoutedEventArgs e) {
            try {
                System.Diagnostics.Process.Start("calc.exe");
            }
            catch (Exception ex) {
                MessageBox.Show("Sistema não encontrou a calculadora!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(ex);
                Log.logMessage(ex.Message);
            }
        }

        // botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            if (txtCodigo.Text != "") {
                verificaVazios();
                if (txtDescricao.Text == "") {
                    return;
                }
                cp.vl_total = cp.vl_unitario;
                double? temp = 0.0;
                foreach (CONTAS_PAGAR item in listaPagar) {                    
                    item.vl_total = temp;
                    item.vl_total += item.vl_unitario;
                    temp = item.vl_total;
                }
                conexao.CONTAS_PAGAR.AddOrUpdate(cp);
                conexao.SaveChanges();
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
        }

        // carregar comboBox pesquisa
        private void carregaPesquisa() {
            DateTime mes = DateTime.Now;
           var sql = from cp in conexao.CONTAS_PAGAR
                     where cp.codigo > 0 && cp.data_vencto.Value.Month == mes.Month
                     select cp.codigo + "    - " + cp.descricao;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }

        // pesquisar por data
        private void dpCurrent_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            atualizaPesquisa();
        }
        private void atualizaPesquisa() {
            var sql = from cp in conexao.CONTAS_PAGAR
                      where cp.data_vencto.Value.Month == dpCurrent.SelectedDate.Value.Month
                      select cp.codigo + "    - " + cp.descricao;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }
    }
}
