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
        private KS3ObjectSummary curObject = null;
        private Owner curOwner = null;
        private StringBuilder curText = null;
        private Boolean insideCommonPrefixes = false;

        private ObjectListing objectListing = new ObjectListing();

        private String bucketName = null;
        private String lastKey = null;
        private String nextMarker = null;

        public ListObjectsUnmarshallers()
        { 
            this.curText = new StringBuilder();
        }

        public ObjectListing unmarshall(Stream inputStream)
        {
            //Console.WriteLine((new StreamReader(inputStream)).ReadToEnd());
            //return null;
            XmlReader xr = XmlReader.Create(new BufferedStream(UnmarshallerUtils.sanitizeXmlDocument(inputStream)));
            while (xr.Read())
            {
                if (xr.NodeType.Equals(XmlNodeType.Element))
                {
                    if (xr.Name.Equals("Contents")) this.curObject = new KS3ObjectSummary();
                    else if (xr.Name.Equals("CommonPrefixes")) this.insideCommonPrefixes = true;
                    else if (xr.Name.Equals("Owner")) this.curOwner = new Owner();
                    
                }
                else if (xr.NodeType.Equals(XmlNodeType.EndElement))
                {
                    if (xr.Name.Equals("Name")) this.bucketName = this.curText.ToString();
                    else if (xr.Name.Equals("Delimiter"))
                    {
                        String s = this.curText.ToString();
                        if (s.Length > 0) this.objectListing.setDelimiter(s);
                    }
                    else if (xr.Name.Equals("MaxKeys"))
                    {
                        String s = this.curText.ToString();
                        if (s.Length > 0) this.objectListing.setMaxKeys(int.Parse(this.curText.ToString()));
                    }
                    else if (xr.Name.Equals("Prefix"))
                    {
                        if (this.insideCommonPrefixes) this.objectListing.getCommonPrefixes().Add(curText.ToString());
                        else
                        {
                            String s = this.curText.ToString();
                            if (s.Length > 0) this.objectListing.setPrefix(s);
                        }
                    }
                    else if (xr.Name.Equals("Marker"))
                    {
                        String s = this.curText.ToString();
                        if (s.Length > 0) this.objectListing.setMarker(s);
                    }
                    else if (xr.Name.Equals("NextMarker")) this.nextMarker = this.curText.ToString();
                    else if (xr.Name.Equals("IsTruncated")) this.objectListing.setTruncated(Boolean.Parse(this.curText.ToString()));
                    else if (xr.Name.Equals("Contents"))
                    {
                        curObject.setBucketName(this.bucketName);
                        this.objectListing.getObjectSummaries().Add(curObject);
                    }
                    else if (xr.Name.Equals("Owner")) this.curObject.setOwner(this.curOwner);
                    else if (xr.Name.Equals("DisplayName")) this.curOwner.setDisplayName(this.curText.ToString());
                    else if (xr.Name.Equals("ID")) this.curOwner.setId(this.curText.ToString());
                    else if (xr.Name.Equals("LastModified")) this.curObject.setLastModified(DateTime.Parse(this.curText.ToString()));
                    else if (xr.Name.Equals("ETag")) this.curObject.setETag(this.curText.ToString());
                    else if (xr.Name.Equals("CommonPrefixes")) this.insideCommonPrefixes = false;
                    else if (xr.Name.Equals("Key"))
                    {
                        this.lastKey = this.curText.ToString();
                        this.curObject.setKey(this.lastKey);
                    }
                    else if (xr.Name.Equals("Size")) this.curObject.setSize(long.Parse(this.curText.ToString()));

                    this.curText.Clear();
                }
                else if (xr.NodeType.Equals(XmlNodeType.Text))
                {
                    this.curText.Append(xr.Value);
                }
            }
            
            this.objectListing.setBucketName(this.bucketName);
            this.objectListing.setNextMarker(this.getMarkerForNextListing());
            return this.objectListing;
        }

        /**
         * If the listing is truncated this method will return the marker that
         * should be used in subsequent bucket list calls to complete the
         * listing.
         */
        public String getMarkerForNextListing()
        {
            if (nextMarker != null) return nextMarker;
            if (lastKey != null) return lastKey;
            return null;
        }
    }
}
