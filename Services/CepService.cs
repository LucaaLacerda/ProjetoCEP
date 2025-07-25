using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ProjetoCEP.Models;

// conexão com a API viacep fazendo o teste se esta conectando e tendo resposta

namespace ProjetoCEP.Services
{
    public class CepService
    {
        private readonly HttpClient _httpClient;
        public CepService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Endereco> BuscarEnderecoAsync(string cep)
        {
            try
            {
                var url = $"https://viacep.com.br/ws/{cep}/json/";

                var response = await _httpClient.GetAsync(url);
                var endereco = await response.Content.ReadFromJsonAsync<Endereco>();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao conectar com a API.");
                }

                if (endereco == null || endereco.Cep == null)
                {
                    throw new Exception("CEP não encontrado.");
                }
                return endereco;
            }
            catch (Exception ex)
            {
            throw new Exception(ex.Message);
            }
        }
    }
}
