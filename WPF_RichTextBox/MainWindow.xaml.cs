using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;

namespace WPF_RichTextBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // count field
        int count = 0;

        // Count of primitives
        public int Count { get; set; }

        // constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Создание главного документа
            FlowDocument myFlowDocument = new FlowDocument();

            // Добавление блоков текста со своей разметкой
            myFlowDocument.Blocks.Add(new Paragraph(new Run("Most of the time, you won’t create flow documents programmatically. However, you might want to create an application that browses through portions of a flow document and modifies them dynamically.")));
            myFlowDocument.Blocks.Add(new Paragraph(new Run("However, because flow documents use deeply nested content with a freeflowing structure, you may need to dig through several layers to find the actual content you want to modify.")));

            // Создание жирного текста
            Bold bold1 = new Bold();
            bold1.Inlines.Add(new Run("If you need to modify thetext inside a flow document, the easiest way is to isolate exactly what you want to change (and no more) using a Span element."));
            bold1.Inlines.Add(new LineBreak());

            // Добавление жирного текста в параграф
            myFlowDocument.Blocks.Add(new Paragraph(bold1));

            // Показать документ в richTextBox1
            richTextBox1.Document = myFlowDocument;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение документа в формате XAML
            TextWriter wr1 = new StreamWriter("doc1.txt");
            XamlWriter.Save(richTextBox1.Document, wr1);
            wr1.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            // Загрузка документа в формате XAML
            TextReader reader1 = new StreamReader("doc1.txt");
            XmlReader xmlReader = XmlReader.Create(reader1);
            FlowDocument doc = (FlowDocument)XamlReader.Load(xmlReader);
            richTextBox1.Document = doc;
            reader1.Close();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            // Загрузка из RTF
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "RichText Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                TextRange documentTextRange = new TextRange(
                richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
                FileStream fs = File.Open(openFile.FileName, FileMode.Open);
                documentTextRange.Load(fs, DataFormats.Rtf);
                fs.Close();
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            // Сохранение документа в формате RTF
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XAML Files (*.xaml)|*.xaml|RichText Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
		// create textrange
                TextRange documentTextRange = new TextRange(
                richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd);
                FileStream fs = File.Create(saveFile.FileName);

                if (System.IO.Path.GetExtension(saveFile.FileName).ToLower() == ".rtf")
                {
                    documentTextRange.Save(fs, DataFormats.Rtf);
                }
                else
                {
                    documentTextRange.Save(fs, DataFormats.Xaml);
                }
                fs.Close();
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            Table t1 = new Table();

            // Добавление столбца в таблицу
            TableColumn tc1 = new TableColumn();
            t1.Columns.Add(tc1);
            t1.Columns.Add(new TableColumn());
            //t1.Columns.Add(new TableColumn());

            // Создание строки таблицы
            TableRow tr1 = new TableRow();

            // Создание ячейки для строки
            TableCell cell1 = new TableCell(new Paragraph(new Run("row1")));
            cell1.BorderBrush = Brushes.BlueViolet;
            cell1.BorderThickness = new Thickness(1);

            // Добавление ячейки в строку
            tr1.Cells.Add(cell1);

            // Добавление 2 ячейки в строку
            tr1.Cells.Add(new TableCell(new Paragraph(new Run("Item 2"))));

            // Создание группы строк и добавление строки
            TableRowGroup tg1 = new TableRowGroup();
            tg1.Rows.Add(tr1);

            // Создание 2 строки
            TableRow tr2 = new TableRow();
            TableCell cell2 = new TableCell(new Paragraph(new Run("row2")));
            cell2.BorderBrush = Brushes.Blue;
            cell2.BorderThickness = new Thickness(1);
            tr2.Cells.Add(cell2);
            tr2.Cells.Add(new TableCell(new Paragraph(new Run("Item 2"))));
            TableRowGroup tg2 = new TableRowGroup();
            tg2.Rows.Add(tr2);

            t1.RowGroups.Add(tg1);
            t1.RowGroups.Add(tg2);

            // Добавление таблицы в документ
            richTextBox1.Document.Blocks.Add(t1);
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            // Создание списка и добавление в него первого пункта списка
            List lst = new List(new ListItem(new Paragraph(new Run("Hello world!!!"))));

            // Добавление других пунктов
            ListItem li = new ListItem(new Paragraph(new Run("Item 2")));
            lst.ListItems.Add(li);
            lst.ListItems.Add(new ListItem(new Paragraph(new Bold(new Run("Item 3")))));

            // Установка маркера списка
            lst.MarkerStyle = TextMarkerStyle.Square;

            // Добавление списка в документ
            richTextBox1.Document.Blocks.Add(lst);
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            // Секция - логическая часть документа

            // Создание секции
            Section sec = new Section(new Paragraph(new Run("Start section")));

            // Добавление блока в секцию
            sec.Blocks.Add(new Paragraph(new Run("End section")));
            sec.Background = Brushes.Beige;

            // Добавление секции в документ
            richTextBox1.Document.Blocks.Add(sec);
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            // Добавление кнопки в документ
            Button button1 = new Button();
            button1.Content = "Ok";
            button1.Click += Button1_Click;

            // Создание контейнера для элементов управления
            BlockUIContainer cont = new BlockUIContainer(button1);

            richTextBox1.Document.Blocks.Add(cont);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Document button clicked!");
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            richTextBox1.Selection.Text = "";
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            // Установка жирности для выделенного текста
            richTextBox1.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            // Установка наклонности для выделенного текста
            richTextBox1.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            // Установка фона для выделенного текста
            richTextBox1.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);

            // Пробежать по всем таблицам и поменять содержимое 1 ячейки
            /*foreach (Block tb in richTextBox1.Document.Blocks)
            {
                if (tb is Table)
                {
                    BlockCollection bl = ((Table)tb).RowGroups[0].Rows[0].Cells[0].Blocks;
                    Block b = bl.ElementAt(0);
                    if (b is Paragraph)
                    {
                        // очистить весь текст в параграфе
                        ((Paragraph)b).Inlines.Clear();

                        // добавить новый текст в первый параграф
                        ((Paragraph)b).Inlines.Add(new Run("!!!"));
                    }
                    break;
                }
            }*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // показать системные шрифты в выпадающем списке
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                comboBox1.Items.Add(fontFamily.Source);
            }
        }

        // Установка шрифта для выделенного текста
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            richTextBox1.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, comboBox1.Items[comboBox1.SelectedIndex]);
        }

        // Установка цвета текста для выделенного текста
        private void button14_Click(object sender, RoutedEventArgs e)
        {
            richTextBox1.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {
            // Добавление фигуры в документ
            Figure figure1 = new Figure(new Paragraph(new Run("Figure test!")));
            figure1.BorderThickness = new Thickness(1);
            figure1.BorderBrush = Brushes.Black;
            figure1.FlowDirection = FlowDirection.LeftToRight;
            figure1.HorizontalAnchor = FigureHorizontalAnchor.ContentCenter;
            //figure1.HorizontalOffset = 500;   //// НЕ работает!!!
            richTextBox1.Document.Blocks.Add(new Paragraph(figure1));
        }

        private void button16_Click(object sender, RoutedEventArgs e)
        {
            // Создание ссылки
            Hyperlink hl = new Hyperlink(new Run("Link test"));
            hl.Click += new RoutedEventHandler(hl_Click);
            richTextBox1.Document.Blocks.Add(new Paragraph(hl));
            richTextBox1.Document.IsHyphenationEnabled = true;
        }

        void hl_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("OK!");
        }
    }
}
