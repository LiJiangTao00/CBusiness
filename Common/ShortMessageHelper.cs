using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Description: ���Ͷ���
    /// Version: 1.0
    /// Created: 2015/4/27
    /// Author:  jyc
    /// Company: ����XXXX��Ϣ�Ƽ��������ι�˾
    /// 
    /// ModifyEditTime: �޸�ʱ��
    /// ModifyContent:  �޸�����
    /// ModifyPerson :  �޸���
    /// </summary>
    public static class ShortMessageHelper
    {
        //------------------------------------------------
        //����:	������ͨHTTP�ӿ�C#����˵��
        //����:	2013-05-08
        //˵��:	http://m.5c.com.cn/api/send/?apikey=32λ������&username=�û���&password=����&mobile=�ֻ���&content=����
        //״̬:


        //����ֵ										˵��
        //success:msgid								�ύ�ɹ�������״̬���4.1
        //error:msgid								�ύʧ��
        //error:Missing username					�û���Ϊ��
        //error:Missing password					����Ϊ��
        //error:Missing apikey						APIKEYΪ��
        //error:Missing recipient					�ֻ�����Ϊ��
        //error:Missing message content				��������Ϊ��
        //error:Account is blocked					�ʺű�����
        //error:Unrecognized encoding				����δ��ʶ��
        //error:APIKEY or password error			APIKEY ���������
        //error:Unauthorized IP address				δ��Ȩ IP ��ַ
        //error:Account balance is insufficient		����
        //error:Black keywords is:������			���δ�

        /// <summary>
        /// ���Ͷ���--������ͨ
        /// </summary>
        /// <param name="phoneNumber">�ֻ�����</param>
        /// <param name="content">��������</param>
        public static string SendMessage(string phoneNumber, string content)
        {
            const string userName = "bsdfsdfa";
            const string password = "Hhasdfsdf14";
            const string apikey = "c846a02eadasfasdfasdfsadf66b5";
            //POST
            var sbTemp = new StringBuilder();
            sbTemp.Append("apikey=" + apikey + "&username=" + userName + "&password=" + password + "&mobile=" + phoneNumber + "&content=" + content);
            var bData = System.Text.Encoding.GetEncoding("GBK").GetBytes(sbTemp.ToString());
            var result = PostRequest("http://m.5c.com.cn/api/send/?", bData);
            return result;
        }



        //1	    �����ɹ�
        //0	    �ʻ���ʽ����ȷ(��ȷ�ĸ�ʽΪ:Ա�����@��ҵ���)
        //-1	�������ܾ�(�ٶȹ��졢��ʱ���IP���Ե�)�����ٶȹ������ʱ�ٷ�
        //-2	��Կ����ȷ
        //-3	��Կ������
        //-4	��������ȷ(���ݺͺ��벻��Ϊ�գ��ֻ����������࣬����ʱ������)
        //-5	�޴��ʻ�
        //-6	�ʻ����������ѹ���
        //-7	�ʻ�δ�����ӿڷ���
        //-8	����ʹ�ø�ͨ����
        //-9	�ʻ�����
        //-10	�ڲ�����
        //-11	�۷�ʧ��
        /// <summary>
        /// ���Ͷ���--Ī��
        /// </summary>
        /// <param name="phoneNumber">�ֻ�����</param>
        /// <param name="content">��������</param>
        public static string SendMessageNew(string phoneNumber, string content)
        {
            const string ac = "100asdfasdf90001";
            const string key = "E94B09Fsdafsdfasdfsd8981998B1";
            //POST
            var sbTemp = new StringBuilder();
            sbTemp.Append("action=sendOnce&ac=" + ac + "&authkey=" + key + "&cgid=31&c=" + content + "&m=" + phoneNumber);
            var result = postSend("http://180.97.163.89:8012/OpenPlatform/OpenApi?", sbTemp.ToString());
            return result;
        }

        /// <summary>
        /// ���Ͷ���--������ͨ
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SendMessageNewHX(string phoneNumber, string content)
        {
            var myEncode = System.Text.Encoding.GetEncoding("UTF-8");

            //���²���Ϊ����Ҫ�Ĳ���������ʱ���޸�
            var strReg = "1011asdfasdfsd4828";   //ע��ţ��ɻ�����ͨ�ṩ��
            var strPwd = "ZQsdfsdfsdsFE";                 //���루�ɻ�����ͨ�ṩ��
            var strSourceAdd = "";                   //��ͨ���ţ���Ϊ�գ�Ԥ��������
            var strPhone = phoneNumber;//�ֻ����룬����ֻ����ð�Ƕ��ŷֿ������1000��
            var strContent = System.Web.HttpUtility.UrlEncode(content, myEncode);
            //��������
            //������ͨ���Ͷ��ŵ�ַ
            var url = "http://www.stongnet.com/sdkhttp/sendsms.aspx";

            //Ҫ���͵�����
            var strSend = "reg=" + strReg + "&pwd=" + strPwd + "&sourceadd=" + strSourceAdd +
                             "&phone=" + strPhone + "&content=" + strContent;

            //����
            var strRes = postSendHX(url, strSend);

            return strRes;
        }

        /// <summary>
        /// POST��ʽ���͵ý��--������ͨ
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bData"></param>
        /// <returns></returns>
        private static String PostRequest(string url, byte[] bData)
        {
            System.Net.HttpWebRequest hwRequest;
            System.Net.HttpWebResponse hwResponse;

            var strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                hwRequest.Timeout = 5000;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                var smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (System.Exception err)
            {
                WriteErrLog(err.ToString());
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                var srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                WriteErrLog(err.ToString());
            }

            return strResult;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="strErr"></param>
        private static void WriteErrLog(string strErr)
        {
            Console.WriteLine(strErr);
            System.Diagnostics.Trace.WriteLine(strErr);
        }

        /// <summary>
        /// /POST��ʽ���͵ý��---Ī��
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string postSend(string url, string param)
        {
            var myEncode = System.Text.Encoding.GetEncoding("UTF-8");
            var postBytes = System.Text.Encoding.UTF8.GetBytes(param);

            var req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                using (var res = req.GetResponse())
                {
                    using (var sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        var strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }
            }
            catch (WebException ex)
            {
                return "�޷����ӵ�������\r\n������Ϣ��" + ex.Message;
            }

        }

        /// <summary>
        /// POST��ʽ���͵ý��--������ͨ
        /// </summary>
        /// <param name="url">������URL</param>
        /// <param name="param">Ҫ���͵Ĳ����ַ���</param>
        /// <returns>�����������ַ���</returns>
        public static string postSendHX(string url, string param)
        {
            var myEncode = System.Text.Encoding.GetEncoding("UTF-8");
            var postBytes = System.Text.Encoding.ASCII.GetBytes(param);

            var req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                using (var res = req.GetResponse())
                {
                    using (var sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        var strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }
            }
            catch (WebException ex)
            {
                return "�޷����ӵ�������\r\n������Ϣ��" + ex.Message;
            }
        }
    }
}
