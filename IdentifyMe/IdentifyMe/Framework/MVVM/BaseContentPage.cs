using System;
using System.Threading.Tasks;
using IdentifyMe.MVVM.Abstractions;
using Xamarin.Forms;

namespace IdentifyMe.MVVM
{
    public abstract class BaseContentPage<T> : ContentPage, IViewFor<T> where T : BaseNavigationViewModel
    {
        object IViewFor.ViewModel
        {
            get => _viewModel;
            set => ViewModel = (T)value;
        }

        private T _viewModel;
        public T ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel == value) return;

                BindingContext = _viewModel = value;
                
                if (_viewModel is null) return;
                
                Task.Run(Init).SafeAwait();;
            }
        }
        
        protected override void OnAppearing()
        {
            _viewModel?.OnAppearing();
            base.OnAppearing();
        }

        private Task Init()
        {
            try
            {
                return ViewModel == null
                    ? Task.CompletedTask
                    : ViewModel.InitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Exception in {typeof(T).Name}.{nameof(ViewModel.InitAsync)}() {ex.Message}");
                throw;
            }
        }
    }
}
