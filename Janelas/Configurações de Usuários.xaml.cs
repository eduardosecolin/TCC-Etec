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

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Configurações_de_Usuários.xaml
    /// </summary>
    public partial class Configurações_de_Usuários {

        USUARIOS usuario = new USUARIOS();
        BancoDados conexao = new BancoDados();
        private List<USUARIOS> listaUsuario = new List<USUARIOS>();

        public Configurações_de_Usuários() {
            InitializeComponent();
            dgUsuario.RowBackground = null;
            carregaGrid();
            carregaCombopesquisa();
        }

        // metodo para limpar os campos
        public void limpaCampos(){
            txtCodigo.Clear();
            txtUsuario.Clear();
            txtSenha.Clear();
            txtEndereco.Clear();
            txtBairro.Clear();
            txtCidade.Clear();
            cbEstado.Text = "";
            MtxtCpf.Clear();
            txtEmail.Clear();
            cbTipo.Text = "";
            cbPesquisar.Text = string.Empty;
            txtSenhaOculta.Clear();
            btnGravar.IsEnabled = true;
        }

        // metodo para carregar o datagrid
        public void carregaGrid(){
            listaUsuario = conexao.USUARIOS.ToList();
            dgUsuario.ItemsSource = null;
            dgUsuario.ItemsSource = listaUsuario.OrderBy(user => user.nome_usuario);
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtUsuario.Focus();
            limpaCampos();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            try {
                usuario.nome_usuario = Util.VerificarCamposVazios(txtUsuario.Text);
                usuario.senha = Util.VerificarCamposVazios(Criptografia.Encrypt(txtSenhaOculta.Password));
                usuario.endereco = Util.VerificarCamposVazios(txtEndereco.Text);
                usuario.bairro = Util.VerificarCamposVazios(txtBairro.Text);
                usuario.cidade = Util.VerificarCamposVazios(txtCidade.Text);
                usuario.estado = Util.VerificarCamposVazios(cbEstado.Text);
                usuario.cpf = MtxtCpf.Text;
                usuario.email = Util.VerificarCamposVazios(txtEmail.Text);
                usuario.tipo = Util.VerificarCamposVazios(cbTipo.Text);

                if (Util.vazio == true) {
                    return;
                }

                if (txtUsuario.Text != "") {
                    var query = from u in conexao.USUARIOS where u.nome_usuario == txtUsuario.Text select u.nome_usuario;
                    if (query.FirstOrDefault() == txtUsuario.Text) {
                        MessageBox.Show("Usuário já existe!", "BarberSystem Information", MessageBoxButton.OK, MessageBoxImage.Stop);
                        limpaCampos();
                        return;
                    }
                }

                txtSenha.Text = Criptografia.Decrypt(usuario.senha);

                conexao.USUARIOS.Add(usuario);
                conexao.SaveChanges();

                txtCodigo.Text = usuario.codigo.ToString();
                carregaGrid();
                carregaCombopesquisa();

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
                    usuario = conexao.USUARIOS.Find(codigo);
                    txtCodigo.Text = usuario.codigo.ToString();
                    txtUsuario.Text = usuario.nome_usuario;
                    txtSenha.Text = Criptografia.Decrypt(usuario.senha);
                    txtSenhaOculta.Password = usuario.senha;
                    txtEndereco.Text = usuario.endereco;
                    txtBairro.Text = usuario.bairro;
                    cbEstado.Text = usuario.estado;
                    MtxtCpf.Text = usuario.cpf;
                    txtEmail.Text = usuario.email;
                    cbTipo.Text = usuario.tipo;
                    txtCidade.Text = usuario.cidade;
               }else{
                    MessageBox.Show("Usuário não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
               }
            }catch (Exception a) {
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
                    usuario = conexao.USUARIOS.Remove(usuario);
                    usuario.nome_usuario = null;
                    usuario.senha = null;
                    usuario.endereco = null;
                    usuario.bairro = null;
                    usuario.estado = null;
                    usuario.cpf = null;
                    usuario.email = null;
                    usuario.tipo = null;
                    conexao.SaveChanges();
                    int? codigo = conexao.AGENDA.Max(a => (int?)a.codigo);
                    Util.redefinirPK_AutoIncremento("USUARIOS", codigo);
                    MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    carregaGrid();
                    limpaCampos();
                    carregaCombopesquisa();
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

        // botao exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
          Util.exportarExcel(dgUsuario);
        }

        // mostrar senha
        private void checkBox_Checked(object sender, RoutedEventArgs e) {
            txtSenha.Visibility = Visibility.Visible;
            txtSenhaOculta.Visibility = Visibility.Hidden;
        }
        private void checkBox_Unchecked(object sender, RoutedEventArgs e) {
            txtSenhaOculta.Visibility = Visibility.Visible;
            txtSenha.Visibility = Visibility.Hidden;
        }

        // botao alterar
        private void btnAlterar_Click(object sender, RoutedEventArgs e) {
            try {
                if (txtCodigo.Text != "") {
                    usuario.nome_usuario = txtUsuario.Text;
                    usuario.senha = Criptografia.Encrypt((txtSenhaOculta.Password));
                    usuario.endereco = txtEndereco.Text;
                    usuario.bairro = txtBairro.Text;
                    usuario.cidade = txtCidade.Text;
                    usuario.estado = cbEstado.Text;
                    usuario.cpf = MtxtCpf.Text;
                    usuario.email = txtEmail.Text;
                    usuario.tipo = cbTipo.Text;
                    conexao.USUARIOS.AddOrUpdate(usuario);
                    conexao.SaveChanges();
                    MessageBox.Show("Dados alterados com sucesso!", "Alterar", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                    carregaGrid();
                    carregaCombopesquisa();
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
        }


        // carregar combo de pesquisa
        private void carregaCombopesquisa() {
            var sql = from u in conexao.USUARIOS
                      where u.codigo > 0
                      select u.codigo + "    - " + u.nome_usuario;
            cbPesquisar.ItemsSource = null;
            cbPesquisar.ItemsSource = sql.ToList();
        }
    }
}
