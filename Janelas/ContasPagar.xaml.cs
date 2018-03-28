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

namespace BarberSystem.Janelas {
    /// <summary>
    /// Lógica interna para ContasPagar.xaml
    /// </summary>
    public partial class ContasPagar : Excel.Window {

        CONTAS_PAGAR cp = new CONTAS_PAGAR();
        BancoDados conexao = new BancoDados();
        private List<CONTAS_PAGAR> listaPagar = new List<CONTAS_PAGAR>();

        public ContasPagar() {
            InitializeComponent();
            dgPagar.RowBackground = null;
            carregaGrid();
        }

        // limpar os campos(textBox)
        public void limpaCampos(){
            txtCodigo.Clear();
            txtDescricao.Clear();
            txtPesquisar.Clear();
            txtUnitario.Clear();
            lblTotal.Content = "0";
            dpPagto.Text = "";
            dpVencto.Text = "";
        }

        //carregar o dataGrid
        public void carregaGrid(){
            listaPagar = conexao.CONTAS_PAGAR.ToList();
            dgPagar.ItemsSource = null;
            dgPagar.ItemsSource = listaPagar.OrderBy(user => user.codigo);
        }

        // botao novo
        private void btnNovo_Click(object sender, RoutedEventArgs e) {
            txtDescricao.Focus();
            limpaCampos();
        }

        // calcular valor total e mostrar na Label
        public void calculaValorTotal(){
            cp.vl_total = 0.0;
            foreach (CONTAS_PAGAR item in listaPagar) {
                cp.vl_total += item.vl_unitario;
            }
            lblTotal.Content = cp.vl_total.ToString();
        }

        // botao gravar
        private void btnGravar_Click(object sender, RoutedEventArgs e) {
            calculaValorTotal();
            cp.descricao = txtDescricao.Text;
            cp.data_pagto = DateTime.Parse(dpPagto.SelectedDate.ToString());
            cp.data_vencto = DateTime.Parse(dpVencto.SelectedDate.ToString());
            cp.vl_unitario = double.Parse(txtUnitario.Text);
            cp.vl_total += cp.vl_unitario;

            conexao.CONTAS_PAGAR.Add(cp);
            conexao.SaveChanges();


            txtCodigo.Text = cp.codigo.ToString();
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
            try {
             if(txtPesquisar.Text != ""){
                    cp = conexao.CONTAS_PAGAR.Find(int.Parse(txtPesquisar.Text));
                    txtCodigo.Text = cp.codigo.ToString();
                    txtDescricao.Text = cp.descricao;
                    dpPagto.Text = cp.data_pagto.ToString();
                    dpVencto.Text = cp.data_vencto.ToString();
                    txtUnitario.Text = cp.vl_unitario.ToString();
                    lblTotal.Content = cp.vl_total.ToString();
             }else {
                    MessageBox.Show("Registro não encontrado!", "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
                    limpaCampos();
                }
            }
            catch (Exception a) {
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
                cp = conexao.CONTAS_PAGAR.Remove(cp);
                cp.descricao = null;
                cp.data_pagto = null;
                cp.data_vencto = null;
                cp.vl_unitario = 0;
                cp.vl_total = 0;
                limpaCampos();
                conexao.SaveChanges();
                MessageBox.Show("Registro excluido com sucesso!", "Excluir", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                carregaGrid();
                limpaCampos();
            }else{
                limpaCampos();
                return;
            }
        }

        // botao calcular valor total
        private void btnCalcularValorTotal_Click(object sender, RoutedEventArgs e) {
            calculaValorTotal();
        }

        // exportar para o excel
        private void btnExportar_Click(object sender, RoutedEventArgs e) {
                Excel.Application excel = new Excel.Application();
                excel.Visible = true;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

                for (int j = 0; j < dgPagar.Columns.Count; j++) {
                    Range myRange = (Range)sheet1.Cells[1, j + 1];
                    sheet1.Cells[1, j + 1].Font.Bold = true;
                    sheet1.Columns[j + 1].ColumnWidth = 15;
                    myRange.Value2 = dgPagar.Columns[j].Header;
                }
                for (int i = 0; i < dgPagar.Columns.Count; i++) {
                    for (int j = 0; j < dgPagar.Items.Count; j++) {
                        TextBlock b = dgPagar.Columns[i].GetCellContent(dgPagar.Items[j]) as TextBlock;
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                        myRange.Value2 = b.Text;
                    }
                }
        }















        //------------ REFERENCIAS PARA EXCEL -------------------------------------------------------------------
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
