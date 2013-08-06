using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

using KS3.Model;
using KS3.Auth;
using KS3.Http;
using KS3.Internal;
using KS3.Transform;

namespace KS3
{
    public class KS3Client : KS3
    {
        private XmlResponseHandler<Type> voidResponseHandler = new XmlResponseHandler<Type>(null);

        /** KS3 credentials. */
        private KS3Credentials ks3Credentials;

        /** The service endpoint to which this client will send requests. */
        private Uri endpoint;

        /** The client configuration */
        private ClientConfiguration clientConfiguration;

        /** Low level client for sending requests to KS3. */
        private KS3HttpClient client;

        /** Optional offset (in seconds) to use when signing requests */
        private int timeOffset;

        public KS3Client(KS3Credentials ks3Credentials)
            : this(ks3Credentials, new ClientConfiguration()) { }


        /**
         * Constructs a new KS3Client object using the specified configuration.
         */
        public KS3Client(KS3Credentials ks3Credentials, ClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
            this.client = new KS3HttpClient(clientConfiguration);
            this.ks3Credentials = ks3Credentials;

            this.init();
        }

        private void init()
        {
            this.setEndpoint(Constants.KS3_HOSTNAME);
        }

        public void setEndpoint(String endpoint)
        {
            if (!endpoint.Contains("://")) endpoint = clientConfiguration.getProtocol() + "://" + endpoint;
            this.endpoint = new Uri(endpoint);
        }

        public void setConfiguration(ClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
            client = new KS3HttpClient(clientConfiguration);
        }

        /**
         * Sets the optional value for time offset for this client.  This
         * value will be applied to all requests processed through this client.
         * Value is in seconds, positive values imply the current clock is "fast",
         * negative values imply clock is slow.
         */
        public void setTimeOffset(int timeOffset)
        {
            this.timeOffset = timeOffset;
        }

        /**
         * Returns the optional value for time offset for this client.  This
         * value will be applied to all requests processed through this client.
         * Value is in seconds, positive values imply the current clock is "fast",
         * negative values imply clock is slow.
         */
        public int getTimeOffset()
        {
            return this.timeOffset;
        }

        /**
         * Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
         */
        public List<Bucket> listBuckets()
        {
            return this.listBuckets(new ListBucketsRequest());
        }

        /**
         * Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
         */
        public List<Bucket> listBuckets(ListBucketsRequest listBucketRequest)
        {
            Request<ListBucketsRequest> request = this.createRequest(null, null, listBucketRequest, HttpMethodName.GET);
            return this.invoke(request, new ListBucketsUnmarshaller(), null, null);
        }

        /**
         * Deletes the specified bucket. 
         */
        public void deleteBucket(String bucketName)
        {
            this.deleteBucket(new DeleteBucketRequest(bucketName));
        }

        /**
         * Deletes the specified bucket. 
         */
        public void deleteBucket(DeleteBucketRequest deleteBucketRequest)
        {
            String bucketName = deleteBucketRequest.getBucketName();

            Request<DeleteBucketRequest> request = this.createRequest(bucketName, null, deleteBucketRequest, HttpMethodName.DELETE);
            this.invoke(request, voidResponseHandler, bucketName, null);
        }

        /**
         * Gets the AccessControlList (ACL) for the specified KS3 bucket.
         */
        public AccessControlList getBucketAcl(String bucketName)
        {
            return getBucketAcl(new GetBucketAclRequest(bucketName));
        }

        /**
         * Gets the AccessControlList (ACL) for the specified KS3 bucket.
         */
        public AccessControlList getBucketAcl(GetBucketAclRequest getBucketAclRequest)
        {
            String bucketName = getBucketAclRequest.getBucketName();

            Request<GetBucketAclRequest> request = createRequest(bucketName, null, getBucketAclRequest, HttpMethodName.GET);
            request.addParameter("acl", null);

            return invoke(request, new AccessControlListUnmarshaller(), bucketName, null);
        }

        /**
         * Creates a new KS3 bucket. 
         */
        public Bucket createBucket(String bucketName)
        {
            return createBucket(new CreateBucketRequest(bucketName));
        }

        /**
         * Creates a new KS3 bucket. 
         */
        public Bucket createBucket(CreateBucketRequest createBucketRequest)
        {
            String bucketName = createBucketRequest.getBucketName();

            Request<CreateBucketRequest> request = createRequest(bucketName, null, createBucketRequest, HttpMethodName.PUT);
            request.getHeaders()[Headers.CONTENT_LENGTH] = "0";
            
            if (createBucketRequest.getCannedAcl() != null)
                request.addHeader(Headers.KS3_CANNED_ACL, createBucketRequest.getCannedAcl().getCannedAclHeader());

            this.invoke(request, voidResponseHandler, bucketName, null);

            return new Bucket(bucketName);
        }

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        public void setBucketAcl(String bucketName, AccessControlList acl)
        {
            this.setBucketAcl(new SetBucketAclRequest(bucketName, acl));
        }

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        public void setBucketAcl(String bucketName, CannedAccessControlList cannedAcl)
        {
            this.setBucketAcl(new SetBucketAclRequest(bucketName, cannedAcl));
        }

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        public void setBucketAcl(SetBucketAclRequest setBucketAclRequest)
        {
            String bucketName = setBucketAclRequest.getBucketName();
            Request<SetBucketAclRequest> request = this.createRequest(bucketName, null, setBucketAclRequest, HttpMethodName.PUT);

            if (setBucketAclRequest.getCannedAcl().getCannedAclHeader() != null)
                request.addHeader(Headers.KS3_CANNED_ACL, setBucketAclRequest.getCannedAcl().getCannedAclHeader());
            request.getHeaders()[Headers.CONTENT_LENGTH] = "0";

            this.invoke(request, this.voidResponseHandler, bucketName, null);
        }

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        public ObjectListing listObjects(String bucketName)
        {
            return this.listObjects(new ListObjectsRequest(bucketName, null, null, null, null));
        }

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        public ObjectListing listObjects(String bucketName, String prefix)
        {
            return this.listObjects(new ListObjectsRequest(bucketName, prefix, null, null, null));
        }

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        public ObjectListing listObjects(ListObjectsRequest listObjectRequest)
        {
            String bucketName = listObjectRequest.getBucketName();
            Request<ListObjectsRequest> request = this.createRequest(bucketName, null, listObjectRequest, HttpMethodName.GET);

            if (listObjectRequest.getPrefix() != null) request.addParameter("prefix", listObjectRequest.getPrefix());
            if (listObjectRequest.getMarker() != null) request.addParameter("marker", listObjectRequest.getMarker());
            if (listObjectRequest.getDelimiter() != null) request.addParameter("delimiter", listObjectRequest.getDelimiter());
            if (listObjectRequest.getMaxKeys() != null) request.addParameter("max-keys", listObjectRequest.getMaxKeys().ToString());

            return this.invoke(request, new ListObjectsUnmarshallers(), bucketName, null);
        }

        /**
         * Deletes the specified object in the specified bucket.
         */
        public void deleteObject(String bucketName, String key)
        {
            this.deleteObject(new DeleteObjectRequest(bucketName, key));
        }

        /**
         * Deletes the specified object in the specified bucket.
         */
        public void deleteObject(DeleteObjectRequest deleteObjectRequest)
        { 
            String bucketName = deleteObjectRequest.getBucketName();
            String key = deleteObjectRequest.getKey();
            Request<DeleteObjectRequest> request = this.createRequest(bucketName, key, deleteObjectRequest, HttpMethodName.DELETE);

            this.invoke(request, voidResponseHandler, bucketName, key);
        }

        /**
         * Gets the object stored in KS3 under the specified bucket and key.
         */
        public KS3Object getObject(String bucketName, String key)
        {
            return this.getObject(new GetObjectRequest(bucketName, key));
        }

        /**
         * Gets the object stored in KS3 under the specified bucket and key, and saves the object contents to the specified file.
         */
        public KS3Object getObject(String bucketName, String key, FileInfo destinationFile)
        {
            return this.getObject(new GetObjectRequest(bucketName, key, destinationFile));
        }

        /**
         * Gets the object stored in KS3 under the specified bucket and key.
         */
        public KS3Object getObject(GetObjectRequest getObjectRequest)
        {
            String bucketName = getObjectRequest.getBucketName();
            String key = getObjectRequest.getKey();

            Request<GetObjectRequest> request = this.createRequest(bucketName, key, getObjectRequest, HttpMethodName.GET);

            if (getObjectRequest.getRange() != null)
            {
                long[] range = getObjectRequest.getRange();
                request.addHeader(Headers.RANGE, range[0].ToString() + "-" + range[1].ToString());
            }

            addDateHeader(request, Headers.GET_OBJECT_IF_MODIFIED_SINCE, getObjectRequest.getModifiedSinceConstraint());
            addDateHeader(request, Headers.GET_OBJECT_IF_UNMODIFIED_SINCE, getObjectRequest.getUnmodifiedSinceConstraint());
            addStringListHeader(request, Headers.GET_OBJECT_IF_MATCH, getObjectRequest.getMatchingETagConstraints());
            addStringListHeader(request, Headers.GET_OBJECT_IF_NONE_MATCH, getObjectRequest.getNonmatchingETagConstraints());

            ProgressListener progressListener = getObjectRequest.getProgressListener();
            if (progressListener != null)
                fireProgressEvent(progressListener, ProgressEvent.STARTED_EVENT_CODE);

            KS3Object ks3Object = null;
            try
            {
                ks3Object = this.invoke(request, new ObjectResponseHandler(getObjectRequest), bucketName, key);
            }
            catch (Exception e)
            {
                if (progressListener != null)
                    fireProgressEvent(progressListener, ProgressEvent.FAILED_EVENT_CODE);
                throw e;
            }
            if (progressListener != null)
                fireProgressEvent(progressListener, ProgressEvent.COMPLETED_EVENT_CODE);

            ks3Object.setBucketName(bucketName);
            ks3Object.setKey(key);
            
            return ks3Object;
        }

        /*
         * Gets the metadata for the specified KS3 object without actually fetching the object itself.
         */
        public ObjectMetadata getObjectMetadata(String bucketName, String key)
        {
            return this.getObjectMetadata(new GetObjectMetadataRequest(bucketName, key));
        }

        /*
         * Gets the metadata for the specified KS3 object without actually fetching the object itself.
         */
        public ObjectMetadata getObjectMetadata(GetObjectMetadataRequest getObjectMetadataRequest)
        {
            String bucketName = getObjectMetadataRequest.getBucketName();
            String key = getObjectMetadataRequest.getKey();
            Request<GetObjectMetadataRequest> request = this.createRequest(bucketName, key, getObjectMetadataRequest, HttpMethodName.HEAD);

            return invoke(request, new MetadataResponseHandler(), bucketName, key);
        }

        /**
         * Uploads the specified file to KS3 under the specified bucket and key name.
         */
        public PutObjectResult putObject(String bucketName, String key, FileInfo file)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, key, file);
            putObjectRequest.setMetadata(new ObjectMetadata());
            return this.putObject(putObjectRequest);
        }

        /**
         * Uploads the specified input stream and object metadata to KS3 under the specified bucket and key name. 
         */
        public PutObjectResult putObject(String bucketName, String key, Stream input, ObjectMetadata metadata)
        {
            return this.putObject(new PutObjectRequest(bucketName, key, input, metadata));
        }

        /**
         * Uploads a new object to the specified KS3 bucket.
         */
        public PutObjectResult putObject(PutObjectRequest putObjectRequest)
        {
            String bucketName = putObjectRequest.getBucketName();
            String key = putObjectRequest.getKey();
            ObjectMetadata metadata = putObjectRequest.getMetadata();
            Stream input = putObjectRequest.getInputStream();
            ProgressListener progressListener = putObjectRequest.getProgressListener();

            if (metadata == null) metadata = new ObjectMetadata();

            // If a file is specified for upload, we need to pull some additional
            // information from it to auto-configure a few options
            if (putObjectRequest.getFile() != null)
            {
                FileInfo file = putObjectRequest.getFile();
                try
                {
                    // Always set the content length, even if it's already set
                    metadata.setContentLength(file.Length);
                }
                catch (FileNotFoundException e)
                {
                    throw new Exception("Unable to find the file to upload: " + e.Message);
                }

                // Only set the content type if it hasn't already been set
                if (metadata.getContentType() == null)
                    metadata.setContentType(Mimetypes.getMimetype(file));

                FileStream fileStream = null;
                try
                {
                    MD5 md5 = MD5.Create();
                    fileStream = file.OpenRead();
                    metadata.setContentMD5(Convert.ToBase64String(md5.ComputeHash(fileStream)));
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to calculate MD5 hash: " + e.Message);
                }
                finally
                {
                    fileStream.Close();
                }

                input = file.OpenRead();
            }
            else
            {
                try
                {
                    metadata.setContentLength(input.Length);
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to use the stream: " + e.Message);
                }
                if (metadata.getContentType() == null) metadata.setContentType(Mimetypes.DEFAULT_MIMETYPE);
                if (metadata.getContentMD5() == null)
                {
                    try
                    {
                        MD5 md5 = MD5.Create();
                        metadata.setContentMD5(Convert.ToBase64String(md5.ComputeHash(input)));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Unable to calculate MD5 hash: " + e.Message);
                    }

                    input.Seek(0, new SeekOrigin()); // It is needed after calculated MD5.
                }
            }
            
            Request<PutObjectRequest> request = this.createRequest(bucketName, key, putObjectRequest, HttpMethodName.PUT);

            if (progressListener != null)
            {
                input = new ProgressReportingInputStream(input, progressListener);
                fireProgressEvent(progressListener, ProgressEvent.STARTED_EVENT_CODE);
            }

            populateRequestMetadata(request, metadata);
            request.setContent(input);

            //-----------------------------------------------

            ObjectMetadata returnedMetadata = null;
            try
            {
                returnedMetadata = this.invoke(request, new MetadataResponseHandler(), bucketName, key);
            }
            catch (Exception e)
            {
                fireProgressEvent(progressListener, ProgressEvent.FAILED_EVENT_CODE);
                throw e;
            }
            finally
            {
                input.Close();
            }

            if (progressListener != null)
                fireProgressEvent(progressListener, ProgressEvent.COMPLETED_EVENT_CODE);

            PutObjectResult result = new PutObjectResult();
            result.setETag(returnedMetadata.getETag());
            result.setContentMD5(metadata.getContentMD5());

            return result;
        }

        /**
         * Creates and initializes a new request object for the specified KS3 resource.
         * Three parameters needed to be set
         * 1. http method (GET, PUT, HEAD or DELETE)
         * 2. endpoint (http or https, and the host name. e.g. http://kss.ksyun.com)
         * 3. resource path (bucketName/[key], e.g. my-bucket/my-object)
         */
        private Request<X> createRequest<X>(String bucketName, String key, X originalRequest, HttpMethodName httpMethod) where X : KS3Request 
        {
            Request<X> request = new DefaultRequest<X>(originalRequest);
            request.setHttpMethod(httpMethod);
            request.setEndpoint(endpoint);
            if (bucketName != null) request.setResourcePath(bucketName + "/" + (key != null ? key : ""));
            return request;
        }

        private X invoke<X, Y>(Request<Y> request, Unmarshaller<X, Stream> unmarshaller, String bucketName, String key) where Y : KS3Request
        {
            return this.invoke(request, new XmlResponseHandler<X>(unmarshaller), bucketName, key);
        }

        /**
         * Before the KS3HttpClient deal with the request, we want the request looked like a collection of that:
         * 1. http method
         * 2. endpoint
         * 3. resource path
         * 4. headers
         * 5. parameters
         * 6. content
         * 7. time offset
         * 
         * The first three points are done in "createRequest".
         * The content was set before "createRequest" when we need to put a object to server. And some metadata like Content-Type, Content-Length, etc.
         * So at here, we need to complete 4, 5, and 7.
         */
        private X invoke<X, Y>(Request<Y> request, HttpResponseHandler<X> responseHandler, String bucket, String key) where Y : KS3Request
        {
            Dictionary<String, String> parameters = request.getOriginalRequest().copyPrivateRequestParameters();
            foreach (String name in parameters.Keys)
                request.addParameter(name, parameters[name]);
            request.setTimeOffset(timeOffset);

            /*
             * The string we sign needs to include the exact headers that we
             * send with the request, but the client runtime layer adds the
             * Content-Type header before the request is sent if one isn't set, so
             * we have to set something here otherwise the request will fail.
             */
            if (!request.getHeaders().ContainsKey(Headers.CONTENT_TYPE))
                request.addHeader(Headers.CONTENT_TYPE, Mimetypes.DEFAULT_MIMETYPE);
            
            KS3Credentials credentials = ks3Credentials;
            KS3Request originalRequest = request.getOriginalRequest();
            if (originalRequest != null && originalRequest.getRequestCredentials() != null)
                credentials = originalRequest.getRequestCredentials();
            request.getOriginalRequest().setRequestCredentials(credentials);

            return client.excute(request, responseHandler, createSigner(request, bucket, key));
        }

        private KS3Signer<T> createSigner<T>(Request<T> request, String bucketName, String key) where T : KS3Request
        {
            String resourcePath = "/" + (bucketName != null ? bucketName + "/" : "") + (key != null ? key : "");
            return new KS3Signer<T>(request.getHttpMethod().ToString(), resourcePath);
        }


        /**
         * Fires a progress event with the specified event type to the specified
         * listener.
         */
        private static void fireProgressEvent(ProgressListener listener, int eventType) {
            if (listener == null) return;
            ProgressEvent e = new ProgressEvent(0, eventType);
            listener.progressChanged(e);
        }

        /**
         * Populates the specified request object with the appropriate headers from
         * the {@link ObjectMetadata} object.
         */

        private static void populateRequestMetadata<X>(Request<X> request, ObjectMetadata metadata)
        {
            Dictionary<String, Object> rawMetadata = metadata.getRawMetadata();
            if (rawMetadata != null)
            {
                foreach (String name in rawMetadata.Keys)
                    request.addHeader(name, rawMetadata[name].ToString());
            }

            Dictionary<String, String> userMetadata = metadata.getUserMetadata();
            if (userMetadata != null)
            {
                foreach (String name in userMetadata.Keys)
                    request.addHeader(Headers.KS3_USER_METADATA_PREFIX + name, userMetadata[name]);
            }
        }

        private static void addDateHeader<X>(Request<X> request, String header, DateTime? value)
        {
            if (value != null) request.addHeader(header, SignerUtils.getSignatrueDate(value.Value));
        }


        /*
         * Adds the specified string list header, joined together separated with
         * commas, to the specified request.
         * This method will not add a string list header if the specified values
         * are <code>null</code> or empty.
         */
        private static void addStringListHeader<X>(Request<X> request, String header, List<String> values)
        {
            if (values != null && values.Count > 0)
                request.addHeader(header, String.Join(",", values));
        }

    }
}
