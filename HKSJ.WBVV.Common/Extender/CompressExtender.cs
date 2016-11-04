using System.IO;
using System.IO.Compression;
using System.Text;

namespace HKSJ.WBVV.Common.Extender
{

    /// <summary>
    /// 压缩辅助类
    /// </summary>
    public static class CompressExtender
    {
        /// <summary>
        /// 转换成json格式并压缩对象
        /// </summary>
        /// <param name="obj">原始数据</param>
        /// <returns>压缩数据</returns>
        public static byte[] Compress<T>(this T obj)
        {
            byte[] byteArray = SerializeExtender.ToBinary(obj);

            using (MemoryStream inStream = new MemoryStream(byteArray))
            {
                inStream.Position = 0;
                MemoryStream outStream = new MemoryStream();
                using (GZipStream compress = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    byte[] buf = new byte[10240];
                    int len;
                    while ((len = inStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        compress.Write(buf, 0, len);
                    }
                }
                outStream.Position = 0;
                return outStream.ToArray();
            }
        }
        /// <summary>
        /// 解压缩json格式，还原成对象
        /// </summary>
        /// <param name="buffer">压缩数据</param>
        /// <returns>原始数据</returns>
        public static T Decompress<T>(this byte[] buffer)
        {
            using (MemoryStream inStream = new MemoryStream(buffer))
            {
                inStream.Position = 0;
                MemoryStream outStream = new MemoryStream();
                using (GZipStream compress = new GZipStream(inStream, CompressionMode.Decompress, true))
                {
                    int len;
                    byte[] buf = new byte[10240];
                    while ((len = compress.Read(buf, 0, buf.Length)) > 0)
                    {
                        outStream.Write(buf, 0, len);
                    }
                }
                outStream.Position = 0;
                return (T)SerializeExtender.FromBinary(outStream.ToArray());
            }
        }
    }
}
