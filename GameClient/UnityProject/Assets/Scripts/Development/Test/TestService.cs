using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TIZSoft.Services;
using TIZSoft.UnityHTTP.Client;
using TIZSoft.UnknownGame.Common.API;
using TIZSoft;

public class TestService : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ClientHttpManager httpManager = new ClientHttpManager();
        ClientHostConfigure.Settings settings = new ClientHostConfigure.Settings {
            Hosts = new List<ClientHostConfigure.Settings.Entry> {
                new ClientHostConfigure.Settings.Entry { HostId = "suck_hostid", Host = "http://app.domain.com" },
                new ClientHostConfigure.Settings.Entry { HostId = "suck_hostid_2", Host = "http://app.domain.com" }
            },
            DefaultHostId = "suck_hostid"
        };
        ClientHostConfigure hostManager = new ClientHostConfigure(settings);
        ClientHttpSender webService = new ClientHttpSender(httpManager, hostManager);

        ClientHTTPNetwork clientHTTPNetwork = new ClientHTTPNetwork();
        //GameServices tempService = new GameServices(webService, Constants.APPLICATION_PROJECT_NAME, clientHTTPNetwork);
        GameApiServices tempService = new GameApiServices(webService, string.Empty, clientHTTPNetwork);

        clientHTTPNetwork.AddServer("suck_groupname", ServerType.GameHost, "http://127.0.0.1:4050/");

        tempService.CallAPI<UserRequest>( API_METHOD.HTTP_GET, new UserRequest { m=1 }, OnResponse);
    }

    public void OnResponse(ClientHttpRequest request)
    {
        Debug.Log("request: [" + request.GetText() + "]");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}