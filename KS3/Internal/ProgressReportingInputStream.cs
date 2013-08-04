using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3.Model;

namespace KS3.Internal
{
    public class ProgressReportingInputStream : Stream
    {
        /** The threshold of bytes between notifications. */
        private static int NOTIFICATION_THRESHOLD =  Constants.DEFAULT_STREAM_BUFFER_SIZE;
        
        /** The listener to notify. */
        private ProgressListener listener;

        /** The original stream. */
        private Stream stream;

        /** The number of bytes read that the listener hasn't been notified about yet. */
        private int unnotifiedByteCount = 0;

        /**
         * Creates a repeatable input stream based on a file.
         */
        public ProgressReportingInputStream(Stream stream, ProgressListener listener)
        {
            this.stream = stream;
            this.listener = listener;
        }

        private void notify(int bytesRead)
        {
            this.unnotifiedByteCount += bytesRead;
            if (this.unnotifiedByteCount >= NOTIFICATION_THRESHOLD)
            {
                listener.progressChanged(new ProgressEvent(this.unnotifiedByteCount, 0));
                this.unnotifiedByteCount = 0;
            }
        }

        public override int ReadByte()
        {
            int data = this.stream.ReadByte();
            if (data != -1) this.notify(1);
            return data;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead =  this.stream.Read(buffer, offset, count);
            if (bytesRead > 0) this.notify(bytesRead);
            return bytesRead;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.stream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return this.stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.stream.CanWrite; }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.stream.Seek(offset, origin);
        }

        public override long Length
        {
            get { return this.stream.Length; }
        }

        public override void SetLength(long value)
        {
            this.stream.SetLength(value);
        }

        public override long Position
        {
            get
            {
                return this.stream.Position;
            }
            set
            {
                this.stream.Position = value;
            }
        }

        public override void Flush()
        {
            this.stream.Flush();
        }
    }
}
