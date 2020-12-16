using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureBlobStorage.ETL
{
    public class Documento
    {
        public IFormFile archivo { get; set; }
        public string nombreContenedor { get; set; }
        public string nombreArchivo { get; set; }
        public string tamannoArchivo { get; set; }
        public string modificado { get; set; }
    }
}
