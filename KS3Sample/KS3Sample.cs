using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3;
using KS3.Auth;
using KS3.Http;
using KS3.Model;
using KS3.Internal;
using KS3.KS3Exception;

namespace KS3Sample
{
    class KS3Sample
    {
        static String accessKey = "YOUR ACCESS KEY";
        static String secretKey = "YOUR SECRET KEY";

		// KS3 Operation class 
		static KS3Client ks3Client = null;

		static String bucketName = null;
		static String objKeyNameMemoryData	= "short-content";
		static String objKeyNameFileData	= "file-data";

		static String inFilePath  = "D:/test.in.data";
		static String outFilePath = "D:/test.out.data";


		static void Main(string[] args)
        {
            if (!init())
                return;		// init failed 

            Console.WriteLine("========== Begin ==========\n");
            createBucket();
            listBuckets();
            getBucketACL();
            setBucketACL();
            putObject();
            listObjects();
            getObjectACL();
            setObjectACL();
            getObject();
            deleteObject();
            deleteBucket();
            catchServiceException();
            Console.WriteLine("\n==========  End  ==========");
		}

		private static bool init()
		{
			if ( accessKey.Equals("YOUR ACCESS KEY") || secretKey.Equals("YOUR SECRET KEY") )
			{
				Console.WriteLine("You should be set your Access Key and Secret Key");
				return false;
			}
			ks3Client = new KS3Client(accessKey, secretKey);

			FileInfo fi = new FileInfo(inFilePath);
            if (!fi.Exists)
            {
				FileStream fs = null;
				try
				{
					fs = fi.OpenWrite();
					for (int i = 0; i < 1024 * 1024; i++)
						fs.WriteByte( (byte)i );
				}
				catch (System.Exception e)
				{
					Console.WriteLine("Init Data File Fail");
					Console.WriteLine(e.ToString());
					return false;
				}
				finally
				{
					fs.Close();
				}
            }

			bucketName = "test-ks3-bucket-" + new Random().Next();
			return true;
		}

		private static bool createBucket()
		{
            // Create Bucket
			try
			{
                Console.WriteLine("--- Create Bucket: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

				Bucket bucket = ks3Client.createBucket(bucketName);

				Console.WriteLine("Success.");
				Console.WriteLine("----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine("Create Bucket Fail! " + e.ToString());				
				return false;
			}

			return true;
		}

		private static bool listBuckets()
		{
            // List Buckets
			try
			{
				Console.WriteLine("--- List Buckets: ---");

				List<Bucket> bucketsList = ks3Client.listBuckets();
				foreach (Bucket b in bucketsList)
				{
					Console.WriteLine(b.ToString());
				}
            
				Console.WriteLine("---------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}
			    
			return true;
		}

		private static bool getBucketACL()
		{
            // Get Bucket ACL
            try 
			{
				Console.WriteLine("--- Get Bucket ACL: ---");
            
			    AccessControlList acl = ks3Client.getBucketAcl(bucketName);
                Console.WriteLine("Bucket Name: " + bucketName);
                Console.WriteLine(acl.ToString());

				Console.WriteLine("-----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;

			}

			return true;
		}


		private static bool setBucketACL()
		{
            // Set Bucket ACL
			try 
			{
				Console.WriteLine("--- Set Bucket ACL: ---");

				CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
				ks3Client.setBucketAcl(bucketName, cannedAcl);

                Console.WriteLine("Bucket Name: " + bucketName);
				Console.WriteLine("Success, now the ACL is:\n" + ks3Client.getBucketAcl(bucketName));
				Console.WriteLine("-----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}

			return true;
		}

		private static bool putObject()
		{
			try
            {
                // Put Object(upload a short content)
				Console.WriteLine("--- Upload a Short Content: ---");

				String sampleContent = "This is a sample content.(25 characters before, included the 4 spaces)";
				Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleContent));
				PutObjectResult shortContentResult = ks3Client.putObject(bucketName, objKeyNameMemoryData, stream, null);
	
				Console.WriteLine("Upload Completed. eTag=" + shortContentResult.getETag() + ", MD5=" + shortContentResult.getContentMD5());
				Console.WriteLine("-------------------------------\n");

				// Put Object(upload a file)
				Console.WriteLine("--- Upload a File ---");

				FileInfo file = new FileInfo(inFilePath);
				PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, objKeyNameFileData, file);
				SampleListener sampleListener = new SampleListener(file.Length);
				putObjectRequest.setProgressListener(sampleListener);
				PutObjectResult putObjectResult = ks3Client.putObject(putObjectRequest);
				
				Console.WriteLine("Upload Completed. eTag=" + putObjectResult.getETag() + ", MD5=" + putObjectResult.getContentMD5());
				Console.WriteLine("---------------------\n");
			}
			catch (System.Exception e) 
			{
				Console.WriteLine(e.ToString());
				return false;
			}

			return true;
		}

		private static bool listObjects()
		{
			try
			{
				// List Objects
				Console.WriteLine("--- List Objects: ---");

				ObjectListing objects = ks3Client.listObjects(bucketName);

				Console.WriteLine(objects.ToString());
				Console.WriteLine("---------------------\n");

				// Get Object Metadata
				Console.WriteLine("--- Get Object Metadata ---");

				ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameMemoryData); 
				Console.WriteLine(objMeta.ToString());
                Console.WriteLine();
				objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameFileData);
				Console.WriteLine(objMeta.ToString());
				
				Console.WriteLine("---------------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());		
				return false;
			}

			return true;
		}

        private static bool getObjectACL()
        {
            // Get Object ACL
            try
            {
                Console.WriteLine("--- Get Object ACL: ---");

                AccessControlList acl = ks3Client.getObjectAcl(bucketName ,objKeyNameMemoryData);
                Console.WriteLine("Object Key: " + objKeyNameMemoryData);
                Console.WriteLine(acl.ToString());

                Console.WriteLine("-----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;

            }

            return true;
        }


        private static bool setObjectACL()
        {
            // Set Object ACL
            try
            {
                Console.WriteLine("--- Set Object ACL: ---");

                CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                Console.WriteLine("Object Key: " + objKeyNameMemoryData);
                ks3Client.setObjectAcl(bucketName, objKeyNameMemoryData, cannedAcl);

                Console.WriteLine("Success, now the ACL is:\n" + ks3Client.getObjectAcl(bucketName, objKeyNameMemoryData));
                Console.WriteLine("-----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

		private static bool getObject()
		{
			try
			{
                // Get Object(download and store in memory)
				Console.WriteLine("--- Download and Store in Memory ---");
				
				GetObjectRequest getShortContent = new GetObjectRequest(bucketName, objKeyNameMemoryData);
				getShortContent.setRange(0, 24);
				KS3Object ks3Object = ks3Client.getObject(getShortContent);
				
				StreamReader sr = new StreamReader(ks3Object.getObjectContent());
				Console.WriteLine("Content:\n" + sr.ReadToEnd());
				sr.Close();
				ks3Object.getObjectContent().Close();
				
				Console.WriteLine("------------------------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}


			try
			{
                // Get Object(download and save as a file)
				Console.WriteLine("--- Download a File ---");

				// I need to get the Content-Length to set the listener.
				ObjectMetadata objectMetadata = ks3Client.getObjectMetadata(bucketName, objKeyNameFileData); 
				
				SampleListener downloadListener = new SampleListener(objectMetadata.getContentLength());
				GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, objKeyNameFileData, new FileInfo(outFilePath));
				getObjectRequest.setProgressListener(downloadListener);
				KS3Object obj = ks3Client.getObject(getObjectRequest);
				obj.getObjectContent().Close(); // The file was opened in [KS3ObjectResponseHandler], so I close it first. 
				
				Console.WriteLine("Success. See the file downloaded at {0}", outFilePath);
				Console.WriteLine("-----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}

			return true;
		}

		private static bool deleteObject()
		{
			// Delete Object
			try
			{
				Console.WriteLine("--- Delete Object: ---");

				ks3Client.deleteObject(bucketName, objKeyNameMemoryData);
				ks3Client.deleteObject(bucketName, objKeyNameFileData);

				Console.WriteLine("Delete Object completed.");
				Console.WriteLine("---------------------\n");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}
			
			return true;
		}

		private static bool deleteBucket()
		{
            // Delete Bucket
            try
            {
                Console.WriteLine("--- Delete Bucket: ---");

                ks3Client.deleteBucket(bucketName);
                
                Console.WriteLine("Delete Bucket completed.");
				Console.WriteLine("----------------------\n");
			}
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
				return false;
            }
			return true;
        }

        private static bool catchServiceException()
        {
            // This is a sample of catch ServiceException of KS3.
            // You can catch the ServiceException when some illegal operations appear.
            // But note that, if we have done some illegal operations, there also may appear some other unexcepted exceptions too.
            // Now we will see a ServiceException because will try to delete a bucket that does not exist.
            try
            {
                Console.WriteLine("--- Catch ServiceException: ---");

                ks3Client.deleteBucket(bucketName);

                Console.WriteLine("-------------------------------\n");
            }
            catch (ServiceException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }

    class SampleListener : ProgressListener
    {
        long size = -1;
        long completedSize = 0;
        int rate = 0;
        bool cancled = false;

        public SampleListener() { }

        public SampleListener(long size)
        {
            this.size = size;
        }

        public void progressChanged(ProgressEvent progressEvent)
        {
            int eventCode = progressEvent.getEventCode();

            if (eventCode == ProgressEvent.STARTED)
                Console.WriteLine("Started.");
            else if (eventCode == ProgressEvent.COMPLETED)
                Console.WriteLine("Completed.");
            else if (eventCode == ProgressEvent.FAILED)
                Console.WriteLine("Failed.");
            else if (eventCode == ProgressEvent.CANCELED)
                Console.WriteLine("Cancled.");
            else if (eventCode == ProgressEvent.TRANSFERRED)
            {
                this.completedSize += progressEvent.getBytesTransferred();
                int newRate = (int)((double)completedSize / size * 100 + 0.5);
                if (newRate > this.rate)
                {
                    this.rate = newRate;
                    Console.WriteLine("Processing ... " + this.rate + "%");
                }
            }
        } // end of progressChanged

        public bool askContinue()
        {
            return !this.cancled;
        }
    }

}