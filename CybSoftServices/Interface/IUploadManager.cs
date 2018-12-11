using CybSoftServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CybSoftServices.Interface
{
    public interface IUploadManager
    {
        Operation<List<ServiceModel>> UploadServiceNames(Stream stream, ServiceModel model);
        Operation<List<ServerModel>> UploadServerNames(Stream stream, ServerModel model);

    }
}