using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        public Role Role { get; set; } = Role.None;
        public Client Client { get; internal set; }
    }
}
