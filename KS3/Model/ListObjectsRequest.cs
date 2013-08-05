﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class ListObjectsRequest : KS3Request
    {
        /** The name of the KS3 bucket to list. */
        private String bucketName;

        /**
         * Optional parameter restricting the response to keys which begin with the
         * specified prefix. You can use prefixes to separate a bucket into
         * different sets of keys in a way similar to how a file system uses
         * folders.
         */
        private String prefix;

        /**
         * Optional parameter indicating where in the bucket to begin listing. The
         * list will only include keys that occur lexicographically after the
         * marker. This enables pagination; to get the next page of results use the
         * current value from {@link ObjectListing#getNextMarker()} as the marker
         * for the next request to list objects.
         */
        private String marker;

        /**
         * Optional parameter that causes keys that contain the same string between
         * the prefix and the first occurrence of the delimiter to be rolled up into
         * a single result element in the
         * {@link ObjectListing#getCommonPrefixes()} list. These rolled-up keys
         * are not returned elsewhere in the response. The most commonly used
         * delimiter is "/", which simulates a hierarchical organization similar to
         * a file system directory structure.
         */
        private String delimiter;

        /**
         * Optional parameter indicating the maximum number of keys to include in
         * the response. KS3 might return fewer than this, but will not return
         * more. Even if maxKeys is not specified, KS3 will limit the number
         * of results in the response.
         */
        private int? maxKeys;



        public ListObjectsRequest() {}

        public ListObjectsRequest(String bucketName, String prefix, String marker, String delimiter, int? maxKeys)
        {
            this.bucketName = bucketName;
            this.prefix = prefix;
            this.marker = marker;
            this.delimiter = delimiter;
            this.maxKeys = maxKeys;
        }

        public String getBucketName()
        {
            return this.bucketName;
        }

        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

        public String getPrefix()
        {
            return prefix;
        }

        public void setPrefix(String prefix)
        {
            this.prefix = prefix;
        }

        public String getMarker()
        {
            return marker;
        }

        public void setMarker(String marker)
        {
            this.marker = marker;
        }

        public String getDelimiter()
        {
            return this.delimiter;
        }

        public void setDelimiter(String delimiter)
        {
            this.delimiter = delimiter;
        }

        public int? getMaxKeys()
        {
            return this.maxKeys;
        }

        public void setMaxKeys(int? maxKeys)
        {
            this.maxKeys = maxKeys;
        }
    }
}
