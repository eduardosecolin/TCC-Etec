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

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Fornecedores.xaml
    /// </summary>
    public partial class Fornecedores {

        private FORNECEDORES fornecedores = new FORNECEDORES();
        private List<FORNECEDORES> listaFornecedores;
        private BancoDados conexao = new BancoDados();
        public Fornecedores() {
            InitializeComponent();
            dgFornecedores.RowBackground = null;
            carregaGrid();
        }

        // metodo para limpar campos
        public void limpaCampos() {
            txtCodigo.Clear();
            txtNome.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtCidade.Clear();
            cbEstado.Text = "";
            MtxtCep.Clear();
            cbTipo.Text = "";
            MtxtTelefone.Clear();
            txtPesquisar.Clear();
            txtBairro.Clear();
        }

        // carregar a grid
        public void carregaGrid() {
            listaFornecedores = conexao.FORNECEDORES.ToList();
            dgFornecedores.ItemsSource = null;
            dgFornecedores.ItemsSource = listaFornecedores.OrderBy(user => user.nome);
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtNome.Focus();
            limpaCampos();
        }

        // botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    fornecedores.nome = txtNome.Text;
                    fornecedores.endereco = txtEndereco.Text;
                    fornecedores.numero = int.Parse(txtNumero.Text);
                    fornecedores.bairro = txtBairro.Text;
                    fornecedores.cidade = txtCidade.Text;
                    fornecedores.estado = cbEstado.Text;
                    fornecedores.cep = MtxtCep.Text;
                    fornecedores.tipo = cbTipo.Text;
                    fornecedores.telefone = MtxtTelefone.Text;
                    conexao.FORNECEDORES.AddOrUpdate(fornecedores);
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
                    fornecedores = conexao.FORNECEDORES.Find(int.Parse(txtPesquisar.Text));
                    txtCodigo.Text = fornecedores.codigo.ToString();
                    txtNome.Text = fornecedores.nome;
                    txtEndereco.Text = fornecedores.endereco;
                    txtNumero.Text = fornecedores.numero.ToString();
                    txtBairro.Text = fornecedores.bairro;
                    txtCidade.Text = fornecedores.cidade;
                    cbEstado.Text = fornecedores.estado;
                    MtxtCep.Text = fornecedores.cep;
                    cbTipo.Text = fornecedores.tipo;
                    MtxtTelefone.Text = fornecedores.telefone;
                }
                else {
                    MessageBox.Show("Fornecedor não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
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
            MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado == MessageBoxResult.Yes) {
                fornecedores = conexao.FORNECEDORES.Remove(fornecedores);
                fornecedores.nome = null;
                fornecedores.endereco = null;
                fornecedores.numero = null;
                fornecedores.bairro = null;
                fornecedores.cidade = null;
                fornecedores.estado = null;
                fornecedores.cep = null;
                fornecedores.tipo = null;
                fornecedores.telefone = null;
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
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                fornecedores.nome = Util.VerificarCamposVazios(txtNome.Text);
                fornecedores.endereco = Util.VerificarCamposVazios(txtEndereco.Text);
                fornecedores.numero = int.Parse(txtNumero.Text);
                fornecedores.bairro = Util.VerificarCamposVazios(txtBairro.Text);
                fornecedores.cidade = Util.VerificarCamposVazios(txtCidade.Text);
                fornecedores.estado = Util.VerificarCamposVazios(cbEstado.Text);
                fornecedores.cep = MtxtCep.Text;
                fornecedores.tipo = Util.VerificarCamposVazios(cbTipo.Text);
                fornecedores.telefone = MtxtTelefone.Text;

                if (Util.vazio == true) {
                    return;
                }

                conexao.FORNECEDORES.Add(fornecedores);
                conexao.SaveChanges();

                txtCodigo.Text = fornecedores.codigo.ToString();
                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                carregaGrid();
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

        // botao excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgFornecedores);
        }
    }
}
