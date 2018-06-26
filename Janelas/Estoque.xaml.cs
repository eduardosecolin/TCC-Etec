﻿using System;
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
using BarberSystem.Utils;
using BarberSystem.Dados;
using System.Data.Entity.Migrations;
using BarberSystem.Controle;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Estoque.xaml
    /// </summary>
    ///

    public partial class Estoque {


        ESTOQUE estoque = new ESTOQUE();
        BancoDados conexao = new BancoDados();
        private List<ESTOQUE> listaEstoque = new List<ESTOQUE>();

        public Estoque() {
            InitializeComponent();
            dgEstoque.RowBackground = null;
            carregaGrid();
            carregaComboBox();
            carregaPesquisa();
        }

        // limpar os campos
        private void limpaCampos(){
            cbPesquisar.Text = string.Empty;
            txtCodigo.Clear();
            cbCodProduto.Text = "";
            txtProduto.Clear();
            txtUnitario.Clear();
            txtTotal.Clear();
            txtQuantidade.Clear();
            txtEntrada.Clear();
            txtSaida.Clear();
            btnGravar.IsEnabled = true;
        }

        // carregar o dataGrid
        private void carregaGrid(){
            listaEstoque = conexao.ESTOQUE.ToList();
            dgEstoque.ItemsSource = null;
            dgEstoque.ItemsSource = listaEstoque.OrderBy(user => user.codigo);
        }

        // carregar comboBox com o codigo do produto
        private void carregaComboBox(){
            var sql = from p in conexao.PRODUTOS where p.codigo > 0 select p.codigo + "    - " + p.descricao;
            cbCodProduto.ItemsSource = null;
            cbCodProduto.ItemsSource = sql.ToList();
        }

        // preencher nome do produto automatico
        private void preencherCamposAuto() {
            try {
                if (cbCodProduto.Text != null) {
                    int codigo = int.Parse(cbCodProduto.Text.Substring(0, 4).Trim());
                    PRODUTOS produto = new PRODUTOS();
                    produto = conexao.PRODUTOS.Find(codigo);
                    txtProduto.Text = produto.descricao;
                    txtUnitario.Text = produto.vl_unitario.ToString();
                }
            }
            catch (Exception) {
                MessageBox.Show("Código do produto invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodProduto.Text = "";
                txtProduto.Clear();
                cbCodProduto.Focus();
            }
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Insira o código do produto.", "Informação", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            cbCodProduto.Focus();
            limpaCampos();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                estoque.codproduto = int.Parse(cbCodProduto.Text.Substring(0,4).Trim());
                estoque.produto = Util.VerificarCamposVazios(txtProduto.Text);
                estoque.vl_produto = double.Parse(txtUnitario.Text);
                estoque.vl_total = double.Parse(txtTotal.Text);
                estoque.quantidade = int.Parse(txtQuantidade.Text);

                if (Util.vazio == true) {
                    return;
                }

                conexao.ESTOQUE.Add(estoque);
                conexao.SaveChanges();

                txtCodigo.Text = estoque.codigo.ToString();
                carregaGrid();
                carregaPesquisa();

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
            }catch(Exception a){
                MessageBox.Show("Erro ao gravar!" + "\n" + a.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // botao entrada
        private void btnEntrada_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtProduto.Text == "") {
                    MessageBox.Show("O campo produto não pode estar vazio", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    return;
                }
                estoque.entradaEstoque(int.Parse(txtEntrada.Text));
                txtQuantidade.Text = estoque.quantidade.ToString();
                estoque.vl_produto = double.Parse(txtUnitario.Text);
                txtTotal.Text = estoque.calculaTotal().ToString();
                txtEntrada.Clear();
            }catch(Exception){
                MessageBox.Show("Erro imprevisto ou campo vazio", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // botao saida
        private void btnSaida_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtProduto.Text == "") {
                    MessageBox.Show("O campo produto não pode estar vazio", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    return;
                }
                estoque.saidaEstoque(int.Parse(txtSaida.Text));
                txtQuantidade.Text = estoque.quantidade.ToString();
                estoque.vl_produto = double.Parse(txtUnitario.Text);
                txtTotal.Text = estoque.calculaTotal().ToString();
                txtSaida.Clear();
            }catch(Exception){
                MessageBox.Show("Erro imprevisto ou campo vazio", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    estoque.produto = txtProduto.Text;
                    estoque.vl_produto = double.Parse(txtUnitario.Text);
                    estoque.vl_total = double.Parse(txtTotal.Text);
                    estoque.quantidade = int.Parse(txtQuantidade.Text);
                    conexao.ESTOQUE.AddOrUpdate(estoque);
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
            }
            catch (Exception a) {
                MessageBox.Show("Alguns campos não podem ficar vazios" + "\n" + a.StackTrace, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                limpaCampos();
            }
            btnGravar.IsEnabled = true;
        }

        // botao pesquisar
        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            try {
                if (cbPesquisar.Text != null) {
                    int codigo = int.Parse(cbPesquisar.Text.Substring(0, 4).Trim());
                    estoque = conexao.ESTOQUE.Find(codigo);
                    cbCodProduto.Text = estoque.codproduto.ToString();
                    txtProduto.Text = estoque.produto;
                    txtCodigo.Text = estoque.codigo.ToString();
                    txtUnitario.Text = estoque.vl_produto.ToString();
                    txtTotal.Text = estoque.vl_total.ToString();
                    txtQuantidade.Text = estoque.quantidade.ToString();
                }
                else {
                    MessageBox.Show("Produto no estoque não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    estoque = conexao.ESTOQUE.Remove(estoque);
                    limpaCampos();
                    estoque.produto = null;
                    estoque.vl_produto = null;
                    estoque.vl_total = null;
                    estoque.quantidade = null;
                    conexao.SaveChanges();
                    int? codigo = conexao.ESTOQUE.Max(a => (int?)a.codigo);
                    Util.redefinirPK_AutoIncremento("ESTOQUE", codigo);
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
            }catch(Exception){
                MessageBox.Show("Erro imprevisto ou campos vazios", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                carregaGrid();
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

        // botao excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgEstoque);
        }

        // mostrar informações do produto automatico ao fechar a comboBox
        private void cbCodProduto_DropDownClosed(object sender, EventArgs e) {
            if (cbCodProduto.SelectedItem == null) {
                return;
            }
            preencherCamposAuto();
        }

        // carregar comboBox pesquisa
        private void carregaPesquisa() {
            var sql = from e in conexao.ESTOQUE
                      where e.codigo > 0 
                      select e.codigo + "    - " + e.produto;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }
    }
}
