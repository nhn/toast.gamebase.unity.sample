using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public interface IGamebaseAuth
    {
        void Login(string providerName, int handle);
        void Login(string providerName, Dictionary<string, object> additionalInfo, int handle);
        void Login(Dictionary<string, object> credentialInfo, int handle);
        void LoginForLastLoggedInProvider(int handle);
        void ChangeLogin(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle);
        
        void AddMapping(string providerName, int handle);
        void AddMapping(string providerName, Dictionary<string, object> additionalInfo, int handle);
        void AddMapping(Dictionary<string, object> credentialInfo, int handle);

        void AddMappingForcibly(string providerName, string forcingMappingKey, int handle);
        void AddMappingForcibly(string providerName, string forcingMappingKey, Dictionary<string, object> additionalInfo, int handle);
        void AddMappingForcibly(Dictionary<string, object> credentialInfo, string forcingMappingKey, int handle);
        void AddMappingForcibly(GamebaseResponse.Auth.ForcingMappingTicket forcingMappingTicket, int handle);

        void RemoveMapping(string providerName, int handle);
        void Logout(int handle);
        void Withdraw(int handle);
        void WithdrawImmediately(int handle);
        void RequestTemporaryWithdrawal(int handle);
        void CancelTemporaryWithdrawal(int handle);

        void IssueTransferAccount(int handle);
        void QueryTransferAccount(int handle);
        void RenewTransferAccount(GamebaseRequest.Auth.TransferAccountRenewConfiguration configuration, int handle);
        void TransferAccountWithIdPLogin(string accountId, string accountPassword, int handle);

        List<string> GetAuthMappingList();
        string GetAuthProviderUserID(string providerName);
        string GetAuthProviderAccessToken(string providerName);
        GamebaseResponse.Auth.AuthProviderProfile GetAuthProviderProfile(string providerName);
        GamebaseResponse.Auth.BanInfo GetBanInfo();
        void IssueShortTermTicket(int handle);
    }
}