using AzureBlobStorage.DAL;
using AzureBlobStorage.ETL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureBlobStorage.BLL
{
    public class AzureBlobStorageBLL
{
        private IConfiguration Configuration;
        public AzureBlobStorageBLL(IConfiguration _configuration )
        {
            Configuration = _configuration;
        }
        

        public async Task<Respuesta<Documento>> GuardarArchivo(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();

            try
            {
                AzureBlobStorageDALL azureBlobStorageDALL = new AzureBlobStorageDALL(Configuration);
                respuesta = await azureBlobStorageDALL.GuardarArchivo(documento);

            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = oex.Message;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }

        public async Task<Respuesta<List<Documento>>> MostrarArchivos(Documento documento)
        {
            Respuesta<List<Documento>> respuesta = new Respuesta<List<Documento>>();

            try
            {
                AzureBlobStorageDALL azureBlobStorageDALL = new AzureBlobStorageDALL(Configuration);
                respuesta = await azureBlobStorageDALL.MostrarArchivos(documento);

            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = oex.Message;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }

        public async Task<Respuesta<DescargarArchivo>> DescargarArchivo(Documento documento)
        {
            Respuesta<DescargarArchivo> respuesta = new Respuesta<DescargarArchivo>();

            try
            {

                AzureBlobStorageDALL azureBlobStorageDALL = new AzureBlobStorageDALL(Configuration);
                respuesta = await azureBlobStorageDALL.DescargarArchivo(documento);

            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = oex.Message;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }

        public async Task<Respuesta<Documento>> EliminarArchivo(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();

            try
            {

                AzureBlobStorageDALL azureBlobStorageDALL = new AzureBlobStorageDALL(Configuration);
                respuesta = await azureBlobStorageDALL.EliminarArchivo(documento);

            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = oex.Message;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }

        public async Task<Respuesta<Documento>> EliminarContenedor(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();

            try
            {

                AzureBlobStorageDALL azureBlobStorageDALL = new AzureBlobStorageDALL(Configuration);
                respuesta = await azureBlobStorageDALL.EliminarContenedor(documento);

            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = oex.Message;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }
    }
}
