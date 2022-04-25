using Microsoft.AspNetCore.Components;

namespace Avto.UI.Front.ViewModels
{
    public abstract class BasePage<TPageModel> : ComponentBase 
        where TPageModel:BasePageModel, new()
    {
        protected TPageModel Model { get; } = new TPageModel();

        protected void ShowError(string errorMessage)
        {
            Model.ErrorMessage = errorMessage;
        }
    }
}
