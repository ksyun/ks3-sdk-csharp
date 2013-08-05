using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KS3.Model
{
    public class PutObjectRequest : KS3Request
    {
        /**
         * The name of an existing bucket, to which this request will upload a new
         * object.
         */
        private String bucketName;

        /**
         * The key under which to store the new object.
         */
        private String key;

        /**
         * The file containing the data to be uploaded to KS3. You must either
         * specify a file or a Stream containing the data to be uploaded to KS3.
         */
        private FileInfo file;

        /**
         * The Stream containing the data to be uploaded to KS3. You must
         * either specify a file or an Stream containing the data to be
         * uploaded to KS3.
         */
        private Stream inputStream;

        /**
         * Optional metadata instructing KS3 how to handle the uploaded data
         * (e.g. custom user metadata, hooks for specifying content type, etc.). If
         * you are uploading from an Stream, you <bold>should always</bold>
         * specify metadata with the content size set, otherwise the contents of the
         * Stream will have to be buffered in memory before they can be sent to
         * KS3, which can have very negative performance impacts.
         */
        private ObjectMetadata metadata;


        /**
         * The optional progress listener for receiving updates about object upload
         * status.
         */
        private ProgressListener progressListener;


        /**
         * Constructs a new
         * {@link PutObjectRequest} object to upload a file to the
         * specified bucket and key. After constructing the request,
         * users may optionally specify object metadata or a canned ACL as well.
         */
        public PutObjectRequest(String bucketName, String key, FileInfo file)
        {
            this.bucketName = bucketName;
            this.key = key;
            this.file = file;
        }

        /**
         * Constructs a new
         * {@link PutObjectRequest} object to upload a stream of data to
         * the specified bucket and key. After constructing the request,
         * users may optionally specify object metadata or a canned ACL as well.
         * <p>
         * Content length for the data stream <b>must</b> be
         * specified in the object metadata parameter; Amazon S3 requires it
         * be passed in before the data is uploaded. Failure to specify a content
         * length will cause the entire contents of the input stream to be buffered
         * locally in memory so that the content length can be calculated, which can
         * result in negative performance problems.
         * </p>
         */
        public PutObjectRequest(String bucketName, String key, Stream input, ObjectMetadata metadata)
        {
            this.bucketName = bucketName;
            this.key = key;
            this.inputStream = input;
            this.metadata = metadata;
        }

        /**
         * Gets the name of the existing bucket where this request will
         * upload a new object to.
         */
        public String getBucketName()
        {
            return this.bucketName;
        }

        /**
         * Sets the name of an existing bucket where this request will
         * upload a new object to. 
         */
        public void setBucketName(String bucketName)
        {
            this.bucketName = bucketName;
        }

        /**
         * Gets the key under which to store the new object.
         */
        public String getKey()
        {
            return this.key;
        }

        /**
         * Sets the key under which to store the new object.
         */
        public void setKey(String key)
        {
            this.key = key;
        }

        /**
         * Gets the path and name of the file
         * containing the data to be uploaded to KS3.
         * Either specify a file or an input stream containing the data to be
         * uploaded to KS3; both cannot be specified.
         */
        public FileInfo getFile()
        {
            return this.file;
        }

        /**
         * Sets the path and name of the file
         * containing the data to be uploaded to KS3.
         * Either specify a file or an input stream containing the data to be
         * uploaded to KS3; both cannot be specified.
         */
        public void setFile(FileInfo file)
        {
            this.file = file;
        }

        /**
         * Gets the optional metadata instructing KS3 how to handle the
         * uploaded data (e.g. custom user metadata, hooks for specifying content
         * type, etc.).
         * <p>
         * If uploading from an input stream,
         * <b>always</b> specify metadata with the content size set. Otherwise the
         * contents of the input stream have to be buffered in memory before
         * being sent to KS3. This can cause very negative performance
         * impacts.
         * </p>
         */
        public ObjectMetadata getMetadata()
        {
            return this.metadata;
        }

        /**
         * Sets the optional metadata instructing KS3 how to handle the
         * uploaded data (e.g. custom user metadata, hooks for specifying content
         * type, etc.).
         * <p>
         * If uploading from an input stream,
         * <b>always</b> specify metadata with the content size set. Otherwise the
         * contents of the input stream have to be buffered in memory before
         * being sent to KS3. This can cause very negative performance
         * impacts.
         * </p>
         */
        public void setMetadata(ObjectMetadata metadata)
        {
            this.metadata = metadata;
        }

        /**
         * Gets the input stream containing the data to be uploaded to KS3.
         * The user of this request
         * must either specify a file or an input stream containing the data to be
         * uploaded to KS3; both cannot be specified.
         */
        public Stream getInputStream()
        {
            return this.inputStream;
        }

        /**
         * Sets the input stream containing the data to be uploaded to KS3.
         * Either specify a file or an input stream containing the data to be
         * uploaded to KS3; both cannot be specified.
         */
        public void setInputStream(Stream inputStream)
        {
            this.inputStream = inputStream;
        }

        /**
         * Sets the optional progress listener for receiving updates for object
         * upload status.
         */
        public void setProgressListener(ProgressListener progressListener)
        {
            this.progressListener = progressListener;
        }

        /**
         * Returns the optional progress listener for receiving updates about object
         * upload status.
         */
        public ProgressListener getProgressListener()
        {
            return progressListener;
        }
    }
}
