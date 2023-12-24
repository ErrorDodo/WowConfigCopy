using Prism.Mvvm;
using WowConfigCopy.Common.Models;

namespace WowConfigCopy.UI.Models
{
    public class AccountVisibilityModel : BindableBase
    {
        private bool _isExpanded;
        private readonly RealmAccountsModel _realmAccount;

        public AccountVisibilityModel(RealmAccountsModel realmAccount)
        {
            _realmAccount = realmAccount;
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }
        
        public string AccountName => _realmAccount.AccountName;
        public string ConfigPath => _realmAccount.ConfigPath;
    }
}