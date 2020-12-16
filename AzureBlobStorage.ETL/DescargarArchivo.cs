using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AzureBlobStorage.ETL
{
    public class DescargarArchivo
    {
        public Stream secuenciaBlob { get; set; }
        public CloudBlockBlob blockBlob { get; set; }

    }
}
