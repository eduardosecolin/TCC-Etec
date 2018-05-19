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
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using BarberSystem.Controle;

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para Estoque.xaml
    /// </summary>
    ///

    public partial class Estoque : Excel.Window {


        ESTOQUE estoque = new ESTOQUE();
        BancoDados conexao = new BancoDados();
        private List<ESTOQUE> listaEstoque = new List<ESTOQUE>();

        public Estoque() {
            InitializeComponent();
            dgEstoque.RowBackground = null;
            carregaGrid();
            carregaComboBox();
        }

        // limpar os campos
        private void limpaCampos(){
            txtPesquisar.Clear();
            txtCodigo.Clear();
            cbCodProduto.Text = "";
            txtProduto.Clear();
            txtUnitario.Clear();
            txtTotal.Clear();
            txtQuantidade.Clear();
            txtEntrada.Clear();
            txtSaida.Clear();
        }

        // carregar o dataGrid
        private void carregaGrid(){
            listaEstoque = conexao.ESTOQUE.ToList();
            dgEstoque.ItemsSource = null;
            dgEstoque.ItemsSource = listaEstoque.OrderBy(user => user.codigo);
        }

        // carregar comboBox com o codigo do produto
        private void carregaComboBox(){
            List<PRODUTOS> listaProduto = conexao.PRODUTOS.ToList();
            cbCodProduto.ItemsSource = null;
            cbCodProduto.ItemsSource = listaProduto.OrderBy(user => user.codigo);
            cbCodProduto.DisplayMemberPath = "codigo";
        }

        // preencher nome do produto automatico
        private void txtProduto_GotFocus(object sender, RoutedEventArgs e) {
            try {
                if (cbCodProduto.SelectedItem != null) {
                    PRODUTOS produto = new PRODUTOS();
                    produto = conexao.PRODUTOS.Find(int.Parse(cbCodProduto.Text));
                    txtProduto.Text = produto.descricao;
                    txtUnitario.Text = produto.vl_unitario.ToString();
                }
            }
            catch (Exception a) {
                MessageBox.Show("Código do produto invalido!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                cbCodProduto.Text = "";
                txtProduto.Clear();
                cbCodProduto.Focus();
                Log.logException(a);
                Log.logMessage(a.Message);
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
                estoque.codproduto = int.Parse(cbCodProduto.Text);
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

                MessageBox.Show("Dados salvo com sucesso!!!", "Salvando...", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
            }catch(Exception a){
                MessageBox.Show(a.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Log.logException(a);
                Log.logMessage(a.Message);
                return;
            }
        }

        // botao entrada
        private void btnEntrada_Click(object sender, RoutedEventArgs e) {
           if(txtProduto.Text == ""){
                MessageBox.Show("O campo produto não pode estar vazio", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                limpaCampos();
                return;
           }
            estoque.entradaEstoque(int.Parse(txtEntrada.Text));
            txtQuantidade.Text = estoque.quantidade.ToString();
            estoque.vl_produto = double.Parse(txtUnitario.Text);
            txtTotal.Text = estoque.calculaTotal().ToString();
            txtEntrada.Clear();
        }

        // botao saida
        private void btnSaida_Click(object sender, RoutedEventArgs e) {
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
                    estoque = conexao.ESTOQUE.Find(int.Parse(txtPesquisar.Text));
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
                estoque = conexao.ESTOQUE.Remove(estoque);
                limpaCampos();
                estoque.produto = null;
                estoque.vl_produto = null;
                estoque.vl_total = null;
                estoque.quantidade = null;
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
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < dgEstoque.Columns.Count; j++) {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 15;
                myRange.Value2 = dgEstoque.Columns[j].Header;
            }
            for (int i = 0; i < dgEstoque.Columns.Count; i++) {
                for (int j = 0; j < dgEstoque.Items.Count; j++) {
                    TextBlock b = dgEstoque.Columns[i].GetCellContent(dgEstoque.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }














        // ---------------------- REFERENCIAS PARA EXCEL ---------------------------------------------------------------------------------
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
