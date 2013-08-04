using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3.Internal;

namespace KS3.Transform
{
    public static class UnmarshallerUtils
    {
        public static Stream sanitizeXmlDocument(Stream inputStream)
        {
            Stream sanitizedInputStream = null;
            StreamReader sr = new StreamReader(new BufferedStream(inputStream), Constants.DEFAULT_ENCODING);

            /*
             * Replace any carriage return (\r) characters with explicit XML
             * character entities, to prevent the SAX parser from
             * misinterpreting 0x0D characters as 0x0A and being unable to
             * parse the XML.
             */
            String listingDoc = sr.ReadToEnd().Replace("\r", "&#013;");

            sanitizedInputStream = new MemoryStream(Encoding.UTF8.GetBytes(listingDoc));
            return sanitizedInputStream;
        }
    }
}
