using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Kinect;


namespace KinectExile.Models
{


    public struct RingBufferData
    {

        public byte[] ColorPixelData { get; private set; }
        public short[] DepthPixelData { get; private set; }

        public double[] DepthLengthData { get; private set; }
        public int[] PlayerIndexData { get; private set; }

        public void InitArray(int ColorStream_FramePixelDataLength, int DepthStream_FramePixelDataLength)
        {
            ColorPixelData = new byte[ColorStream_FramePixelDataLength];
            DepthPixelData = new short[DepthStream_FramePixelDataLength];
            //DepthLengthData = new double[ColorStream_FramePixelDataLength];
            PlayerIndexData = new int[ColorStream_FramePixelDataLength];
        }

        /// <summary>
        /// RGB配列をコピー
        /// </summary>
        /// <param name="value"></param>
        public void copy_framedata(ref byte[] value)
        {
            System.Array.Copy(value, ColorPixelData, value.Length);
        }


        /// <summary>
        /// Depth配列をコピー
        /// </summary>
        /// <param name="value"></param>
        public void copy_DepthPixelData(short[] value)
        {
            System.Array.Copy(value, DepthPixelData, value.Length);
        }


        ///// <summary>
        ///// バイト配列をコピー
        ///// </summary>
        ///// <param name="value"></param>
        //public void copy_DepthLengthData(double[] value)
        //{
        //    System.Array.Copy(value, DepthLengthData, value.Length);
        //}


        /// <summary>
        /// PlayerIndex配列をコピー
        /// </summary>
        /// <param name="value"></param>
        public void copy_PlayerIndexData(int[] value)
        {
            System.Array.Copy(value, PlayerIndexData, value.Length);
        }

    }


    public struct RingBuffer
    {

        public RingBufferData[] _data;

        public int max_buffer_frame { get; private set; }
        public int buffer_index { get; private set; }


        private int get_index(int frameNo)
        {
            int no = frameNo < max_buffer_frame ? frameNo : frameNo - max_buffer_frame;
            if (frameNo < 0) no = max_buffer_frame+ frameNo;
            return no;
        }


        #region 

        public byte[] get_rgb_frame(int frameNo)
        {
            if (_data == null) return null;
            return _data[get_index(frameNo)].ColorPixelData;
        }

        public short[] get_depth_frame(int frameNo)
        {
            if (_data == null) return null;
            return _data[get_index(frameNo)].DepthPixelData;
        }

        public double[] get_depth_length(int frameNo)
        {
            if (_data == null) return null;
            return _data[get_index(frameNo)].DepthLengthData;
        }

        public int[] get_PlayerIndexData(int frameNo)
        {
            if (_data == null) return null;
            return _data[get_index(frameNo)].PlayerIndexData;
        }

        #endregion

        //public RingBuffer()
        //{
        //    _data = new RingBufferData[max_buffer_frame];
        //}

        /// <summary>
        /// リングバッファの初期化
        /// </summary>
        /// <param name="ret_message"></param>
        /// <returns></returns>
        public bool init_ringbuffer(int buffer_sec, int ColorStream_FramePixelDataLength, int DepthStream_FramePixelDataLength, out string ret_message)
        {
            ret_message = string.Empty;

            try
            {
                buffer_index = 0;
                max_buffer_frame = 30 * buffer_sec; //30fps*second

                _data = new RingBufferData[max_buffer_frame];

                for (int i = 0; i < max_buffer_frame; i++)
                {
                    _data[i] = new RingBufferData();
                    _data[i].InitArray(ColorStream_FramePixelDataLength, DepthStream_FramePixelDataLength);
                }

                return true;
            }
            catch (Exception ex)
            {
                ret_message = ex.Message;
                return false;
            }
        }



        public void save_framedata(ref byte[] value)
        {
            _data[buffer_index].copy_framedata(ref value);
        }


        public void save_depthdata(short[] value)
        {
            _data[buffer_index].copy_DepthPixelData(value);
        }

        //public void save_depthdata(double[] value)
        //{
        //    _data[buffer_index].copy_DepthLengthData(value);
        //}
        
        public void save_playerIndexdata(int[] value)
        {
            _data[buffer_index].copy_PlayerIndexData(value);
        }


        public void set_nextframe()
        {
            buffer_index = buffer_index >= max_buffer_frame - 1 ? 0 : buffer_index + 1;
        }


    }
}
