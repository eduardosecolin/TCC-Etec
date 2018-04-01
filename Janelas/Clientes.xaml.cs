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

namespace BarberSystem.Janelas
{
    /// <summary>
    /// Lógica interna para Clientes.xaml
    /// </summary>
    public partial class Clientes : Excel.Window {

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
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtNome.Focus();
            limpaCampos();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
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

            conexao.CLIENTES.Add(cliente);
            conexao.SaveChanges();

            txtCodigo.Text = cliente.codigo.ToString();
            carregarGrid();

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

        // botao excluir
        private void btnExcluir_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult resultado = MessageBox.Show("Tem certeza que deseja excluir o registro?", "Excluir",
                                                          MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(resultado == MessageBoxResult.Yes){
                cliente = conexao.CLIENTES.Remove(cliente);
                cliente.nome = null;
                cliente.sexo = null;
                cliente.endereco = null;
                cliente.numero = 0;
                cliente.bairro = null;
                cliente.cidade = null;
                cliente.estado = null;
                cliente.cep = null;
                cliente.telefone = null;
                cliente.celular = null;
                conexao.SaveChanges();
                MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                carregarGrid();
                limpaCampos();
            }else{
                limpaCampos();
                return;
            }
        }

        // botao pesquisar
        private void btnPesquisar_Click(object sender, RoutedEventArgs e) {
         try{
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
           }else{
                    MessageBox.Show("Cliente não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
           }
         }catch(Exception a){
                MessageBox.Show("Campo vazio ou código invalido!" + "\n" + a.Message, "Erro", MessageBoxButton.OK,
                                      MessageBoxImage.Exclamation);
                limpaCampos();
                return;
            }
        }

        // exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dgCliente.Columns.Count; j++) {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dgCliente.Columns[j].Header;
            }
            for (int i = 0; i < dgCliente.Columns.Count; i++) {
                for (int j = 0; j < dgCliente.Items.Count; j++) {
                    TextBlock b = dgCliente.Columns[i].GetCellContent(dgCliente.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
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
                return;
            }
        }

















        //------------- REFERENCIAS PARA EXCEL ---------------------------------------------------------------
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
        public bool DisplayWhitespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); 
       }
    }
}
