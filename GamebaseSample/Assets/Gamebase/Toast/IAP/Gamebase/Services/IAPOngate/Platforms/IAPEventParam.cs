using Toast.Internal;

namespace Toast.Iap.Ongate
{
    public sealed class IAPEventParam
    {
        private readonly int mEventId;
        private readonly IAPEvent mEvent;
        private readonly JSONObject json;

        public class Builder
        {

            public int mEventId;
            public IAPEvent mEvent;
            public JSONObject json = new JSONObject();

            public Builder(int aEventId, IAPEvent aEvent)
            {
                this.mEventId = aEventId;
                this.mEvent = aEvent;
            }

            public Builder Add(string key, System.Object paramValue)
            {
                if (paramValue is string)
                    json[key] = (string)paramValue;
                else if (paramValue is int)
                {
                    int param = (int)paramValue;
                    json[key] = System.Convert.ToString(param);
                }
                else if (paramValue is float)
                {
                    float param = (float)paramValue;
                    json[key] = System.Convert.ToString(param);
                }
                else if (paramValue is long)
                {
                    long param = (long)paramValue;
                    json[key] = System.Convert.ToString(param);
                }
                else if (paramValue is double)
                {
                    double param = (double)paramValue;
                    json[key] = System.Convert.ToString(param);
                }
                else
                {
                    json[key] = System.Convert.ToString(paramValue);
                }
                return this;
            }

            public IAPEventParam Build()
            {
                return new IAPEventParam(this);
            }
        }

        private IAPEventParam(Builder builder)
        {
            mEventId = builder.mEventId;
            mEvent = builder.mEvent;
            json = builder.json;
        }

        public override string ToString()
        {
            json[IAPCallbackHandler.EVENT_ID_KEY] = System.Convert.ToString(mEventId);
            json[IAPCallbackHandler.EVENT_KEY] = mEvent.ToString();
            return json.ToString();
        }

        public IAPEvent IAPEvent
        {
            get
            {
                return mEvent;
            }
        }

        public int EventId
        {
            get
            {
                return mEventId;
            }
        }

        public JSONObject Json
        {
            get
            {
                return json;
            }
        }
    }
}