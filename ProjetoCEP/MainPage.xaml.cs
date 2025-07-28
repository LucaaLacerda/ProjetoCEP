using ProjetoCEP.Services;
using ProjetoCEP.Models;

namespace ProjetoCEP
{
    //pagina principal faz o tratamento dos dados e exibe ao usuario
    public partial class MainPage : ContentPage
    {
        private readonly CepService _CepService;

        public MainPage()
        {
            InitializeComponent();
            _CepService = new CepService();

        }
        private void CepEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string cep = e.NewTextValue;

            cep = new string(cep.Where(char.IsDigit).ToArray());


            if (cep.Length > 5)
            {
                cep = cep.Insert(5, "-");
            }

            if (entry.Text != cep)
            {
                entry.Text = cep;
                return;
            }
        }

        private async void OnBuscarCepClicked(object sender, EventArgs e)
        {
            ResultadoLabel.Text = "";
            string cep = CepEntry.Text?.Trim().Replace("-", "");


            if (!ValidarCep(cep))
            {
                await DisplayAlert("Erro", "CEP inválido. Digite exatamente 8 números.", "OK");
                return;
            }

            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;

                var endereco = await _CepService.BuscarEnderecoAsync(cep);

                ResultadoLabel.Text =
                    $"CEP: {endereco.Cep}\n" +
                    $"Logradouro: {endereco.Logradouro}\n" +
                    $"Complemento: {endereco.Complemento}\n" +
                    $"Bairro: {endereco.Bairro}\n" +
                    $"Cidade: {endereco.Localidade}\n" +
                    $"UF: {endereco.Uf}\n" +
                    $"Estado: {endereco.Estado}\n" +
                    $"Região: {endereco.Regiao}\n" +
                    $"IBGE: {endereco.Ibge}\n" +
                    $"DDD: {endereco.Ddd}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally 
            {
              LoadingIndicator.IsVisible = false;
              LoadingIndicator.IsRunning = false;
            }
        }

        private bool ValidarCep(string cep)
        {
            return cep.Length == 8 && cep.All(char.IsDigit);
        }
    }

}
