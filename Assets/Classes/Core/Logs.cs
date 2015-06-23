using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Classes.Cloud;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Classes.Core
{
    public class Logs 
    {

        private Logs()
        {
            
        }

        private static Logs instance;
        public static Logs Instance
        {
            get { return instance ?? (instance = new Logs()); }
        }

        public class LogNode : IAnalyticsEventProvider
        {

            public LogNode()
            {
                
            }

            public LogNode(string title, string description, object data, LogNodeType type, object context)
            {
                Title = title;
                Description = description;
                Data = data;
                Type = type;
                Context = context;
            }

            public string Title { get; set; }
            public string Description { get; set; }
            public object Data { get; set; }
            public object Context { get; set; }
            public LogNodeType Type { get; set; }

            public static LogNode Create(string description, object context, LogNodeType type)
            {
                return new LogNode(string.Empty, description, null, type, context);
            }
            public static LogNode CreateFatalError(string description, object context)
            {
                return new LogNode(string.Empty, description, null, LogNodeType.FatalError, context);
            }

            public string GetConsoleLog()
            {
                if (Context == null) return Description;
                return Context.GetType().Name + ": " + Description;
            }

            public string GetToastMessage()
            {
                return Description;
            }

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Title)) return string.Format("{0}: {1}", Title, Description);
                return Description;
            }

            public string GetAnalyticsEventName()
            {
                return "Log-" + Type;
            }

            public IDictionary<string, object> GetAnalyticsEventData()
            {
                return new Dictionary<string, object>
                       {
                           {"Title", Title},
                           {"Description", Description},
                           {"DataType", Data != null ? Data.GetType().Name : "NULL"},
                           {"ContextType",  Context != null ? Context.GetType().Name : "NULL"},
                           {"Type", Type}
                       };
            }
        }

        public enum LogNodeType
        {
            Message,
            Warning,
            Error,
            FatalError
        }
        [Flags]
        public enum LogOutputFlags
        {
            None = 0x0,
            Toast = 0x1,
            ConsoleLog = 0x2,
            Popup = 0x4,
            Cloud = 0x8
        }


        private const float ErrorPopupShowTime = 3f;



        public void ProcessLog(LogNode logNode, LogOutputFlags displayFlags)
        {
            if ((displayFlags & LogOutputFlags.Cloud) == LogOutputFlags.Cloud) logNode.SendCustomEvent();
            if ((displayFlags & LogOutputFlags.ConsoleLog) == LogOutputFlags.ConsoleLog) Debug.LogError(logNode.GetConsoleLog());
            if ((displayFlags & LogOutputFlags.Toast) == LogOutputFlags.Toast) UIPopups.Instance.ShowToast(logNode.GetToastMessage(), ErrorPopupShowTime);
            if ((displayFlags & LogOutputFlags.Popup) == LogOutputFlags.Popup) UIPopups.Instance.ShowDialog(logNode.Title, logNode.Description, new UIDialogPopup.ActionButton("Exit"));
        }

        public void ProcessMessage(string message, Object context)
        {
            ProcessLog(new LogNode("message", message, null, LogNodeType.Message, context), LogOutputFlags.ConsoleLog );
        }
        public void ProcessMessage(string message)
        {
            ProcessMessage(message, null);
        }

        public void ProcessError(string message, Object context)
        {
            ProcessLog(new LogNode("error", message, null, LogNodeType.Error, context), LogOutputFlags.ConsoleLog);
        }
        public void ProcessError(string message)
        {
            ProcessError(message, null);
        }

        public void ProcessFatalError(string message)
        {
            ProcessLog(new LogNode("fatalError", message, null, LogNodeType.FatalError, null), LogOutputFlags.ConsoleLog | LogOutputFlags.Cloud);
        }
    }
}
