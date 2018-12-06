using CybSoftServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CybSoftServices.Interface
{
    public interface IServiceManager
    {
        Operation<ServiceModel> CreateService(ServiceModel model);
        Operation<ServiceModel[]> GetServices();
        Operation<ServiceModel> UpdateService(ServiceModel model);
        Operation<ServiceModel> GetServiceById(int servId);
        Operation DeleteService(int id);
        Operation<List<ServiceModel>> UploadServiceNames(Stream stream, ServiceModel model);



    }
}