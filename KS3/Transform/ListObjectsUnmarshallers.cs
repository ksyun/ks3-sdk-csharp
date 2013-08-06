using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using KS3.Model;

namespace KS3.Transform
{
    class ListObjectsUnmarshallers : Unmarshaller<ObjectListing, Stream>
    {
        public ObjectListing unmarshall(Stream inputStream)
        {
            //Console.WriteLine((new StreamReader(inputStream)).ReadToEnd());
            //return null;
            
            KS3ObjectSummary curObject = null;
            Owner curOwner = null;
            StringBuilder curText = new StringBuilder();
            Boolean insideCommonPrefixes = false;

            ObjectListing objectListing = new ObjectListing();

            String bucketName = null;
            String lastKey = null;
            String nextMarker = null;
            
            XmlReader xr = XmlReader.Create(new BufferedStream(UnmarshallerUtils.sanitizeXmlDocument(inputStream)));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Contents")) curObject = new KS3ObjectSummary();
                    else if (xr.Name.Equals("CommonPrefixes")) insideCommonPrefixes = true;
                    else if (xr.Name.Equals("Owner")) curOwner = new Owner();
                    
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("Name")) bucketName = curText.ToString();
                    else if (xr.Name.Equals("Delimiter"))
                    {
                        String s = curText.ToString();
                        if (s.Length > 0) objectListing.setDelimiter(s);
                    }
                    else if (xr.Name.Equals("MaxKeys"))
                    {
                        String s = curText.ToString();
                        if (s.Length > 0) objectListing.setMaxKeys(int.Parse(curText.ToString()));
                    }
                    else if (xr.Name.Equals("Prefix"))
                    {
                        if (insideCommonPrefixes) objectListing.getCommonPrefixes().Add(curText.ToString());
                        else
                        {
                            String s = curText.ToString();
                            if (s.Length > 0) objectListing.setPrefix(s);
                        }
                    }
                    else if (xr.Name.Equals("Marker"))
                    {
                        String s = curText.ToString();
                        if (s.Length > 0) objectListing.setMarker(s);
                    }
                    else if (xr.Name.Equals("NextMarker")) nextMarker = curText.ToString();
                    else if (xr.Name.Equals("IsTruncated")) objectListing.setTruncated(Boolean.Parse(curText.ToString()));
                    else if (xr.Name.Equals("Contents"))
                    {
                        curObject.setBucketName(bucketName);
                        objectListing.getObjectSummaries().Add(curObject);
                    }
                    else if (xr.Name.Equals("Owner")) curObject.setOwner(curOwner);
                    else if (xr.Name.Equals("DisplayName")) curOwner.setDisplayName(curText.ToString());
                    else if (xr.Name.Equals("ID")) curOwner.setId(curText.ToString());
                    else if (xr.Name.Equals("LastModified")) curObject.setLastModified(DateTime.Parse(curText.ToString()));
                    else if (xr.Name.Equals("ETag")) curObject.setETag(curText.ToString());
                    else if (xr.Name.Equals("CommonPrefixes")) insideCommonPrefixes = false;
                    else if (xr.Name.Equals("Key"))
                    {
                        lastKey = curText.ToString();
                        curObject.setKey(lastKey);
                    }
                    else if (xr.Name.Equals("Size")) curObject.setSize(long.Parse(curText.ToString()));

                    curText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    curText.Append(xr.Value);
                }
            }
            
            objectListing.setBucketName(bucketName);

            if (nextMarker == null && lastKey != null)
                nextMarker = lastKey;
            objectListing.setNextMarker(nextMarker);
            
            return objectListing;
        } // end of unmarshall
    }
}
