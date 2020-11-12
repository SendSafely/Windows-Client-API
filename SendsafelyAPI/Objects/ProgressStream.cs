using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using SendSafely.Utilities;
using System.Diagnostics;

namespace SendSafely.Objects
{
    class ProgressStream : Stream
    {
        Stream _inner;
        ISendSafelyProgress _progress;
        String _prefix;
        long _fileSize;
        long _offset;
        long _readSoFar;
        long _lastProgressCallback;
        int UPDATE_FREQUENCY = 250;
        Stopwatch _stopwatch;

        public ProgressStream(Stream inner, ISendSafelyProgress progress, String prefix, long size, long offset)
        {
            this._inner = inner;
            this._progress = progress;
            this._prefix = prefix;
            this._fileSize = size < 1024 ? size * 1024 : size; // multiple file size by 1024 if fileSize is less than 1024 bytes.
            this._readSoFar = 0;
            this._offset = offset;
            _lastProgressCallback = DateTime.Now.Ticks - UPDATE_FREQUENCY; // Make sure we trigger it the first time.
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _inner.Write(buffer, offset, count);

            _readSoFar += buffer.Length;

            UpdateProgress();
        }

        public override int Read(byte[] buffer, int offset, int count) 
        {
            var result = _inner.Read(buffer, offset, count);

            _readSoFar += result;

            UpdateProgress();
            return result;
        }

        public override bool CanRead
        {
            get { return _inner.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _inner.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _inner.CanWrite; }
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Flush()
        {
            _inner.Flush();
        }

        public override long Length
        {
            get { return _inner.Length; }
        }

        public override long Position
        {
            get { return _inner.Position; }
            set { _inner.Position = value; }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            _readSoFar = offset;
            return _inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _inner.SetLength(value);
        }

        private void UpdateProgress()
        {
            if (_stopwatch.ElapsedMilliseconds > UPDATE_FREQUENCY)
            {
                _stopwatch.Reset();
                _stopwatch.Start();

                _progress.UpdateProgress(_prefix, ((double)(_readSoFar+_offset) / (double)_fileSize) * 100);
            }
        }
    }
}
