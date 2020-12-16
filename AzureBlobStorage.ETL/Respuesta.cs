using System;

namespace AzureBlobStorage.ETL
{
    public class Respuesta<T>
    {
        public bool hayError { get; set; }
        public string mensaje { get; set; }
        public T objetoRespuesta { get; set; }
    }
}
