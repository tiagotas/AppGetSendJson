using AppGetSendJson.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AppGetSendJson.Service
{
    public class DataServicePessoa
    {
        /**
         * $data = json_decode(file_get_contents('php://input'));



            $dados_cliente = array(
                'nome' => ucwords(strtolower($data->Nome)),
                'cnpj' => String_Utils::toNumber($data->Cnpj),
                'email' => strtolower($data->Email),
                'contato' => ucwords(strtolower($data->Contato))
            );
         */


        /**
         * Atributo para definir o URL base das requisições ao serviço no site.
         * Caso implantar o HTTPS, alterar somente aqui, na base.
         */
        protected static readonly string query_base = "http://10.0.0.2/";

        public static async Task<Pessoa> Cadastrar(Pessoa c)
        {
            string queryString = query_base + "?query=cliente-cadastrar";

            // Convertendo a requisição para JSON
            var json = JsonConvert.SerializeObject(c);

            // Enviando os dados ao servidor.
            dynamic resultado = await PostDataToService(json, queryString).ConfigureAwait(false);

            if (resultado != null)
            {
                // retorna algo do servidor
                return JsonConvert.DeserializeObject<Pessoa>(resultado);

            }
            else return null;
        }

        protected static async Task<dynamic> GetDataFromService(string queryString)
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                throw new Exception("Por favor, conecte-se à Internet.");
            }

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(queryString);

            if (response.IsSuccessStatusCode)
            {
                dynamic data = null;

                if (response != null)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject(json);
                }

                return data;
            }
            else throw new Exception(DecodeServerError(response.StatusCode));
        }


        /**
         * Método que envia os dados para o servidor via post
         */
        protected static async Task<dynamic> PostDataToService(string json_object, string uri)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                throw new Exception("Por favor, conecte-se à Internet.");
            }

            // Instância do objeto para requisição HTTP
            HttpClient client = new HttpClient();


            // Codificando a string do objeto como Json 
            var content = new StringContent(json_object, Encoding.UTF8, "application/json");


            // Envia a requisição ao servidor.
            var result_request = await client.PostAsync(uri, content);

            if (result_request.IsSuccessStatusCode)
            {
                var server_response = await result_request.Content.ReadAsStringAsync();

                return server_response;

            }
            throw new Exception(DecodeServerError(result_request.StatusCode));
        }

        private static string DecodeServerError(System.Net.HttpStatusCode status_code)
        {
            string msg_erro;

            switch (status_code)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    msg_erro = "A requisição não pode ser atendida agora. Tente mais tarde.";
                    break;

                case System.Net.HttpStatusCode.NotFound:
                    msg_erro = "Recurso indisponível no momento. Tente mais tarde.";
                    break;

                case System.Net.HttpStatusCode.InternalServerError:
                    msg_erro = "Nosso banco de dados está indisponível. Tente mais tarde.";
                    break;

                case System.Net.HttpStatusCode.RequestTimeout:
                    msg_erro = "O servidor está demorando muito para responder. Tente novamente.";
                    break;

                case System.Net.HttpStatusCode.Forbidden:
                    msg_erro = "Recurso temporariamente indisponível. Tente mais tarde.";
                    break;

                default:
                    msg_erro = "Estamos com dificuldades para acessar nosso servidor, tente novamente.";
                    break;
            }

            return msg_erro;

        }
    }
}
