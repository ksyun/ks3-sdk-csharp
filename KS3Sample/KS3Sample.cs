using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Globalization;
using System.Xml;
using System.Diagnostics;
using System.Threading;

using KS3;
using KS3.Auth;
using KS3.Http;
using KS3.Model;
using KS3.Internal;

namespace KS3Sample
{
    class KS3Sample
    {
        static String accessKey = "YOUR ACCESS KEY";
        static String secretKey = "YOUR SECRET KEY";
        static String filePath = "A FILE PATH IN YOUR COMPUTER"; // It is used when we upload a file.
        static Random random = new Random();
        static void Main(string[] args)
        {
            KS3Client ks3 = new KS3Client(new BasicKS3Credentials(accessKey, secretKey));

            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
            {
                Console.WriteLine("File not Found: " + filePath);
                return;
            }

            Console.WriteLine("========== Begin ==========\n");

            // Create Bucket
            Console.WriteLine("--- Create Bucket : ---");
            String bucketName = "ks3-bucket-" + random.Next();
            Console.WriteLine("Bucket Name: " + ks3.createBucket(bucketName).getName());
            Console.WriteLine("-----------------------\n");

            // List Buckets
            Console.WriteLine("--- List buckets: ---");
            List<Bucket> bucketsList = ks3.listBuckets();
            foreach (Bucket b in bucketsList)
                Console.WriteLine(b.ToString());
            Console.WriteLine("---------------------\n");

            // Get Bucket ACL
            Console.WriteLine("--- Get Bucket ACL: ---");
            CannedAccessControlList cacl = ks3.getBucketAcl(bucketName);
            Console.WriteLine(cacl.ToString());
            Console.WriteLine("-----------------------\n");

            // Put Bucket ACL
            Console.WriteLine("--- Set Bucket ACL: ---");
            CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
            ks3.setBucketAcl(bucketName, cannedAcl);
            Console.WriteLine("Success, now the ACL is: " + ks3.getBucketAcl(bucketName));
            Console.WriteLine("-----------------------\n");

            // Put Object(upload a short content)
            Console.WriteLine("--- Upload a Short Content: ---");
            String sampleContent = "This is a sample content.(25 characters before, included the 4 spaces)";
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleContent));
            PutObjectResult shortContentResult = ks3.putObject(bucketName, "short-content", stream, null);
            Console.WriteLine("Upload Completed. eTag=" + shortContentResult.getETag() + ", MD5=" + shortContentResult.getContentMD5());
            Console.WriteLine("-------------------------------\n");

            // Put Object(upload a file)
            Console.WriteLine("--- Upload a File ---");
            PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, "file", file);
            SampleListener sampleListener = new SampleListener(file.Length);
            putObjectRequest.setProgressListener(sampleListener);
            PutObjectResult putObjectResult = ks3.putObject(putObjectRequest);
            Console.WriteLine("Upload Completed. eTag=" + putObjectResult.getETag() + ", MD5=" + putObjectResult.getContentMD5());
            Console.WriteLine("---------------------\n");

            // List Objects
            Console.WriteLine("--- List Objects: ---");
            Console.WriteLine(ks3.listObjects(bucketName).ToString());
            Console.WriteLine("---------------------\n");

            // Get Object Metadata
            Console.WriteLine("--- Get Object Metadata ---");
            Console.WriteLine(ks3.getObjectMetadata(bucketName, "short-content").ToString());
            Console.WriteLine("---------------------------\n");

            // Get Object(download and store in memory)
            Console.WriteLine("--- Download and Store in Memory ---");
            GetObjectRequest getShortContent = new GetObjectRequest(bucketName, "short-content");
            getShortContent.setRange(0, 24);
            KS3Object ks3Object = ks3.getObject(getShortContent);
            StreamReader sr = new StreamReader(ks3Object.getObjectContent());
            Console.WriteLine("Content:\n" + sr.ReadToEnd());
            sr.Close();
            ks3Object.getObjectContent().Close();
            Console.WriteLine("------------------------------------\n");


            //// Wait a period time to download the file we upload
            //Console.WriteLine("Wait five minutes to download the file we upload ...");
            //for (int i = 5; i > 0; i--)
            //{
            //    Console.WriteLine(i);
            //    Thread.Sleep(1000);
            //}
            //Console.WriteLine();

            //String bucketName = "ks3-bucket-1859740557";

            // Get Object(download a file)
            try
            {
                Console.WriteLine("--- Download a File ---");
                ObjectMetadata objectMetadata = ks3.getObjectMetadata(bucketName, "file"); // I need to get the Content-Length to set the listener.
                SampleListener downloadListener = new SampleListener(objectMetadata.getContentLength());
                GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, "file", new FileInfo("D:\\temp.txt"));
                getObjectRequest.setProgressListener(downloadListener);
                KS3Object obj = ks3.getObject(getObjectRequest);
                obj.getObjectContent().Close(); // The file was opened in [KS3ObjectResponseHandler], so I close it first. 
                Console.WriteLine("Success. See the file downloaded at D:\\temp.txt");
                Console.WriteLine("-----------------------\n");
            }
            catch
            {
                // Do nothing here... The disk drive D may not exist. 
            }

            // Delete Object
            Console.WriteLine("--- Delete Object: ---");
            ks3.deleteObject(bucketName, "short-content");
            ks3.deleteObject(bucketName, "file");
            Console.WriteLine("Delete Object completed.");
            Console.WriteLine("---------------------\n");


            // Delete Bucket
            Console.WriteLine("--- Delete Bucket: ---");
            ks3.deleteBucket(bucketName);
            Console.WriteLine("Delete Bucket completed.");
            Console.WriteLine("----------------------\n");


            // A Simple KS3Exception
            // You can catch the KS3Exception when some illegal operations appear.
            // But note that, if we have done some illegal operations, there also may appear some other unexcepted exceptions too.
            // Now we will see a KS3Excetpion because will try to delete a bucket that does not exist.
            try
            {
                Console.WriteLine("--- Delete Bucket: ---");
                ks3.deleteBucket(bucketName);
            }
            catch (KS3Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("----------------------\n");
            }
            
            Console.WriteLine("\n========== End ==========");

        }
    }

    class SampleListener : ProgressListener
    {
        long size = -1;
        long completedSize = 0;
        int rate = 0;

        public SampleListener() { }

        public SampleListener(long size)
        {
            this.size = size;
        }

        public void progressChanged(ProgressEvent progressEvent)
        {
            int eventCode = progressEvent.getEventCode();
            if (eventCode == ProgressEvent.STARTED_EVENT_CODE) Console.WriteLine("Started.");
            else if (eventCode == ProgressEvent.COMPLETED_EVENT_CODE) Console.WriteLine("Completed.");
            else if (eventCode == ProgressEvent.FAILED_EVENT_CODE) Console.WriteLine("Failed.");
            else
            {
                this.completedSize += progressEvent.getBytesTransferred();
                int newRate = (int)((double)completedSize / size * 100 + 0.5);
                if (newRate > this.rate)
                {
                    this.rate = newRate;
                    Console.WriteLine("Processing ... " + this.rate + "%");
                }
            }
        }
    }
}