using System;
using System.Collections.Generic;
using System.Web.UI;

namespace EBook.Service
{
    public static class Session
    {
        // session->userId
        private static readonly Dictionary<int, int> SessionMap = new Dictionary<int, int>();

        public static int SetSessionId(int userId)
        {
            var random = new Random();
            var sessionId = random.Next();
            SessionMap.Add(sessionId, userId);
            return sessionId;
        }

        public static int GetUserIdFromSession(int session)
        {
            return SessionMap.ContainsKey(session) ? SessionMap[session] : 0;
        }
    }
}