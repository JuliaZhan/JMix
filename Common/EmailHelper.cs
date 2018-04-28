using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Net;

namespace Common
{
    /// <summary>
    /// 概述：邮箱公共类
    /// 文件：Common.EmailHelper
    /// 作者：詹詹    创建时间：2018/4/27 16:34:02
    /// 描述：
    ///    > add description for EmailHelper
    ///    修改历史：
    ///
    /// <summary>
    public class EmailHelper
    {
        private static ExchangeService _exchangeService = new ExchangeService(ExchangeVersion.Exchange2013);
        /// <summary>
        /// 获取收件箱
        /// </summary>
        /// <param name="userId">当前用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="domain">域</param>
        /// <param name="pageSize">一次加载的数量</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public static List<Email> GetInbox(string userId, string pwd, string domain, int pageSize, int offset)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(domain))
                {
                    throw new ArgumentNullException("当前用户信息为空，无法访问exchange服务器");
                }
                List<Email> lstEmails = new List<Email>();
                _exchangeService.Credentials = new NetworkCredential(userId, pwd, domain);//发送邮箱
                _exchangeService.Url = new Uri("https://mail.jcgroup.com.cn/Exchange.asmx");//服务器邮箱
                ItemView view = new ItemView(pageSize, offset);
                FindItemsResults<Item> findResults = _exchangeService.FindItems(WellKnownFolderName.Inbox, SetFilter(), view);
                foreach (Item item in findResults.Items)
                {
                    item.Load(PropertySet.FirstClassProperties);
                    lstEmails.Add(new Email()
                    {
                        ExchangeItemId = item.Id.ChangeKey,
                        body = item.Body.Text,
                        Mail_cc = item.DisplayCc,
                        Mail_from = item.LastModifiedName,
                        IsRead = item.IsNew,
                        Subject = item.Subject,
                        CreateOn = item.DateTimeCreated
                    });
                }
                return lstEmails;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 根据用户邮件地址返回用户的未读邮件数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int GetUnReadMailCountByUserMailAddress(string userId, string pwd, string domain, string email)
        {
            int unRead = 0;
            try
            {
                _exchangeService.Credentials = new NetworkCredential(userId, pwd, domain);
                _exchangeService.Url = new Uri("https://mail.jcgroup.com.cn/Exchange.asmx");//服务器邮箱
                _exchangeService.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, email);

                unRead = Folder.Bind(_exchangeService, WellKnownFolderName.Inbox).UnreadCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return unRead;
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <returns></returns>
        private static SearchFilter SetFilter()
        {
            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
            //searchFilterCollection.Add(new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
            //searchFilterCollection.Add(new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, true));
            //筛选今天的邮件
            SearchFilter start = new SearchFilter.IsGreaterThanOrEqualTo(EmailMessageSchema.DateTimeCreated, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")));
            SearchFilter end = new SearchFilter.IsLessThanOrEqualTo(EmailMessageSchema.DateTimeCreated, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")));
            searchFilterCollection.Add(start);
            searchFilterCollection.Add(end);
            SearchFilter filter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());
            return filter;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static void SendMail(Email email, string userId, string pwd, string domain)
        {
            try
            {
                _exchangeService.Credentials = new NetworkCredential(userId, pwd, domain);
                _exchangeService.Url = new Uri("https://mail.jcgroup.com.cn/Exchange.asmx");//服务器邮箱
                //发送人
                Mailbox mail = new Mailbox(email.Mail_from);
                //邮件内容
                EmailMessage message = new EmailMessage(_exchangeService);
                string[] strTos = email.Mail_to.Split(';');
                //接收人
                foreach (string item in strTos)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        message.ToRecipients.Add(item);
                    }
                }
                //抄送人
                foreach (string item in email.Mail_cc.Split(';'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        message.CcRecipients.Add(item);
                    }

                }
                //邮件标题
                message.Subject = email.Subject;
                //邮件内容
                message.Body = new MessageBody(email.body);
                //发送并且保存
                message.SendAndSaveCopy();

            }
            catch (Exception ex)
            {
                throw new Exception("发送邮件出错，" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }
    }
    public class Email
    {
        /// <summary>
        /// 
        /// </summary>
        public string ExchangeItemId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 抄送
        /// </summary>
        public string Mail_cc { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string Mail_from { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string Mail_to { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }                       
    }
}
