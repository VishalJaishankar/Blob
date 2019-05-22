using System;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace blobby
{
    class Program
    {
        static void Main(string[] args)
        {

            var first = new ParseCheck();
            first.CheckParse().GetAwaiter().GetResult();
               
            //if successful
            
        }
    }
}
