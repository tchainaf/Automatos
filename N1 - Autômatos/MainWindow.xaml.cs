using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace N1___Autômatos
{
    public partial class MainWindow : Window
    {
        Gramatica gr;
        string arquivo;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnArquivoIN_Click(object sender, RoutedEventArgs e)
        {
            listPalavras.Items.Clear();
            btnGerar.Visibility = Visibility.Hidden;
            btnArquivoOUT.Visibility = Visibility.Hidden;
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Arquivo IN|*.IN";
                openFile.ShowDialog();
                if (String.IsNullOrEmpty(openFile.FileName))
                    return;
                imgVerifica.Visibility = Visibility.Hidden;
                arquivo = openFile.FileName;
                txtCaminhoIN.Text = arquivo;
                lbValidacao.Content = "Aguarde a verificação do arquivo...";
                await Task.Delay(1000);
                gr = Arquivo.ValidacaoIN(arquivo);
                gr.PreencheRegras();
                imgVerifica.Visibility = Visibility.Visible;
                imgVerifica.Source = new BitmapImage(new Uri("/Imagens/yes.png", UriKind.Relative));
                lbValidacao.Content = "O arquivo é válido!";
                btnGerar.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                imgVerifica.Visibility = Visibility.Visible;
                imgVerifica.Source = new BitmapImage(new Uri("/Imagens/no.png", UriKind.Relative));
                lbValidacao.Content = ex.Message;
            }
        }

        private async void btnGerar_Click(object sender, RoutedEventArgs e)
        {
            string[] palavras = null;
            listPalavras.Items.Clear();
            imgVerifica.Source = null;
            btnGerar.Visibility = Visibility.Hidden;
            btnArquivoOUT.Visibility = Visibility.Hidden;
            int tempomax = 35;
            int qtdepalavras = 20;
            bool feito;
            do
            {
                feito = false;
                imgVerifica.Visibility = Visibility.Hidden;
                lbValidacao.Content = "Aguarde a geração de palavras...";
                await Task.Delay(2000).ContinueWith(_ => { palavras = gr.GerarPalavras(tempomax, qtdepalavras); });
                imgVerifica.Visibility = Visibility.Visible;
                if (palavras == null)
                {
                    lbValidacao.Content = "";
                    if (tempomax==50 && qtdepalavras==10)
                    {
                        MessageBox.Show("Não foi possível gerar palavras com essa gramática.", "Ops...", MessageBoxButton.OK, MessageBoxImage.Error);
                        imgVerifica.Source = new BitmapImage(new Uri("/Imagens/no.png", UriKind.Relative));
                        lbValidacao.Content = "Selecione outro arquivo IN!";
                        return;
                    }
                    if (MessageBox.Show("A geração demorou muito e pode ser que a gramática não seja válida. Deseja tentar gerar palavras com essa gramática novamente?", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        imgVerifica.Source = new BitmapImage(new Uri("/Imagens/no.png", UriKind.Relative));
                        lbValidacao.Content = "Selecione outro arquivo IN!";
                        feito = true;
                    }
                    else if (tempomax == 50)
                        qtdepalavras -= 5;
                    else
                    { 
                        tempomax += 15;
                        qtdepalavras -= 5;
                    }
                }
                else
                {
                    Arquivo.SalvarOUT(palavras);
                    foreach (string linha in palavras)
                        if (linha != null)
                            listPalavras.Items.Add(linha);
                    imgVerifica.Source = new BitmapImage(new Uri("/Imagens/yes.png", UriKind.Relative));
                    lbValidacao.Content = listPalavras.Items.Count + " palavras geradas com sucesso!";
                    btnArquivoOUT.Visibility = Visibility.Visible;
                    btnGerar.Visibility = Visibility.Visible;
                    feito = true;
                }
            } while (!feito);
        }

        private void btnArquivoOUT_Click(object sender, RoutedEventArgs e)
        {
            if (arquivo != null)
                Process.Start(arquivo.Replace(".IN", ".OUT"));
        }

        private void imgVerifica_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (imgVerifica.Visibility == Visibility.Hidden)
                carregando.Visibility = Visibility.Visible;
            else
                carregando.Visibility = Visibility.Hidden;
        }
    }
}
