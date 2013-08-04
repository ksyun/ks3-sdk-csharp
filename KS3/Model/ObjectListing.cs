using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class ObjectListing
    {
        /** A list of summary information describing the objects stored in the bucket */
        private List<KS3ObjectSummary> objectSummaries = new List<KS3ObjectSummary>();

        /**
         * A list of the common prefixes included in this object listing - common
         * prefixes will only be populated for requests that specified a delimiter
         */
        private List<String> commonPrefixes = new List<String>();

        /** The name of the KS3 bucket containing the listed objects */
        private String bucketName;

        /**
         * The marker to use in order to request the next page of results - only
         * populated if the isTruncated member indicates that this object listing is
         * truncated
         */
        private String nextMarker;

        /**
         * Indicates if this is a complete listing, or if the caller needs to make
         * additional requests to KS3 to see the full object listing for an KS3
         * bucket
         */
        private Boolean truncated;

        /**
         * The prefix parameter originally specified by the caller when this object
         * listing was returned
         */
        private String prefix;


        /**
         * The marker parameter originally specified by the caller when this object
         * listing was returned
         */
        private String marker;


        /**
         * The maxKeys parameter originally specified by the caller when this object
         * listing was returned
         */
        private int? maxKeys;

        /**
         * The delimiter parameter originally specified by the caller when this
         * object listing was returned
         */
        private String delimiter;


        public List<KS3ObjectSummary> getObjectSummaries()
        {
            return this.objectSummaries;
        }

        public List<String> getCommonPrefixes()
        {
            return this.commonPrefixes;
        }

        public void setCommonPrefixes(List<String> commonPrefixes)
        {
            this.commonPrefixes = commonPrefixes;
        }

        /**
         * Gets the marker to use in the next <code>listObjects</code>
         * request in order to see
         * the next page of results. 
         * If an object listing is not truncated, this
         * method will return <code>null</code>. For
         * truncated requests, this value is equal to the greatest
         * lexicographical value of the object keys and common prefixes included
         * in this listing.
         */
        public String getNextMarker()
        {
            return this.nextMarker;
        }

        /**
         * For internal use only. Sets the marker to use in the
         * next list objects request in order to see the next page of results for a
         * truncated object listing.
         */
        public void setNextMarker(String nextMarker)
        {
            this.nextMarker = nextMarker;
        }

        public String getBucketName()
        {
            return this.bucketName;
        }

        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Gets the prefix parameter originally used to request this object listing, or
         * <code>null</code> if no prefix was specified.
         * All objects and common prefixes included
         * in this object listing start with the specified prefix.
         */
        public String getPrefix()
        {
            return this.prefix;
        }

        public void setPrefix(String prefix)
        {
            this.prefix = prefix;
        }

        /**
         * Gets the marker parameter originally used to request this object listing, or
         * <code>null</code> if no marker was specified. 
         * If specified, all objects and common
         * prefixes included in this object listing will occur
         * alphabetically after the specified marker.
         */
        public String getMarker()
        {
            return marker;
        }

        public void setMarker(String marker)
        {
            this.marker = marker;
        }

        /**
         * Gets the <code>maxKeys</code> parameter originally used to request this object
         * listing, or the default <code>maxKeys</code> value provided by KS3 if the
         * requester didn't specify a value. The <code>maxKeys</code> parameter limits the
         * combined number of objects and common prefixes included in this object
         * listing. An object listing will never contain more objects plus common
         * prefixes than indicated by the <code>maxKeys</code>, but can of course contain less.
         */
        public int? getMaxKeys()
        {
            return this.maxKeys;
        }

        public void setMaxKeys(int? maxKeys)
        {
            this.maxKeys = maxKeys;
        }

        /**
         * Gets the delimiter parameter originally used to request this object
         * listing, or <code>null</code> if no delimiter specified. 
         * <p>
         * The delimiter value allows
         * callers to condense S3 keys into common prefix listings. For example, if
         * a caller specifies a delimiter of "/" (a common used value for
         * delimiter), any keys that contain a common prefix between the start
         * of the key and the first occurrence of "/" will not be included in the
         * list of object summaries. Instead, the common prefixes list will have
         * one entry for the common prefix.
         * </p>
         */
        public String getDelimiter()
        {
            return this.delimiter;
        }

        public void setDelimiter(String delimiter)
        {
            this.delimiter = delimiter;
        }

        /**
         * Gets whether or not this object listing is complete.
         * 
         * @return The value <code>true</code> if the object listing is <b>not complete</b>.
         *         Returns the value <code>false</code> if otherwise.  
         *         When returning <code>true</code>,
         *         additional calls to KS3 may be needed in order to
         *         obtain more results.
         */
        public Boolean isTruncated()
        {
            return this.truncated;
        }

        public void setTruncated(Boolean truncated)
        {
            this.truncated = truncated;
        }

        public override String ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ObjectListing [bucketName=" + this.bucketName);
            if (this.delimiter != null) builder.Append(", delimiter=" + this.delimiter);
            if (this.maxKeys != null) builder.Append(", maxKeys=" + this.maxKeys);
            if (this.prefix != null) builder.Append(", prefix=" + this.prefix);
            if (this.marker != null) builder.Append(", marker=" + this.marker);
            if (this.nextMarker != null) builder.Append(", nextMarker=" + this.nextMarker);
            builder.Append(", isTruncated=" + this.truncated + "]");

            foreach (KS3ObjectSummary objectSummary in this.objectSummaries)
                builder.Append("\nObject:\n" + objectSummary.ToString());
            foreach (String s in this.commonPrefixes)
                builder.Append("\nCommonPrefix:\n" + s);

            return builder.ToString();
        }
    }
}
