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
            carregaComboFornecedor();
            carregaPesquisa();
        }

        // metodo para limpar os campos
        public void limpaCampos(){
            txtCodigo.Clear();
            cbPesquisar.Text = string.Empty;
            txtDescricao.Clear();
            txtFornecedor.Clear();
            txtUnitario.Clear();
            cbCodFornecedor.Text = "";
            txtCodigoFornecedor.Clear();
            btnGravar.IsEnabled = true;
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
                    produto.descricao = Util.VerificarCamposVazios(txtDescricao.Text);
                    produto.vl_unitario = double.Parse(Util.VerificarCamposVazios(txtUnitario.Text));
                    produto.codfornecedor = int.Parse(Util.VerificarCamposVazios(txtCodigoFornecedor.Text));
                    produto.nome_fornecedor = Util.VerificarCamposVazios(txtFornecedor.Text);

                    if (Util.vazio == true) {
                        return;
                    }

                    conexao.PRODUTOS.AddOrUpdate(produto);
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
                MessageBox.Show("Alguns campos não podem ficar vazios" + "\n" + a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    produto = conexao.PRODUTOS.Find(codigo);
                    txtDescricao.Text = produto.descricao;
                    txtCodigo.Text = produto.codigo.ToString();
                    txtUnitario.Text = produto.vl_unitario.ToString();
                    txtFornecedor.Text = produto.nome_fornecedor;
                    txtCodigoFornecedor.Text = produto.codfornecedor.ToString();
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
                    int? codigo = conexao.AGENDA.Max(a => (int?)a.codigo);
                    Util.redefinirPK_AutoIncremento("PRODUTO", codigo);
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
                produto.descricao = Util.VerificarCamposVazios(txtDescricao.Text);
                produto.vl_unitario = double.Parse(txtUnitario.Text);
                produto.codfornecedor = int.Parse(Util.VerificarCamposVazios(txtCodigoFornecedor.Text));
                produto.nome_fornecedor = Util.VerificarCamposVazios(txtFornecedor.Text);

                if (Util.vazio == true) {
                    return;
                }

                conexao.PRODUTOS.Add(produto);
                conexao.SaveChanges();

                txtCodigo.Text = produto.codigo.ToString();
                carregaGrid();
                carregaPesquisa();

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
            }catch(Exception a){
                MessageBox.Show(a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
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
       
        // metodo para preencher campo fornecedor automatico
        private void txtFornecedor_GotFocus(object sender, RoutedEventArgs e) {
            FORNECEDORES fornecedor = new FORNECEDORES();
            try {
                if (cbCodFornecedor.SelectedItem != null) {
                    int codigo = int.Parse(cbCodFornecedor.Text.Substring(0, 4).Trim());
                    fornecedor = conexao.FORNECEDORES.Find(codigo);
                    txtFornecedor.Text = fornecedor.nome;
                }
            }
            catch (Exception a) {
                MessageBox.Show("Código do fornecedor invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodFornecedor.Text = "";
                txtFornecedor.Clear();
                cbCodFornecedor.Focus();
            }
        }

        // botao excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgProdutos);
        }

        // carregar comboBox fornecedor
        private void carregaComboFornecedor(){
            var sql = from f in conexao.FORNECEDORES
                      where f.codigo > 0
                      select f.codigo + "    - " + f.nome;

            cbCodFornecedor.ItemsSource = null;
            cbCodFornecedor.ItemsSource = sql.ToList();
        }

        private void txtFornecedor_LostFocus(object sender, RoutedEventArgs e) {
            try {
                if (cbCodFornecedor.SelectedItem != null) {
                    int codigo = int.Parse(cbCodFornecedor.Text.Substring(0, 4).Trim());
                    FORNECEDORES f = new FORNECEDORES();
                    f = conexao.FORNECEDORES.Find(codigo);
                    string aux = f.nome;

                    if (txtFornecedor.Text != aux) {
                        txtFornecedor.Text = string.Empty;
                        txtFornecedor.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex) {
                return;
            }
        }

        private void cbCodFornecedor_DropDownClosed(object sender, EventArgs e) {
            try {
                int texto = cbCodFornecedor.Text.Length - 7;
                string resultado = cbCodFornecedor.Text.Substring(cbCodFornecedor.Text.Length - texto);
                int codigo = conexao.Database.SqlQuery<int>("select codigo from FORNECEDORES where nome='" + resultado + "'").SingleOrDefault();
                txtCodigoFornecedor.Text = codigo.ToString();
            }
            catch (Exception ex) {
                MessageBox.Show("Código do barbeiro invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodFornecedor.Text = "";
                txtCodigoFornecedor.Clear();
                cbCodFornecedor.Focus();
            }
        }

        // carregar comboBox pesquisa
        private void carregaPesquisa() {
            var sql = from p in conexao.PRODUTOS
                      where p.codigo > 0
                      select p.codigo + "    - " + p.descricao;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }
    }
}
