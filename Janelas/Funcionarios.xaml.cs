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
    /// Lógica interna para Funcionarios.xaml
    /// </summary>
    public partial class Funcionarios {

        BancoDados conexao = new BancoDados();
        FUNCIONARIOS funcionario = new FUNCIONARIOS();
        private List<FUNCIONARIOS> listaFuncionario = new List<FUNCIONARIOS>(); 
       
        public Funcionarios() {
            InitializeComponent();
            dgFuncionarios.RowBackground = null;
            carregaGrid();
        }

        //metodo para limpar os campos
        private void limpaCampos(){
            txtNome.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtBairro.Clear();
            txtCidade.Clear();
            cbEstado.Text = "";
            MtxtCep.Clear();
            cbSexo.Text = "";
            txtCargo.Clear();
            MtxtTelefone.Clear();
            MtxtCelular.Clear();
            txtSalario.Clear();
            txtPesquisar.Clear();
            txtCodigo.Clear();
        }

        //metodo para carregar o dataGrid
        private void carregaGrid(){
            listaFuncionario = conexao.FUNCIONARIOS.ToList();
            dgFuncionarios.ItemsSource = null;
            dgFuncionarios.ItemsSource = listaFuncionario.OrderBy(user => user.nome);
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
                    funcionario.nome = txtNome.Text;
                    funcionario.endereco = txtEndereco.Text;
                    funcionario.numero = int.Parse(txtNumero.Text);
                    funcionario.bairro = txtBairro.Text;
                    funcionario.cidade = txtCidade.Text;
                    funcionario.estado = cbEstado.Text;
                    funcionario.cep = MtxtCep.Text;
                    funcionario.sexo = cbSexo.Text;
                    funcionario.telefone = MtxtTelefone.Text;
                    funcionario.celular = MtxtCelular.Text;
                    funcionario.cargo = txtCargo.Text;
                    funcionario.salario = double.Parse(txtSalario.Text);
                    conexao.FUNCIONARIOS.AddOrUpdate(funcionario);
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
                    funcionario = conexao.FUNCIONARIOS.Find(int.Parse(txtPesquisar.Text));
                    txtCodigo.Text = funcionario.codigo.ToString();
                    txtNome.Text = funcionario.nome;
                    txtEndereco.Text = funcionario.endereco;
                    txtNumero.Text = funcionario.numero.ToString();
                    txtBairro.Text = funcionario.bairro;
                    txtCidade.Text = funcionario.cidade;
                    cbEstado.Text = funcionario.estado;
                    MtxtCep.Text = funcionario.cep;
                    cbSexo.Text = funcionario.sexo;
                    MtxtTelefone.Text = funcionario.telefone;
                    MtxtCelular.Text = funcionario.celular;
                    txtSalario.Text = funcionario.salario.ToString();
                    txtCargo.Text = funcionario.cargo;
                }
                else {
                    MessageBox.Show("Funcionario não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }
            catch (Exception a) {
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.Message, "Erro", MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);

                Log.logException(a);
                Log.logMessage(a.Message);
                limpaCampos();
                return;
            }
        }

        // botao excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            try {
                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    funcionario = conexao.FUNCIONARIOS.Remove(funcionario);
                    funcionario.nome = null;
                    funcionario.endereco = null;
                    funcionario.numero = null;
                    funcionario.bairro = null;
                    funcionario.cidade = null;
                    funcionario.estado = null;
                    funcionario.cep = null;
                    funcionario.sexo = null;
                    funcionario.telefone = null;
                    funcionario.celular = null;
                    funcionario.salario = null;
                    funcionario.cargo = null;
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
                funcionario.nome = Util.VerificarCamposVazios(txtNome.Text);
                funcionario.endereco = Util.VerificarCamposVazios(txtEndereco.Text);
                funcionario.numero = int.Parse(txtNumero.Text);
                funcionario.bairro = Util.VerificarCamposVazios(txtBairro.Text);
                funcionario.cidade = Util.VerificarCamposVazios(txtCidade.Text);
                funcionario.estado = Util.VerificarCamposVazios(cbEstado.Text);
                funcionario.cep = MtxtCep.Text;
                funcionario.sexo = Util.VerificarCamposVazios(cbSexo.Text);
                funcionario.telefone = MtxtTelefone.Text;
                funcionario.celular = MtxtCelular.Text;
                funcionario.cargo = Util.VerificarCamposVazios(txtCargo.Text);
                funcionario.salario = double.Parse(txtSalario.Text);

                if (Util.vazio == true) {
                    return;
                }

                conexao.FUNCIONARIOS.Add(funcionario);
                conexao.SaveChanges();

                txtCodigo.Text = funcionario.codigo.ToString();
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

        // botao sair
        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        // botao excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgFuncionarios);
        }
    }
}
