using UnityEngine;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;

public class GoogleLoginHandler : BaseServiceBootstrapper
{
    [SerializeField] private ClientDataObject googleClientDataObject;
    [SerializeField] private ClientDataObject googleClientDataObjectEditor;

    protected override void RegisterServices()
    {
        OpenIDConnectService oidc = new OpenIDConnectService();
        oidc.OidcProvider = new GoogleOidcProvider();

#if UNITY_EDITOR             
        oidc.OidcProvider.ClientData = googleClientDataObjectEditor.clientData;
#elif PLATFORM_ANDROID
        oidc.OidcProvider.ClientData = googleClientDataObject.clientData;
#else
        oidc.OidcProvider.ClientData = googleClientDataObjectEditor.clientData;
#endif
        ServiceManager.RegisterService<OpenIDConnectService>(oidc);
    }

    protected override void UnRegisterServices()
    {
        throw new System.NotImplementedException();
    }
}