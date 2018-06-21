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
using BarberSystem.Utils;
using BarberSystem.Dados;
using System.Data.Entity.Migrations;
using BarberSystem.Controle;

namespace BarberSystem.Janelas
{
    /// <summary>
    /// Lógica interna para Produtos.xaml
    /// </summary>
    public partial class Produtos {

        PRODUTOS produto = new PRODUTOS();
        BancoDados conexao = new BancoDados();
        private List<PRODUTOS> listaProdutos = new List<PRODUTOS>(); 

        public Produtos(){
            InitializeComponent();
            dgProdutos.RowBackground = null;
            carregaGrid();
            carregaComboBoxFornecedor();
        }

        // metodo para limpar os campos
        public void limpaCampos(){
            txtCodigo.Clear();
            txtPesquisar.Clear();
            txtDescricao.Clear();
            txtFornecedor.Clear();
            txtUnitario.Clear();
            cbCodFornecedor.Text = "";
        }

        // metodo para carregar o dataGrid
        public void carregaGrid(){
            listaProdutos = conexao.PRODUTOS.ToList();
            dgProdutos.ItemsSource = null;
            dgProdutos.ItemsSource = listaProdutos.OrderBy(user => user.codigo);
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
                    produto.descricao = txtDescricao.Text;
                    produto.vl_unitario = double.Parse(txtUnitario.Text);
                    produto.codfornecedor = int.Parse(cbCodFornecedor.Text);
                    produto.nome_fornecedor = txtFornecedor.Text;
                    conexao.PRODUTOS.AddOrUpdate(produto);
                    conexao.SaveChanges();
                    MessageBox.Show("Dados alterados com sucesso!", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    carregaGrid();
                }
                else {
                    MessageBox.Show("Insira um código ou pesquise para alterar", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    return;
                }
            }
            catch (Exception a) {
                MessageBox.Show("Alguns campos não podem ficar vazios" + "\n" + a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                limpaCampos();
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
            btnGravar.IsEnabled = true;
        }

        // botao pesquisar
        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            try {
                if (txtPesquisar.Text != "") {
                    produto = conexao.PRODUTOS.Find(int.Parse(txtPesquisar.Text));
                    txtDescricao.Text = produto.descricao;
                    txtCodigo.Text = produto.codigo.ToString();
                    txtUnitario.Text = produto.vl_unitario.ToString();
                    txtFornecedor.Text = produto.nome_fornecedor;
                    cbCodFornecedor.Text = produto.codfornecedor.ToString();
                }
                else {
                    MessageBox.Show("Produto não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    produto = conexao.PRODUTOS.Remove(produto);
                    limpaCampos();
                    produto.descricao = null;
                    produto.vl_unitario = null;
                    produto.codfornecedor = null;
                    produto.nome_fornecedor = null;
                    conexao.SaveChanges();
                    MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    carregaGrid();
                    limpaCampos();
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

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                produto.descricao = Util.VerificarCamposVazios(txtDescricao.Text);
                produto.vl_unitario = double.Parse(txtUnitario.Text);
                produto.codfornecedor = int.Parse(Util.VerificarCamposVazios(cbCodFornecedor.Text));
                produto.nome_fornecedor = Util.VerificarCamposVazios(txtFornecedor.Text);

                if (Util.vazio == true) {
                    return;
                }

                conexao.PRODUTOS.Add(produto);
                conexao.SaveChanges();

                txtCodigo.Text = produto.codigo.ToString();
                carregaGrid();

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

        // carregar combobox com codigos do fornecedor
        public void carregaComboBoxFornecedor(){
            List<FORNECEDORES> listaFornecedores = conexao.FORNECEDORES.ToList();
            cbCodFornecedor.ItemsSource = null;
            cbCodFornecedor.ItemsSource = listaFornecedores.OrderBy(user => user.codigo);
            cbCodFornecedor.DisplayMemberPath = "codigo";
        }
       
        // metodo para preencher campo fornecedor automatico
        private void txtFornecedor_GotFocus(object sender, RoutedEventArgs e) {
            FORNECEDORES fornecedor = new FORNECEDORES();
            try {
                if (cbCodFornecedor.SelectedItem != null) {
                    fornecedor = conexao.FORNECEDORES.Find(int.Parse(cbCodFornecedor.Text));
                    txtFornecedor.Text = fornecedor.nome;
                }
            }
            catch (Exception a) {
                MessageBox.Show("Código do fornecedor invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodFornecedor.Text = "";
                txtFornecedor.Clear();
                cbCodFornecedor.Focus();
                Log.logException(a);
                Log.logMessage(a.Message);
            }
        }

        // botao excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgProdutos);
        }

    }
}
