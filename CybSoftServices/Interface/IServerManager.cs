using CybSoftServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CybSoftServices.Interface
{
    public interface IServerManager
    {
        bool CreateServer(ServerModel model);
        Operation<ServerModel[]> GetServers();
        Operation<ServerModel> UpdateServer(ServerModel model);
        Operation<ServerModel> GetServerById(int serverId);
        Operation DeleteServer(int id);
        Operation<List<ServerModel>> UploadServerNames(Stream stream, ServerModel model);
    }
}