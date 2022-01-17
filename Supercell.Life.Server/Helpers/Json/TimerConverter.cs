namespace Supercell.Life.Server.Helpers.Json
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.Logic.Math;

    internal class TimerConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        public override bool CanConvert(Type type)
        {
            return type == typeof(LogicTimer) || type == typeof(Dictionary<int, LogicTimer>);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter" /> can read JSON.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter" /> can write JSON.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Reads the json.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jTimer = JObject.Load(reader);

            if (objectType == typeof(Dictionary<int, LogicTimer>))
            {
                if (jTimer.HasValues)
                {
                    var timers = (Dictionary<int, LogicTimer>)existingValue;

                    foreach (var timer in jTimer.ToObject<Dictionary<int, LogicTimer>>())
                    {
                        var t = (LogicTimer)this.ReadTimer((int)(jTimer[$"{timer.Key}"]["t"] ?? -1), new LogicTimer(new LogicTime()));
                        timers.Add(timer.Key, t);
                    }

                    return timers;
                }
            }

            return this.ReadTimer((int)(jTimer["t"] ?? -1), existingValue);
        }

        /// <summary>
        /// Reads the timer from the JSON.
        /// </summary>
        private object ReadTimer(int remainingTime, object existingValue)
        {
            if (remainingTime >= 0)
            {
                LogicTimer timer = (LogicTimer)existingValue;

                if (timer.Time != null)
                {
                    timer.StartTimer(remainingTime);
                }
                else
                {
                    timer.TotalSeconds = remainingTime;
                    timer.StartSubTick = 0;
                }
            }

            return existingValue;
        }

        /// <summary>
        /// Writes the json.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer.Path.Split(".")[1].Equals("Timers"))
            {
                writer.WriteStartObject();

                if (value != null)
                {
                    Dictionary<int, LogicTimer> timers = (Dictionary<int, LogicTimer>)value;

                    foreach (var (id, timer) in timers)
                    {
                        if (timer.Started)
                        {
                            writer.WritePropertyName($"{id}");
                            writer.WriteStartObject();
                            writer.WritePropertyName("t");
                            writer.WriteValue(timer.RemainingSecs);
                            writer.WriteEndObject();
                        }
                    }
                }

                writer.WriteEndObject();
            }
            else
            {
                int remaining = -1;

                if (value != null)
                {
                    LogicTimer timer = (LogicTimer)value;

                    if (timer.Started)
                    {
                        remaining = timer.RemainingSecs;
                    }
                }

                writer.WriteStartObject();
                writer.WritePropertyName("t");
                writer.WriteValue(remaining);
                writer.WriteEndObject();
            }
        }
    }
}