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
using BarberSystem.Utils;
using BarberSystem.Dados;
using System.Data.Entity.Migrations;
using BarberSystem.Controle;

namespace BarberSystem.Janelas
{
    /// <summary>
    /// Lógica interna para Clientes.xaml
    /// </summary>
    public partial class Clientes {

        CLIENTES cliente = new CLIENTES();
        BancoDados conexao = new BancoDados();
        private List<CLIENTES> listaClientes = new List<CLIENTES>();
       
        public Clientes()
        {
            InitializeComponent();
            dgCliente.RowBackground = null;
            carregarGrid();
        }

        // carregar datagrid
        public void carregarGrid(){
            listaClientes = conexao.CLIENTES.ToList();
            dgCliente.ItemsSource = null;
            dgCliente.ItemsSource = listaClientes.OrderBy(user => user.nome);
        }

        // limpar os campos
        public void limpaCampos(){
            txtCodigo.Clear();
            txtNome.Clear();
            txtEndereco.Clear();
            txtNumero.Clear();
            txtBairro.Clear();
            txtCidade.Clear();
            cbEstado.Text = "";
            MtxtCep.Clear();
            MtxtTelefone.Clear();
            MtxtCelular.Clear();
            txtPesquisar.Clear();
            cbSexo.Text = "";
            cbStatus.Text = "";
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtNome.Focus();
            limpaCampos();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                cliente.nome = Util.VerificarCamposVazios(txtNome.Text);
                cliente.sexo = Util.VerificarCamposVazios(cbSexo.Text);
                cliente.endereco = Util.VerificarCamposVazios(txtEndereco.Text);
                cliente.numero = int.Parse(txtNumero.Text);
                cliente.bairro = Util.VerificarCamposVazios(txtBairro.Text);
                cliente.cidade = Util.VerificarCamposVazios(txtCidade.Text);
                cliente.estado = Util.VerificarCamposVazios(cbEstado.Text);
                cliente.cep = MtxtCep.Text;
                cliente.telefone = MtxtTelefone.Text;
                cliente.celular = MtxtCelular.Text;
                cliente.status_cliente = Util.VerificarCamposVazios(cbStatus.Text);

                if (Util.vazio == true) {
                    return;
                }

                conexao.CLIENTES.Add(cliente);
                conexao.SaveChanges();

                txtCodigo.Text = cliente.codigo.ToString();
                carregarGrid();

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

        // botao excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != string.Empty) {
                    string status = conexao.Database.SqlQuery<string>("select status_cliente from clientes where codigo=" + int.Parse(txtCodigo.Text)).FirstOrDefault();
                    if (status.Equals("Inativo", StringComparison.OrdinalIgnoreCase)) {
                        MessageBox.Show("Cliente inativado! impossivél excluir!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }


                MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir",
                                                              MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes) {
                    cliente = conexao.CLIENTES.Remove(cliente);
                    cliente.nome = null;
                    cliente.sexo = null;
                    cliente.endereco = null;
                    cliente.numero = null;
                    cliente.bairro = null;
                    cliente.cidade = null;
                    cliente.estado = null;
                    cliente.cep = null;
                    cliente.telefone = null;
                    cliente.celular = null;
                    cliente.status_cliente = null;
                    conexao.SaveChanges();
                    int? codigo = conexao.AGENDA.Max(a => (int?)a.codigo);
                    Util.redefinirPK_AutoIncremento("CLIENTES", codigo);
                    MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    carregarGrid();
                    limpaCampos();
                }
                else {
                    btnGravar.IsEnabled = true;
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

        // botao pesquisar
        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
            btnGravar.IsEnabled = false;
            try {
              if(txtPesquisar.Text != ""){
                    cliente = conexao.CLIENTES.Find(int.Parse(txtPesquisar.Text));
                    txtCodigo.Text = cliente.codigo.ToString();
                    txtNome.Text = cliente.nome;
                    cbSexo.Text = cliente.sexo;
                    txtEndereco.Text = cliente.endereco;
                    txtNumero.Text = cliente.numero.ToString();
                    txtBairro.Text = cliente.bairro;
                    txtCidade.Text = cliente.cidade;
                    cbEstado.Text = cliente.estado;
                    MtxtCep.Text = cliente.cep;
                    MtxtTelefone.Text = cliente.telefone;
                    MtxtCelular.Text = cliente.celular;
                    cbStatus.Text = cliente.status_cliente;
              }else{
                    MessageBox.Show("Cliente não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
              }
            }catch(Exception a){
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.Message, "Erro", MessageBoxButton.OK,
                                      MessageBoxImage.Exclamation);

                Log.logException(a);
                Log.logMessage(a.Message);
                limpaCampos();
                return;
            }
        }

        // exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Util.exportarExcel(dgCliente);
        }

        // botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    cliente.nome = txtNome.Text;
                    cliente.sexo = cbSexo.Text;
                    cliente.endereco = txtEndereco.Text;
                    cliente.numero = int.Parse(txtNumero.Text);
                    cliente.bairro = txtBairro.Text;
                    cliente.cidade = txtCidade.Text;
                    cliente.estado = cbEstado.Text;
                    cliente.cep = MtxtCep.Text;
                    cliente.telefone = MtxtTelefone.Text;
                    cliente.celular = MtxtCelular.Text;
                    cliente.status_cliente = cbStatus.Text;
                    conexao.CLIENTES.AddOrUpdate(cliente);
                    conexao.SaveChanges();
                    MessageBox.Show("Dados alterados com sucesso!", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    carregarGrid();
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
    }
}
