using AppGetSendJson.Model;
using AppGetSendJson.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppGetSendJson.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listagem : ContentPage
    {
        ObservableCollection<Pessoa> ListaPessoas = new ObservableCollection<Pessoa>();

        public Listagem()
        {
            InitializeComponent();

            lst_pessoas.ItemsSource= ListaPessoas;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new View.FormAdd());
        }

        protected override void OnAppearing()
        {
            if (ListaPessoas.Count == 0)
            {
                /**
                 * Inicializando a Thread que irá buscar o array de objetos no arquivo db3
                 * via classe SQLiteDatabaseHelper encapsulada na propriedade Database da
                 * classe App.
                 */
                System.Threading.Tasks.Task.Run(async () =>
                {
                    /**
                     * Retornando o array de objetos vindos do db3, foi usada uma variável tem do tipo
                     * List para que abaixo no foreach possamos percorrer a lista temporária e add
                     * os itens à ObservableCollection
                     */
                    List<Pessoa> temp = await DataServicePessoa.GetPessoasAsync();

                    foreach (Pessoa item in temp)
                    {
                        ListaPessoas.Add(item);
                    }

                    /**
                     * Após carregar os registros para a ObservableCollection removemos o loading da tela.
                     */
                   // ref_carregando.IsRefreshing = false;
                });
            }
        }

        private async void getAllRows()
        {
            act_carregando.IsRunning= true;

            try
            {
                //List<Pessoa> arr_pessoa = await DataServicePessoa.GetAllRows();

                ListaPessoas.Clear();

                //arr_pessoa.ForEach(i => ListaPessoas.Add(i));
            }
            catch (Exception ex)
            {   
                await DisplayAlert("Ops", ex.Message, "OK");

            } finally
            {
                act_carregando.IsRunning = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            act_carregando.IsRunning = true;
            act_carregando.IsVisible = true;

            try
            {
                List<Pessoa> temp = await DataServicePessoa.SearchAsync(txt_q.Text);

                ListaPessoas.Clear();

                foreach (Pessoa item in temp)
                {
                    ListaPessoas.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");

            }
            finally
            {
                act_carregando.IsRunning = false;
                act_carregando.IsVisible = false;
            }

        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            act_carregando.IsRunning = true;
            act_carregando.IsVisible = true;

            try
            {
                
                MenuItem menu = sender as MenuItem;
                Pessoa pessoa_selecionada = menu.BindingContext as Pessoa;

                await DataServicePessoa.DeleteAsync(pessoa_selecionada.Id);

                ListaPessoas.Remove(pessoa_selecionada);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");

            }
            finally
            {
                act_carregando.IsRunning = false;
                act_carregando.IsVisible = false;
            }
        }
    }
}