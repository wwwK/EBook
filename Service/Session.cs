using System;
using System.Collections.Generic;
using System.Web.UI;

namespace EBook.Service
{
    public static class Session
    {
        private class SessionItem
        {
            public int UserId;
            public DateTime Expire;
        }
        
        // session->userId
        private static readonly Dictionary<int, SessionItem> SessionMap = new Dictionary<int, SessionItem>();

        public static int SetSessionId(int userId)
        {
            var random = new Random();
            var sessionId = random.Next();
            SessionMap.Add(sessionId, new SessionItem()
            {
                UserId = userId,
                Expire = DateTime.Now.AddHours(1)
            });
            return sessionId;
        }

        // :return: userId:success, -1: Session not found, -2: session expired
        public static int GetUserIdFromSession(int session)
        {
            if (SessionMap.ContainsKey(session))
            {
                if (SessionMap[session].Expire < DateTime.Now)
                {
                    SessionMap.Remove(session);
                    return -2;
                }
                SessionMap[session].Expire = DateTime.Now.AddHours(1);
                return SessionMap[session].UserId;
            }
            else
            {
                return -1;
            }
        }
    }
}