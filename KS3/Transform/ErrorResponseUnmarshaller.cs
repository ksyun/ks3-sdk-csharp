using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using KS3.KS3Exception;

namespace KS3.Transform
{
    public class ErrorResponseUnmarshaller : Unmarshaller<ServiceException, Stream>
    {
        public ServiceException unmarshall(Stream inputStream)
        {
            //Console.WriteLine((new StreamReader(inputStream)).ReadToEnd());

            String requestId = null;
            String errorCode = null;
            String message = "Unknow error, no response body.";
            ServiceException serviceException = null;

            StringBuilder curText = new StringBuilder();
            XmlReader xr = XmlReader.Create(new BufferedStream(UnmarshallerUtils.sanitizeXmlDocument(inputStream)));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("Message")) message = curText.ToString();
                    else if (xr.Name.Equals("Code")) errorCode = curText.ToString();
                    else if (xr.Name.Equals("RequestId")) requestId = curText.ToString();

                    curText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    curText.Append(xr.Value);
                }

            }

            serviceException = new ServiceException(message);
            serviceException.setErrorCode(errorCode);
            serviceException.setRequestId(requestId);

            return serviceException;
        }
    }
}
