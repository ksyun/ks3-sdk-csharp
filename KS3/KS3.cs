﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3.Model;

namespace KS3
{
    /**
     * Provides an interface for accessing the KS3.
     */
    public interface KS3
    {
        /**
         * Overrides the default endpoint for this client.
         */
        void setEndpoint(String endpoint);

        /**
         * Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
         */
        IList<Bucket> listBuckets();

        /**
         * Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
         */
        IList<Bucket> listBuckets(ListBucketsRequest listBucketRequest);

        /**
         * Deletes the specified bucket. 
         */
        void deleteBucket(String bucketName);

        /**
         * Deletes the specified bucket. 
         */
        void deleteBucket(DeleteBucketRequest deleteBucketRequest);

        /**
         * Gets the AccessControlList (ACL) for the specified KS3 bucket.
         */
        AccessControlList getBucketAcl(String bucketName);

        /**
         * Gets the AccessControlList (ACL) for the specified KS3 bucket.
         */
        AccessControlList getBucketAcl(GetBucketAclRequest getBucketAclRequest);

        /**
         * Creates a new KS3 bucket. 
         */
        Bucket createBucket(String bucketName);

        /**
         * Creates a new KS3 bucket. 
         */
        Bucket createBucket(CreateBucketRequest createBucketRequest);

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        void setBucketAcl(String bucketName, AccessControlList acl);

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        void setBucketAcl(String bucketName, CannedAccessControlList cannedAcl);

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        void setBucketAcl(SetBucketAclRequest setBucketAclRequset);

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        ObjectListing listObjects(String bucketName);

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        ObjectListing listObjects(String bucketName, String prefix);

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        ObjectListing listObjects(ListObjectsRequest listObjectRequest);

        /**
         * Deletes the specified object in the specified bucket.
         */
        void deleteObject(String bucketName, String key);

        /**
         * Deletes the specified object in the specified bucket.
         */
        void deleteObject(DeleteObjectRequest deleteObjectRequest);

        /**
         * Gets the object stored in KS3 under the specified bucket and key.
         */
        KS3Object getObject(String bucketName, String key);

        /**
         * Gets the object stored in KS3 under the specified bucket and key, and saves the object contents to the specified file.
         */
        KS3Object getObject(String bucketName, String key, FileInfo destinationFile);

        /**
         * Gets the object stored in KS3 under the specified bucket and key.
         */
        KS3Object getObject(GetObjectRequest getObjectRequest);

        /**
         * Gets the metadata for the specified KS3 object without actually fetching the object itself.
         */
        ObjectMetadata getObjectMetadata(String bucketName, String key);

        /**
         * Gets the metadata for the specified KS3 object without actually fetching the object itself.
         */
        ObjectMetadata getObjectMetadata(GetObjectMetadataRequest getObjectMetadataRequest);

        /**
         * Uploads the specified file to KS3 under the specified bucket and key name.
         */
        PutObjectResult putObject(String bucketName, String key, FileInfo file);

        /**
         * Uploads the specified input stream and object metadata to KS3 under the specified bucket and key name. 
         */
        PutObjectResult putObject(String bucketName, String key, Stream input, ObjectMetadata metadata);

        /**
         * Uploads a new object to the specified KS3 bucket.
         */
        PutObjectResult putObject(PutObjectRequest putObjectRequest);

        /**
         * Gets the AccessControlList (ACL) for the specified object in KS3.
         */
        AccessControlList getObjectAcl(String bucketName, String key);

        /**
         * Gets the AccessControlList (ACL) for the specified object in KS3.
         */
        AccessControlList getObjectAcl(GetObjectAclRequest getObjectAclRequest);

        /**
         * Sets the AccessControlList for the specified object in KS3.
         */
        void setObjectAcl(String bucketName, String key, AccessControlList acl);

        /**
         * Sets the AccessControlList for the specified object in KS3.
         */
        void setObjectAcl(String bucketName, String key, CannedAccessControlList cannedAcl);

        /**
         * Sets the AccessControlList for the specified object in KS3.
         */
        void setObjectAcl(SetObjectAclRequest setObjectRequestAcl);
    }
}
