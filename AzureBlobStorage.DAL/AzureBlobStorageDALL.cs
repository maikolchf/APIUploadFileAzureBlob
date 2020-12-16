using AzureBlobStorage.ETL;
using AzureBlobStorage.LIB;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureBlobStorage.DAL
{
    public class AzureBlobStorageDALL
    {
        private IConfiguration Configuration;

        public AzureBlobStorageDALL(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public async Task<Respuesta<Documento>> GuardarArchivo(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();
            try
            {
                var coneccionBlob = this.Configuration.GetSection(Constantes.conexion.seccion)[Constantes.conexion.url];
                byte[] datosDocumento;

                CloudStorageAccount cuentaAzureBlob = CloudStorageAccount.Parse(coneccionBlob);
                CloudBlobClient blob = cuentaAzureBlob.CreateCloudBlobClient();

                CloudBlobContainer contenedor = blob.GetContainerReference(documento.nombreContenedor);
                await contenedor.CreateIfNotExistsAsync();

                BlobContainerPermissions permisos = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };

                string nombreArchivo = documento.archivo.FileName;
                await contenedor.SetPermissionsAsync(permisos);
                await using (var target = new MemoryStream())
                {
                    documento.archivo.CopyTo(target);
                    datosDocumento = target.ToArray();
                }

                CloudBlockBlob bloqueBlob = contenedor.GetBlockBlobReference(nombreArchivo);
                await bloqueBlob.UploadFromByteArrayAsync(datosDocumento, 0, datosDocumento.Length);

                respuesta.hayError = false;
                respuesta.mensaje = Mensajes.MsjGuardado;

            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = Mensajes.MsjErrorNoControlado;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }
        public async Task<Respuesta<List<Documento>>> MostrarArchivos(Documento documento)
        {
            Respuesta<List<Documento>> respuesta = new Respuesta<List<Documento>>();
            try
            {
                List<Documento> lstDocumentos = new List<Documento>();
                var coneccionBlob = this.Configuration.GetSection(Constantes.conexion.seccion)[Constantes.conexion.url];
                CloudStorageAccount cuentaAzureBlob = CloudStorageAccount.Parse(coneccionBlob);

                CloudBlobClient blob = cuentaAzureBlob.CreateCloudBlobClient();

                CloudBlobContainer contenedor = blob.GetContainerReference(documento.nombreContenedor);
                CloudBlobDirectory direcctorio = contenedor.GetDirectoryReference(documento.nombreContenedor);


                BlobResultSegment resultSegment = await contenedor.ListBlobsSegmentedAsync(string.Empty,
                    true, BlobListingDetails.Metadata, 100, null, null, null);

                foreach (var item in resultSegment.Results)
                {
                    var Almablob = (CloudBlob)item;
                    lstDocumentos.Add(new Documento()
                    {
                        nombreArchivo = Almablob.Name,
                        tamannoArchivo = Math.Round((Almablob.Properties.Length / 1024f) / 1024f, 2).ToString(),
                        modificado = DateTime.Parse(Almablob.Properties.LastModified.ToString()).ToLocalTime().ToString(),
                        nombreContenedor = documento.nombreContenedor
                    });
                }

                respuesta.hayError = false;
                respuesta.objetoRespuesta = lstDocumentos;
            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = Mensajes.MsjErrorNoControlado;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }
        public async Task<Respuesta<DescargarArchivo>> DescargarArchivo(Documento documento)
        {
            Respuesta<DescargarArchivo> respuesta = new Respuesta<DescargarArchivo>();
            try
            {
                DescargarArchivo descargar = new DescargarArchivo();
                CloudBlockBlob blockBlob;
                await using (MemoryStream memoryStream = new MemoryStream())
                {
                    var coneccionBlob = this.Configuration.GetSection(Constantes.conexion.seccion)[Constantes.conexion.url];
                    CloudStorageAccount cuentaAzureBlob = CloudStorageAccount.Parse(coneccionBlob);
                    CloudBlobClient blob = cuentaAzureBlob.CreateCloudBlobClient();
                    CloudBlobContainer contenedor = blob.GetContainerReference(documento.nombreContenedor);
                    blockBlob = contenedor.GetBlockBlobReference(documento.nombreArchivo);
                    await blockBlob.DownloadToStreamAsync(memoryStream);
                }

                Stream blobStream = blockBlob.OpenReadAsync().Result;


                descargar = new DescargarArchivo()
                {
                    secuenciaBlob = blobStream,
                    blockBlob = blockBlob
                };

                respuesta.hayError = false;
                respuesta.objetoRespuesta = descargar;

            }
            catch (Exception oex)
            {
                respuesta.hayError = true;
                respuesta.mensaje = Mensajes.MsjErrorNoControlado;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }
        public async Task<Respuesta<Documento>> EliminarArchivo(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();
            try
            {
                CloudBlockBlob blockBlob;
                var coneccionBlob = this.Configuration.GetSection(Constantes.conexion.seccion)[Constantes.conexion.url];
                CloudStorageAccount cuentaAzureBlob = CloudStorageAccount.Parse(coneccionBlob);
                CloudBlobClient blob = cuentaAzureBlob.CreateCloudBlobClient();
                CloudBlobContainer contenedor = blob.GetContainerReference(documento.nombreContenedor);
                blockBlob = contenedor.GetBlockBlobReference(documento.nombreArchivo);
                var eliminado = await blockBlob.DeleteIfExistsAsync();

                if (eliminado)
                {
                    respuesta.hayError = false;
                    respuesta.mensaje = Mensajes.MsjEliminado;

                }

            }
            catch (Exception oEx)
            {
                respuesta.hayError = true;
                respuesta.mensaje = Mensajes.MsjErrorNoControlado;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }

        public async Task<Respuesta<Documento>> EliminarContenedor(Documento documento)
        {
            Respuesta<Documento> respuesta = new Respuesta<Documento>();

            try
            {
                var coneccionBlob = this.Configuration.GetSection(Constantes.conexion.seccion)[Constantes.conexion.url];
                CloudStorageAccount cuentaAzureBlob = CloudStorageAccount.Parse(coneccionBlob);
                CloudBlobClient blob = cuentaAzureBlob.CreateCloudBlobClient();
                CloudBlobContainer contenedor = blob.GetContainerReference(documento.nombreContenedor);

                await contenedor.DeleteAsync();
                respuesta.hayError = false;
                respuesta.mensaje = Mensajes.MsjEliminarContenedor;


            }
            catch (Exception oEx)
            {
                respuesta.hayError = true;
                respuesta.mensaje = oEx.Message;
                respuesta.objetoRespuesta = null;
            }

            return respuesta;
        }

    }
}
