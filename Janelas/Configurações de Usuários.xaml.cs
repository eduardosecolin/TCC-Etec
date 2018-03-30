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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Data.Entity.Migrations;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Configurações_de_Usuários.xaml
    /// </summary>
    public partial class Configurações_de_Usuários : Excel.Window {

        USUARIOS usuario = new USUARIOS();
        BancoDados conexao = new BancoDados();
        private List<USUARIOS> listaUsuario = new List<USUARIOS>();

        public Configurações_de_Usuários() {
            InitializeComponent();
            dgUsuario.RowBackground = null;
            carregaGrid();
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
            txtCpf.Clear();
            txtEmail.Clear();
            cbTipo.Text = "";
            txtPesquisar.Clear();
            txtSenhaOculta.Clear();
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
            usuario.nome_usuario = txtUsuario.Text;
            usuario.senha = txtSenha.Text;
            usuario.endereco = txtEndereco.Text;
            usuario.bairro = txtBairro.Text;
            usuario.cidade = txtCidade.Text;
            usuario.estado = cbEstado.Text;
            usuario.cpf = txtCpf.Text;
            usuario.email = txtEmail.Text;
            usuario.tipo = cbTipo.Text;

                if (txtUsuario.Text != "") {
                var query = from u in conexao.USUARIOS where u.nome_usuario == txtUsuario.Text select u.nome_usuario;
                    if (query.FirstOrDefault() == txtUsuario.Text) {
                        MessageBox.Show("Usuário já existe!", "BarberSystem Information", MessageBoxButton.OK, MessageBoxImage.Stop);
                        limpaCampos();
                        return;
                    }
                }

            txtSenhaOculta.Password = usuario.senha;

            conexao.USUARIOS.Add(usuario);
            conexao.SaveChanges();

            txtCodigo.Text = usuario.codigo.ToString();
            carregaGrid();

            MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
            limpaCampos();
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
         try{
          if(txtPesquisar.Text != ""){
                    usuario = conexao.USUARIOS.Find(int.Parse(txtPesquisar.Text));
                    txtCodigo.Text = usuario.codigo.ToString();
                    txtUsuario.Text = usuario.nome_usuario;
                    txtSenha.Text = usuario.senha;
                    txtSenhaOculta.Password = usuario.senha;
                    txtEndereco.Text = usuario.endereco;
                    txtBairro.Text = usuario.bairro;
                    cbEstado.Text = usuario.estado;
                    txtCpf.Text = usuario.cpf;
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
                return;
            }
        }

        // botao excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir",
                                                           MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(resultado == MessageBoxResult.Yes){
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
                MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                carregaGrid();
                limpaCampos();
            }else{
                limpaCampos();
                return;
            }
        }

        // botao exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dgUsuario.Columns.Count; j++) {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dgUsuario.Columns[j].Header;
            }
            for (int i = 0; i < dgUsuario.Columns.Count; i++) {
                for (int j = 0; j < dgUsuario.Items.Count; j++) {
                    TextBlock b = dgUsuario.Columns[i].GetCellContent(dgUsuario.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
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
            if (txtCodigo.Text != "") {
                usuario.nome_usuario = txtUsuario.Text;
                usuario.senha = txtSenha.Text;
                usuario.endereco = txtEndereco.Text;
                usuario.bairro = txtBairro.Text;
                usuario.cidade = txtCidade.Text;
                usuario.estado = cbEstado.Text;
                usuario.cpf = txtCpf.Text;
                usuario.email = txtEmail.Text;
                usuario.tipo = cbTipo.Text;
                conexao.USUARIOS.AddOrUpdate(usuario);
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















        //------------------ REFERENCIAS PARA EXCEL ----------------------------------------------------------------
        dynamic Excel.Window.Activate() {
            throw new NotImplementedException();
        }

        public dynamic ActivateNext() {
            throw new NotImplementedException();
        }

        public dynamic ActivatePrevious() {
            throw new NotImplementedException();
        }

        public bool Close(object SaveChanges, object Filename, object RouteWorkbook) {
            throw new NotImplementedException();
        }

        public dynamic LargeScroll(object Down, object Up, object ToRight, object ToLeft) {
            throw new NotImplementedException();
        }

        public Excel.Window NewWindow() {
            throw new NotImplementedException();
        }

        public dynamic _PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) {
            throw new NotImplementedException();
        }

        public dynamic PrintPreview(object EnableChanges) {
            throw new NotImplementedException();
        }

        public dynamic ScrollWorkbookTabs(object Sheets, object Position) {
            throw new NotImplementedException();
        }

        public dynamic SmallScroll(object Down, object Up, object ToRight, object ToLeft) {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsX(int Points) {
            throw new NotImplementedException();
        }

        public int PointsToScreenPixelsY(int Points) {
            throw new NotImplementedException();
        }

        public dynamic RangeFromPoint(int x, int y) {
            throw new NotImplementedException();
        }

        public void ScrollIntoView(int Left, int Top, int Width, int Height, object Start) {
            throw new NotImplementedException();
        }

        public dynamic PrintOut(object From, object To, object Copies, object Preview, object ActivePrinter, object PrintToFile, object Collate, object PrToFileName) {
            throw new NotImplementedException();
        }

        public Excel.Application Application => throw new NotImplementedException();

        public XlCreator Creator => throw new NotImplementedException();

        dynamic Excel.Window.Parent => throw new NotImplementedException();

        public Range ActiveCell => throw new NotImplementedException();

        public Chart ActiveChart => throw new NotImplementedException();

        public Pane ActivePane => throw new NotImplementedException();

        public dynamic ActiveSheet => throw new NotImplementedException();

        public dynamic Caption { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayFormulas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayGridlines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHeadings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayHorizontalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayOutline { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool _DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayVerticalScrollBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWorkbookTabs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayZeros { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableResize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool FreezePanes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int GridlineColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public XlColorIndex GridlineColorIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Index => throw new NotImplementedException();

        public string OnWindow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Panes Panes => throw new NotImplementedException();

        public Range RangeSelection => throw new NotImplementedException();

        public int ScrollColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ScrollRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Sheets SelectedSheets => throw new NotImplementedException();

        public dynamic Selection => throw new NotImplementedException();

        public bool Split { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitHorizontal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int SplitRow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SplitVertical { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double TabRatio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public XlWindowType Type => throw new NotImplementedException();

        public double UsableHeight => throw new NotImplementedException();

        public double UsableWidth => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Range VisibleRange => throw new NotImplementedException();

        public int WindowNumber => throw new NotImplementedException();

        XlWindowState Excel.Window.WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic Zoom { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public XlWindowView View { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayRightToLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SheetViews SheetViews => throw new NotImplementedException();

        public dynamic ActiveSheetView => throw new NotImplementedException();

        public bool DisplayRuler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoFilterDateGrouping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool DisplayWhitespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
