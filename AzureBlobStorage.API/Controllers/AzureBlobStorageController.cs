using AzureBlobStorage.BLL;
using AzureBlobStorage.ETL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureBlobStorage.API.Controllers
{
    public class AzureBlobStorageController : Controller
    {
        private IConfiguration Configuration;
        public AzureBlobStorageController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GuardarArchivo(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();
            AzureBlobStorageBLL azureBlobStorageBLL = new AzureBlobStorageBLL(Configuration);            
           respuesta = await azureBlobStorageBLL.GuardarArchivo(documento);
            return Ok(respuesta);

        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> MostrarArchivos(Documento documento)
        {
            Respuesta<List<Documento>> respuesta = new Respuesta<List<Documento>>();
            AzureBlobStorageBLL azureBlobStorageBLL = new AzureBlobStorageBLL(Configuration);
            respuesta = await azureBlobStorageBLL.MostrarArchivos(documento);
            return Ok(respuesta);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> DescargarArchivo(Documento documento)
        {
            Respuesta<DescargarArchivo> respuesta = new Respuesta<DescargarArchivo>();
            AzureBlobStorageBLL azureBlobStorageBLL = new AzureBlobStorageBLL(Configuration);
            respuesta =  await azureBlobStorageBLL.DescargarArchivo(documento);

            return File(respuesta.objetoRespuesta.secuenciaBlob, respuesta.objetoRespuesta.blockBlob.Properties.ContentType, respuesta.objetoRespuesta.blockBlob.Name);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> EliminarArchivo(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();
            AzureBlobStorageBLL azureBlobStorageBLL = new AzureBlobStorageBLL(Configuration);
            respuesta = await azureBlobStorageBLL.EliminarArchivo(documento);

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> EliminarContenedor(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();
            AzureBlobStorageBLL azureBlobStorageBLL = new AzureBlobStorageBLL(Configuration);
            respuesta = await azureBlobStorageBLL.EliminarContenedor(documento);

            return Ok(respuesta);
        }

    }
}
