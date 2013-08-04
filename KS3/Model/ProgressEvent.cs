using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KS3.Model
{
    public class ProgressEvent
    {
        // Single part event codes
        public static int STARTED_EVENT_CODE   = 1;
        public static int COMPLETED_EVENT_CODE = 2;
        public static int FAILED_EVENT_CODE    = 4;
        public static int CANCELED_EVENT_CODE  = 8;

        // Multipart event codes
        public static int PART_STARTED_EVENT_CODE   = 1024;
        public static int PART_COMPLETED_EVENT_CODE = 2048;
        public static int PART_FAILED_EVENT_CODE    = 4096;

        /** The number of bytes transferred since the last progress event. */
        private int bytesTransferred;

        /**
         * The unique event code that identifies what type of specific type of event
         * this object represents.
         */
        private int eventCode;

        public ProgressEvent(int bytesTransferred, int eventCode)
        {
            this.bytesTransferred = bytesTransferred;
            this.eventCode = eventCode;
        }

        /**
         * Sets the number of bytes transferred since the last progress event.
         */
        public void setBytesTransferred(int bytesTransferred)
        {
            this.bytesTransferred = bytesTransferred;
        }

        /**
         * Returns the number of bytes transferred since the last progress event.
         */
        public int getBytesTransferred()
        {
            return this.bytesTransferred;
        }

        /**
         * Returns the unique event code that identifies what type of specific type
         * of event this object represents.
         */
        public int getEventCode()
        {
            return this.eventCode;
        }
        
        /**
         * Sets the unique event code that identifies what type of specific type of
         * event this object represents.
         */
        public void setEventCode(int eventType)
        {
            this.eventCode = eventType;
        }
    }
}
