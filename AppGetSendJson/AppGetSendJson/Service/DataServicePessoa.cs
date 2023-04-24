using AppGetSendJson.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppGetSendJson.Service
{
    public class DataServicePessoa : DataService
    {
        /**
         * Obtém a lista de pessoas
         */
        public static async Task<List<Pessoa>> GetPessoasAsync()
        {
            string json = await DataService.GetDataFromService("/pessoa");

            List<Pessoa> arr_pessoas = JsonConvert.DeserializeObject<List<Pessoa>>(json);

            return arr_pessoas;
        }
        
        /**
         * Envia um Model em forma de JSON ara insert no banco.
         */
        public static async Task<bool> Cadastrar(Pessoa c)
        {
            var json_a_enviar = JsonConvert.SerializeObject(c);

            string json = await DataService.PostDataToService(json_a_enviar, "/pessoa/salvar");

            return true;
        }

        /**
         * Realiza uma busca de pessoas no banco de dados.
         */
        public static async Task<List<Pessoa>> SearchAsync(string q)
        {
            var json_a_enviar = JsonConvert.SerializeObject(q);

            string json = await DataService.PostDataToService(json_a_enviar, "/pessoa/buscar");

            List<Pessoa> arr_pessoas = JsonConvert.DeserializeObject<List<Pessoa>>(json);

            return arr_pessoas;
        }

        /**
         * Deleta uma pessoa do banco de dados.
         */
        public static async Task<List<Pessoa>> DeleteAsync(int id)
        {
            var json_a_enviar = JsonConvert.SerializeObject(id);

            string json = await DataService.PostDataToService(json_a_enviar, "/pessoa/delete");

            List<Pessoa> arr_pessoas = JsonConvert.DeserializeObject<List<Pessoa>>(json);

            return arr_pessoas;
        }

    }
}
