using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using KS3.Model;

namespace KS3.Transform
{
    public class CannedAccessControlListUnmarshaller : Unmarshaller<CannedAccessControlList, Stream>
    {
        private CannedAccessControlList cacl;
        private Owner bucketOwner;
        private String cannedAclHeader;
        private StringBuilder curText;

        public CannedAccessControlListUnmarshaller()
        {
            this.cacl = new CannedAccessControlList();
            this.curText = new StringBuilder();
        }

        public CannedAccessControlList unmarshall(Stream inputStream)
        {
            //Console.WriteLine((new StreamReader(inputStream)).ReadToEnd());

            XmlReader xr = XmlReader.Create(new BufferedStream(UnmarshallerUtils.sanitizeXmlDocument(inputStream)));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Owner")) bucketOwner = new Owner();
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("DisplayName")) bucketOwner.setDisplayName(curText.ToString());
                    else if (xr.Name.Equals("ID")) bucketOwner.setId(curText.ToString());
                    else if (xr.Name.Equals("Grant"))
                    {
                        cannedAclHeader = curText.ToString();
                        cacl.setCannedAclHeader(cannedAclHeader);
                    }
                    else if (xr.Name.Equals("Owner")) cacl.setOwner(bucketOwner);
                    curText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    curText.Append(xr.Value);
                }

            }

            return cacl;
        }
    }
}
