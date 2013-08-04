using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3.Model;
using KS3.Internal;
using System.Xml;

namespace KS3.Transform
{
    public class ListBucketsUnmarshaller : Unmarshaller<List<Bucket>, Stream>
    {
        private Owner bucketsOwner = null;
        private Bucket curBucket = null;
        private StringBuilder curText = null;

        private List<Bucket> buckets = null;

        public ListBucketsUnmarshaller()
        {
            this.buckets = new List<Bucket>();
            this.curText = new StringBuilder();
        }

        public List<Bucket> unmarshall(Stream inputStream)
        {
            //Console.WriteLine((new StreamReader(inputStream)).ReadToEnd());
            XmlReader xr = XmlReader.Create(new BufferedStream(UnmarshallerUtils.sanitizeXmlDocument(inputStream)));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Owner")) bucketsOwner = new Owner();
                    else if (xr.Name.Equals("Bucket")) curBucket = new Bucket();
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("DisplayName")) bucketsOwner.setDisplayName(curText.ToString());
                    else if (xr.Name.Equals("ID")) bucketsOwner.setId(curText.ToString());
                    else if (xr.Name.Equals("CreationDate")) curBucket.setCreationDate(DateTime.Parse(curText.ToString()));
                    else if (xr.Name.Equals("Name")) curBucket.setName(curText.ToString());
                    else if (xr.Name.Equals("Bucket"))
                    {
                        curBucket.setOwner(bucketsOwner);
                        buckets.Add(curBucket);
                    }
                    curText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    curText.Append(xr.Value);
                }

            }
            return buckets;
        }
    }
}
